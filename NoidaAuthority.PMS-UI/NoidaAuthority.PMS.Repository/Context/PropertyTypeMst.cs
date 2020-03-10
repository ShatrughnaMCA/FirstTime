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
    
    public partial class PropertyTypeMst
    {
        public PropertyTypeMst()
        {
            this.SchemeCostTrans = new HashSet<SchemeCostTran>();
            this.SchemePropTrans = new HashSet<SchemePropTran>();
        }
    
        public Nullable<int> departmentId { get; set; }
        public int propertyTypeId { get; set; }
        public string propertyTypeName { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual DepartmentMst DepartmentMst { get; set; }
        public virtual ICollection<SchemeCostTran> SchemeCostTrans { get; set; }
        public virtual ICollection<SchemePropTran> SchemePropTrans { get; set; }
    }
}
