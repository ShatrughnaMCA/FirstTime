using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity.NaUser
{
    public class UserModel
    {
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Pasword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<bool> Status { get; set; }
        public bool IsLockedOut { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public Nullable<System.Guid> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<System.DateTime> LastPasswordChangeDate { get; set; }
        public string RoleName { get; set; }
        public string PropertyId { get; set; }
        public string MobileNo { get; set; }
        public string FullName { get; set; }
        public string AuthorityLetterType { get; set; }
        public string CustomerIdFileType { get; set; }
        public string CustomerIdFileName { get; set; }
        public string AuthorityLetter { get; set; }
        public Nullable<int> DeptId { get; set; }
        public Nullable<Boolean> IsRejected { get; set; }
        public string Remarks { get; set; }
        public Nullable<Boolean> IsFirstTimeActivated { get; set; }
        public string UserEmail { get; set; }
    }
}
