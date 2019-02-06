using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Line
    {
        public int LineId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Percentage { get; set; }
    }
}