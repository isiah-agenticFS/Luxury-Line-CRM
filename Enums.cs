namespace ScrubCRM.Models
{
    public enum OrganizationType
    {
        University = 0,
        WholesaleClient = 1
    }

    public enum OrderStatus
    {
        Submitted = 0,
        UnderReview = 1,
        Approved = 2,
        AwaitingInventory = 3,
        Ready = 4,
        Completed = 5,
        Cancelled = 6
    }
}
