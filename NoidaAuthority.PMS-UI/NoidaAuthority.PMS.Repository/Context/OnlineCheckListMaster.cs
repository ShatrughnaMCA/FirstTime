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
    
    public partial class OnlineCheckListMaster
    {
        public OnlineCheckListMaster()
        {
            this.OnlineCheckLisTrans = new HashSet<OnlineCheckLisTran>();
        }
    
        public int CheckListId { get; set; }
        public string CheckListName { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SchemeTypeId { get; set; }
        public string CheckListType { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string MainList { get; set; }
    
        public virtual ICollection<OnlineCheckLisTran> OnlineCheckLisTrans { get; set; }
    }
}
