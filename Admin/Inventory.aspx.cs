using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Inventory : Page
    {
        protected Label lblMessage;
        protected Label lblError;
        protected TextBox txtSearch;
        protected GridView gvInventory;

        private readonly InventoryService _inventoryService = new InventoryService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindInventory();
            }
        }

        private void BindInventory()
        {
            gvInventory.DataSource = _inventoryService.GetVariantsWithStock(txtSearch.Text);
            gvInventory.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindInventory();
        }

        protected void gvInventory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Adjust") return;

            var variantId = Convert.ToInt32(e.CommandArgument);
            var row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            var txtAdjustQty = (TextBox)row.FindControl("txtAdjustQty");
            var ddlReason = (DropDownList)row.FindControl("ddlReason");

            int delta;
            if (!int.TryParse(txtAdjustQty.Text, out delta) || delta == 0)
            {
                lblError.Text = "Enter a non-zero whole number to adjust (use a negative number to decrease).";
                lblError.Visible = true;
                BindInventory();
                return;
            }

            try
            {
                _inventoryService.AdjustInventory(variantId, delta, ddlReason.SelectedValue, User.Identity.GetUserId());
                lblMessage.Text = "Inventory updated.";
                lblMessage.Visible = true;
            }
            catch (BusinessRuleException ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
            }

            BindInventory();
        }

        protected string GetStockStatusText(int quantityOnHand, int reorderLevel)
        {
            if (quantityOnHand <= 0) return "Out of Stock";
            if (quantityOnHand <= reorderLevel) return "Low Stock";
            return "In Stock";
        }

        protected string GetStockStatusClass(int quantityOnHand, int reorderLevel)
        {
            if (quantityOnHand <= 0) return "bg-danger";
            if (quantityOnHand <= reorderLevel) return "bg-warning text-dark";
            return "bg-success";
        }
    }
}
