using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScrubCRM.Models;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Orders : Page
    {
        protected TextBox txtSearch;
        protected DropDownList ddlStatusFilter;
        protected GridView gvOrders;

        private readonly OrderService _orderService = new OrderService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindOrders();
            }
        }

        private void BindOrders()
        {
            OrderStatus? status = null;
            if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
            {
                status = (OrderStatus)Enum.Parse(typeof(OrderStatus), ddlStatusFilter.SelectedValue);
            }

            gvOrders.DataSource = _orderService.GetOrders(txtSearch.Text, status);
            gvOrders.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindOrders();
        }
    }
}
