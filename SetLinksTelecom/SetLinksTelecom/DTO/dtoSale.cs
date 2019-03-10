using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

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
        [Required]
        public int PersonId { get; set; }
        [Required]
        [Display(Name = "Person Name")]
        public string PersonName { get; set; }

        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Overall Total")]
        public decimal OverAllTotal { get; set; }
        [MaxLength(150)]
        public string Remarks { get; set; }

        public List<DtoTangibleItemSale> ItemSales { get; set; }
    }

    public class DtoTangibleItemSale
    {
        public int PurchaseId { get; set; }

        public int ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        public decimal Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal SubTotal { get; set; }


    }

    public class DtoInTangibleSale
    {
        public DtoInTangibleSale()
        {
            ItemSales=new List<DtoInTangibleItemSale>();
        }
        public int SaleId { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        [Display(Name = "Person Name")]
        public string PersonName { get; set; }

        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Overall Total")]
        public decimal OverAllTotal { get; set; }
        [MaxLength(150)]
        public string Remarks { get; set; }

        public List<DtoInTangibleItemSale> ItemSales { get; set; }
    }


    public class DtoInTangibleItemSale
    {
        public DtoInTangibleItemSale()
        {
            Lines = new List<DtoLinesWithNumbers>();
        }
        public int PurchaseId { get; set; }

        public int ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        public decimal Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal SubTotal { get; set; }

        public int LineId { get; set; }
        //public List<Line> Lines { get; set; }
        public List<DtoLinesWithNumbers> Lines { get; set; }
    }


    public class DtoSaleReturnView
    {
        public int PurchaseId { get; set; }
        public int SaleDetailId { get; set; }
        public int PortalId { get; set; }
        public string PortalName { get; set; }
        public string InventoryType { get; set; }
        public string CategoryName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        //public decimal Qty { get; set; }
        public DateTime DatePurchased { get; set; }
        public DateTime DateSold { get; set; }
    }

    public class DtoTangibleSaleDetailItem
    {
        public int SaleDetailId { get; set; }

        public int ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemName { get; set; }

        public decimal Qty { get; set; }

        public decimal Rate { get; set; }

        public decimal SubTotal { get; set; }
    }

}