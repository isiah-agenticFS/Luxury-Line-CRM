using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Organizations : Page
    {
        protected Label lblMessage;
        protected HiddenField hfOrganizationId;
        protected Literal litFormHeading;
        protected TextBox txtOrgName;
        protected DropDownList ddlOrgType;
        protected TextBox txtContactName;
        protected TextBox txtEmail;
        protected TextBox txtPhone;
        protected TextBox txtAddress;
        protected TextBox txtNotes;
        protected TextBox txtSearch;
        protected GridView gvOrganizations;

        private readonly OrganizationService _organizationService = new OrganizationService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrganizations();
            }
        }

        private void BindOrganizations()
        {
            gvOrganizations.DataSource = _organizationService.GetOrganizations(txtSearch.Text);
            gvOrganizations.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindOrganizations();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var organization = new Organization
            {
                OrganizationId = Convert.ToInt32(hfOrganizationId.Value),
                OrganizationName = txtOrgName.Text.Trim(),
                OrganizationType = (OrganizationType)Enum.Parse(typeof(OrganizationType), ddlOrgType.SelectedValue),
                PrimaryContactName = txtContactName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };

            _organizationService.SaveOrganization(organization);

            lblMessage.Text = "Organization saved.";
            lblMessage.Visible = true;
            ClearForm();
            BindOrganizations();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfOrganizationId.Value = "0";
            litFormHeading.Text = "Add Organization";
            txtOrgName.Text = string.Empty;
            ddlOrgType.SelectedIndex = 0;
            txtContactName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }

        protected void gvOrganizations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditOrg") return;

            var organizationId = Convert.ToInt32(e.CommandArgument);
            var organization = _organizationService.GetOrganization(organizationId);
            if (organization == null) return;

            hfOrganizationId.Value = organization.OrganizationId.ToString();
            litFormHeading.Text = "Edit Organization: " + organization.OrganizationName;
            txtOrgName.Text = organization.OrganizationName;
            ddlOrgType.SelectedValue = organization.OrganizationType.ToString();
            txtContactName.Text = organization.PrimaryContactName;
            txtEmail.Text = organization.Email;
            txtPhone.Text = organization.Phone;
            txtAddress.Text = organization.Address;
            txtNotes.Text = organization.Notes;
        }
    }
}
