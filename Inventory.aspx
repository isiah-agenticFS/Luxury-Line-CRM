<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="ScrubCRM.Admin.Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4">Inventory</h1>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-block" Visible="false" />
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-sm-4">
                    <label class="form-label">Search (product, SKU)</label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body p-0">
            <asp:GridView ID="gvInventory" runat="server" CssClass="table table-striped mb-0 align-middle" AutoGenerateColumns="false" GridLines="None"
                DataKeyNames="ProductVariantId" OnRowCommand="gvInventory_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Product / SKU">
                        <ItemTemplate>
                            <%# Eval("Product.ProductName") %><br />
                            <span class="text-muted small"><%# Eval("Product.SKU") %> / <%# Eval("VariantSku") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Size" HeaderText="Size" />
                    <asp:BoundField DataField="Color" HeaderText="Color" />
                    <asp:BoundField DataField="QuantityOnHand" HeaderText="Qty On Hand" />
                    <asp:TemplateField HeaderText="Reorder Level">
                        <ItemTemplate><%# Eval("Product.ReorderLevel") %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='badge <%# GetStockStatusClass(Convert.ToInt32(Eval("QuantityOnHand")), Convert.ToInt32(Eval("Product.ReorderLevel"))) %>'>
                                <%# GetStockStatusText(Convert.ToInt32(Eval("QuantityOnHand")), Convert.ToInt32(Eval("Product.ReorderLevel"))) %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Adjust Inventory">
                        <ItemTemplate>
                            <div class="d-flex gap-1">
                                <asp:TextBox ID="txtAdjustQty" runat="server" CssClass="form-control form-control-sm" style="width:80px" placeholder="+/-" />
                                <asp:DropDownList ID="ddlReason" runat="server" CssClass="form-select form-select-sm" style="width:140px">
                                    <asp:ListItem Text="Restock" Value="Restock" />
                                    <asp:ListItem Text="Correction" Value="Correction" />
                                    <asp:ListItem Text="Damaged" Value="Damaged" />
                                    <asp:ListItem Text="Returned" Value="Returned" />
                                    <asp:ListItem Text="Other" Value="Other" />
                                </asp:DropDownList>
                                <asp:LinkButton ID="btnApply" runat="server" CssClass="btn btn-sm btn-outline-primary" CommandName="Adjust" CommandArgument='<%# Eval("ProductVariantId") %>'>Apply</asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No product variants yet. Add variants from the Products page.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
