using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.DTO
{
    public class DtoJvEntry
    {
        public DtoJvEntry()
        {
            Accounts = new List<AccAccount>();
        }
        [Display(Name = "Transaction Id")]
        public int TransactionId { get; set; }

        //[Display(Name = "Date Purchased")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Voucher Type")]
        public string VoucherType { get; set; }


        public List<AccAccount> Accounts { get; set; }

        public string AccString { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal AccBalance { get; set; }

        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal Difference { get; set; }

        //[Display(Name = "Description")]
        public string Remarks { get; set; }
    }
}