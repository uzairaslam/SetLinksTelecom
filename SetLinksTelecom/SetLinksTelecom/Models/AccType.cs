using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class AccType
    {
        public AccType()
        {
            AccHeads = new List<AccHead>();
        }
        public int AccTypeId { get; set; }
        public int TypeCode { get; set; }
        [MaxLength(25)]
        //[Required]
        public string TypeName { get; set; }

        public virtual ICollection<AccHead> AccHeads { get; set; }
    }
}