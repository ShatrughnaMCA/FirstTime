using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NoidaAuthority.PMS.Entity.NaUser
{
    public class NaUser
    {
        public System.Guid UserId { get; set; }

        [Display(Name = "User Email Address")]
        [Required(ErrorMessage = "User email Address is Required.")]
        [StringLength(200, MinimumLength = 8)]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid Email Format.")]
        [Remote("CheckEmailAddressRegistation", "Account", AdditionalFields = "Email", HttpMethod = "POST", ErrorMessage = "Email already exists.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "First name cannot be longer than 100 and less than 2 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid First Name.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name cannot be longer than 100 and less than 2 characters.")]
        [RegularExpression(@"^([a-zA-Z0-9 \.\&\'\-]+)$", ErrorMessage = "Invalid Last Name.")]
        public string LastName { get; set; }


        [Display(Name = "Mobile No")]
        [Required(ErrorMessage = "Phone No. is required.")]
        [StringLength(maximumLength: 10, ErrorMessage = "Mobile No. cannot be greater than 10 digits.")]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid Mobile Format.")]
        public string MobileNo { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Status is Required.")]
        public Boolean Status { get; set; }

        [Display(Name = "Administrator")]
        public Nullable<int> RoleId { get; set; }

        public bool IsLockedOut { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }



        //[Required(ErrorMessage = "PropertyId is Required.")]
        [Display(Name = "Property Id")]
        [Required(ErrorMessage = "PropertyId is Required.")]
        [StringLength(maximumLength: 8, ErrorMessage = "Property id cannot be greater than 8 digits.")]
        [RegularExpression(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{2})(?: *x(\d+))?\s*$", ErrorMessage = "Invalid Property Id Format.")]
        [Remote("CheckRegistationID", "Account", AdditionalFields = "PropertyId", HttpMethod = "POST", ErrorMessage = "PropertyId does not exists.")]
        public string PropertyId { get; set; }

        public string Role { get; set; }
        [Display(Name = "Department")]
        public string RoleType { get; set; }
        [Required(ErrorMessage = "Role is Required.")]
        public int id { get; set; }
        public int DeptId { get; set; }
        public Boolean IsRejected { get; set; }
        public string Remarks { get; set; }
        public Boolean IsFirstTimeActivated { get; set; }
    }
}