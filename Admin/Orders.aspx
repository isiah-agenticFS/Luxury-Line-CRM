<%@ Page Title="Orders" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="ScrubCRM.Admin.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h1 class="h3 mb-0">Order Requests</h1>
        <asp:HyperLink ID="lnkAddOrder" runat="server" NavigateUrl="~/Admin/OrderEdit.aspx" CssClass="btn btn-primary">New Order Request</asp:HyperLink>
    </div>

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-sm-4">
                    <label class="form-label">Search (request #, customer, organization)</label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                </div>
                <div class="col-sm-3">
                    <label class="form-label">Status</label>
                    <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-select">
                        <asp:ListItem Text="All Statuses" Value="" />
                        <asp:ListItem Text="Submitted" Value="Submitted" />
                        <asp:ListItem Text="Under Review" Value="UnderReview" />
                        <asp:ListItem Text="Approved" Value="Approved" />
                        <asp:ListItem Text="Awaiting Inventory" Value="AwaitingInventory" />
                        <asp:ListItem Text="Ready" Value="Ready" />
                        <asp:ListItem Text="Completed" Value="Completed" />
                        <asp:ListItem Text="Cancelled" Value="Cancelled" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body p-0">
            <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None" DataKeyNames="OrderRequestId">
                <Columns>
                    <asp:BoundField DataField="RequestNumber" HeaderText="Request #" />
                    <asp:BoundField DataField="CustomerOrOrganizationName" HeaderText="Customer / Organization" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="RequestedDeliveryDate" HeaderText="Delivery Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="OrderTotal" HeaderText="Total" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <a class="btn btn-sm btn-outline-primary" href='<%# "OrderDetails.aspx?id=" + Eval("OrderRequestId") %>'>View</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No order requests found.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
