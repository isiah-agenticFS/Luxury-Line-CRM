using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class OrderDetails : Page
    {
        protected Literal litRequestNumber;
        protected HyperLink lnkEdit;
        protected Label lblMessage;
        protected Label lblError;
        protected Literal litCustomerOrg;
        protected Literal litRequestDate;
        protected Literal litDeliveryDate;
        protected Literal litNotes;
        protected Literal litCreatedUpdated;
        protected Literal litStatus;
        protected DropDownList ddlNewStatus;
        protected GridView gvItems;
        protected Literal litOrderTotal;
        protected GridView gvHistory;

        private readonly OrderService _orderService = new OrderService();

        private int OrderId
        {
            get { return Convert.ToInt32(Request.QueryString["id"]); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["created"] != null)
                {
                    lblMessage.Text = "Order request created.";
                    lblMessage.Visible = true;
                }
                else if (Request.QueryString["saved"] != null)
                {
                    lblMessage.Text = "Order request updated.";
                    lblMessage.Visible = true;
                }

                BindOrder();
            }
        }

        private void BindOrder()
        {
            var order = _orderService.GetOrder(OrderId);
            if (order == null)
            {
                Response.Redirect("~/Admin/Orders.aspx", false);
                return;
            }

            litRequestNumber.Text = order.RequestNumber;
            litCustomerOrg.Text = Server.HtmlEncode(order.CustomerOrOrganizationName);
            litRequestDate.Text = order.RequestDate.ToString("MM/dd/yyyy");
            litDeliveryDate.Text = order.RequestedDeliveryDate.HasValue ? order.RequestedDeliveryDate.Value.ToString("MM/dd/yyyy") : "(none)";
            litNotes.Text = string.IsNullOrEmpty(order.Notes) ? "(none)" : Server.HtmlEncode(order.Notes);
            litCreatedUpdated.Text = order.CreatedDate.ToString("MM/dd/yyyy") + " / " + order.UpdatedDate.ToString("MM/dd/yyyy");
            litStatus.Text = order.Status.ToString();
            litOrderTotal.Text = order.OrderTotal.ToString("C");

            ddlNewStatus.SelectedValue = order.Status.ToString();

            lnkEdit.NavigateUrl = "~/Admin/OrderEdit.aspx?id=" + order.OrderRequestId;
            lnkEdit.Visible = order.Status != OrderStatus.Completed;

            gvItems.DataSource = order.Items;
            gvItems.DataBind();

            gvHistory.DataSource = order.StatusHistory;
            gvHistory.DataBind();
        }

        protected void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            var newStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), ddlNewStatus.SelectedValue);

            try
            {
                _orderService.ChangeStatus(OrderId, newStatus, User.Identity.GetUserId());
                lblMessage.Text = "Status updated to " + newStatus + ".";
                lblMessage.Visible = true;
                lblError.Visible = false;
            }
            catch (BusinessRuleException ex)
            {
                lblError.Text = ex.Message;
                lblError.Visible = true;
                lblMessage.Visible = false;
            }

            BindOrder();
        }
    }
}
