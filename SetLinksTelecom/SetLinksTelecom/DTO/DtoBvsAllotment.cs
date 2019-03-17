using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoBvsAllotment
    {
        public DtoBvsAllotment()
        {
            AllotmentServiceses = new List<DtoBvsAllotmentServices>();
        }
        public int PersonId { get; set; }
        public string PersonName { get; set; }

        public int? ItemId { get; set; }
        public string ItemName { get; set; }

        public List<DtoBvsAllotmentServices> AllotmentServiceses { get; set; }
    }

    public class DtoBvsAllotmentServices
    {
        public int? BVSAllotServiceId { get; set; }
        public int BvsServiceId { get; set; }
        public string BvsServiceName { get; set; }
        public bool Active { get; set; }
    }
}