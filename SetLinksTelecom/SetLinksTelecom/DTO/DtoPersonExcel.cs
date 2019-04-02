using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SetLinksTelecom.DTO
{
    public class DtoPersonExcel
    {

        [Required(ErrorMessage = "Name is required")]
        [MinLength(3), MaxLength(30)]
        public string Name { get; set; }


        [MaxLength(30)]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }



        [Required(ErrorMessage = "CNIC is Required")]
        [RegularExpression(@"^[1-4]{1}[0-9]{4}(-)?[0-9]{7}(-)?[0-9]{1}$", ErrorMessage = "Not Valid CNIC \nFormat: 12345-1234567-1 or 1234512345671")]
        public string CNIC { get; set; }

        [Required]
        public string Gender { get; set; }


        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }


        [Display(Name = "CNIC Issue Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CNICIssueDate { get; set; }


        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }


        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }


        [Required(ErrorMessage = "You must provide a Phone Number")]
        [Display(Name = "Mobile Business")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Not a valid Mobile Business number\nFormat: +923031234567 OR 03031234567")]
        public string MobileBusiness { get; set; }

        public string BusinessLine { get; set; }


        [Required(ErrorMessage = "You must provide a Phone Number")]
        [Display(Name = "Mobile Personal")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Not a valid Mobile Personal number\nFormat: +923031234567 OR 03031234567")]
        public string MobilePersonal { get; set; }

        public string PersonalLine { get; set; }


        public string Qualification { get; set; }

        public string Designation { get; set; }

        public bool IsInvalid { get; set; }

        public string ErrorMessage { get; set; }
    }
}