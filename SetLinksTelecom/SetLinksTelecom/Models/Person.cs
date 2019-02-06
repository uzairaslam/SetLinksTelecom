using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SetLinksTelecom.GeneralFolder;

namespace SetLinksTelecom.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MinLength(3),MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(30)]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }

        [Required]
        [Column("Gender")]
        [Display(Name = "Gender")]
        [MaxLength(7)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "CNIC is Required")]
        [RegularExpression(@"^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$", ErrorMessage = "Not Valid CNIC \nFormat: 12345-1234567-1")]
        public string CNIC { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime DOB { get; set; }

        [NotMapped]
        public string DOBFormatted { get { return this.DOB.ToString("dd/MM/yyyy"); } }

        [Display(Name = "CNIC Issue Date")]
        public DateTime CNICIssueDate { get; set; }

        [NotMapped]
        public string CNICIssueDateFormatted { get { return this.CNICIssueDate.ToString("dd/MM/yyyy"); } }

        [Display(Name = "Current Address")]
        public string CurrentAddress { get; set; }

        [Display(Name = "Permanent Address")]
        public string PermanentAddress { get; set; }

        [Required(ErrorMessage = "You must provide a Phone Number")]
        [Display(Name = "Mobile Bussiness")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Not a valid Mobile Business number\nFormat: +923031234567 OR 03031234567")]
        public string MobileBusiness { get; set; }

        [Required(ErrorMessage = "You must provide a Phone Number")]
        [Display(Name = "Mobile Personal")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$", ErrorMessage = "Not a valid Mobile Personal number\nFormat: +923031234567 OR 03031234567")]
        public string MobilePersonal { get; set; }

        public string Qualification { get; set; }

        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }

        [NotMapped]
        public List<Designation> Designations { get; set; }
    }
}