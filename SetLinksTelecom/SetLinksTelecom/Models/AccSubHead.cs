using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccSubHead
    {
        public AccSubHead()
        {
            //this.AccHead = new AccHead();
        }
        public int AccSubHeadId { get; set; }
        public int SubHeadCode { get; set; }
        public int HeadCode { get; set; }
        public int AccHeadId { get; set; }
        [ForeignKey("AccHeadId")]
        //[NotMapped]
        public virtual AccHead AccHead { get; set; }
        [MaxLength(7)]
        public string SubHeadString { get; set; }
        [MaxLength(50)]
        public string SubHeadName { get; set; }

        public int OID { get; set; }


    }
}