//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoidaAuthority.PMS.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public partial class NAcustomer
    {
        [Key]
        public System.Guid CustomerId { get; set; }
        public string AllotteeName { get; set; }
        public string RegistrationId { get; set; }
        public string PropertyNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> IsLockedOut { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CustomerIdFileName { get; set; }
        public string AuthorityLetter { get; set; }
        public string CustomerIdFileType { get; set; }
        public string AuthorityLetterType { get; set; }
    }
}
