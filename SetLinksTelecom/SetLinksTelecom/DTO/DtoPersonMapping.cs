using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SetLinksTelecom.Models;

namespace SetLinksTelecom.DTO
{
    public class DtoPersonMapping
    {
        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public List<Designation> Designations { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
    }
}