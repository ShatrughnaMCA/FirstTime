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
    
    public partial class AllotmentMaster
    {
        public AllotmentMaster()
        {
            this.RentPermissionDetails = new HashSet<RentPermissionDetail>();
            this.Director_Request_Master = new HashSet<Director_Request_Master>();
            this.Extension_Details = new HashSet<Extension_Details>();
            this.Firm_Master = new HashSet<Firm_Master>();
            this.PossessionDetails = new HashSet<PossessionDetail>();
            this.RegistryDetails = new HashSet<RegistryDetail>();
        }
    
        public int rid { get; set; }
        public Nullable<int> applicationId { get; set; }
        public Nullable<int> schemeId { get; set; }
        public Nullable<int> departmentId { get; set; }
        public string formNo { get; set; }
        public int propertyId { get; set; }
        public Nullable<System.DateTime> allotmentDate { get; set; }
        public Nullable<System.DateTime> instalmentStartDate { get; set; }
        public string isSubmitted { get; set; }
        public string isStatus { get; set; }
        public Nullable<System.DateTime> submitDate { get; set; }
        public string approverBy { get; set; }
        public Nullable<System.DateTime> approveDate { get; set; }
        public Nullable<int> isActive { get; set; }
        public string comment { get; set; }
        public Nullable<bool> isDocAvailable { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
        public Nullable<int> submittedBy { get; set; }
        public string RComment { get; set; }
        public Nullable<System.DateTime> RCommentDate { get; set; }
    
        public virtual ApplicationDetail ApplicationDetail { get; set; }
        public virtual DepartmentMst DepartmentMst { get; set; }
        public virtual ICollection<RentPermissionDetail> RentPermissionDetails { get; set; }
        public virtual SchemeMst SchemeMst { get; set; }
        public virtual ICollection<Director_Request_Master> Director_Request_Master { get; set; }
        public virtual ICollection<Extension_Details> Extension_Details { get; set; }
        public virtual ICollection<Firm_Master> Firm_Master { get; set; }
        public virtual ICollection<PossessionDetail> PossessionDetails { get; set; }
        public virtual ICollection<RegistryDetail> RegistryDetails { get; set; }
    }
}
