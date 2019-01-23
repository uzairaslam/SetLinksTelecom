using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        //public string ItemType { get; set; }
        public int ItemCode { get; set; }

        [MinLength(3),MaxLength(50)]
        public string Subname { get; set; }
        
        [Required]
        [Display(Name = "ProductCategory")]
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }

        [NotMapped]
        public List<ProductCategory> ProductCategories { get; set; }

        public Item()
        {
            ProductCategory = new ProductCategory();
            ProductCategories = new List<ProductCategory>();
        }
    }
}