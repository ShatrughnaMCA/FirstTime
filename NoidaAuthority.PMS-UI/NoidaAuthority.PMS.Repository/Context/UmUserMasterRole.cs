//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NoidaAuthority.PMS.Repository.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class UmUserMasterRole
    {
        public int RefId { get; set; }
        public Nullable<int> UserRefId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual UmRoleMaster UmRoleMaster { get; set; }
        public virtual UmUserMaster UmUserMaster { get; set; }
    }
}
