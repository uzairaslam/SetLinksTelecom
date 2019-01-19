using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }

        public bool Active { get; set; }
    }
}