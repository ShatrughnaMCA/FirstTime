using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Common
{
    public class NAConstant
    {
        public const int SuperAdminRoleId = 0;
        public const int AdminRoleId = 1;
        public const int CustomerRoleId = 2;
        public const int DepartmentRoleId = 4;
        public const int BackOfficeRoleId = 5;
        public const int IndividualId = 1;
        public const int CompanyId = 2;
        public const int Active = 1;
       

        public const string BackOffice = "Back Office";
        public const string DepartmentHead = "Department Head";
        public const string Yes = "Yes";
        public const string No = "No";
        public const string Yes_Y = "Y";
        public const string No_N = "N";
        public const string Transfer = "T";
        public const string Mutation = "M";
        public const string Male = "Male";
        public const string Female = "Female";
        public const string Company = "Company";
        public const string Individual = "Individual";
        public const string Select = "Select";
        public const string Offline = "O";
        public const string Manual = "M";
        //public const string Other = "Other";
        public const string Reschedule = "R";//Used in DB for Rescheduled Payments

        public const string UserId = "033BE396-A045-4284-B958-9FD7CA8224F6";

        public const string PasswordMailSubject = "Noida Authority PMS Password";
        public const string RejectedMailSubject = "Noida Authority Form Rejection";
        public const string DeactivatedMailSubject = "Noida Authority Account Deactivation Rejection";
        public const string CopyrightText = "Copyright &copy; 2016 Noida Authority Developed And Maintained By North Shore Tecnologies (P) Ltd.";

        public const string JSKService = "O";
        public const string SDService = "P";
        public const string CustomerService = "C";
        public const string Registered = "Registered";
        public const string UnRegistered = "UnRegistered";
        public const string PradhikaranDiwas = "PradhikaranDiwas";
        public const string Other = "Other";

        public const string IdType = "IdType";
        public const string LetterType = "LetterType";
        public const string LockUnlock = "LockUnlock";
        public const string ActiveDeactive = "ActiveDeactive";
        public const string Reject = "Reject";
        public const string Success = "Success";
        public const string Failed = "Failed";

        public const string OTP = "OTP";

        public const bool True = true;
        public const bool IsActive = true;

        public const string NoidaAuthorityGSTNo = "09AAALN0120A1ZV";
        public const string ChallanBankCopy = "Bank Copy";
        public const string ChallanAuthorityCopy = "Authority Copy";
        public const string ChallanAllotteeCopy = "Allottee Copy";

        public const int OnlinePayment = 1;
        public const int OnlineOtherPayments = 2;
        public const int OfflineChallanPayment = 3;
        public const int OfflinePrevoiusChallanPayment = 4;
        public const int NICPayment = 5;
    }

    public static class SubDepartment
    {
        public const string P = "Property";
        public const string A = "Account";
        public const string Property = "Property";
        public const string Account = "Account";
    }

    //public class NAServiceId
    //{
    //    public const int Transfer = 1;
    //    public const int Renting = 2;
    //    public const int CIC = 3;
    //    public const int Mortgage = 4;
    //    public const int Mutation = 9;
    //    public const int GPA = 8;
    //    public const int Extension = 6;

    //    public const int ChangeInDirector = 1;
    //    public const int ChangeInFirmName = 2;
    //    public const int ChangeInFirmStatus = 3;
    //    public const int ChangeInProduct = 4;
    //}

    public static class NAServiceId
    {
        public const int Transfer = 1;
        public const int Rent = 2;
        public const int CIC = 3;
        public const int Mortgage = 4;
        public const int LeaseDeed = 5;
        public const int Extension = 6;
        public const int Amalgmation = 7;
        public const int Deamalgmation = 7;
        public const int GPA = 8;
        public const int Mutation = 9;
        public const int Functional = 10;
        public const int CoAllottee = 11;
        public const int DuesCalculation = 12;
        public const int NDC = 13;
        public const int Other = 21;
        public const int MapService = 29;
        public const int Completion = 28;


        public const int ChangeInDirector = 1;
        public const int ChangeInFirmName = 2;
        public const int ChangeInFirmStatus = 3;
        public const int ChangeInProduct = 4;
    }

    public static class NADepartmentId
    {
        public const int Institutional = 1;
        public const int Commercial = 2;
        public const int Residential = 3;
        public const int Industrial = 4;
        public const int Housing = 5;
        public const int GroupHousing = 6;
        public const int Village = 7;
    }

    public static class NAStatusId
    {

        public const int Approved = 1;
        public const int Rejected = 2;
        public const int Cancelled = 3;
        public const int Pending = 4;
        public const int InProgress = 5;
        public const int Accepted = 6;
        public const int PendingPayment = 7;
        public const int Initiated = 8;
        public const int Completed = 9;
        public const int Objection = 10;
        public const int Validated = 11;
        public const int CompletedOnSpot = 12;
        public const int Forwarded = 14;
        public const int CancelAfterTransfer = 15;
        public const int Closed = 16;
        public const int InProcess = 17;
        public const int Resubmitted = 18;
        public const int Withdraw = 19;

        public const int Active = 1;
        public const int Submitted = 1;
        public const int NotActive = 0;
        public const int NotSubmitted = 0;
        public const int Allotted = 1;

        public const int Yes = 1;
        public const int No = 2;
        public const int Invalid = 3;

        public const int Success = 1;
        public const int Failed = 0;
    }

    public class NAServiceStatusId
    {
        public const int Approved = 1;
        public const int Rejected = 2;
        public const int Canceled = 3;
        public const int Pending = 4;
        public const int InProgress = 5;
        public const int Accepted = 6;
        public const int PendingPayment = 7;
        public const int Initiated = 8;
        public const int Completed = 9;
        public const int Locked = 10;

        public const int Yes = 1;
        public const int No = 2;
        public const int Invalid = 3;
    }

    public static class ActiveStatusId
    {
        public const int Yes = 1;
        public const int No = 0;
    }

    public class NAGraph
    {
        public const string PropertyType = "GET_PROPERTY_TYPE";
        public const string PropertyArea = "GET_PROPERTY_AREA";
        public const string PropertyStatus = "GET_ALLOTMENT_STATUS";
        public const string PropertyRevenue = "GET_PROPERTY_REVENUE";
        public const string PropertyTypeArea = "GET_PROPERTY_TYPE_AREA";
        public const string PropertyRevenueTrendGraph = "REVENUE_TREND_GRAPH";
        public const string PropertyComplaintStatusGraph = "GET_PROPERTY_COMPLAINT_GRAPH";
        public const string PropertyLegalGraph = "GET_LEGAL_GRAPH";
        public const string PropertyRTIGraph = "GET_RTI_GRAPH";
    }
    public class NASearch
    {
        public const string Property = "Property";
        public const string Payment = "Payment";
        public const string Legal = "Legal";
        public const string Defaulter = "Defaulter";
    }

    public class NAPayment
    {
        public const string Paid = "Paid";
        public const string Defaulted = "Defaulted";
        public const string PartiallyPaid = "Partially Paid";
        public const string Due = "Due";
        public const string ProcessCharge = "ProcessingCharge";
        public const string ServiceCharge = "ServiceCharge";
    }

    public class NAQueryString
    {
        public const string PropertyType = "pt";
        public const string PropertyArea = "pa";
        public const string PropertyStatus = "ps";
        public const string Year = "yr";
        public const string Revenue = "rn";
        public const string Sector = "sc";
        public const string Block = "bk";
        public const string PropertyNumber = "pn";
        public const string PropertyID = "id";
        public const string AllotteeName = "an";
        public const string PlotNumber = "pln";
        public const string SearchType = "st";
        public const string PaymentStatus = "pts";
        public const string SelectedQuarter = "qtr";
        public const string DeptId = "DeptId";

    }

    public static class ReturnType
    {
        public const int None = 0;
        public const int Success = 1;
        public const int Failure = 2;
        public const int Exist = 3;
        public const int NotExist = 4;
        public const int Saved = 5;
        public const int Updated = 6;
        public const int Removed = 7;
        public const int Failed = 8;
        public const int UserNameNotExist = 8;
        public const int PasswordNotExist = 8;
        public const int NotPaid = 9;
        public const int Paid = 10;
        public const int Allotted = 11;
        public const int NotAllotted = 12;
        public const int Locked = 13;
        public const int Rejected = 14;
        public const int Resend = 15;
        public const int Forwarded = 16;
        public const int Unlock = 17;
        public const int Active = 18;
        public const int NotActive = 19;
        
        public const int Other = 20;
        public const int Cancelled = 21;
        public const int Mismatch = 22;
        public const int Verified = 23;
        public const int Rescheduled = 24;
        public const int Completed = 25;
        public const int Pending = 26;
        public const int NotRegistered = 27;
        public const int NotAvailable = 28;
    }

    public class NACategoryType
    {
        public const string CIC = "CIC";
        public const string GPA = "GPA";
        public const string Merge = "Merge";
        public const string Scheme = "Scheme";
        public const string Lease = "Lease";
        public const string Status = "Status";
        public const string FirmStatus = "Firm Status";
        public const string Director = "Director";
        public const string Cancellation = "Cancellation";
        public const string Application = "Application";
        public const string CompanyType = "CompanyType";
    }
}
