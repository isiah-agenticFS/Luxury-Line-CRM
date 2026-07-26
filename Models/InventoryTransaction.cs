using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class InventoryTransaction
    {
        public int InventoryTransactionId { get; set; }

        [Required]
        public int ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        public int QuantityChange { get; set; }

        [Required, StringLength(200)]
        public string Reason { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required, StringLength(128)]
        public string AdministratorId { get; set; }

        [ForeignKey("AdministratorId")]
        public virtual ApplicationUser Administrator { get; set; }

        public InventoryTransaction()
        {
            TransactionDate = DateTime.UtcNow;
        }
    }
}
