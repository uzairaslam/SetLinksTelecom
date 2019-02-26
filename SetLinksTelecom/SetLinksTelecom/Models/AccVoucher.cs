using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccVoucher
    {
        public int AccVoucherId { get; set; }
        public DateTime VDate { get; set; }
        public int SessionId { get; set; }
        [MaxLength(12)]
        public string AccString { get; set; }

        public int VNo { get; set; }
        [MaxLength(3)]
        public string VType { get; set; }

        public int VSrNo { get; set; }
        [MaxLength(300)]
        public string VDescription { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public int UserCode { get; set; }
        public int OID { get; set; }
        public int BID { get; set; }
        public int CID { get; set; }
        public int HeadCode { get; set; }
        public int SubHeadCode { get; set; }
        public int AccCode { get; set; }
        [MaxLength(25)]
        public string ChequeNo { get; set; }
        [MaxLength(50)]
        public string InvNo { get; set; }
        [MaxLength(50)]
        public string InvType { get; set; }
    }
}