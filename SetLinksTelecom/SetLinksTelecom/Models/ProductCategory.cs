using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        [Required]
        [MinLength(3),MaxLength(30)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Inventory Type")]
        public int InventoryTypeId { get; set; }
        [ForeignKey("InventoryTypeId")]
        [NotMapped]
        public virtual InventoryType InventoryType { get; set; }
        [NotMapped]
        public List<InventoryType> InventoryTypes { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public ProductCategory()
        {
            InventoryType= new InventoryType();
            InventoryTypes = new List<InventoryType>();
        }
    }
}