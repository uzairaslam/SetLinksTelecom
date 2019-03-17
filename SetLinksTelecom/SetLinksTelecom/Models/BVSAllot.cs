using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class BVSAllot
    {
        public int BVSAllotId { get; set; }

        [Display(Name = "Person")]
        public int PersonId { get; set; }
        [NotMapped]
        public virtual Person Person { get; set; }

        public int ItemId { get; set; }
        [NotMapped]
        public virtual Item Item { get; set; }
    }
}