using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        [Required]
        [MinLength(3),MaxLength(50)]
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int ItemCode { get; set; }
    }
}