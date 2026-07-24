using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class OrderStatusHistory
    {
        public int OrderStatusHistoryId { get; set; }

        [Required]
        public int OrderRequestId { get; set; }

        [ForeignKey("OrderRequestId")]
        public virtual OrderRequest OrderRequest { get; set; }

        public OrderStatus? OldStatus { get; set; }

        [Required]
        public OrderStatus NewStatus { get; set; }

        public DateTime DateChanged { get; set; }

        [Required, StringLength(128)]
        public string AdministratorId { get; set; }

        [ForeignKey("AdministratorId")]
        public virtual ApplicationUser Administrator { get; set; }

        public OrderStatusHistory()
        {
            DateChanged = DateTime.UtcNow;
        }
    }
}
