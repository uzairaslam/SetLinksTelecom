using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MinLength(3),MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(20,MinimumLength = 3,ErrorMessage = "Password is Required")]
        public string Password { get; set; }
    }
}