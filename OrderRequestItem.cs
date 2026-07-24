using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class OrderRequestItem
    {
        public int OrderRequestItemId { get; set; }

        [Required]
        public int OrderRequestId { get; set; }

        [ForeignKey("OrderRequestId")]
        public virtual OrderRequest OrderRequest { get; set; }

        [Required]
        public int ProductVariantId { get; set; }

        [ForeignKey("ProductVariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity requested must be at least 1.")]
        public int QuantityRequested { get; set; }

        [Column(TypeName = "decimal")]
        [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
        public decimal UnitPrice { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [NotMapped]
        public decimal LineTotal
        {
            get { return QuantityRequested * UnitPrice; }
        }
    }
}
