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
    
    public partial class OnlineApplicationDetail
    {
        public OnlineApplicationDetail()
        {
            this.OnlineSchemeDraws = new HashSet<OnlineSchemeDraw>();
            this.OnlineApplicationProcessDetails = new HashSet<OnlineApplicationProcessDetail>();
            this.OnlineCheckLisTrans = new HashSet<OnlineCheckLisTran>();
        }
    
        public int onlineapplicationId { get; set; }
        public Nullable<int> schemeId { get; set; }
        public Nullable<int> departmentId { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string marritalStatus { get; set; }
        public string fatherHusbandName { get; set; }
        public string motherName { get; set; }
        public Nullable<System.DateTime> dateOfBirth { get; set; }
        public string signingAuthority { get; set; }
        public string registeredOffice { get; set; }
        public string correspondanceAdd { get; set; }
        public string permanentAdd { get; set; }
        public string mobileNumberP1 { get; set; }
        public string mobileNumberP2 { get; set; }
        public string phoneNumberP1 { get; set; }
        public string phoneNumberP2 { get; set; }
        public string faxNumberP1 { get; set; }
        public string faxNumberP2 { get; set; }
        public string email { get; set; }
        public Nullable<int> occupationId { get; set; }
        public Nullable<int> quotaId { get; set; }
        public Nullable<int> religionId { get; set; }
        public string pan { get; set; }
        public Nullable<decimal> annualIncome { get; set; }
        public string CompanyName { get; set; }
        public Nullable<System.DateTime> ApplicationDate { get; set; }
        public Nullable<decimal> ApplicationFee { get; set; }
        public Nullable<decimal> EarnestMoney { get; set; }
        public Nullable<decimal> ProcessingCharge { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<int> CompanyType { get; set; }
        public string area { get; set; }
        public string Sector { get; set; }
        public Nullable<int> Phase { get; set; }
        public string Signaturefilename { get; set; }
        public string Photographfilename { get; set; }
        public string Documentfilename { get; set; }
        public Nullable<int> RefundBankId { get; set; }
        public string RefundInfaverof { get; set; }
        public string RefundaccountNo { get; set; }
        public string createdBy { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public string modifiedBy { get; set; }
        public Nullable<System.DateTime> modifiedDate { get; set; }
        public Nullable<bool> isActive { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public Nullable<int> StatusCode { get; set; }
        public string IFSCCode { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> PropertyTypeID { get; set; }
        public Nullable<decimal> FormSGST { get; set; }
        public Nullable<decimal> FormCGST { get; set; }
        public Nullable<decimal> ProcessingSGST { get; set; }
        public Nullable<decimal> ProcessingCGST { get; set; }
        public Nullable<bool> IsSubmited { get; set; }
        public string paymentTypeId { get; set; }
        public string AadharNumber { get; set; }
        public string FormCategory { get; set; }
        public string FormSubCategory { get; set; }
        public string RentingLetterNo { get; set; }
        public Nullable<System.DateTime> RentingDate { get; set; }
        public string RentingPropertyNo { get; set; }
        public string ExistingPropertyNo { get; set; }
        public Nullable<System.DateTime> AllotmentDate { get; set; }
        public Nullable<System.DateTime> DispatchDate { get; set; }
        public string PropertyNo { get; set; }
        public string projectname { get; set; }
        public string projectcost { get; set; }
        public string projecttimeempl { get; set; }
        public string GSTNO { get; set; }
        public string Userpassword { get; set; }
        public string Online_offline { get; set; }
        public string PreviousFormNo { get; set; }
        public Nullable<int> ScrutinyStatusId { get; set; }
    
        public virtual ICollection<OnlineSchemeDraw> OnlineSchemeDraws { get; set; }
        public virtual ICollection<OnlineApplicationProcessDetail> OnlineApplicationProcessDetails { get; set; }
        public virtual ICollection<OnlineCheckLisTran> OnlineCheckLisTrans { get; set; }
    }
}