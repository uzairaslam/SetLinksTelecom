using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoUserLogin
    {
        [Required(ErrorMessage = "This Field is Required")]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required(ErrorMessage = "This Field is Required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}