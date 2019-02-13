using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccHead
    {
        public int AccHeadId { get; set; }
        [MaxLength(25)]
        public string HeadString { get; set; }
        [MaxLength(50)]
        public string HeadName { get; set; }

        public int? AccTypeId { get; set; }
        [ForeignKey("AccTypeId")]
        [NotMapped]
        public AccType AccType { get; set; }
    }
}