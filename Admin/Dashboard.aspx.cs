using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using ScrubCRM.Services;

namespace ScrubCRM.Admin
{
    public partial class Dashboard : Page
    {
        protected Literal litTotalProducts;
        protected Literal litTotalUnits;
        protected Literal litLowStock;
        protected Literal litOutOfStock;
        protected Literal litTotalCustomers;
        protected Literal litTotalOrganizations;
        protected Literal litPendingOrders;
        protected Literal litCompletedOrders;
        protected GridView gvRecentOrders;

        private readonly OrderService _orderService = new OrderService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDashboard();
            }
        }

        private void BindDashboard()
        {
            var summary = _orderService.GetDashboardSummary();

            litTotalProducts.Text = summary.TotalProducts.ToString();
            litTotalUnits.Text = summary.TotalInventoryUnits.ToString();
            litLowStock.Text = summary.LowStockVariants.ToString();
            litOutOfStock.Text = summary.OutOfStockVariants.ToString();
            litTotalCustomers.Text = summary.TotalCustomers.ToString();
            litTotalOrganizations.Text = summary.TotalOrganizations.ToString();
            litPendingOrders.Text = summary.PendingOrderRequests.ToString();
            litCompletedOrders.Text = summary.CompletedOrders.ToString();

            gvRecentOrders.DataSource = _orderService.GetRecentOrders(5);
            gvRecentOrders.DataBind();
        }
    }
}
