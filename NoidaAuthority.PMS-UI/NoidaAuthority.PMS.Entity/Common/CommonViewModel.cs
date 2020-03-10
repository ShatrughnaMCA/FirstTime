using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DropdownViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string FilterType { get; set; }
        public string ActionType { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> BankId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> SchemeId { get; set; }
        public Nullable<int> SectorId { get; set; }
        public Nullable<int> BlockId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ReceiptHeadId { get; set; }
    }

    public class ActionViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> RequestId { get; set; }
        public Nullable<int> OTP { get; set; }
        public Nullable<int> ActionTypeId { get; set; }
        public Nullable<int> ReturnTypeId { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string ActionType { get; set; }
        public string ReturnType { get; set; }
        public string MobileNo { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string ServiceStatus { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsLocked { get; set; }
    }

    public class PropertyFilterViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> PropertyTypeId { get; set; } 
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> LegalTypeId { get; set; }
        public Nullable<int> AllotmentStatusId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; } // 1 : Paid ,2:Due 3:Defaulted
        public Nullable<int> ComplaintStatusId { get; set; }
        public Nullable<int> PaymentScheduleId { get; set; }
        public string PropertyNo { get; set; }
        public string Block { get; set; }
        public string Sector { get; set; }
        public string PlotNo { get; set; }
        public string PropertyType { get; set; }        
        public string AllotteeName { get; set; }       
        public string SearchType { get; set; }
        public string MonYear { get; set; }      
        public string SelectedQuarter { get; set; }       
        public string Username { get; set; }
    }

    public class OnlinePaymentViewModel
    {
        public Nullable<int> Id { get; set; }
        public Nullable<int> BankId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public Nullable<int> StatusId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> ApplicationFormId { get; set; }
        public Nullable<int> TransactionStatusId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<int> AccountHeadId { get; set; }
        public Nullable<int> AccountSubHeadId { get; set; }
        public Nullable<int> OnlineRequestId { get; set; }
        public Nullable<int> ChallanId { get; set; }
        public Nullable<int> ActionTypeId { get; set; }
        public Nullable<int> ReturnTypeId { get; set; }
        public string TransactionKey { get; set; }
        public string TransactionId { get; set; }
        public string ProductInfo { get; set; }
        public string Udf1 { get; set; }
        public string Udf2 { get; set; }
        public string Udf3 { get; set; }
        public string Udf4 { get; set; }
        public string Udf5 { get; set; }
        public string Mihpayid { get; set; }
        public string Mode { get; set; }
        public string TransactionStatus { get; set; }
        public string Status { get; set; }
        public string Applicant { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string AddressI { get; set; }
        public string AddressII { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zipcode { get; set; }
        public string PaymentSource { get; set; }
        public string PaymentGatewayType { get; set; }
        public string BankReferenceNo { get; set; }
        public string BankName { get; set; }
        public string BranchAddress { get; set; }
        public string IFSCCode { get; set; }
        public string GatewayName { get; set; }
        public string AccountNo { get; set; }
        public string VirtualAccountNo { get; set; }
        public string BankCode { get; set; }
        public string Error { get; set; }
        public string ErrorMessage { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CardHash { get; set; }
        public string IssuingBank { get; set; }
        public string CardType { get; set; }
        public string DocumentPath { get; set; }
        public string AccountHead { get; set; }
        public string AccountSubHead { get; set; }
        public string ServiceName { get; set; }
        public string Department { get; set; }
        public string PaymentType { get; set; }
        public string ActionType { get; set; }
        public string ReturnType { get; set; }
        public string HtmlContent { get; set; }

        public Nullable<bool> IsOnlinePaid { get; set; }
        public Nullable<bool> IsOfflinePaid { get; set; }
        public Nullable<bool> IsChallanExist { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> GSTInPercent { get; set; }
        public Nullable<decimal> GSTAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }

        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<DateTime> EntryDate { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public Nullable<DateTime> ChallanDate { get; set; }
        //public OnlineFormViewModel FormModel { get; set; }
        //public string EncryptedFormId { get { return CommonHelper.Encode(ApplicationFormId.ToString()); } }
    }
}
