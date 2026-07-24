using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class OrderEdit : Page
    {
        protected Literal litHeading;
        protected HiddenField hfOrderId;
        protected Label lblFormError;
        protected DropDownList ddlCustomer;
        protected DropDownList ddlOrganization;
        protected TextBox txtDeliveryDate;
        protected TextBox txtNotes;
        protected Label lblItemError;
        protected GridView gvItems;
        protected Literal litOrderTotal;
        protected DropDownList ddlVariant;
        protected TextBox txtItemQty;
        protected TextBox txtItemPrice;

        private readonly OrderService _orderService = new OrderService();
        private readonly CustomerService _customerService = new CustomerService();
        private readonly OrganizationService _organizationService = new OrganizationService();
        private readonly ProductService _productService = new ProductService();

        [Serializable]
        public class OrderItemRow
        {
            public int ProductVariantId { get; set; }
            public string DisplayName { get; set; }
            public int QuantityRequested { get; set; }
            public decimal UnitPrice { get; set; }

            public decimal LineTotal
            {
                get { return QuantityRequested * UnitPrice; }
            }
        }

        private List<OrderItemRow> Items
        {
            get
            {
                if (ViewState["Items"] == null) ViewState["Items"] = new List<OrderItemRow>();
                return (List<OrderItemRow>)ViewState["Items"];
            }
            set { ViewState["Items"] = value; }
        }

        private string CurrentAdminId
        {
            get { return User.Identity.GetUserId(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDowns();

                var idParam = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idParam))
                {
                    LoadOrder(Convert.ToInt32(idParam));
                }
                else
                {
                    hfOrderId.Value = "0";
                    Items = new List<OrderItemRow>();
                }

                BindItems();
            }
        }

        private void BindDropDowns()
        {
            ddlCustomer.Items.Clear();
            ddlCustomer.Items.Add(new ListItem("-- none --", "0"));
            foreach (var c in _customerService.GetCustomers(null))
            {
                ddlCustomer.Items.Add(new ListItem(c.FullName, c.CustomerId.ToString()));
            }

            ddlOrganization.Items.Clear();
            ddlOrganization.Items.Add(new ListItem("-- none --", "0"));
            foreach (var o in _organizationService.GetOrganizations(null))
            {
                ddlOrganization.Items.Add(new ListItem(o.OrganizationName, o.OrganizationId.ToString()));
            }

            ddlVariant.Items.Clear();
            foreach (var v in _productService.GetActiveVariantsForOrderEntry())
            {
                ddlVariant.Items.Add(new ListItem(v.DisplayName, v.ProductVariantId.ToString()));
            }

            if (ddlVariant.Items.Count > 0)
            {
                SetPriceFromSelectedVariant();
            }
        }

        private void LoadOrder(int orderRequestId)
        {
            var order = _orderService.GetOrder(orderRequestId);
            if (order == null)
            {
                Response.Redirect("~/Admin/Orders.aspx", false);
                return;
            }

            if (order.Status == OrderStatus.Completed)
            {
                Response.Redirect("~/Admin/OrderDetails.aspx?id=" + orderRequestId, false);
                return;
            }

            hfOrderId.Value = order.OrderRequestId.ToString();
            litHeading.Text = "Edit Order Request: " + order.RequestNumber;

            ddlCustomer.SelectedValue = order.CustomerId.HasValue ? order.CustomerId.Value.ToString() : "0";
            ddlOrganization.SelectedValue = order.OrganizationId.HasValue ? order.OrganizationId.Value.ToString() : "0";
            txtDeliveryDate.Text = order.RequestedDeliveryDate.HasValue ? order.RequestedDeliveryDate.Value.ToString("yyyy-MM-dd") : "";
            txtNotes.Text = order.Notes;

            Items = order.Items.Select(i => new OrderItemRow
            {
                ProductVariantId = i.ProductVariantId,
                DisplayName = i.ProductVariant.DisplayName,
                QuantityRequested = i.QuantityRequested,
                UnitPrice = i.UnitPrice
            }).ToList();
        }

        private void BindItems()
        {
            gvItems.DataSource = Items;
            gvItems.DataBind();
            litOrderTotal.Text = Items.Sum(i => i.LineTotal).ToString("C");
        }

        protected void ddlVariant_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPriceFromSelectedVariant();
        }

        private void SetPriceFromSelectedVariant()
        {
            if (ddlVariant.SelectedItem == null) return;
            var variantId = Convert.ToInt32(ddlVariant.SelectedValue);
            var variants = _productService.GetActiveVariantsForOrderEntry();
            var variant = variants.FirstOrDefault(v => v.ProductVariantId == variantId);
            if (variant != null)
            {
                txtItemPrice.Text = variant.Product.RetailPrice.ToString("0.00");
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            if (ddlVariant.Items.Count == 0)
            {
                lblItemError.Text = "There are no active product variants to add. Create a product and variant first.";
                lblItemError.Visible = true;
                BindItems();
                return;
            }

            int quantity;
            decimal price;

            if (!int.TryParse(txtItemQty.Text, out quantity) || quantity < 1)
            {
                lblItemError.Text = "Quantity must be at least 1.";
                lblItemError.Visible = true;
                BindItems();
                return;
            }

            if (!decimal.TryParse(txtItemPrice.Text, out price) || price < 0)
            {
                lblItemError.Text = "Unit price cannot be negative.";
                lblItemError.Visible = true;
                BindItems();
                return;
            }

            var variantId = Convert.ToInt32(ddlVariant.SelectedValue);
            var items = Items;
            items.Add(new OrderItemRow
            {
                ProductVariantId = variantId,
                DisplayName = ddlVariant.SelectedItem.Text,
                QuantityRequested = quantity,
                UnitPrice = price
            });
            Items = items;

            lblItemError.Visible = false;
            txtItemQty.Text = "1";
            BindItems();
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "RemoveItem") return;

            var index = Convert.ToInt32(e.CommandArgument);
            var items = Items;
            if (index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
                Items = items;
            }
            BindItems();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var customerId = ddlCustomer.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlCustomer.SelectedValue);
            var organizationId = ddlOrganization.SelectedValue == "0" ? (int?)null : Convert.ToInt32(ddlOrganization.SelectedValue);

            if ((customerId == null && organizationId == null) || (customerId != null && organizationId != null))
            {
                lblFormError.Text = "Select exactly one: a customer or an organization.";
                lblFormError.Visible = true;
                BindItems();
                return;
            }

            if (Items.Count == 0)
            {
                lblFormError.Text = "Add at least one item to the order.";
                lblFormError.Visible = true;
                BindItems();
                return;
            }

            DateTime? deliveryDate = null;
            DateTime parsedDate;
            if (DateTime.TryParse(txtDeliveryDate.Text, out parsedDate))
            {
                deliveryDate = parsedDate;
            }

            var order = new OrderRequest
            {
                OrderRequestId = Convert.ToInt32(hfOrderId.Value),
                CustomerId = customerId,
                OrganizationId = organizationId,
                RequestedDeliveryDate = deliveryDate,
                Notes = txtNotes.Text.Trim()
            };

            var items = Items.Select(i => new OrderRequestItem
            {
                ProductVariantId = i.ProductVariantId,
                QuantityRequested = i.QuantityRequested,
                UnitPrice = i.UnitPrice
            }).ToList();

            try
            {
                if (order.OrderRequestId == 0)
                {
                    var newId = _orderService.CreateOrder(order, items, CurrentAdminId);
                    Response.Redirect("~/Admin/OrderDetails.aspx?id=" + newId + "&created=1", false);
                }
                else
                {
                    _orderService.UpdateOrderDetails(order, items);
                    Response.Redirect("~/Admin/OrderDetails.aspx?id=" + order.OrderRequestId + "&saved=1", false);
                }
            }
            catch (BusinessRuleException ex)
            {
                lblFormError.Text = ex.Message;
                lblFormError.Visible = true;
                BindItems();
            }
        }
    }
}
