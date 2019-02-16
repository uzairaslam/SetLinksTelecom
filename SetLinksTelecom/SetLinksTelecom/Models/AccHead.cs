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
        public AccHead()
        {
            //AccType = new AccType();
            SubHeads = new List<AccSubHead>();
        }
        public int AccHeadId { get; set; }
        public int HeadCode { get; set; }
        public int TypeCode { get; set; }
        [MaxLength(25)]
        public string HeadString { get; set; }
        [MaxLength(50)]
        public string HeadName { get; set; }

        public int AccTypeId { get; set; }
        [ForeignKey("AccTypeId")]
        public virtual AccType AccType { get; set; }

        public virtual ICollection<AccSubHead> SubHeads { get; set; }

    }
}