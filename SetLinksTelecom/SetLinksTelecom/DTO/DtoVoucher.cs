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
        public int UserCode { get; set; }
        public string VSrNo { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string DCType { get; set; }
    }

    public class DtoTrailBalance
    {
        public string SubHeadName { get; set; }
        public string HeadName { get; set; }
        public int HeadCode { get; set; }
        public int SubHeadCode { get; set; }
        public int AccCode { get; set; }
        public string AccString { get; set; }
        public string AccName { get; set; }
        public double Opening { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public double Balance { get; set; }
        public string Status { get; set; }
        public string TypeName { get; set; }
    }

    public class DtoBalanceSheet
    { 
         public int HeadCode { get; set; }
         public int SubHeadCode { get; set; }     
         public string SubHeadString { get; set; }   
         public string SubHeadName { get; set; }     
         public string HeadName { get; set; }        
         public string AccString { get; set; }       
         public string AccName { get; set; }         
         public decimal Balance { get; set; }
         public string TypeName { get; set; }        
          
    }

}