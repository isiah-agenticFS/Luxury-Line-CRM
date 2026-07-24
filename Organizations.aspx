<%@ Page Title="Organizations" Language="C#" MasterPageFile="~/Admin/Site.Master" AutoEventWireup="true" CodeBehind="Organizations.aspx.cs" Inherits="ScrubCRM.Admin.Organizations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="h3 mb-4">Wholesale Clients &amp; Universities</h1>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-success d-block" Visible="false" />

    <asp:HiddenField ID="hfOrganizationId" runat="server" Value="0" />

    <div class="card mb-4">
        <div class="card-header"><asp:Literal ID="litFormHeading" runat="server" Text="Add Organization" /></div>
        <div class="card-body">
            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger py-2" DisplayMode="BulletList" />
            <div class="row g-3">
                <div class="col-md-5">
                    <label class="form-label">Organization Name</label>
                    <asp:TextBox ID="txtOrgName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtOrgName" ErrorMessage="Organization name is required." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-3">
                    <label class="form-label">Organization Type</label>
                    <asp:DropDownList ID="ddlOrgType" runat="server" CssClass="form-select">
                        <asp:ListItem Text="University" Value="University" />
                        <asp:ListItem Text="Wholesale Client" Value="WholesaleClient" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label class="form-label">Primary Contact Name</label>
                    <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Enter a valid email address." CssClass="text-danger small" Display="Dynamic" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Phone</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                </div>
                <div class="col-md-12">
                    <label class="form-label">Notes</label>
                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" />
                </div>
            </div>
            <div class="mt-3">
                <asp:Button ID="btnSave" runat="server" Text="Save Organization" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                <asp:Button ID="btnClear" runat="server" Text="Clear / New" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click" CausesValidation="false" />
            </div>
        </div>
    </div>

    <div class="card mb-3">
        <div class="card-body">
            <div class="row g-2 align-items-end">
                <div class="col-sm-4">
                    <label class="form-label">Search (organization name)</label>
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
            <asp:GridView ID="gvOrganizations" runat="server" CssClass="table table-striped mb-0" AutoGenerateColumns="false" GridLines="None"
                DataKeyNames="OrganizationId" OnRowCommand="gvOrganizations_RowCommand">
                <Columns>
                    <asp:BoundField DataField="OrganizationName" HeaderText="Organization" />
                    <asp:BoundField DataField="OrganizationType" HeaderText="Type" />
                    <asp:BoundField DataField="PrimaryContactName" HeaderText="Contact" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Phone" HeaderText="Phone" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary" CommandName="EditOrg" CommandArgument='<%# Eval("OrganizationId") %>' CausesValidation="false">Edit</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No organizations found.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
