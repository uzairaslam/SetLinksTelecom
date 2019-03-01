using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoLedger
    {
        public int AccAccountId { get; set; }
        [Display( Name = "Description")]
        public string AccDesc { get; set; }

        public string AccString { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool WithoutDate { get; set; }
    }

    //public class DtoLedgerView
    //{
    //    public DateTime VoucherDate { get; set; }
    //    public string Ref { get; set; }
    //    public string Description { get; set; }
    //    public decimal Debit { get; set; }
    //    public decimal Credit { get; set; }
    //    public decimal Balance { get; set; }
    //}
}