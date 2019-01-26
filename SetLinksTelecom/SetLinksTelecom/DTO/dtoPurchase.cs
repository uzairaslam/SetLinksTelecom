using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.DTO
{
    public class dtoPurchase
    {
        public dtoPurchase()
        {
            Portals = new List<Portal>();
            InventoryTypes = new List<InventoryType>();
            ProductCategories = new List<ProductCategory>();
            Items = new List<Item>();
        }

        public int PurchaseId { get; set; }
        [Required]
        [Display(Name = "Portal")]
        public int PortalId { get; set; }
        //[ForeignKey("PortalId")]
        //public Portal Portal { get; set; }
        public List<Portal> Portals { get; set; }

        [Required]
        [Display(Name = "Inventory Type")]
        public int InventoryTypeId { get; set; }
        //[ForeignKey("InventoryTypeId")]
        //public InventoryType InventoryType { get; set; }
        public List<InventoryType> InventoryTypes { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public int ProductCategoryId { get; set; }
        //[ForeignKey("ProductCategoryId")]
        //public ProductCategory ProductCategory { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }

        [Required]
        [Display(Name = "Item")]
        public int ItemId { get; set; }
        //[ForeignKey("ItemId")]
        //public Item Item { get; set; }
        public List<Item> Items { get; set; }


        [MinLength(3), MaxLength(50)]
        public string Subname { get; set; }


        public int Qty { get; set; }
        public decimal PaidAmount { get; set; }

        [MaxLength(150)]
        public string Comments { get; set; }

    }

    public class dtoDisplayPurchase
    {
        public string PortalName { get; set; }
        public string InventoryType { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public int PurchaseId { get; set; }
        public string Subname { get; set; }
        public string Comments { get; set; }
        public int Qty { get; set; }
        public decimal PaidAmount { get; set; }
    }
}