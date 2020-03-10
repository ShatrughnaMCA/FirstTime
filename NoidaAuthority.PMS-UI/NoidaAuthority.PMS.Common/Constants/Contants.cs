using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Common
{
    public class Contants
    {
        public const string backOffice = "Back Office";
        public const string deptHead = "Department Head";
        public const string yes = "Yes";
        public const string no = "No";
        public const string transfer = "T";
        public const string mut = "M";
        public const int GPATransType = 2;
        public const int individual = 1;
        public const int company = 2;
        public const string male = "Male";
        public const string female = "Female";
        public const string select = "Select";

        public const string offline = "O";
        public const string manual = "M";

        public const string ReqComplete = "_Complete";
        public const string ReqObjection = "_Objection";

        public class GraphType
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
        public class SearchType
        {
            public const string Property = "Property";
            public const string Payment = "Payment";
            public const string Legal = "Legal";
            public const string Defaulter = "Defaulter";
        }

        public class PaymentType
        {
            public const string Paid = "Paid";
            public const string Defaulted = "Defaulted";
            public const string PartiallyPaid = "Partially Paid";
            public const string Due = "Due";
        }

        public class QueryString
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

        public enum Roles   
        {
            Administrator,
            Customer
        }

        public enum AllotmentStatus
        {
            NotSubmitted,
            InProgress,
            Approved,
            RefundInitiated,
            Rejected,
            Pending,
            Cancelled
        }

       public const string UserId = "033BE396-A045-4284-B958-9FD7CA8224F6";
       public const int AdminRoleId = 1;
       public const int CustomerRoleId = 2;
       public const int DeptRoleId = 4;
       public const int BackOfficeRoleId = 5;
       public const bool status = true;
       public const int RequestStatusCodeInitiated = 8;
       public const int RequestStatusCodeDraft = 1;

       public const string passwordMailSubject = "Noida Authority PMS Password";
       public const string rejectedMailSubject = "Noida Authority Form Rejection";
       public const string deactivatedMailSubject = "Noida Authority Account Deactivation Rejection";
       public const string copyrightText = "Copyright &copy; 2016 Noida Authority Developed And Maintained By North Shore Tecnologies (P) Ltd.";
       
        public const int trans = 1;
        //public const int trans2 = 2;
        //public const int trans3 = 3;
        ////public const int trans4 = 18;
        //public const int trans5 = 23;
        //public const int trans6 = 20;
        //public const int trans7 = 15;
        //public const int trans8 = 14;
        //public const int trans9 = 47;
        ////public const int trans10 = 65;
        //public const int trans11 = 68;
        //public const int trans12 = 69;
        //public const int trans13 = 70;
        ////public const int trans14 = 82;
        //public const int trans15 = 85;
        //public const int trans16 = 86;
        //public const int trans17 = 87;
        ////public const int trans18 = 98;
        //public const int trans19 = 100;
        //public const int trans20 = 101;
        //public const int trans21 = 102;
        //public const int trans22 = 113;

        public const int MortgageID = 4;
        //public const int MortgageID1=10;
        //public const int MortgageID2=11;
        //public const int MortgageID3=36;
        //public const int MortgageID4=37;
        //public const int MortgageID5=55;
        //public const int MortgageID6=56;
        //public const int MortgageID7=72;
        //public const int MortgageID8=89;
        //public const int MortgageID9=90;
        //public const int MortgageID10=104;
        //public const int MortgageID11=105;
        //public const int MortgageID12 = 12;

        public const int CICD = 3;
        //public const int CICD2 = 5;
        //public const int CICD3 = 6;
        //public const int CICD4 = 7;
        //public const int CICD5 = 8;
        //public const int CICD6 = 9;
        //public const int CICD7 = 29;
        //public const int CICD8 = 30;
        //public const int CICD9 = 31;
        //public const int CICD10 = 32;
        //public const int CICD11 = 33;
        //public const int CICD12 = 49;
        //public const int CICD13 = 50;
        //public const int CICD14 = 51;
        //public const int CICD15 = 52;
        //public const int CICD16 = 53;

        public const int ChangeInDirector = 1;
        public const int ChangeInFirmName = 2;
        public const int ChangeInFirmStatus = 3;
        public const int ChangeInProduct = 4;

        public const int mutation = 9;
        //public const int mutation2 = 18;
        //public const int mutation3 = 20;
        //public const int mutation4 = 15;
        //public const int mutation5 = 21; 
        public const int GPAModel = 8;

        public const int InProgress = 5;
        public const int ExtensionServiceRequest = 6;
        public const int RequestInitiated = 8;
        public enum MortgageType
        {
            [Description("Collateral")]
            Collateral = 1,
            [Description("Normal")]
            Normal = 2
        }

        public enum MortgagePrevLoan
        {
            [Description("Yes")]
            Yes = 1,
            [Description("No")]
            No = 2
        }

        public static class MortPrevLoan
        {
            public const int yes = 1;
            public const int no = 2;
            public const int invalid = 3;
        }

        public enum Gender
        {
            [Description("Male")]
            Male = 1,
            [Description("Female")]
            Female = 2,
            [Description("Company")]
            Company = 3
        }

        public static class Department
        {
            public const int Institutional = 1;
            public const int Commercial = 2;
            public const int Residential = 3;
            public const int Industrial = 4;
            public const int Housing = 5;
            public const int GroupHousing = 6;
             public const int Village = 7;
        }
    }

    public class RentService
    {
        public const int ServiceType1 = 2;
    }

    public class RequestStatus
    {
        public const int Approved = 1;
        public const int Rejected = 2;
        public const int Canceled = 3;
        public const int Pending = 4;
        public const int InProgress = 5;
        public const int Accepted = 6;
        public const int PendingPayment = 7;
        public const int Initiated = 1;
    }


    public enum EnumStatusType
    {
        No = 0,
        Yes = 1
    }
}
