using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccAccount
    {
        public int AccAccountId { get; set; }
        public int HeadCode { get; set; }

        public int SubHeadCode { get; set; }

        [MaxLength(12)]
        public string AccString { get; set; }

        [MaxLength(80)]
        public string AccName { get; set; }

        public int OID { get; set; }
    }
}