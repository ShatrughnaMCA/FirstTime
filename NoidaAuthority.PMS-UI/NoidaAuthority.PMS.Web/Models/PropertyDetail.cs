using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoidaAuthority.PMS.Web.Models
{
    public class PropertyFilterViewModel
    {
        public Nullable<int> RegistrationId { get; set; }
        public Nullable<int> PropertyId { get; set; }
        public Nullable<int> PropertyTypeId { get; set; } 
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> AllotmentStatusId { get; set; }
        public Nullable<int> Year { get; set; }
        public Nullable<int> LegalTypeId { get; set; }
        public Nullable<int> PaymentStatusId { get; set; } // 1 : Paid ,2:Due 3:Defaulted
        public Nullable<int> ComplaintStatusId { get; set; }
        public Nullable<int> PaymentScheduleId { get; set; }

        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string PropertyNo { get; set; }
        public string PropertyType { get; set; }        
        public string AllotteeName { get; set; }       
        public string SearchType { get; set; }
        public string MonthOrYear { get; set; }        
        public string SelectedQuarter { get; set; }      
        public string UserName { get; set; }
    }

    public class PropertyDetail
    {
        public int RID { get; set; }
        public int PropertyId { get; set; }
        public int RegistrationId { get; set; }
        public int FloorAreaRatio { get; set; }
        public int NoOfInstallments { get; set; }
        public int ConstructionPeriodAllowed { get; set; }

        public double RatePerSqMtr { get; set; }
        public double TotalAmountDue { get; set; }
        public double TotalAmountPaid { get; set; }

        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string PlotNo { get; set; }
        public string Scheme { get; set; }
        public string Area { get; set; }       
        public string PropertyCategory { get; set; }        
        public string AlottmentDate { get; set; }
        public string RegistryDate { get; set; }
        public string PossessionDate { get; set; }
        public string PaymentDue { get; set; }
        public string Location { get; set; }
        public string CustomerAddress { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public string Mobile { get; set; }
        public string CoApplicantName { get; set; }
        public string ApplicantRelationShip { get; set; }
        public string CoApplicantAddress { get; set; }
        public string PaymentSchedule { get; set; }
        public string Status { get; set; }
        public string PropertyType { get; set; }
        public string LegalPreceedings { get; set; }
        public string NoDuesCertificateIssued { get; set; }
        public string BuildingPlanApproved { get; set; }
        public string ConstructionCompleted { get; set; }
        public string FunctionalDate { get; set; }
        public string RentPermission { get; set; }
        public string MortgagePermission { get; set; }
        public string OnetimeLeaseRentPaid { get; set; }
        public string TransferCase { get; set; }
        public string OtherDue { get; set; }
        public string JalConnection { get; set; }
        public string ExtensionGiven { get; set; }
        public string QuotaAlotment { get; set; }
        public string LastInspectionDate { get; set; }
        public string SubLeasingReq { get; set; }
        public string NoOfSubleased { get; set; }      
        public string PlotNumber { get; set; }
        public string PropertyNumber { get; set; }
        public string LastPaymentDate { get; set; }
        public string NextPaymentDueDate { get; set; }
        public string Allotment_Doc { get; set; }
        public string Leasedeed_Doc { get; set; }
        public string BuildingPlan_Doc { get; set; }
        public string No_Dues_Doc { get; set; }
        public string Completion_Doc { get; set; }       
    }
}