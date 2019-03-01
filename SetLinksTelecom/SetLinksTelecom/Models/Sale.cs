using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int PersonId { get; set; }
        [Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public decimal OverAllTotal { get; set; }
        [MaxLength(150)]
        public string Remarks { get; set; }
        public virtual List<SaleDetail> SaleDetails { get; set; }
    }
}