using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Portal
    {
        public int PortalId { get; set; }
        [Required]
        [MinLength(3),MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Url { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}