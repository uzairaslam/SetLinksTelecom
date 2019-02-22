using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoVoucher
    {
        public int VId { get; set; }
        [Display(Name = "Description")]
        public string VDescription { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public bool WithoutDate { get; set; }
    }

    public class DtoVoucherDisplay
    {
        public int Id { get; set; }
        public string AccName { get; set; }
        public string VDescription { get; set; }
        public string AccString { get; set; }
        public string InvNo { get; set; }
        public string VType { get; set; }
        public string ChequeNo { get; set; }
        public string InvType { get; set; }
        public int VNo { get; set; }
    }
}