using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoSale
    {
        public int PurchaseId { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }
    }

    public class DtoTangibleSale
    {
        public int SaleId { get; set; }
        public int PersonId { get; set; }
        [Display(Name = "Person Name")]
        public string PersonName { get; set; }

        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public List<DtoTangibleItemSale> ItemSales { get; set; }
    }

    public class DtoTangibleItemSale
    {
        public int PurchaseId { get; set; }

        public int ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        public int Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal SubTotal { get; set; }


    }
}