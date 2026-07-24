<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="OrderDetails.aspx.cs" Inherits="ScrubCRM.Admin.OrderDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="h3 mb-0">Order <asp:Literal ID="litRequestNumber" runat="server" /></h1>
        <asp:HyperLink ID="lnkEdit" runat="server" CssClass="btn btn-outline-primary">Edit Order</asp:HyperLink>
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-block" Visible="false" />
    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger d-block" Visible="false" />

    <div class="row g-3 mb-4">
        <div class="col-md-8">
            <div class="card h-100">
                <div class="card-body">
                    <dl class="row mb-0">
                        <dt class="col-sm-4">Customer / Organization</dt>
                        <dd class="col-sm-8"><asp:Literal ID="litCustomerOrg" runat="server" /></dd>
                        <dt class="col-sm-4">Request Date</dt>
                        <dd class="col-sm-8"><asp:Literal ID="litRequestDate" runat="server" /></dd>
                        <dt class="col-sm-4">Requested Delivery Date</dt>
                        <dd class="col-sm-8"><asp:Literal ID="litDeliveryDate" runat="server" /></dd>
                        <dt class="col-sm-4">Notes</dt>
                        <dd class="col-sm-8"><asp:Literal ID="litNotes" runat="server" /></dd>
                        <dt class="col-sm-4">Created / Updated</dt>
                        <dd class="col-sm-8"><asp:Literal ID="litCreatedUpdated" runat="server" /></dd>
                    </dl>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card h-100">
                <div class="card-body">
                    <div class="text-muted small">Current Status</div>
                    <div class="mb-3"><span class="badge bg-primary fs-6"><asp:Literal ID="litStatus" runat="server" /></span></div>

                    <label class="form-label">Change Status</label>
                    <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-select mb-2">
                        <asp:ListItem Text="Submitted" Value="Submitted" />
                        <asp:ListItem Text="Under Review" Value="UnderReview" />
                        <asp:ListItem Text="Approved" Value="Approved" />
                        <asp:ListItem Text="Awaiting Inventory" Value="AwaitingInventory" />
                        <asp:ListItem Text="Ready" Value="Ready" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                        <asp:ListItem Text="Cancelled" Value="Cancelled" />
                    </asp:DropDownList>
                    <asp:Button ID="btnUpdateStatus" runat="server" Text="Update Status" CssClass="btn btn-primary w-100" OnClick="btnUpdateStatus_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="card mb-4">
        <div class="card-header">Items</div>
        <div class="card-body p-0">
            <asp:GridView ID="gvItems" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="ProductVariant.DisplayName" HeaderText="Product / Size / Color" />
                    <asp:BoundField DataField="ProductVariant.VariantSku" HeaderText="Variant SKU" />
                    <asp:BoundField DataField="QuantityRequested" HeaderText="Qty" />
                    <asp:BoundField DataField="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="LineTotal" HeaderText="Line Total" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <div class="text-end p-3 fw-semibold">Order Total: <asp:Literal ID="litOrderTotal" runat="server" /></div>
        </div>
    </div>

    <div class="card">
        <div class="card-header">Status History</div>
        <div class="card-body p-0">
            <asp:GridView ID="gvHistory" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="DateChanged" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy HH:mm}" />
                    <asp:BoundField DataField="OldStatus" HeaderText="Old Status" />
                    <asp:BoundField DataField="NewStatus" HeaderText="New Status" />
                    <asp:TemplateField HeaderText="Administrator">
                        <ItemTemplate><%# Eval("Administrator.Email") %></ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
