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
    
    public partial class SchemeDepartmentTran
    {
        public Nullable<int> schemeId { get; set; }
        public Nullable<int> departmentId { get; set; }
        public int refId { get; set; }
        public Nullable<double> normalInt { get; set; }
        public Nullable<double> penalInt { get; set; }
        public Nullable<int> frequency { get; set; }
        public Nullable<int> noOfInstallments { get; set; }
        public Nullable<double> far { get; set; }
        public Nullable<double> allotmentMoneyPercent { get; set; }
        public Nullable<double> installmentMoneyPercent { get; set; }
        public string selectionType { get; set; }
        public Nullable<double> leaseRentPercent { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
    
        public virtual DepartmentMst DepartmentMst { get; set; }
        public virtual SchemeMst SchemeMst { get; set; }
    }
}
