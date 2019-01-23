using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class InventoryType
    {
        public int InventoryTypeId { get; set; }

        [MaxLength(12)]
        public string Name { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}