using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class SalePurchaseStockOutMap
    {
        public int SalePurchaseStockOutMapId { get; set; }
        public int PurchaseId { get; set; }
        public int SaleId { get; set; }
        public decimal Amount { get; set; }
    }
}