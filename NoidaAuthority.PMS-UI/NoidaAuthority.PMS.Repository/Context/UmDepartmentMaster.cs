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
    
    public partial class UmDepartmentMaster
    {
        public UmDepartmentMaster()
        {
            this.UmUserDepartmentTrans = new HashSet<UmUserDepartmentTran>();
        }
    
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<bool> Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual ICollection<UmUserDepartmentTran> UmUserDepartmentTrans { get; set; }
    }
}
