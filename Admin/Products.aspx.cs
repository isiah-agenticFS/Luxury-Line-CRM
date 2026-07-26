using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Products : Page
    {
        protected Label lblMessage;
        protected TextBox txtSearch;
        protected CheckBox chkIncludeInactive;
        protected GridView gvProducts;

        private readonly ProductService _productService = new ProductService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["saved"] != null)
                {
                    lblMessage.Text = "Product saved.";
                    lblMessage.Visible = true;
                }
                BindProducts();
            }
        }

        private void BindProducts()
        {
            gvProducts.DataSource = _productService.GetProducts(txtSearch.Text, chkIncludeInactive.Checked);
            gvProducts.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProducts();
        }

        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Deactivate")
            {
                var productId = Convert.ToInt32(e.CommandArgument);
                _productService.DeactivateProduct(productId);
                lblMessage.Text = "Product deactivated.";
                lblMessage.Visible = true;
                BindProducts();
            }
        }
    }
}
