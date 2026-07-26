<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ScrubCRM.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4">Dashboard</h1>

    <div class="row g-3 mb-4">
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Total Products</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litTotalProducts" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Total Inventory Units</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litTotalUnits" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Low-Stock Variants</div>
                <div class="fs-3 fw-semibold text-warning"><asp:Literal ID="litLowStock" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Out-of-Stock Variants</div>
                <div class="fs-3 fw-semibold text-danger"><asp:Literal ID="litOutOfStock" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Total Customers</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litTotalCustomers" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Total Organizations</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litTotalOrganizations" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Pending Order Requests</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litPendingOrders" runat="server" /></div>
            </div></div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card"><div class="card-body">
                <div class="text-muted small">Completed Orders</div>
                <div class="fs-3 fw-semibold"><asp:Literal ID="litCompletedOrders" runat="server" /></div>
            </div></div>
        </div>
    </div>

    <div class="card">
        <div class="card-header">Five Most Recent Orders</div>
        <div class="card-body p-0">
            <asp:GridView ID="gvRecentOrders" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="RequestNumber" HeaderText="Request #" />
                    <asp:BoundField DataField="CustomerOrOrganizationName" HeaderText="Customer / Organization" />
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="OrderTotal" HeaderText="Total" DataFormatString="{0:C}" />
                </Columns>
                <EmptyDataTemplate>No orders yet.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
