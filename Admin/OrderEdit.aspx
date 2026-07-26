<%@ Page Title="Order Request" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs" Inherits="ScrubCRM.Admin.OrderEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4"><asp:Literal ID="litHeading" runat="server" Text="New Order Request" /></h1>

    <asp:HiddenField ID="hfOrderId" runat="server" Value="0" />

    <asp:Label ID="lblFormError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="card mb-4">
        <div class="card-body">
            <div class="row g-3">
                <div class="col-md-4">
                    <label class="form-label">Customer</label>
                    <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Organization</label>
                    <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-select" />
                </div>
                <div class="col-md-4">
                    <p class="form-text mt-4">Select a customer OR an organization, not both.</p>
                </div>
                <div class="col-md-4">
                    <label class="form-label">Requested Delivery Date</label>
                    <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-8">
                    <label class="form-label">Notes</label>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" />
                </div>
            </div>
        </div>
    </div>

    <div class="card mb-4">
        <div class="card-header">Order Items</div>
        <div class="card-body">
            <asp:Label ID="lblItemError" runat="server" CssClass="alert alert-danger py-2 d-block" Visible="false" />

            <asp:GridView ID="gvItems" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvItems_RowCommand">
                <Columns>
                    <asp:BoundField DataField="DisplayName" HeaderText="Product / Size / Color" />
                    <asp:BoundField DataField="QuantityRequested" HeaderText="Qty" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="LineTotal" HeaderText="Line Total" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnRemove" runat="server" CssClass="btn btn-sm btn-outline-danger" CommandName="RemoveItem" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false">Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No items added yet.</EmptyDataTemplate>
            </asp:GridView>

            <p class="text-end fw-semibold">Order Total: <asp:Literal ID="litOrderTotal" runat="server" Text="$0.00" /></p>

            <hr />
            <h2 class="h6">Add Item</h2>
            <div class="row g-2 align-items-end">
                <div class="col-sm-5">
                    <label class="form-label">Product Variant</label>
                    <asp:DropDownList ID="ddlVariant" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlVariant_SelectedIndexChanged" />
                </div>
                <div class="col-sm-2">
                    <label class="form-label">Quantity</label>
                    <asp:TextBox ID="txtItemQty" runat="server" CssClass="form-control" TextMode="Number" Text="1" />
                </div>
                <div class="col-sm-2">
                    <label class="form-label">Unit Price</label>
                    <asp:TextBox ID="txtItemPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01" />
                </div>
                <div class="col-sm-3">
                    <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="btn btn-secondary w-100" OnClick="btnAddItem_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:Button ID="btnSave" runat="server" Text="Save Order Request" CssClass="btn btn-primary" OnClick="btnSave_Click" CausesValidation="false" />
    <asp:HyperLink ID="lnkCancel" runat="server" NavigateUrl="~/Admin/Orders.aspx" CssClass="btn btn-outline-secondary">Cancel</asp:HyperLink>
</asp:Content>
