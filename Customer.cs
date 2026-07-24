using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScrubCRM.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; }

        [Required, StringLength(100)]
        public string LastName { get; set; }

        [StringLength(256), EmailAddress]
        public string Email { get; set; }

        [StringLength(30)]
        public string Phone { get; set; }

        [StringLength(300)]
        public string Address { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual ICollection<OrderRequest> OrderRequests { get; set; }

        public Customer()
        {
            CreatedDate = DateTime.UtcNow;
            OrderRequests = new List<OrderRequest>();
        }

        [NotMapped]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
    }
}
