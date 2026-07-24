<%@ Page Title="Customers" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="ScrubCRM.Admin.Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4">Customers</h1>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-block" Visible="false" />

    <asp:HiddenField ID="hfCustomerId" runat="server" Value="0" />

    <div class="card mb-4">
        <div class="card-header"><asp:Literal ID="litFormHeading" runat="server" Text="Add Customer" /></div>
        <div class="card-body">
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger py-2" DisplayMode="BulletList" />
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">First Name</label>
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvFirst" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Last Name</label>
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvLast" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-6">
                    <label class="form-label">Notes</label>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" Text="Save Customer" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear / New" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click" CausesValidation="false" />
            </div>
        </div>
    </div>

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-sm-4">
                    <label class="form-label">Search (name or email)</label>
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" />
                </div>
                <div class="col-sm-2">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="card">
        <div class="card-body p-0">
            <asp:GridView ID="gvCustomers" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None"
                DataKeyNames="CustomerId" OnRowCommand="gvCustomers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary" CommandName="EditCustomer" CommandArgument='<%# Eval("CustomerId") %>' CausesValidation="false">Edit</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No customers found.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
