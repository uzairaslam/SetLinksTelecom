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

        //[MinLength(3),MaxLength(50)]
        //public string Subname { get; set; }

        [Display(Name = "Sale Rate/Percentage")]
        public decimal SaleRate { get; set; }

        [Required]
        [Display(Name = "ProductCategory")]
        public int ProductCategoryId { get; set; }
        [ForeignKey("ProductCategoryId")]
        [NotMapped]
        public ProductCategory ProductCategory { get; set; }

        [MaxLength(12)]
        public string AccString { get; set; }

        [MaxLength(12)]
        public string RevString { get; set; }

        [MaxLength(12)]
        public string CosString { get; set; }
        [MaxLength(12)]
        public string PurDiscString { get; set; }
        [MaxLength(12)]
        public string SaleCommString { get; set; }

        [NotMapped]
        public List<ProductCategory> ProductCategories { get; set; }

        public Item()
        {
            ProductCategory = new ProductCategory();
            ProductCategories = new List<ProductCategory>();
        }
    }
}