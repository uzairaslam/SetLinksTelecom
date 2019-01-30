using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Stock
    {
        public int StockId { get; set; }

        [Display(Name = "Item")]
        public int ItemId { get; set; }
        [NotMapped]
        [ForeignKey("ItemId")]
        private Item Item { get; set; }

        [Display(Name = "Net Quantity")]
        public int NetQty { get; set; }

        [Display(Name = "Average Rate")]
        public decimal AvgRate { get; set; }

        [Display(Name = "Purchase")]
        public int PurchaseId { get; set; }
        [NotMapped]
        [ForeignKey("PurchaseId")]
        private Purchase Purchase { get; set; }
    }
}