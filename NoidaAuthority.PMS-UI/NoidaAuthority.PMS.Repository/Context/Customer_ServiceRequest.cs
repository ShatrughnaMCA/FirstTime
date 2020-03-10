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
    
    public partial class Customer_ServiceRequest
    {
        public Customer_ServiceRequest()
        {
            this.OnlineExtensionDetails = new HashSet<OnlineExtensionDetail>();
            this.OnlineFirmDirectorMasters = new HashSet<OnlineFirmDirectorMaster>();
            this.OnlineFirmRequestMasters = new HashSet<OnlineFirmRequestMaster>();
            this.OnlineFunctionalDetails = new HashSet<OnlineFunctionalDetail>();
            this.OnlineMortgageDetails = new HashSet<OnlineMortgageDetail>();
            this.OnlinePropertyCancellationDetails = new HashSet<OnlinePropertyCancellationDetail>();
            this.OnlineRentPermissionDetails = new HashSet<OnlineRentPermissionDetail>();
            this.OnlineTransferMutations = new HashSet<OnlineTransferMutation>();
            this.ServiceRequest_Documents = new HashSet<ServiceRequest_Documents>();
        }
    
        public int Id { get; set; }
        public string Registration_No { get; set; }
        public string SectorName { get; set; }
        public string BlockName { get; set; }
        public string PlotNo { get; set; }
        public string Property_No { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string SubDepartment { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceType { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string RequestorName { get; set; }
        public string RequestorAddress { get; set; }
        public Nullable<bool> IsUploadedLetter { get; set; }
        public string UploadedDocumentName { get; set; }
        public string Description { get; set; }
        public Nullable<int> Request_Status { get; set; }
        public Nullable<int> ValidatorId { get; set; }
        public Nullable<System.DateTime> ValidatedDate { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<System.DateTime> ApprovalDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string RequestThrough { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<int> ChallanId { get; set; }
        public Nullable<decimal> ServiceFee { get; set; }
        public Nullable<decimal> DuesAmount { get; set; }
        public Nullable<System.DateTime> LastDateofPayment { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public string DispatchNumber { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public Nullable<int> DispatchDocumentId { get; set; }
        public string DispatchDocumentName { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ObjectionStatus { get; set; }
        public Nullable<int> DocumentStatus { get; set; }
    
        public virtual ICollection<OnlineExtensionDetail> OnlineExtensionDetails { get; set; }
        public virtual ICollection<OnlineFirmDirectorMaster> OnlineFirmDirectorMasters { get; set; }
        public virtual ICollection<OnlineFirmRequestMaster> OnlineFirmRequestMasters { get; set; }
        public virtual ICollection<OnlineFunctionalDetail> OnlineFunctionalDetails { get; set; }
        public virtual ICollection<OnlineMortgageDetail> OnlineMortgageDetails { get; set; }
        public virtual ICollection<OnlinePropertyCancellationDetail> OnlinePropertyCancellationDetails { get; set; }
        public virtual ICollection<OnlineRentPermissionDetail> OnlineRentPermissionDetails { get; set; }
        public virtual ICollection<OnlineTransferMutation> OnlineTransferMutations { get; set; }
        public virtual Challan_Master Challan_Master { get; set; }
        public virtual ICollection<ServiceRequest_Documents> ServiceRequest_Documents { get; set; }
    }
}