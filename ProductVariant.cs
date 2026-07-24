using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class ProductVariant
    {
        public int ProductVariantId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Required, StringLength(20)]
        public string Size { get; set; }

        [Required, StringLength(50)]
        public string Color { get; set; }

        [Required, StringLength(60)]
        public string VariantSku { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity on hand cannot be negative.")]
        public int QuantityOnHand { get; set; }

        public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; }
        public virtual ICollection<OrderRequestItem> OrderRequestItems { get; set; }

        public ProductVariant()
        {
            InventoryTransactions = new List<InventoryTransaction>();
            OrderRequestItems = new List<OrderRequestItem>();
        }

        [NotMapped]
        public string DisplayName
        {
            get { return (Product != null ? Product.ProductName : "") + " / " + Color + " / " + Size; }
        }
    }
}
