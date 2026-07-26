<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ScrubCRM.Admin.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4"><i class="bi bi-speedometer2 me-2"></i>Dashboard</h1>

    <div class="row g-3 mb-4">
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-box-seam stat-icon"></i>
                    <div class="stat-label">Total Products</div>
                    <div class="stat-value"><asp:Literal ID="litTotalProducts" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-layers stat-icon"></i>
                    <div class="stat-label">Total Inventory Units</div>
                    <div class="stat-value"><asp:Literal ID="litTotalUnits" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card stat-warning">
                <div class="card-body">
                    <i class="bi bi-exclamation-triangle stat-icon"></i>
                    <div class="stat-label">Low-Stock Variants</div>
                    <div class="stat-value"><asp:Literal ID="litLowStock" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card stat-danger">
                <div class="card-body">
                    <i class="bi bi-x-circle stat-icon"></i>
                    <div class="stat-label">Out-of-Stock Variants</div>
                    <div class="stat-value"><asp:Literal ID="litOutOfStock" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-people stat-icon"></i>
                    <div class="stat-label">Total Customers</div>
                    <div class="stat-value"><asp:Literal ID="litTotalCustomers" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-building stat-icon"></i>
                    <div class="stat-label">Total Organizations</div>
                    <div class="stat-value"><asp:Literal ID="litTotalOrganizations" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-clock stat-icon"></i>
                    <div class="stat-label">Pending Order Requests</div>
                    <div class="stat-value"><asp:Literal ID="litPendingOrders" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card">
                <div class="card-body">
                    <i class="bi bi-check-circle stat-icon"></i>
                    <div class="stat-label">Completed Orders</div>
                    <div class="stat-value"><asp:Literal ID="litCompletedOrders" runat="server" /></div>
                </div>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-header"><i class="bi bi-receipt me-1"></i>Five Most Recent Orders</div>
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
