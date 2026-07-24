<%@ Page Title="Products" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="ScrubCRM.Admin.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1 class="h3 mb-0">Products</h1>
        <asp:HyperLink ID="lnkAddProduct" runat="server" NavigateUrl="~/Admin/ProductEdit.aspx" CssClass="btn btn-primary">Add Product</asp:HyperLink>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-block" Visible="false" />

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-sm-4">
                    <label class="form-label">Search (name or SKU)</label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                </div>
                <div class="col-sm-3">
                    <div class="form-check mt-4">
                        <asp:CheckBox ID="chkIncludeInactive" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label">Include inactive</label>
                    </div>
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body p-0">
            <asp:GridView ID="gvProducts" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None"
                DataKeyNames="ProductId" OnRowCommand="gvProducts_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                    <asp:BoundField DataField="SKU" HeaderText="SKU" />
                    <asp:BoundField DataField="Brand" HeaderText="Brand" />
                    <asp:BoundField DataField="Category" HeaderText="Category" />
                    <asp:BoundField DataField="RetailPrice" HeaderText="Retail" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="WholesalePrice" HeaderText="Wholesale" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge <%# (bool)Eval("IsActive") ? "bg-success" : "bg-secondary" %>'>
                                <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a class="btn btn-sm btn-outline-primary" href='<%# "ProductEdit.aspx?id=" + Eval("ProductId") %>'>Edit</a>
                            <asp:LinkButton ID="btnDeactivate" runat="server" CssClass="btn btn-sm btn-outline-danger"
                                CommandName="Deactivate" CommandArgument='<%# Eval("ProductId") %>'
                                OnClientClick="return confirm('Deactivate this product? It will no longer appear in new orders.');"
                                Visible='<%# (bool)Eval("IsActive") %>'>Deactivate</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No products found.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
