using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public int PurchaseId { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal SubTotal { get; set; }
        public int LineId { get; set; }
        public int SaleId { get; set; }
        public decimal CommProfit { get; set; }

        [NotMapped]
        [ForeignKey("SaleId")]
        public Sale Sale { get; set; }
    }
}