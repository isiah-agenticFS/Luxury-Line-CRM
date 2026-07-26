using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ScrubCRM.Data;
using ScrubCRM.Models;

namespace ScrubCRM.Services
{
    // Thrown when a requested change violates a business rule (e.g. completing
    // an order without enough inventory, or reversing a completed order).
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }

    public class ProductService
    {
        public List<Product> GetProducts(string search, bool includeInactive)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Products.AsQueryable();
                if (!includeInactive) query = query.Where(p => p.IsActive);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(p => p.ProductName.Contains(search) || p.SKU.Contains(search));
                }
                return query.OrderBy(p => p.ProductName).ToList();
            }
        }

        public Product GetProduct(int productId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Products.Include(p => p.Variants).FirstOrDefault(p => p.ProductId == productId);
            }
        }

        public List<ProductVariant> GetActiveVariantsForOrderEntry()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.ProductVariants
                    .Include(v => v.Product)
                    .Where(v => v.Product.IsActive)
                    .OrderBy(v => v.Product.ProductName).ThenBy(v => v.Color).ThenBy(v => v.Size)
                    .ToList();
            }
        }

        public int SaveProduct(Product product)
        {
            using (var db = new ApplicationDbContext())
            {
                if (product.ProductId == 0)
                {
                    product.CreatedDate = DateTime.UtcNow;
                    product.UpdatedDate = DateTime.UtcNow;
                    db.Products.Add(product);
                }
                else
                {
                    var existing = db.Products.Single(p => p.ProductId == product.ProductId);
                    existing.ProductName = product.ProductName;
                    existing.SKU = product.SKU;
                    existing.Brand = product.Brand;
                    existing.Category = product.Category;
                    existing.Style = product.Style;
                    existing.Description = product.Description;
                    existing.UnitCost = product.UnitCost;
                    existing.RetailPrice = product.RetailPrice;
                    existing.WholesalePrice = product.WholesalePrice;
                    existing.ReorderLevel = product.ReorderLevel;
                    existing.IsActive = product.IsActive;
                    existing.UpdatedDate = DateTime.UtcNow;
                }
                db.SaveChanges();
                return product.ProductId;
            }
        }

        public void DeactivateProduct(int productId)
        {
            using (var db = new ApplicationDbContext())
            {
                var product = db.Products.Single(p => p.ProductId == productId);
                product.IsActive = false;
                product.UpdatedDate = DateTime.UtcNow;
                db.SaveChanges();
            }
        }

        public void SaveVariant(ProductVariant variant)
        {
            using (var db = new ApplicationDbContext())
            {
                if (variant.ProductVariantId == 0)
                {
                    db.ProductVariants.Add(variant);
                }
                else
                {
                    var existing = db.ProductVariants.Single(v => v.ProductVariantId == variant.ProductVariantId);
                    existing.Size = variant.Size;
                    existing.Color = variant.Color;
                    existing.VariantSku = variant.VariantSku;
                    // QuantityOnHand is intentionally not updated here; adjustments
                    // must go through InventoryService so a transaction is logged.
                }
                db.SaveChanges();
            }
        }
    }

    public class InventoryService
    {
        public const int LowStockThresholdBufferAboveReorder = 0;

        public List<ProductVariant> GetVariantsWithStock(string search)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.ProductVariants.Include(v => v.Product).AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(v => v.Product.ProductName.Contains(search)
                        || v.Product.SKU.Contains(search)
                        || v.VariantSku.Contains(search));
                }
                return query.OrderBy(v => v.Product.ProductName).ThenBy(v => v.Color).ThenBy(v => v.Size).ToList();
            }
        }

        public string GetStockStatus(ProductVariant variant)
        {
            if (variant.QuantityOnHand <= 0) return "Out of Stock";
            if (variant.QuantityOnHand <= variant.Product.ReorderLevel) return "Low Stock";
            return "In Stock";
        }

        public void AdjustInventory(int productVariantId, int quantityChange, string reason, string administratorId)
        {
            using (var db = new ApplicationDbContext())
            {
                var variant = db.ProductVariants.Single(v => v.ProductVariantId == productVariantId);
                var newQuantity = variant.QuantityOnHand + quantityChange;
                if (newQuantity < 0)
                {
                    throw new BusinessRuleException("This adjustment would take inventory below zero. Current quantity is " + variant.QuantityOnHand + ".");
                }

                variant.QuantityOnHand = newQuantity;
                db.InventoryTransactions.Add(new InventoryTransaction
                {
                    ProductVariantId = productVariantId,
                    QuantityChange = quantityChange,
                    Reason = reason,
                    AdministratorId = administratorId,
                    TransactionDate = DateTime.UtcNow
                });
                db.SaveChanges();
            }
        }

        // Deducts inventory for every item on an order. All items are validated
        // before anything is written, so a shortage on one item blocks the
        // whole completion rather than deducting a partial order.
        public void DeductForCompletedOrder(int orderRequestId, string administratorId)
        {
            using (var db = new ApplicationDbContext())
            {
                var order = db.OrderRequests.Include(o => o.Items.Select(i => i.ProductVariant)).Single(o => o.OrderRequestId == orderRequestId);

                var shortages = order.Items
                    .Where(i => i.ProductVariant.QuantityOnHand < i.QuantityRequested)
                    .Select(i => i.ProductVariant.DisplayName + " (need " + i.QuantityRequested + ", have " + i.ProductVariant.QuantityOnHand + ")")
                    .ToList();

                if (shortages.Any())
                {
                    throw new BusinessRuleException("Not enough inventory to complete this order: " + string.Join("; ", shortages));
                }

                foreach (var item in order.Items)
                {
                    item.ProductVariant.QuantityOnHand -= item.QuantityRequested;
                    db.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductVariantId = item.ProductVariantId,
                        QuantityChange = -item.QuantityRequested,
                        Reason = "Order " + order.RequestNumber + " completed",
                        AdministratorId = administratorId,
                        TransactionDate = DateTime.UtcNow
                    });
                }

                db.SaveChanges();
            }
        }
    }

    public class CustomerService
    {
        public List<Customer> GetCustomers(string search)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Customers.AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(c => c.FirstName.Contains(search) || c.LastName.Contains(search) || c.Email.Contains(search));
                }
                return query.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            }
        }

        public Customer GetCustomer(int customerId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            }
        }

        public int SaveCustomer(Customer customer)
        {
            using (var db = new ApplicationDbContext())
            {
                if (customer.CustomerId == 0)
                {
                    customer.CreatedDate = DateTime.UtcNow;
                    db.Customers.Add(customer);
                }
                else
                {
                    var existing = db.Customers.Single(c => c.CustomerId == customer.CustomerId);
                    existing.FirstName = customer.FirstName;
                    existing.LastName = customer.LastName;
                    existing.Email = customer.Email;
                    existing.Phone = customer.Phone;
                    existing.Address = customer.Address;
                    existing.Notes = customer.Notes;
                }
                db.SaveChanges();
                return customer.CustomerId;
            }
        }
    }

    public class OrganizationService
    {
        public List<Organization> GetOrganizations(string search)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.Organizations.AsQueryable();
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(o => o.OrganizationName.Contains(search));
                }
                return query.OrderBy(o => o.OrganizationName).ToList();
            }
        }

        public Organization GetOrganization(int organizationId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Organizations.FirstOrDefault(o => o.OrganizationId == organizationId);
            }
        }

        public int SaveOrganization(Organization organization)
        {
            using (var db = new ApplicationDbContext())
            {
                if (organization.OrganizationId == 0)
                {
                    organization.CreatedDate = DateTime.UtcNow;
                    db.Organizations.Add(organization);
                }
                else
                {
                    var existing = db.Organizations.Single(o => o.OrganizationId == organization.OrganizationId);
                    existing.OrganizationName = organization.OrganizationName;
                    existing.OrganizationType = organization.OrganizationType;
                    existing.PrimaryContactName = organization.PrimaryContactName;
                    existing.Email = organization.Email;
                    existing.Phone = organization.Phone;
                    existing.Address = organization.Address;
                    existing.Notes = organization.Notes;
                }
                db.SaveChanges();
                return organization.OrganizationId;
            }
        }
    }

    public class OrderService
    {
        private readonly InventoryService _inventoryService = new InventoryService();

        public List<OrderRequest> GetOrders(string search, OrderStatus? statusFilter)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.OrderRequests.Include(o => o.Customer).Include(o => o.Organization).AsQueryable();
                if (statusFilter.HasValue) query = query.Where(o => o.Status == statusFilter.Value);
                if (!string.IsNullOrWhiteSpace(search))
                {
                    search = search.Trim();
                    query = query.Where(o => o.RequestNumber.Contains(search)
                        || (o.Customer != null && (o.Customer.FirstName + " " + o.Customer.LastName).Contains(search))
                        || (o.Organization != null && o.Organization.OrganizationName.Contains(search)));
                }
                return query.OrderByDescending(o => o.RequestDate).ToList();
            }
        }

        public List<OrderRequest> GetRecentOrders(int count)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.OrderRequests.Include(o => o.Customer).Include(o => o.Organization)
                    .OrderByDescending(o => o.CreatedDate).Take(count).ToList();
            }
        }

        public OrderRequest GetOrder(int orderRequestId)
        {
            using (var db = new ApplicationDbContext())
            {
                return db.OrderRequests
                    .Include(o => o.Customer)
                    .Include(o => o.Organization)
                    .Include(o => o.Items.Select(i => i.ProductVariant.Product))
                    .Include(o => o.StatusHistory.Select(h => h.Administrator))
                    .FirstOrDefault(o => o.OrderRequestId == orderRequestId);
            }
        }

        public string GenerateNextRequestNumber()
        {
            using (var db = new ApplicationDbContext())
            {
                var count = db.OrderRequests.Count();
                return "ORD-" + (count + 1).ToString("D5");
            }
        }

        public int CreateOrder(OrderRequest order, List<OrderRequestItem> items, string administratorId)
        {
            using (var db = new ApplicationDbContext())
            {
                order.RequestNumber = string.IsNullOrWhiteSpace(order.RequestNumber)
                    ? "ORD-" + (db.OrderRequests.Count() + 1).ToString("D5")
                    : order.RequestNumber;
                order.CreatedDate = DateTime.UtcNow;
                order.UpdatedDate = DateTime.UtcNow;
                order.Status = OrderStatus.Submitted;
                foreach (var item in items) order.Items.Add(item);
                db.OrderRequests.Add(order);
                db.SaveChanges();

                db.OrderStatusHistories.Add(new OrderStatusHistory
                {
                    OrderRequestId = order.OrderRequestId,
                    OldStatus = null,
                    NewStatus = OrderStatus.Submitted,
                    AdministratorId = administratorId,
                    DateChanged = DateTime.UtcNow
                });
                db.SaveChanges();

                return order.OrderRequestId;
            }
        }

        public void UpdateOrderDetails(OrderRequest order, List<OrderRequestItem> items)
        {
            using (var db = new ApplicationDbContext())
            {
                var existing = db.OrderRequests.Include(o => o.Items).Single(o => o.OrderRequestId == order.OrderRequestId);

                if (existing.Status == OrderStatus.Completed)
                {
                    throw new BusinessRuleException("This order is Completed and its items can no longer be edited.");
                }

                existing.CustomerId = order.CustomerId;
                existing.OrganizationId = order.OrganizationId;
                existing.RequestedDeliveryDate = order.RequestedDeliveryDate;
                existing.Notes = order.Notes;
                existing.UpdatedDate = DateTime.UtcNow;

                db.OrderRequestItems.RemoveRange(existing.Items);
                foreach (var item in items)
                {
                    existing.Items.Add(new OrderRequestItem
                    {
                        ProductVariantId = item.ProductVariantId,
                        QuantityRequested = item.QuantityRequested,
                        UnitPrice = item.UnitPrice,
                        Notes = item.Notes
                    });
                }

                db.SaveChanges();
            }
        }

        // Enforces: no inventory deduction until Completed; a Completed order
        // cannot move to any other status; completion requires sufficient stock.
        public void ChangeStatus(int orderRequestId, OrderStatus newStatus, string administratorId)
        {
            using (var db = new ApplicationDbContext())
            {
                var order = db.OrderRequests.Single(o => o.OrderRequestId == orderRequestId);
                var oldStatus = order.Status;

                if (oldStatus == newStatus) return;

                if (oldStatus == OrderStatus.Completed)
                {
                    throw new BusinessRuleException("Completed orders cannot change status because inventory has already been deducted. Create a new order instead.");
                }

                if (newStatus == OrderStatus.Completed)
                {
                    _inventoryService.DeductForCompletedOrder(orderRequestId, administratorId);
                }

                order.Status = newStatus;
                order.UpdatedDate = DateTime.UtcNow;
                db.SaveChanges();

                db.OrderStatusHistories.Add(new OrderStatusHistory
                {
                    OrderRequestId = orderRequestId,
                    OldStatus = oldStatus,
                    NewStatus = newStatus,
                    AdministratorId = administratorId,
                    DateChanged = DateTime.UtcNow
                });
                db.SaveChanges();
            }
        }

        public DashboardSummary GetDashboardSummary()
        {
            using (var db = new ApplicationDbContext())
            {
                var variants = db.ProductVariants.Include(v => v.Product).ToList();
                return new DashboardSummary
                {
                    TotalProducts = db.Products.Count(p => p.IsActive),
                    TotalInventoryUnits = variants.Sum(v => v.QuantityOnHand),
                    LowStockVariants = variants.Count(v => v.QuantityOnHand > 0 && v.QuantityOnHand <= v.Product.ReorderLevel),
                    OutOfStockVariants = variants.Count(v => v.QuantityOnHand <= 0),
                    TotalCustomers = db.Customers.Count(),
                    TotalOrganizations = db.Organizations.Count(),
                    PendingOrderRequests = db.OrderRequests.Count(o => o.Status != OrderStatus.Completed && o.Status != OrderStatus.Cancelled),
                    CompletedOrders = db.OrderRequests.Count(o => o.Status == OrderStatus.Completed)
                };
            }
        }
    }

    public class DashboardSummary
    {
        public int TotalProducts { get; set; }
        public int TotalInventoryUnits { get; set; }
        public int LowStockVariants { get; set; }
        public int OutOfStockVariants { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalOrganizations { get; set; }
        public int PendingOrderRequests { get; set; }
        public int CompletedOrders { get; set; }
    }
}
