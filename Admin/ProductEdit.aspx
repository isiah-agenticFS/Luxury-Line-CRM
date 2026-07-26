<%@ Page Title="Edit Product" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="ScrubCRM.Admin.ProductEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4"><asp:Literal ID="litHeading" runat="server" /></h1>

    <asp:HiddenField ID="hfProductId" runat="server" Value="0" />

    <div class="card mb-4">
        <div class="card-body">
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger py-2" DisplayMode="BulletList" />
            <asp:Label ID="lblFormError" runat="server" CssClass="alert alert-danger py-2 d-block" Visible="false" />

            <div class="row g-3">
                <div class="col-md-6">
                    <label class="form-label">Product Name</label>
                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtProductName" ErrorMessage="Product name is required." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">SKU</label>
                    <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvSKU" runat="server" ControlToValidate="txtSKU" ErrorMessage="SKU is required." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Brand</label>
                    <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Category</label>
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Style</label>
                    <asp:TextBox ID="txtStyle" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Unit Cost</label>
                    <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                    <asp:CompareValidator ID="cvUnitCost" runat="server" ControlToValidate="txtUnitCost" Type="Currency" Operator="GreaterThanEqual" ValueToCompare="0" ErrorMessage="Unit cost cannot be negative." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Retail Price</label>
                    <asp:TextBox ID="txtRetailPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                    <asp:CompareValidator ID="cvRetail" runat="server" ControlToValidate="txtRetailPrice" Type="Currency" Operator="GreaterThanEqual" ValueToCompare="0" ErrorMessage="Retail price cannot be negative." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Wholesale Price</label>
                    <asp:TextBox ID="txtWholesalePrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                    <asp:CompareValidator ID="cvWholesale" runat="server" ControlToValidate="txtWholesalePrice" Type="Currency" Operator="GreaterThanEqual" ValueToCompare="0" ErrorMessage="Wholesale price cannot be negative." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Reorder Level</label>
                    <asp:TextBox ID="txtReorderLevel" runat="server" CssClass="form-control" TextMode="Number" />
                    <asp:CompareValidator ID="cvReorder" runat="server" ControlToValidate="txtReorderLevel" Type="Integer" Operator="GreaterThanEqual" ValueToCompare="0" ErrorMessage="Reorder level cannot be negative." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3 d-flex align-items-end">
                    <div class="form-check">
                        <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                        <label class="form-check-label">Active</label>
                    </div>
                </div>
            </div>

            <div class="mt-4">
                <asp:Button ID="btnSave" runat="server" Text="Save Product" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="~/Admin/Products.aspx" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlVariants" runat="server" Visible="false">
        <div class="card mb-4">
            <div class="card-header">Product Variants (Size / Color)</div>
            <div class="card-body">
                <asp:Label ID="lblVariantError" runat="server" CssClass="alert alert-danger py-2 d-block" Visible="false" />

                <asp:GridView ID="gvVariants" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" GridLines="None"
                    DataKeyNames="ProductVariantId" OnRowEditing="gvVariants_RowEditing" OnRowCancelingEdit="gvVariants_RowCancelingEdit" OnRowUpdating="gvVariants_RowUpdating">
                    <Columns>
                        <asp:TemplateField HeaderText="Size">
                            <ItemTemplate><%# Eval("Size") %></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtEditSize" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Size") %>' /></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Color">
                            <ItemTemplate><%# Eval("Color") %></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtEditColor" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Color") %>' /></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Variant SKU">
                            <ItemTemplate><%# Eval("VariantSku") %></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtEditSku" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("VariantSku") %>' /></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="QuantityOnHand" HeaderText="Qty On Hand" ReadOnly="true" />
                        <asp:CommandField ShowEditButton="true" ButtonType="Button" ControlStyle-CssClass="btn btn-sm btn-outline-secondary" />
                    </Columns>
                    <EmptyDataTemplate>No variants yet. Add one below.</EmptyDataTemplate>
                </asp:GridView>

                <p class="text-muted small">Quantity on hand is adjusted from the Inventory page so every change is logged.</p>

                <hr />
                <h2 class="h6">Add Variant</h2>
                <div class="row g-2 align-items-end">
                    <div class="col-sm-2">
                        <label class="form-label">Size</label>
                        <asp:DropDownList ID="ddlNewSize" runat="server" CssClass="form-select">
                            <asp:ListItem Text="Small" Value="Small" />
                            <asp:ListItem Text="Medium" Value="Medium" />
                            <asp:ListItem Text="Large" Value="Large" />
                            <asp:ListItem Text="X-Large" Value="X-Large" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-3">
                        <label class="form-label">Color</label>
                        <asp:TextBox ID="txtNewColor" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-sm-3">
                        <label class="form-label">Variant SKU</label>
                        <asp:TextBox ID="txtNewSku" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-sm-2">
                        <label class="form-label">Starting Qty</label>
                        <asp:TextBox ID="txtNewQty" runat="server" CssClass="form-control" TextMode="Number" Text="0" />
                    </div>
                    <div class="col-sm-2">
                        <asp:Button ID="btnAddVariant" runat="server" Text="Add Variant" CssClass="btn btn-secondary w-100" OnClick="btnAddVariant_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
