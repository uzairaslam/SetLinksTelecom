using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class BVSAllotService
    {
        public int BVSAllotServiceId { get; set; }

        public int BVSAllotId { get; set; }
        public virtual BVSAllot BvsAllot { get; set; }

        public int BvsServiceId { get; set; }
        [NotMapped]
        public virtual BvsService BvsService { get; set; }
        [NotMapped]
        public virtual List<BvsService> BvsServices { get; set; }
    }
}