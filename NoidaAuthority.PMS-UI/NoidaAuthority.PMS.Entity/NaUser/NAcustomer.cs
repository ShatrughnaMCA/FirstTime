using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoidaAuthority.PMS.Entity.NaUser
{
    public class NAcustomer
    {
        [Key]
        public System.Guid CustomerId { get; set; }
        [Display(Name = "Registration Id")]
        [Required(ErrorMessage = "Registration Id is required")]
        [StringLength(maximumLength: 8, ErrorMessage = "Registration id can't be more than 8 digits.")]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{2})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid Registration Id Format.")]
        //[Remote("CheckRegistationIDforNAcustomer", "Property", AdditionalFields = "RegistrationId", HttpMethod = "POST", ErrorMessage = " This RegistrationId  already exists.")]
        public string RegistrationId { get; set; }

        //[Required(ErrorMessage = "PropertyId is Required.")]
        //[Display(Name = "Property Id")]
        // [Required(ErrorMessage = "PropertyId is Required.")]
        //[StringLength(maximumLength: 8, ErrorMessage = "Property id cannot be greater than 8 digits.")]
        //[RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{2})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid Propeerty Id Format.")]
        //[Remote("CheckRegistationID", "Account", AdditionalFields = "PropertyId", HttpMethod = "POST", ErrorMessage = "PropertyId does not exists.")]
        // public string PropertyId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name can't be longer than 100 and less than 2 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last Name can't be longer than 100 and less than 2 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        //[Required(ErrorMessage = "User Email Address is Required.")]
        [StringLength(200, MinimumLength = 8)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Format.")]
        public string Email { get; set; }

        [Display(Name = "User Name")]
        public string UserNameEmail { get; set; }

        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Mobile No. is required.")]
        [StringLength(maximumLength: 10, ErrorMessage = "Mobile No. cannot be greater than 10 digits.")]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid Mobile Format.")]
        public string MobileNo { get; set; }

        //public string MobileNo { get; set; }

        public string CustomerIdFileName { get; set; }
        public string CustomerIdFiletype { get; set; }

        public string AuthorityLetter { get; set; }
        public string AuthorityLetterType { get; set; }

        [Display(Name = "Plot/Shop/Flat No")]
        [Required(ErrorMessage = "Plot No is required")]
        [StringLength(maximumLength: 6, ErrorMessage = "Plot No can't be more than 6 digits or alphabets.")]
        public string PropertyNo { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }


        public Boolean Status { get; set; }

        public bool IsLockedOut { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        [Display(Name = "Security Question")]
        public string SecurityQuestion { get; set; }
        [Display(Name = "Answer")]
        public string SecurityAnswer { get; set; }
        public string  RandomKeyVal { get; set; }
        public string RandomKeyEdit { get; set; }
    }

}
