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
    
    public partial class ChecklistTypeMaster
    {
        public ChecklistTypeMaster()
        {
            this.ChecklistMasters = new HashSet<ChecklistMaster>();
            this.ChecklistTrans = new HashSet<ChecklistTran>();
        }
    
        public int Id { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual ICollection<ChecklistMaster> ChecklistMasters { get; set; }
        public virtual ICollection<ChecklistTran> ChecklistTrans { get; set; }
    }
}
