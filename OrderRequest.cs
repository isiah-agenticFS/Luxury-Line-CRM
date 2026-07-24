using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ScrubCRM.Models
{
    public class OrderRequest
    {
        public int OrderRequestId { get; set; }

        [Required, StringLength(30)]
        public string RequestNumber { get; set; }

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        public int? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime? RequestedDeliveryDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<OrderRequestItem> Items { get; set; }
        public virtual ICollection<OrderStatusHistory> StatusHistory { get; set; }

        public OrderRequest()
        {
            Status = OrderStatus.Submitted;
            RequestDate = DateTime.UtcNow;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            Items = new List<OrderRequestItem>();
            StatusHistory = new List<OrderStatusHistory>();
        }

        [NotMapped]
        public decimal OrderTotal
        {
            get { return Items == null ? 0m : Items.Sum(i => i.LineTotal); }
        }

        [NotMapped]
        public string CustomerOrOrganizationName
        {
            get
            {
                if (Organization != null) return Organization.OrganizationName;
                if (Customer != null) return Customer.FullName;
                return "(unknown)";
            }
        }
    }
}
