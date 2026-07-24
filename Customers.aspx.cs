using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Customers : Page
    {
        protected Label lblMessage;
        protected HiddenField hfCustomerId;
        protected Literal litFormHeading;
        protected TextBox txtFirstName;
        protected TextBox txtLastName;
        protected TextBox txtEmail;
        protected TextBox txtPhone;
        protected TextBox txtAddress;
        protected TextBox txtNotes;
        protected TextBox txtSearch;
        protected GridView gvCustomers;

        private readonly CustomerService _customerService = new CustomerService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCustomers();
            }
        }

        private void BindCustomers()
        {
            gvCustomers.DataSource = _customerService.GetCustomers(txtSearch.Text);
            gvCustomers.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCustomers();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var customer = new Customer
            {
                CustomerId = Convert.ToInt32(hfCustomerId.Value),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Notes = txtNotes.Text.Trim()
            };

            _customerService.SaveCustomer(customer);

            lblMessage.Text = "Customer saved.";
            lblMessage.Visible = true;
            ClearForm();
            BindCustomers();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfCustomerId.Value = "0";
            litFormHeading.Text = "Add Customer";
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtNotes.Text = string.Empty;
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "EditCustomer") return;

            var customerId = Convert.ToInt32(e.CommandArgument);
            var customer = _customerService.GetCustomer(customerId);
            if (customer == null) return;

            hfCustomerId.Value = customer.CustomerId.ToString();
            litFormHeading.Text = "Edit Customer: " + customer.FullName;
            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;
            txtEmail.Text = customer.Email;
            txtPhone.Text = customer.Phone;
            txtAddress.Text = customer.Address;
            txtNotes.Text = customer.Notes;
        }
    }
}
