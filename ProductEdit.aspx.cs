using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class ProductEdit : Page
    {
        protected Literal litHeading;
        protected HiddenField hfProductId;
        protected Label lblFormError;
        protected TextBox txtProductName;
        protected TextBox txtSKU;
        protected TextBox txtBrand;
        protected TextBox txtCategory;
        protected TextBox txtStyle;
        protected TextBox txtDescription;
        protected TextBox txtUnitCost;
        protected TextBox txtRetailPrice;
        protected TextBox txtWholesalePrice;
        protected TextBox txtReorderLevel;
        protected CheckBox chkIsActive;
        protected Panel pnlVariants;
        protected Label lblVariantError;
        protected GridView gvVariants;
        protected DropDownList ddlNewSize;
        protected TextBox txtNewColor;
        protected TextBox txtNewSku;
        protected TextBox txtNewQty;

        private readonly ProductService _productService = new ProductService();
        private readonly InventoryService _inventoryService = new InventoryService();

        private int ProductId
        {
            get { return Convert.ToInt32(hfProductId.Value); }
        }

        private string CurrentAdminId
        {
            get { return User.Identity.GetUserId(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var idParam = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(idParam))
                {
                    LoadProduct(Convert.ToInt32(idParam));
                }
                else
                {
                    litHeading.Text = "Add Product";
                    pnlVariants.Visible = false;
                }
            }
        }

        private void LoadProduct(int productId)
        {
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                Response.Redirect("~/Admin/Products.aspx", false);
                return;
            }

            hfProductId.Value = product.ProductId.ToString();
            litHeading.Text = "Edit Product: " + product.ProductName;

            txtProductName.Text = product.ProductName;
            txtSKU.Text = product.SKU;
            txtBrand.Text = product.Brand;
            txtCategory.Text = product.Category;
            txtStyle.Text = product.Style;
            txtDescription.Text = product.Description;
            txtUnitCost.Text = product.UnitCost.ToString("0.00");
            txtRetailPrice.Text = product.RetailPrice.ToString("0.00");
            txtWholesalePrice.Text = product.WholesalePrice.ToString("0.00");
            txtReorderLevel.Text = product.ReorderLevel.ToString();
            chkIsActive.Checked = product.IsActive;

            pnlVariants.Visible = true;
            BindVariants(product);
        }

        private void BindVariants(Product product)
        {
            gvVariants.DataSource = product.Variants;
            gvVariants.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var product = new Product
                {
                    ProductId = ProductId,
                    ProductName = txtProductName.Text.Trim(),
                    SKU = txtSKU.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Style = txtStyle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    UnitCost = Convert.ToDecimal(txtUnitCost.Text),
                    RetailPrice = Convert.ToDecimal(txtRetailPrice.Text),
                    WholesalePrice = Convert.ToDecimal(txtWholesalePrice.Text),
                    ReorderLevel = Convert.ToInt32(txtReorderLevel.Text),
                    IsActive = chkIsActive.Checked
                };

                var savedId = _productService.SaveProduct(product);

                if (ProductId == 0)
                {
                    // Redirect into edit mode for the new product so variants can be added.
                    Response.Redirect("~/Admin/ProductEdit.aspx?id=" + savedId, false);
                }
                else
                {
                    Response.Redirect("~/Admin/Products.aspx?saved=1", false);
                }
            }
            catch (Exception ex)
            {
                lblFormError.Text = "Could not save product: " + ex.Message;
                lblFormError.Visible = true;
            }
        }

        protected void gvVariants_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvVariants.EditIndex = e.NewEditIndex;
            BindVariants(_productService.GetProduct(ProductId));
        }

        protected void gvVariants_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvVariants.EditIndex = -1;
            BindVariants(_productService.GetProduct(ProductId));
        }

        protected void gvVariants_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var variantId = Convert.ToInt32(gvVariants.DataKeys[e.RowIndex].Value);
            var row = gvVariants.Rows[e.RowIndex];

            var size = ((TextBox)row.FindControl("txtEditSize")).Text.Trim();
            var color = ((TextBox)row.FindControl("txtEditColor")).Text.Trim();
            var sku = ((TextBox)row.FindControl("txtEditSku")).Text.Trim();

            if (string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color) || string.IsNullOrEmpty(sku))
            {
                lblVariantError.Text = "Size, color, and variant SKU are required.";
                lblVariantError.Visible = true;
                e.Cancel = true;
                return;
            }

            _productService.SaveVariant(new Models.ProductVariant
            {
                ProductVariantId = variantId,
                Size = size,
                Color = color,
                VariantSku = sku
            });

            gvVariants.EditIndex = -1;
            BindVariants(_productService.GetProduct(ProductId));
        }

        protected void btnAddVariant_Click(object sender, EventArgs e)
        {
            var color = txtNewColor.Text.Trim();
            var sku = txtNewSku.Text.Trim();
            int startingQty;

            if (string.IsNullOrEmpty(color) || string.IsNullOrEmpty(sku))
            {
                lblVariantError.Text = "Color and variant SKU are required to add a variant.";
                lblVariantError.Visible = true;
                BindVariants(_productService.GetProduct(ProductId));
                return;
            }

            if (!int.TryParse(txtNewQty.Text, out startingQty) || startingQty < 0)
            {
                lblVariantError.Text = "Starting quantity must be zero or greater.";
                lblVariantError.Visible = true;
                BindVariants(_productService.GetProduct(ProductId));
                return;
            }

            var variant = new Models.ProductVariant
            {
                ProductId = ProductId,
                Size = ddlNewSize.SelectedValue,
                Color = color,
                VariantSku = sku,
                QuantityOnHand = 0
            };

            try
            {
                _productService.SaveVariant(variant);

                if (startingQty > 0)
                {
                    _inventoryService.AdjustInventory(variant.ProductVariantId, startingQty, "Initial stock", CurrentAdminId);
                }

                txtNewColor.Text = string.Empty;
                txtNewSku.Text = string.Empty;
                txtNewQty.Text = "0";
            }
            catch (BusinessRuleException ex)
            {
                lblVariantError.Text = ex.Message;
                lblVariantError.Visible = true;
            }

            BindVariants(_productService.GetProduct(ProductId));
        }
    }
}
