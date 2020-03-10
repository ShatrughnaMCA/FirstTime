using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class UserViewModel
    {
        public Nullable<Guid> UserId { get; set; }
        public Nullable<Guid> CreatedBy { get; set; }
        public Nullable<Guid> ModifiedBy { get; set; }

        public Nullable<int> Id { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> ActionId { get; set; }
        public Nullable<int> ReturnTypeId { get; set; }

        //[Required]
        public string UserName { get; set; }
        //[Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string PropertyId { get; set; }
        public string Department { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string Remarks { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string NALetterType { get; set; }
        public string CustomerIdType { get; set; }
        public string CustomerIdName { get; set; }
        public string NALetterName { get; set; }
        public string NALetterPath { get; set; }
        public string CustomerIdPath { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string RandomKeyValue { get; set; }
        public string RandomKeyEdit { get; set; }
        public string AadharNo { get; set; }
        public string CorrespondingAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Captcha { get; set; }
        public string InputCaptcha { get; set; }
        public string ErrorMessage { get; set; }
        public string StatusName { get; set; }

        public Nullable<bool> Status { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsLocked { get; set; }
        public Nullable<bool> IsRejected { get; set; }
        public Nullable<bool> IsFirstTimeActivated { get; set; }

        public Nullable<DateTime> CreatedOn { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<DateTime> LastPasswordChangeDate { get; set; }       
    }
}
