using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class CitizenServiceRequest
    {
        public string Registration_No { get; set; }
        public string Property_No { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ServiceId { get; set; }
        public DateTime Created_Date { get; set; }
        public int Created_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public int Modified_By { get; set; }
        public int Request_Status { get; set; }
        public string Description { get; set; }
        public int AmountTobePaid { get; set; }
        public DateTime? LastDateofPayment { get; set; }
        public string Comment { get; set; }
        public int ServiceRequestId { get; set; }
        public List<ServiceRequestDocument> DocumentList { get; set; }
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? DuesAmnt { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public MortgageModel MortgageDetails { get; set; }
        public TransferModel transDetails { get; set; }
        public CICModel CICmodel { get; set; }
        public MutationModel mutationDetails { get; set; }
        public RentingModel RentingModel { get; set; }
        public GPAModel gdaModel { get; set; }
        public ExtensionRequest ExtensionRequest { get; set; }
        public string Name { get; set; }
        public OtherRequests otherReq { get; set; }
        public string ReqType { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string RequesterName { get; set; }
        public string ApplicantAddress { get; set; }
        public string RequestorAddress { get; set; }
        public string SubDepartment { get; set; }
        public string ApplicantName { get; set; }
        public bool IsNull { get; set; }
    }
    public class ServiceRequestDocument
    {
        public int ChkDocumentId { get; set; }
        public string DocumentPath { get; set; }
        public DateTime Uploaded_Date { get; set; }
        public int Uploaded_By { get; set; }
        public string ChkDocumentName { get; set; }

    }

    public class NoidaServiceRequestModel
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string Area { get; set; }
        public string PropertyCategory { get; set; }
        public string CustomerAddress { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Mobile { get; set; }
        public string Status { get; set; }
        public string PropertyType { get; set; }
        public string PlotNumber { get; set; }
        public string PropertyNumber { get; set; }

        public int DepartmentId { get; set; }
        public int ServiceId { get; set; }
        public string Description { get; set; }
        public List<ServiceRequestDocument> DocumentList { get; set; }
    }

    public class CitizenRequestsModel
    {
        public string RId { get; set; }
        public int RefNo { get; set; }
        public DateTime? ReqDate { get; set; }
        public int? SLA { get; set; }
        public string ServiceName { get; set; }
        public int Id { get; set; }
        public int? ChallanId { get; set; }
        public decimal? DuesAmount { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class RequestDetails
    {
        public string RId { get; set; }
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ReqStatus { get; set; }
        public string Description { get; set; }
        public decimal? ServiceFee { get; set; }
        public decimal? DuesAmnt { get; set; }
        public string Comment { get; set; }
        public string PaymentStatus { get; set; }
    }
    public class CitizenPropertyDocument
    {
        public int RID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public string DocumentPath { get; set; }
    }

    public class MortgageModel
    {
        public int RegistrationID { get; set; }
        public Nullable<System.DateTime> MortgageDate { get; set; }
        [DisplayName("Sanctioned Amount")]
        public Nullable<decimal> SanctionedAmount { get; set; }
        [DisplayName("Bank Name")]
        public string BankName { get; set; }
        [DisplayName("Branch Address")]
        public string BranchAddress { get; set; }
        [DisplayName("Mortgage Type")]
        public string MortgageType { get; set; }
        public Nullable<System.DateTime> ValidUpto { get; set; }
        [DisplayName("Processing Fee")]
        public Nullable<decimal> ProcessingFee { get; set; }
        [DisplayName("Previous Loan Noc")]
        public Nullable<short> PreviousLoanNoc { get; set; }
    }

    public class TransferModel
    {
        public int TransType { get; set; }
        public int TransSubType { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal TransCharge { get; set; }
        public int Gender { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string RelativeName { get; set; }
        public string MotherName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string CorrespondenceAdd { get; set; }
        public string PermanentAdd { get; set; }
        public string PAN { get; set; }
        public int? Occupation { get; set; }
        public string TransfereeCompanySigningAuth { get; set; }
        public string TransfereeCompanyName { get; set; }
        public string TransfereeCompanyRegOff { get; set; }
        public int? TypeOfTransferee { get; set; }
    }

    public class CICModel
    {
        public int Id { get; set; }

        public int Director_Id { get; set; }

        public Nullable<int> Rid { get; set; }

        [DisplayName("Director Name")]
        public string Director_Name { get; set; }

        [DisplayName("Type")]
        public Nullable<int> Type { get; set; }

        public string TypeName { get; set; }

        [DisplayName("Director Share")]
        public Nullable<decimal> Director_Share { get; set; }

        [DisplayName("CIC Charge")]
        public Nullable<decimal> CIC_Charge { get; set; }

        public Nullable<int> Change_Type { get; set; }

        public string ChangeTypeName { get; set; }

        [DisplayName("Old Firm Name")]
        public string OldFirmName { get; set; }

        [DisplayName("New Firm Name")]
        public string NewFirmName { get; set; }

        [DisplayName("Old Firm Status")]
        public Nullable<int> OldFirmStatus { get; set; }

        public string OldFirmStatusName { get; set; }

        [DisplayName("New Firm Status")]
        public Nullable<int> NewFirmStatus { get; set; }

        [DisplayName("Old Firm Product")]
        public string OldFirmProduct { get; set; }

        [DisplayName("New Firm Product")]
        public string NewFirmProduct { get; set; }

        public string DepartmentName { get; set; }

        public string PropNo { get; set; }

        public int SNo { get; set; }

        public string User { get; set; }

        public Nullable<System.DateTime> Request_Date { get; set; }

        public Nullable<System.DateTime> Approved_Date { get; set; }

        public Nullable<int> Is_Active { get; set; }

        public Nullable<int> Created_By { get; set; }

        public Nullable<System.DateTime> Created_Date { get; set; }

        public Nullable<int> Modified_By { get; set; }

        public Nullable<System.DateTime> Modified_Date { get; set; }

        public string AssignTo { get; set; }

        public int? Status { get; set; }

        public string StatusName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string ApplicantName { get; set; }

        public string FatherName { get; set; }

        public string PropertyNumber { get; set; }

        public string Gender { get; set; }

        public string Comment { get; set; }

        public int SchemeId { get; set; }

        public int DepartmentId { get; set; }

        public int FormId { get; set; }

        public int applicationId { get; set; }

        public string MobileNumber { get; set; }

        public string Email { get; set; }

        public string IsFirmExists { get; set; }

        public string IsAllotted { get; set; }

    }

    public class RentingModel
    {
        public int RequestNo { get; set; }
        public Nullable<int> Rid { get; set; }
        public Nullable<bool> Permission { get; set; }     
        public string TenantName { get; set; }     
        public string TenantProject { get; set; }       
        public DateTime RequestDate { get; set; }   
        public Nullable<int> RentDuration { get; set; }
        public Nullable<decimal> RentingCharge { get; set; } 
        public Nullable<System.DateTime> RentingDate { get; set; }
        public Nullable<System.DateTime> RentingEndDate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string Comment { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> Modifiedby { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string PropertyType { get; set; }
        public int DepartmentId { get; set; }
       
        public string Department { get; set; }
        public string AllotteeName { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PropertyNo { get; set; }
        public decimal TotalDues { get; set; }
        public bool? Functional { get; set; }
        public string YesOrNo
        {
            get
            {
                if (Functional == true) { return "Yes"; }
                else { return "No"; }
            }
        }

        public string FatherOrHusbandName { get; set; }
        public string SigningAuthority { get; set; }
        public string GenderOrCompany { get; set; }
        public string TotalArea { get; set; }
        public string FloorNo { get; set; }
        public bool IsSelected { get; set; }
        public string RequestStatus { get; set; }


        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> SchemeTypeId { get; set; }
        public string SchemeName { get; set; }

        public bool IsRentingChargePaid { get; set; }
        public bool IsRentPermissionRequested { get; set; }
    }

    public class MutationModel
    {
        //Applicant Details
        public int ReqNo { get; set; }
        public int? RId { get; set; }
        public int DepttId { get; set; }
        public string DepttName { get; set; }
        public string PropNo { get; set; }
        public DateTime? ReqDate { get; set; }
        public DateTime? MutationDate { get; set; }
        public string Status { get; set; }
        public string SchemeName { get; set; }
        public string ApplicantName { get; set; }
        public string Gender { get; set; }
        public string RelationName { get; set; }
        public string PropType { get; set; }
        public int PropTypeId { get; set; }
        public decimal? Area { get; set; }
        public string Floor { get; set; }
        public int FloorId { get; set; }
        public string ApplicantAddress { get; set; }
        //Transfer Details
        public string TransfereeName { get; set; }
        public string TransfereeGender { get; set; }
        public string TransfereeRelationName { get; set; }
        public DateTime? TransferDate { get; set; }
        public string TransferType { get; set; }
        public int TransferTypeId { get; set; }
        public string TransfereeAddress { get; set; }
        //Mutation Details
        public DateTime? TransferDeedDate { get; set; }
        public string BahiNo { get; set; }
        public string BahiZildNo { get; set; }
        public string BahiPageNo { get; set; }
        public string SINo { get; set; }
        public int AssignTo { get; set; }
        //TODO: File Name 
        public string User { get; set; }
        public string From { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string Comment { get; set; }
        public string ApproverComments { get; set; }
        public string TransDeedFile { get; set; }
    }

    public class GPAModel
    {
        public int RequestNo { get; set; }
        public int? RId { get; set; }
        public int GPAId { get; set; }
        public string DepttName { get; set; }
        public int DepttId { get; set; }
        public string PropNo { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string RelationName { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public bool? IsActive { get; set; }
        public string GPAHolderName { get; set; }
        public string GPAHolderAdd { get; set; }
        public int? GPARegistered { get; set; }
        public string GPARegisteredNo { get; set; }
        public string GPAType { get; set; }
        public string NomineeName { get; set; }
        public string NomineeRelation { get; set; }
        public DateTime? NominationDate { get; set; }
        public int SNo { get; set; }
        public int NomineeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int hdnRId { get; set; } //Used when DDL is disabled in EditGPA
        public string hdnGPAType { get; set; } //Used when DDL is disabled in EditGPA
        public DateTime? AllotmentDate { get; set; }
        public DateTime? FromSearch { get; set; }
        public DateTime? ToSearch { get; set; }
        public string Desc { get; set; } //Description
    }

    public class ExtensionRequest
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public string PropertyNo { get; set; }
        public Nullable<System.DateTime> Completion_DueDate { get; set; }
        public Nullable<System.DateTime> Extension_Given_Date { get; set; }
        public Nullable<System.DateTime> Extension_Due_Date { get; set; }
        public Nullable<decimal> Extension_Charge { get; set; }
        public Nullable<int> Approved_By { get; set; }
        public Nullable<System.DateTime> Approved_Date { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> Is_Active { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string User { get; set; }
        public int? ScheduleActionDay { get; set; }
        public string SchemeName { get; set; }
        public string DepartmentName { get; set; }
        public string ApplicantName { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public string PropertyType { get; set; }
        public decimal? Area { get; set; }
        public string PropertyNumber { get; set; }
        public string Floor { get; set; }
        public DateTime? Leasedeeddate { get; set; }
        public DateTime? Possessiondate { get; set; }
        public string BuildingPlan { get; set; }
        public string StatusName { get; set; }
        public string AssignTo { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public bool ExtendableFlag { get; set; }
        public bool IsSelected { get; set; }
    }

    public class OtherRequests
    {
        public string Description { get; set; }
        public int? ServiceId { get; set; } 
    }

    public class ServiceReportModel
    {
        public int Id{get;set;}
        public string RegistrationNo{get;set;}
        public string ApplicantName{get;set;}
        public string ApplicantAddress { get; set; }
        public string RequestorName { get; set; }
        public string RequestorAddress { get; set; }
        public string Sector{get;set;}
        public string Block{get;set;}
        public string PropertyNo{get;set;}
        public string DepartmentName{get;set;}
        public int DepartmentId{get;set;}
        public int ServiceId{get;set;}
        public string ServiceName{get;set;}
        public string Description{get;set;}
        public DateTime CreatedDate{get;set;}
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public bool RequestEntered { get; set; }
        public string Status { get; set; }
        public string SubDepartment { get; set; }
        public string Signature { get; set; }
    }

    public class ServiceReportingModel{
        public int Id { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Registration_No { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string PRDVREGIST_APPLICANT_NAME { get; set; }
        public string SECTOR { get; set; }
        public string BLOCK { get; set; }
        public string PLDIPROPERTY_NO { get; set; }
        public string Description { get; set; }
        public DateTime Created_Date { get; set; }
    }

    public class LetterViewModel
    {
        public int Id { get; set; }
        public Nullable<int> Rid { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<int> RequestId { get; set; }
        public string RequestStatus { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo
        {
            get
            {
                return Sector + "/" + Block + "-" + PlotNo;
            }
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ApplicantName { get; set; }
        public string CorresspondentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string IndividualOrCompany { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string AutorizedSignatory { get; set; }
        public Nullable<int> LetterId { get; set; }
        public string LetterType { get; set; }
        public Nullable<DateTime> LetterDate { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string BarcodeValue { get; set; }
        public string LetterContent { get; set; }
    }
}

