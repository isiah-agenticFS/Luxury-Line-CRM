using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, StringLength(200)]
        public string ProductName { get; set; }

        [Required, StringLength(50)]
        public string SKU { get; set; }

        [StringLength(100)]
        public string Brand { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(100)]
        public string Style { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal")]
        public decimal UnitCost { get; set; }

        [Column(TypeName = "decimal")]
        public decimal RetailPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal WholesalePrice { get; set; }

        public int ReorderLevel { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public virtual ICollection<ProductVariant> Variants { get; set; }

        public Product()
        {
            IsActive = true;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            Variants = new List<ProductVariant>();
        }
    }
}
