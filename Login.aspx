<%@ Page Title="Sign In - Scrub CRM" Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ScrubCRM.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sign In - Scrub CRM</title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="d-flex align-items-center justify-content-center" style="min-height:100vh;">
            <div class="card shadow-sm" style="width: 22rem;">
                <div class="card-body p-4">
                    <h1 class="h4 mb-1 text-center">Scrub CRM</h1>
                    <p class="text-muted text-center mb-4">Administrator sign in</p>

                    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger py-2" DisplayMode="BulletList" />
                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block" Visible="false" />

                    <div class="mb-3">
                        <label class="form-label" for="txtEmail">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" autocomplete="username" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label" for="txtPassword">Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="current-password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
