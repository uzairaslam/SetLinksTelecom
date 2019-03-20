using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Bank
    {
        public int BankId { get; set; }
        public string Name { get; set; }

        public string AccNumber { get; set; }

        [MaxLength(12)]
        public string AccString { get; set; }
    }
}