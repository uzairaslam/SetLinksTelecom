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

        [Display(Name = "Sub Name")]
        [MinLength(3), MaxLength(50)]
        public string Subname { get; set; }

        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal Total { get; set; }

        public decimal Percentage { get; set; }

        [MaxLength(150)]
        public string Remarks { get; set; }

        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePurchased { get; set; }

        public int StockOut { get; set; }
    }

    public class dtoDisplayPurchase
    {
        public string PortalName { get; set; }
        public string InventoryType { get; set; }
        public string CategoryName { get; set; }
        public string ItemName { get; set; }
        public int PurchaseId { get; set; }
        public string Subname { get; set; }
        public string Remarks { get; set; }
        public decimal Rate { get; set; }
        public int Qty { get; set; }
        public decimal Total { get; set; }
        public decimal Percentage { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DatePurchased { get; set; }
        public string DatePurchasedFormatted { get { return this.DatePurchased.ToString("dd/MM/yyyy"); } }
    }
}