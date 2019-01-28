using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Purchase
    {
        [Column("PurchaseId",Order = 0)]
        public int PurchaseId { get; set; }

        [Column("PortalId", Order = 1)]
        [Display(Name = "Portal")]
        public int PortalId { get; set; }
        [NotMapped]
        [ForeignKey("PortalId")]
        private Portal Portal { get; set; }

        [Column("ItemId", Order = 2)]
        [Display(Name = "Item")]
        public int ItemId { get; set; }
        [NotMapped]
        [ForeignKey("ItemId")]
        private Item Item { get; set; }

        [Column("Qty", Order = 3)]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [Column("Rate", Order = 4)]
        public decimal Rate { get; set; }

        [Column("Total", Order = 5)]
        public decimal Total { get; set; }

        [Column("Subname", Order = 6)]
        [MinLength(3), MaxLength(50)]
        public string Subname { get; set; }

        [Column("Remarks", Order = 7)]
        [MaxLength(150)]
        public string Remarks { get; set; }

        [Column("DatePurchased", Order = 8)]
        [Display(Name = "Date Purchased")]
        public DateTime DatePurchased { get; set; }

        [Column("StockOut", Order = 9)]
        [Display(Name = "Stock Out")]
        public int StockOut { get; set; }


        public Purchase()
        {
            Item = new Item();
            Portal = new Portal();
        }
    }
}