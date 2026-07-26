<%@ Page Title="Sign In - Luxury Line CRM" Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ScrubCRM.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Sign In - Luxury Line CRM</title>
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700;800&display=swap" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
    <link rel="stylesheet" href="Content/site.css" />
</head>
<body class="login-page">
    <form id="form1" runat="server">
        <div class="d-flex align-items-center justify-content-center" style="min-height:100vh;">
            <div class="card shadow-lg" style="width: 24rem;">
                <div class="card-body">
                    <i class="bi bi-heart-pulse-fill brand-icon-lg"></i>
                    <h1 class="brand-title text-center mb-0">Luxury Line CRM</h1>
                    <p class="brand-sub text-center mb-4">Administrator sign in</p>

                    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger py-2" DisplayMode="BulletList" />
                    <asp:Label ID="lblError" runat="server" CssClass="alert alert-danger py-2 d-block" Visible="false" />

                    <div class="mb-3">
                        <label class="form-label" for="txtEmail"><i class="bi bi-envelope me-1"></i>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" autocomplete="username" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="text-danger small" Display="Dynamic" />
                    </div>
                    <div class="mb-4">
                        <label class="form-label" for="txtPassword"><i class="bi bi-lock me-1"></i>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" autocomplete="current-password" />
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
