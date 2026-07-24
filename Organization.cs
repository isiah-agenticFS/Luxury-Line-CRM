using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrubCRM.Models
{
    public class Organization
    {
        public int OrganizationId { get; set; }

        [Required, StringLength(200)]
        public string OrganizationName { get; set; }

        [Required]
        public OrganizationType OrganizationType { get; set; }

        [StringLength(150)]
        public string PrimaryContactName { get; set; }

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

        public Organization()
        {
            CreatedDate = DateTime.UtcNow;
            OrderRequests = new List<OrderRequest>();
        }
    }
}
