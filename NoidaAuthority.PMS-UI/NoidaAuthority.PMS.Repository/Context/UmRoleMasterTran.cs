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
    
    public partial class UmRoleMasterTran
    {
        public int RefId { get; set; }
        public int RoleId { get; set; }
        public int ApplicationId { get; set; }
        public int MenuId { get; set; }
        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsWrite { get; set; }
        public Nullable<bool> IsUpdate { get; set; }
        public Nullable<bool> Isdelete { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual UmApplicationMaster UmApplicationMaster { get; set; }
        public virtual UmRoleMaster UmRoleMaster { get; set; }
    }
}
