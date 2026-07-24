using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrubCRM.Models;

namespace ScrubCRM.Data
{
    // Creates the database from the Code-First model on first run and seeds
    // demo data. This avoids depending on the Visual Studio / Package Manager
    // Console EF Migrations tooling (Enable-Migrations / Add-Migration), which
    // is not available in every environment. See Database/Schema.sql for an
    // equivalent hand-written script that can be run directly against Azure SQL.
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        public const string SeedAdminEmail = "admin@luxurylinecrm.com";
        public const string SeedAdminPassword = "ChangeMe!2026";
        public const string AdministratorRole = "Administrator";

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists(AdministratorRole))
            {
                roleManager.Create(new IdentityRole(AdministratorRole));
            }

            var admin = new ApplicationUser
            {
                UserName = SeedAdminEmail,
                Email = SeedAdminEmail,
                DisplayName = "Administrator",
                EmailConfirmed = true
            };
            var createResult = userManager.Create(admin, SeedAdminPassword);
            if (createResult.Succeeded)
            {
                userManager.AddToRole(admin.Id, AdministratorRole);
            }

            var adminId = admin.Id;

            // Products
            var cherokeeTop = new Product
            {
                ProductName = "Cherokee Workwear Revolution V-Neck Top",
                SKU = "CHK-REV-TOP",
                Brand = "Cherokee",
                Category = "Tops",
                Style = "V-Neck",
                Description = "Four-way stretch scrub top with two front pockets.",
                UnitCost = 9.50m,
                RetailPrice = 24.99m,
                WholesalePrice = 17.50m,
                ReorderLevel = 15,
                IsActive = true
            };
            var figsJogger = new Product
            {
                ProductName = "FIGS Livingston Jogger Pant",
                SKU = "FIGS-LVJ-PNT",
                Brand = "FIGS",
                Category = "Pants",
                Style = "Jogger",
                Description = "Slim-fit jogger scrub pant with elastic cuffs.",
                UnitCost = 14.00m,
                RetailPrice = 38.00m,
                WholesalePrice = 27.00m,
                ReorderLevel = 15,
                IsActive = true
            };
            var wonderWinkTop = new Product
            {
                ProductName = "WonderWink Origins Delta Top",
                SKU = "WW-ORG-DELTA",
                Brand = "WonderWink",
                Category = "Tops",
                Style = "Mock Wrap",
                Description = "Classic mock-wrap scrub top with chest pocket.",
                UnitCost = 8.75m,
                RetailPrice = 22.99m,
                WholesalePrice = 16.00m,
                ReorderLevel = 20,
                IsActive = true
            };
            var barcoPant = new Product
            {
                ProductName = "Barco One Boost Cargo Pant",
                SKU = "BAR-BOOST-CARGO",
                Brand = "Barco One",
                Category = "Pants",
                Style = "Cargo",
                Description = "Athletic-fit cargo scrub pant with side pocket.",
                UnitCost = 13.25m,
                RetailPrice = 36.00m,
                WholesalePrice = 25.50m,
                ReorderLevel = 15,
                IsActive = true
            };
            var greysAnatomyTop = new Product
            {
                ProductName = "Grey's Anatomy Signature V-Neck Top",
                SKU = "GA-SIG-VNECK",
                Brand = "Grey's Anatomy",
                Category = "Tops",
                Style = "V-Neck",
                Description = "Signature fit scrub top with three pockets.",
                UnitCost = 10.00m,
                RetailPrice = 27.99m,
                WholesalePrice = 19.50m,
                ReorderLevel = 15,
                IsActive = true
            };

            context.Products.AddRange(new[] { cherokeeTop, figsJogger, wonderWinkTop, barcoPant, greysAnatomyTop });
            context.SaveChanges();

            string[] sizes = { "Small", "Medium", "Large", "X-Large" };

            var variants = new List<ProductVariant>();
            foreach (var color in new[] { "Navy", "Black" })
            {
                foreach (var size in sizes)
                {
                    variants.Add(new ProductVariant
                    {
                        ProductId = cherokeeTop.ProductId,
                        Size = size,
                        Color = color,
                        VariantSku = cherokeeTop.SKU + "-" + color.ToUpperInvariant().Substring(0, 3) + "-" + size.Substring(0, 1),
                        QuantityOnHand = color == "Navy" ? 40 : 25
                    });
                }
            }
            foreach (var color in new[] { "Ceil Blue", "Black" })
            {
                foreach (var size in sizes)
                {
                    variants.Add(new ProductVariant
                    {
                        ProductId = figsJogger.ProductId,
                        Size = size,
                        Color = color,
                        VariantSku = figsJogger.SKU + "-" + (color == "Ceil Blue" ? "CEB" : "BLK") + "-" + size.Substring(0, 1),
                        QuantityOnHand = 5
                    });
                }
            }
            foreach (var size in sizes)
            {
                variants.Add(new ProductVariant
                {
                    ProductId = wonderWinkTop.ProductId,
                    Size = size,
                    Color = "Wine",
                    VariantSku = wonderWinkTop.SKU + "-WIN-" + size.Substring(0, 1),
                    QuantityOnHand = 0
                });
            }
            foreach (var size in sizes)
            {
                variants.Add(new ProductVariant
                {
                    ProductId = barcoPant.ProductId,
                    Size = size,
                    Color = "Pewter",
                    VariantSku = barcoPant.SKU + "-PEW-" + size.Substring(0, 1),
                    QuantityOnHand = 18
                });
            }
            foreach (var size in sizes)
            {
                variants.Add(new ProductVariant
                {
                    ProductId = greysAnatomyTop.ProductId,
                    Size = size,
                    Color = "Galaxy Blue",
                    VariantSku = greysAnatomyTop.SKU + "-GLX-" + size.Substring(0, 1),
                    QuantityOnHand = 22
                });
            }

            context.ProductVariants.AddRange(variants);
            context.SaveChanges();

            // Customers
            context.Customers.AddRange(new[]
            {
                new Customer { FirstName = "Maria", LastName = "Gonzalez", Email = "maria.gonzalez@example.com", Phone = "555-201-4433", Address = "12 Oak St, Springfield, IL", Notes = "Prefers navy scrubs." },
                new Customer { FirstName = "James", LastName = "Whitfield", Email = "james.whitfield@example.com", Phone = "555-201-9821", Address = "48 Birch Ave, Springfield, IL" },
                new Customer { FirstName = "Priya", LastName = "Natarajan", Email = "priya.natarajan@example.com", Phone = "555-201-1120", Address = "901 Maple Dr, Springfield, IL", Notes = "Repeat customer, size Medium." },
                new Customer { FirstName = "David", LastName = "Chen", Email = "david.chen@example.com", Phone = "555-201-7765", Address = "220 Elm St, Springfield, IL" },
                new Customer { FirstName = "Angela", LastName = "Brooks", Email = "angela.brooks@example.com", Phone = "555-201-3390", Address = "77 Cedar Ln, Springfield, IL" }
            });

            // Organizations
            var stateU = new Organization { OrganizationName = "Springfield State University - School of Nursing", OrganizationType = OrganizationType.University, PrimaryContactName = "Dr. Susan Reyes", Email = "sreyes@springfieldstate.edu", Phone = "555-330-1000", Address = "1 University Way, Springfield, IL", Notes = "Annual bulk order each August." };
            var riverside = new Organization { OrganizationName = "Riverside Medical Group", OrganizationType = OrganizationType.WholesaleClient, PrimaryContactName = "Tom Alvarez", Email = "purchasing@riversidemedical.example.com", Phone = "555-330-2200", Address = "500 Riverside Pkwy, Springfield, IL" };
            var lakeside = new Organization { OrganizationName = "Lakeside Community College - Allied Health", OrganizationType = OrganizationType.University, PrimaryContactName = "Karen Miles", Email = "kmiles@lakesidecc.edu", Phone = "555-330-3100", Address = "88 Lakeside Dr, Springfield, IL" };

            context.Organizations.AddRange(new[] { stateU, riverside, lakeside });
            context.SaveChanges();

            var customerList = context.Customers.ToList();
            var cherokeeVariant = variants.First(v => v.ProductId == cherokeeTop.ProductId && v.Color == "Navy" && v.Size == "Medium");
            var barcoVariant = variants.First(v => v.ProductId == barcoPant.ProductId && v.Size == "Large");
            var greysVariant = variants.First(v => v.ProductId == greysAnatomyTop.ProductId && v.Size == "Small");

            var order1 = new OrderRequest
            {
                RequestNumber = "ORD-00001",
                CustomerId = customerList[0].CustomerId,
                RequestDate = DateTime.UtcNow.AddDays(-5),
                RequestedDeliveryDate = DateTime.UtcNow.AddDays(3),
                Status = OrderStatus.Submitted,
                Notes = "Customer requested gift receipt."
            };
            order1.Items.Add(new OrderRequestItem { ProductVariantId = cherokeeVariant.ProductVariantId, QuantityRequested = 2, UnitPrice = cherokeeTop.RetailPrice });

            var order2 = new OrderRequest
            {
                RequestNumber = "ORD-00002",
                OrganizationId = stateU.OrganizationId,
                RequestDate = DateTime.UtcNow.AddDays(-10),
                RequestedDeliveryDate = DateTime.UtcNow.AddDays(20),
                Status = OrderStatus.Approved,
                Notes = "First-year nursing cohort order."
            };
            order2.Items.Add(new OrderRequestItem { ProductVariantId = barcoVariant.ProductVariantId, QuantityRequested = 30, UnitPrice = barcoPant.WholesalePrice });
            order2.Items.Add(new OrderRequestItem { ProductVariantId = greysVariant.ProductVariantId, QuantityRequested = 30, UnitPrice = greysAnatomyTop.WholesalePrice });

            var order3 = new OrderRequest
            {
                RequestNumber = "ORD-00003",
                OrganizationId = riverside.OrganizationId,
                RequestDate = DateTime.UtcNow.AddDays(-20),
                RequestedDeliveryDate = DateTime.UtcNow.AddDays(-5),
                Status = OrderStatus.Completed,
                Notes = "Completed and delivered."
            };
            order3.Items.Add(new OrderRequestItem { ProductVariantId = cherokeeVariant.ProductVariantId, QuantityRequested = 5, UnitPrice = cherokeeTop.WholesalePrice });

            context.OrderRequests.AddRange(new[] { order1, order2, order3 });
            context.SaveChanges();

            context.OrderStatusHistories.AddRange(new[]
            {
                new OrderStatusHistory { OrderRequestId = order1.OrderRequestId, OldStatus = null, NewStatus = OrderStatus.Submitted, AdministratorId = adminId, DateChanged = order1.RequestDate },
                new OrderStatusHistory { OrderRequestId = order2.OrderRequestId, OldStatus = null, NewStatus = OrderStatus.Submitted, AdministratorId = adminId, DateChanged = order2.RequestDate },
                new OrderStatusHistory { OrderRequestId = order2.OrderRequestId, OldStatus = OrderStatus.Submitted, NewStatus = OrderStatus.Approved, AdministratorId = adminId, DateChanged = order2.RequestDate.AddDays(1) },
                new OrderStatusHistory { OrderRequestId = order3.OrderRequestId, OldStatus = null, NewStatus = OrderStatus.Submitted, AdministratorId = adminId, DateChanged = order3.RequestDate },
                new OrderStatusHistory { OrderRequestId = order3.OrderRequestId, OldStatus = OrderStatus.Submitted, NewStatus = OrderStatus.Completed, AdministratorId = adminId, DateChanged = order3.RequestDate.AddDays(2) }
            });

            // order3 was seeded as Completed directly, so reflect the inventory
            // deduction that would have happened when it was completed.
            var completedVariant = context.ProductVariants.Single(v => v.ProductVariantId == cherokeeVariant.ProductVariantId);
            completedVariant.QuantityOnHand -= 5;
            context.InventoryTransactions.Add(new InventoryTransaction
            {
                ProductVariantId = completedVariant.ProductVariantId,
                QuantityChange = -5,
                Reason = "Order ORD-00003 completed",
                AdministratorId = adminId,
                TransactionDate = order3.RequestDate.AddDays(2)
            });

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
