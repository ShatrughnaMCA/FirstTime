﻿//using NoidaAuthority.PMS.Common;
//using NoidaAuthority.PMS.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NoidaAuthority.PMS.Entity
{
    public class ServiceDetailViewModel
    {
    }

    public class ServiceRequestViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> OnlineRequestId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> ActionTypeId { get; set; }
        public Nullable<int> FilterTypeId { get; set; }
        public Nullable<int> ChallanId { get; set; }

        public string RegistrationType { get; set; }
        public string ActionType { get; set; }
        public string FilterType { get; set; }

        public ServiceViewModel ServiceModel { get; set; }
        public PropertyViewModel PropertyModel { get; set; }
        public ApplicantViewModel ApplicantModel { get; set; }
        public TransferViewModel TransferModel { get; set; }
        public RentingViewModel RentModel { get; set; }
        public TransferViewModel MutationModel { get; set; }
        public GPAViewModel GPAModel { get; set; }
        public ExtensionViewModel ExtensionModel { get; set; }
        public MortgageViewModel MortgageModel { get; set; }
        public CICViewModel CICModel { get; set; }
        public PaymentViewModel PaymentModel { get; set; }
        public OnlinePaymentViewModel OnlinePaymentModel { get; set; }
    }

    public class ServiceViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RId { get; set; }
        public Nullable<int> RequestId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> SubDepartmentId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> ServiceStatusId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; }
        public Nullable<int> RequestorId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<int> ValidatorId { get; set; }
        public Nullable<int> Timeline { get; set; }
        public Nullable<int> ActionTypeId { get; set; }
        public Nullable<int> RoleTypeId { get; set; }
        public Nullable<int> ObjectionStatus { get; set; }
        public Nullable<int> DocumentStatus { get; set; }

        public string Department { get; set; }
        public string SchemeName { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationType { get; set; }
        public string SubDepartment { get; set; }
        public string ServiceName { get; set; }
        public string ServiceType { get; set; }
        public string Status { get; set; }
        public string ServiceStatus { get; set; }
        public string RequestStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string RoleType { get; set; }
        public string Applicant { get; set; }
        public string ApplicantMaster { get; set; }
        public string ApplicantAddress { get; set; }
        public string RequestorAddress { get; set; }
        public string Requestor { get; set; }
        public string Approver { get; set; }
        public string Validator { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string GSTNo { get; set; }
        public string AadharNo { get; set; }
        public string Message { get; set; }
        public string ActionType { get; set; }
        public string UploadedDocument { get; set; }
        public string DispatchedDocument { get; set; }
        public string RequestThrough { get; set; }
        public string TransactionId { get; set; }
        public string HtmlContent { get; set; }
        public string FileUploadHtml { get; set; }
        public string ChargeDetailHtml { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> ISUploaded { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public Nullable<bool> IsProcessingFeePaid { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> DuesAmount { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }

        public Nullable<DateTime> RequestDate { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public HttpPostedFileBase Document { get; set; }

        public string EncryptedId { get { return RequestId != null ? ModelHelper.Encode(RequestId.ToString()) : null; } }
        public string EncryptedRId { get { return RegistrationNo != null ? ModelHelper.Encode(RegistrationNo) : null; } }
        public string EncryptedRegistrationId { get { return RegistrationId != null ? ModelHelper.Encode(RegistrationId.ToString()) : null; } }
    }

    public class PropertyViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> ParentPropertyId { get; set; }
        public Nullable<int> PropertyTypeId { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> SchemeStatusId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> FloorAreaId { get; set; }
        public Nullable<int> RegistryId { get; set; }
        public Nullable<int> VillageId { get; set; }
        public Nullable<int> TotalInstallment { get; set; }
        public Nullable<int> Frequency { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> FloorId { get; set; }
        public Nullable<int> ActionTypeId { get; set; }

        public string SchemeName { get; set; }
        public string SchemeStatus { get; set; }
        public string Department { get; set; }
        public string PropertyNo { get; set; }
        public string PropertyType { get; set; }
        public string SectorName { get; set; }
        public string BlockName { get; set; }
        public string PlotNo { get; set; }
        public string Status { get; set; }
        public string FloorArea { get; set; }
        public string FormNo { get; set; }
        public string Applicant { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantMaster { get; set; }
        public string ApplicantAddress { get; set; }
        public string FirstApplicant { get; set; }
        public string FirstApplicantAdd { get; set; }
        public string CorrespondAddress { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string LocationType { get; set; }
        public string Registry { get; set; }
        public string Village { get; set; }
        public string KhasraNumber { get; set; }
        public string KhatoniNumber { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string ActionType { get; set; }
        public string AreaRange { get; set; }
        public string AllotmentYear { get; set; }
        public string AreaPhase { get; set; }
        public string AllotmentMethod { get; set; }

        public Nullable<Decimal> ActualArea { get; set; }
        public Nullable<Decimal> CoveredArea { get; set; }
        public Nullable<Decimal> TotalArea { get; set; }
        public Nullable<Decimal> CivilCost { get; set; }
        public Nullable<Decimal> PropertyCost { get; set; }
        public Nullable<Decimal> TotalPropertyCost { get; set; }
        public Nullable<Decimal> AllotmentMoney { get; set; }
        public Nullable<Decimal> EarnestMoney { get; set; }
        public Nullable<Decimal> LeaseRent { get; set; }
        public Nullable<Decimal> LandRate { get; set; }
        public Nullable<Decimal> LocationCharge { get; set; }
        public Nullable<Decimal> ProcessingFee { get; set; }
        public Nullable<Decimal> NormalInterest { get; set; }
        public Nullable<Decimal> PenalInterest { get; set; }

        public Nullable<Double> FloorAreaRatio { get; set; }

        public Nullable<DateTime> AllotmentDate { get; set; }
        public Nullable<DateTime> PossessionDate { get; set; }
        public Nullable<DateTime> RegistryDate { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public Nullable<DateTime> FunctionalDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> SchemeStartDate { get; set; }
        public Nullable<DateTime> SchemeEndDate { get; set; }
        public Nullable<DateTime> PropertyRateStartDate { get; set; }
        public Nullable<DateTime> PropertyRateEndDate { get; set; }
        public Nullable<DateTime> ActionDate { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDocumentAvailable { get; set; }
        public Nullable<bool> IsNDCIssued { get; set; }
        public Nullable<bool> IsLocationCharged { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public Nullable<bool> IsPropertyActive { get; set; }
        public Nullable<bool> IsPropertyAllotted { get; set; }
        public Nullable<bool> IsPropertyExists { get; set; }
        public Nullable<bool> IsPropertyFunctional { get; set; }

        public string EncryptedPropertyId { get { return (PropertyId == null || PropertyId == 0) ? null : ModelHelper.Encode(PropertyId.ToString()); } }
    }

    public class ApplicantViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> RequestNo { get; set; }
        public Nullable<int> ReferenceNo { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> ParentPropertyId { get; set; }
        public Nullable<int> PropertyTypeId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> PreviousDues { get; set; }
        public Nullable<int> TransferTypeId { get; set; }

        public string SchemeName { get; set; }
        public string Department { get; set; }
        public string Applicant { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantMaster { get; set; }
        public string Guardian { get; set; }
        public string MotherName { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo { get; set; }
        public string Mobile { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string MaritalStatus { get; set; }
        public string RegistryType { get; set; }
        public string TransfereeName { get; set; }
        public string TransfereeType { get; set; }
        public string TransfereeRelation { get; set; }
        public string TransferType { get; set; }
        public string PermanentAddress { get; set; }
        public string CorrespondAddress { get; set; }
        public string PropertyType { get; set; }
        public string FloorArea { get; set; }
        public string ActionType { get; set; }
        public string ResourceMessage { get; set; }
        public string MessageKey { get; set; }
        public string MessageValue { get; set; }
        public string MessageContent { get; set; }
        public string Occupation { get; set; }
        public string PAN { get; set; }

        public Nullable<decimal> Area { get; set; }
        public Nullable<decimal> PropertyCost { get; set; }
        public Nullable<decimal> AllotmentMoney { get; set; }
        public Nullable<decimal> StampDutyAmount { get; set; }
        public Nullable<decimal> DuplicateStampValue { get; set; }
        public Nullable<decimal> BalanceDues { get; set; }
        public Nullable<decimal> AnnualIncome { get; set; }

        public Nullable<DateTime> LeasedeedDueDate { get; set; }
        public Nullable<DateTime> TransferDate { get; set; }
        public Nullable<DateTime> AllotmentDate { get; set; }
        public Nullable<DateTime> ChecklistDuedate { get; set; }
        public Nullable<DateTime> ChecklistDate { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }

        public Nullable<bool> IsLeasedeed { get; set; }
        public Nullable<bool> IsSubLeasedeed { get; set; }
    }

    public class TransferViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RequestNo { get; set; }
        public Nullable<int> ReferenceId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> OccupationId { get; set; }
        public Nullable<int> RequestedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<int> TransfereeType { get; set; }
        public Nullable<int> TransferTypeId { get; set; }
        public Nullable<int> TransferSubTypeId { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

        public string PropertyNo { get; set; }
        public string Applicant { get; set; }
        public string ApplicantMaster { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantAddress { get; set; }
        public string CompanyName { get; set; }
        public string SigningAuthority { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string MotherName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string CorrespondenceAdd { get; set; }
        public string PermanentAdd { get; set; }
        public string RegisteredOffice { get; set; }
        public string Occupation { get; set; }
        public string PAN { get; set; }
        public string Transferee { get; set; }
        public string TransfereeMaster { get; set; }
        public string TransfereeAddress { get; set; }
        public string TransferType { get; set; }
        public string TransferSubType { get; set; }
        public string TransferStatus { get; set; }
        public string TypeOfTransferee { get; set; }
        public string BahiNo { get; set; }
        public string BahiZildNo { get; set; }
        public string BahiPageNo { get; set; }
        public string BahiSeriesNo { get; set; }
        public string BookNo { get; set; }
        public string BookZildNo { get; set; }
        public string BookSeriesNo { get; set; }
        public string GPAHolderName { get; set; }
        public string GPAHolderAddress { get; set; }
        public string Comment { get; set; }
        public string ProjectName { get; set; }
        public string ActionType { get; set; }
        public string Relation { get; set; }
        public string Nominee { get; set; }

        public Nullable<decimal> AnnualIncome { get; set; }
        public Nullable<decimal> SellingCost { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> TransferCharge { get; set; }
        public Nullable<decimal> TotalTransferCharge { get; set; }

        public Nullable<DateTime> TransferDate { get; set; }
        public Nullable<DateTime> MutationDate { get; set; }
        public Nullable<DateTime> TransferdeedDate { get; set; }
        public Nullable<DateTime> RequestedDate { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public Nullable<DateTime> CommentDate { get; set; }
        public Nullable<DateTime> GPAEffectiveFrom { get; set; }
        public Nullable<DateTime> GPAEffectiveTo { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsLeasedeedExecuted { get; set; }
        public Nullable<bool> IsGPAExecuted { get; set; }

        //public ApplicantViewModel ApplicantDetail { get; set; }
        //public PropertyViewModel PropertyModel { get; set; }
        //public PropertyDetailViewModel PropertyDetail { get; set; }
        //public ServiceRequestViewModel ServiceModel { get; set; }
    }

    public class GPAViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> SNo { get; set; }
        public Nullable<int> NomineeId { get; set; }
        public Nullable<int> GPAId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> OnlineRequestId { get; set; }

        public string GPAType { get; set; }
        public string GPAHolderName { get; set; }
        public string RelationName { get; set; }
        public string GPAHolderAdd { get; set; }
        public string GPARegisteredNo { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }

        public Nullable<bool> IsGPAActive { get; set; }
        public Nullable<bool> IsGPARegistered { get; set; }

        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        public Nullable<DateTime> ApplicationDate { get; set; }
        public Nullable<DateTime> AcceptanceDate { get; set; }
        public Nullable<DateTime> NominationDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> AllotmentDate { get; set; }
        public Nullable<DateTime> FromSearch { get; set; }
        public Nullable<DateTime> ToSearch { get; set; }

        public PropertyViewModel PropertyModel { get; set; }
        public ServiceRequestViewModel ServiceModel { get; set; }
    }

    public class RentingViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RentId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> OnlineRequestId { get; set; }
        public Nullable<int> RentStatusId { get; set; }
        public Nullable<int> Approver { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Modifiedby { get; set; }

        [RegularExpression(@"^[0-9 ]*$", ErrorMessage = "Rent Duration must be numeric.")]
        [Range(0, 99, ErrorMessage = "Rent Duration must be less than 100")]
        public Nullable<int> RentDuration { get; set; }

        [MaxLength(60, ErrorMessage = "Tenant Name cannot be more than 60 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "Tenant name must be alphanumeric.")]
        public string TenantName { get; set; }

        [MaxLength(100, ErrorMessage = "Tenant project cannot be more than 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9. ]*$", ErrorMessage = "Tenant project must be alphanumeric.")]
        public string TenantProject { get; set; }

        public string RentStatus { get; set; }
        public string Comment { get; set; }
        public string ViewName { get; set; }
        public string Applicant { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime RequestDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> RentingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> RentingEndDate { get; set; }

        public Nullable<DateTime> ApproveDate { get; set; }
        public Nullable<DateTime> CommentDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }

        [RegularExpression(@"^[0-9. ]*$", ErrorMessage = "Renting charge must be decimal.")]
        [Range(0, 999999999999.99, ErrorMessage = "Renting Charge must be in decimal max [12,2]")]
        public Nullable<decimal> RentingCharge { get; set; }

        public Nullable<decimal> Amount { get; set; }

        public Nullable<bool> Permission { get; set; }
        public Nullable<bool> IsRentActive { get; set; }
        public Nullable<bool> IsRentingChargePaid { get; set; }
        public Nullable<bool> IsRentPermissionRequested { get; set; }
        public Nullable<bool> IsPropertyFunctional { get; set; }

        public PropertyViewModel PropertyModel { get; set; }
        public ServiceRequestViewModel ServiceModel { get; set; }
    }

    public class ExtensionViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
        public Nullable<DateTime> CompletionDueDate { get; set; }
        public Nullable<DateTime> ExtensionGivenDate { get; set; }
        public Nullable<DateTime> ExtensionDueDate { get; set; }
        public Nullable<decimal> ExtensionCharge { get; set; }
        public string Comment { get; set; }
        public Nullable<int> ExtensionStatusId { get; set; }
        public string ExtensionStatus { get; set; }
        public string BuildingPlan { get; set; }

        public Nullable<int> ScheduleActionDay { get; set; }

        public string AssignTo { get; set; }
        public string Approver { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }

        public bool IsSelected { get; set; }
        public Nullable<bool> IsExtendable { get; set; }
        public Nullable<bool> IsExtensionActive { get; set; }

        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }

        public PropertyViewModel PropertyModel { get; set; }
        public ServiceRequestViewModel ServiceModel { get; set; }
    }

    public class MortgageViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
        public Nullable<int> MortgageTypeId { get; set; }
        public Nullable<int> MortgageStatusId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<int> Approver { get; set; }
        public Nullable<int> ApproverId { get; set; }

        public string Applicant { get; set; }
        public string ApplicantAddress { get; set; }
        public string BankName { get; set; }
        public string PreviousBankDetail { get; set; }
        public string BranchAddress { get; set; }
        public string Functional { get; set; }
        public string MortgageType { get; set; }
        public string MortgageStatus { get; set; }
        public string ApproverName { get; set; }
        public string Comment { get; set; }

        public Nullable<decimal> ProcessingFee { get; set; }
        public Nullable<decimal> SanctionedAmount { get; set; }
        public Nullable<decimal> TotalDues { get; set; }

        public Nullable<short> PreviousLoanNoc { get; set; }

        public Nullable<bool> IsMortgaged { get; set; }
        public Nullable<bool> IsMortgageActive { get; set; }

        public Nullable<DateTime> MortgageDate { get; set; }
        public Nullable<DateTime> ApproveDate { get; set; }
        public Nullable<DateTime> CommentDate { get; set; }
        public Nullable<DateTime> ValidUpto { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> MortgageStartDate { get; set; }
        public Nullable<DateTime> MortgageEndDate { get; set; }

        public PropertyViewModel PropertyModel { get; set; }
        public ServiceRequestViewModel ServiceModel { get; set; }

        private string EncryptedRequestNo { get { return ModelHelper.Encode(OnlineRequestNo.ToString()); } }
        public string EncryptedRegistrationId { get { return ModelHelper.Encode(RegistrationId.ToString()); } }

    }

    public class CICViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> OnlineRequestNo { get; set; }
        public Nullable<int> DirectorId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<int> ChangeTypeId { get; set; }
        public Nullable<int> OldFirmStatusId { get; set; }
        public Nullable<int> NewFirmStatusId { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> Approver { get; set; }
        public Nullable<int> CICStatusId { get; set; }
        public Nullable<int> FormId { get; set; }
        public Nullable<int> ApplicationId { get; set; }
        public Nullable<int> IsCICActive { get; set; }

        public string DirectorName { get; set; }
        public string TypeName { get; set; }
        public string ChangeTypeName { get; set; }
        public string OldFirmName { get; set; }
        public string NewFirmName { get; set; }
        public string OldFirmStatus { get; set; }
        public string NewFirmStatus { get; set; }
        public string OldFirmProduct { get; set; }
        public string NewFirmProduct { get; set; }
        public string Comment { get; set; }
        public string AssignTo { get; set; }
        public string CICStatus { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string FirmExists { get; set; }
        public string ActionType { get; set; }
        public string Department { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string ApproverName { get; set; }

        public Nullable<decimal> DirectorShare { get; set; }
        public Nullable<decimal> CICCharge { get; set; }

        public Nullable<DateTime> RequestDate { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }

        public Nullable<bool> IsFirmExists { get; set; }

        //public PropertyViewModel PropertyModel { get; set; }
        //public ServiceRequestViewModel ServiceModel { get; set; }
        //public List<DirectorShareholderModel> DirectorsModel { get; set; }
    }

    public class DirectorShareholderModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> ShareType { get; set; }
        public string ShareholderName { get; set; }
        public Nullable<decimal> ShareValue { get; set; }
        public string ShareTypeName
        {
            get
            {
                if (ShareType == 10)
                {
                    return "Driector";
                }
                if (ShareType == 11)
                {
                    return "Shareholder";
                }
                if (ShareType == 12)
                {
                    return "Driector/Shareholder";
                }
                else
                {
                    return "NA";
                }
            }
        }
    }
    public class ServiceCheckListModel
    {
        public int Id { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ChecklistRefNo { get; set; }
        public string Department { get; set; }
        public string ServiceName { get; set; }
        public string ChecklistName { get; set; }
        public Nullable<bool> Status { get; set; }
    }

    public class DocumentViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> SerialNo { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> TemplateId { get; set; }
        public Nullable<int> ActionId { get; set; }

        public string Department { get; set; }
        public string ServiceName { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string ScannedDocument { get; set; }
        public string GeneratedDocument { get; set; }
        public string UploadedDocument { get; set; }
        public string DocumentContent { get; set; }
        public string Template { get; set; }
        public string Status { get; set; }
        public string ActionType { get; set; }
        public string Barcode { get; set; }
        public string UserName { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsScanned { get; set; }
        public Nullable<bool> IsUploaded { get; set; }
        public Nullable<bool> IsGenerated { get; set; }
        public Nullable<bool> IsDocumentExist { get; set; }
    }

    public class PaymentViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> LeaseRentId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> AccountHeadId { get; set; }
        public Nullable<int> AccountSubHeadId { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public Nullable<int> PaymentSubTypeId { get; set; }
        public Nullable<int> PaymentModeId { get; set; }
        public Nullable<int> ReceiptCode { get; set; }
        public Nullable<int> ReceiptHeadId { get; set; }
        public Nullable<int> ReceiptSubHeadId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> BankId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SubHeadCode { get; set; }
        public Nullable<int> HeadCode { get; set; }
        public Nullable<int> InstallmentNo { get; set; }
        public Nullable<int> ApproverId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> TotalLeaseRentCount { get; set; }
        public Nullable<int> TotalIPCount { get; set; }
        public Nullable<int> TotalNDCCount { get; set; }
        public Nullable<int> OnlineRequestId { get; set; }
        public Nullable<int> ReturnTypeId { get; set; }

        public Nullable<long> ReceiptId { get; set; }
        public Nullable<double> DuePrincipalAmount { get; set; }
        public Nullable<double> DueInterestAmount { get; set; }
        public Nullable<double> LeaseRentAmount { get; set; }

        public string RegistrationNo { get; set; }
        public string DepartmentName { get; set; }
        public string Department { get; set; }
        public string ReceiptHeadName { get; set; }
        public string ReceiptSubHeadName { get; set; }
        public string PropertyRegistryId { get; set; }
        public string PropertyId { get; set; }
        public string SectorName { get; set; }
        public string BlockName { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo { get; set; }
        public string PropertyType { get; set; }
        public string Applicant { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantMaster { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HeadName { get; set; }
        public string SubHeadName { get; set; }
        public string UserId { get; set; }
        public string CONS_NO { get; set; }
        public string FLAG_EDIT { get; set; }
        public string LeaseRentUpto { get; set; }
        public string IsTotalPremiumPaid { get; set; }
        public string IsOneTimeLeasePaid { get; set; }
        public string IsOneTimeLease { get; set; }
        public string PaymentStatus { get; set; }
        public string CreatedBy { get; set; }
        public string AllotteeName { get { return FirstName + " " + MiddleName + " " + LastName; } }
        public string DepositorName { get; set; }
        public string CorresspondentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ApplicantAddress { get; set; }
        public string DepositorAddress { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string ChallanId { get; set; }
        public string TransactionId { get; set; }
        public string PaymentMode { get; set; }
        public string AccountHead { get; set; }
        public string AccountSubHead { get; set; }
        public string PaymentType { get; set; }
        public string PaymentSubType { get; set; }
        public string Comment { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string LeaseRentDuration { get; set; }
        public string InstallmentDuration { get; set; }
        public string OneTimePaidStatus { get; set; }
        public string PremiumPaidStatus { get; set; }
        public string NDCStatus { get; set; }
        public string HeadStatus { get; set; }
        public string SubHeadStatus { get; set; }
        public string PremiumPaidDuration { get; set; }
        public string Status { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string ActionType { get; set; }
        public string FilterType { get; set; }
        public string FloorAreaRange { get; set; }
        public string ReferenceNo { get; set; }
        public string PAN { get; set; }
        public string GSTNo { get; set; }
        public string AadharNo { get; set; }
        public string HTMLContent { get; set; }
        public string ReturnType { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> BudgetAmount { get; set; }
        public Nullable<decimal> DebitAmount { get; set; }
        public Nullable<decimal> CreditAmount { get; set; }
        public Nullable<decimal> BalanceAmount { get; set; }
        public Nullable<decimal> PrincipalAmount { get; set; }
        public Nullable<decimal> InstallmentAmount { get; set; }
        public Nullable<decimal> InstallmentInterest { get; set; }
        public Nullable<decimal> DuesAmount { get; set; }
        public Nullable<decimal> GSTAmount { get; set; }
        public Nullable<decimal> LeaseRentDues { get; set; }
        public Nullable<decimal> LeaseRentPremium { get; set; }
        public Nullable<decimal> PenalInterest { get; set; }
        public Nullable<decimal> GST { get; set; }
        public Nullable<decimal> RevisedLeaseRent { get; set; }
        public Nullable<decimal> RevisedInstallment { get; set; }
        public Nullable<decimal> RevisedPremium { get; set; }
        public Nullable<decimal> TotalBalance { get; set; }
        public Nullable<decimal> BalanceInterest { get; set; }
        public Nullable<decimal> LeaseRentBalance { get; set; }
        public Nullable<decimal> InstallmentBalance { get; set; }
        public Nullable<decimal> TotalArea { get; set; }
        public Nullable<decimal> ProcessingFee { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsCancelled { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public Nullable<bool> IsPaymentActive { get; set; }
        public Nullable<bool> IsOneTimeLeaseRentPaid { get; set; }
        public Nullable<bool> IsTotalInstallmentPaid { get; set; }
        public Nullable<bool> IsNDCGenerated { get; set; }
        public Nullable<bool> IsDuesUptoDate { get; set; }
        public Nullable<bool> IsLeaseRentPaid { get; set; }
        public Nullable<bool> IsChallanExist { get; set; }

        public Nullable<DateTime> RegistryDate { get; set; }
        public Nullable<DateTime> LeaseDeedDate { get; set; }
        public Nullable<DateTime> DepositDate { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> ModifyDate { get; set; }
        public Nullable<DateTime> FromDate { get; set; }
        public Nullable<DateTime> ToDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> DateUptoPremium { get; set; }
        public Nullable<DateTime> NDCDate { get; set; }
        public Nullable<DateTime> InstallmentDueDate { get; set; }
        public Nullable<DateTime> ApprovalDate { get; set; }
        public Nullable<DateTime> BalanceUptoDate { get; set; }
        public Nullable<DateTime> DuesUptoDate { get; set; }
        public Nullable<DateTime> InstallmentDuesUptoDate { get; set; }
        public Nullable<DateTime> LeaseRentDuesUptoDate { get; set; }
        public Nullable<DateTime> PaidUptoDate { get; set; }
        public Nullable<DateTime> RevisedDate { get; set; }
        public Nullable<DateTime> ChallanDate { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public string EncodedRid { get { return RegistrationNo != null ? ModelHelper.Encode(RegistrationNo) : null; } }
    }

    public class FunctionalViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> RequestNo { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> PropertyTypeId { get; set; }
        public Nullable<int> ApproverId { get; set; }

        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo { get; set; }
        public string FloorArea { get; set; }
        public string Status { get; set; }
        public string SchemeName { get; set; }
        public string Department { get; set; }
        public string Applicant { get; set; }
        public string ApplicantType { get; set; }
        public string PropertyType { get; set; }
        public string Approver { get; set; }
        public string Comment { get; set; }
        public string MeterSeallingFile { get; set; }
        public string AffidavitFile { get; set; }
        public string RegisteredCertificateFile { get; set; }
        public string NOCAccountFile { get; set; }
        public string FunctionalStatus { get; set; }

        public Nullable<DateTime> FunctionalDueDate { get; set; }
        public Nullable<DateTime> FunctionalDate { get; set; }
        public Nullable<DateTime> CompletionDate { get; set; }
        public Nullable<DateTime> AffidavitDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ApprovalDate { get; set; }

        public Nullable<decimal> FunctionalCharge { get; set; }
        public Nullable<decimal> TotalArea { get; set; }

        public Nullable<bool> IsFunctional { get; set; }
        public Nullable<bool> IsMeterSealed { get; set; }
        public Nullable<bool> IsAffidavit { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        public Nullable<bool> IsNOCAccount { get; set; }
        public Nullable<bool> IsSelected { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
