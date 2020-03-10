using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPropertyDetails : BaseEntity
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string RegistrationNo { get; set; }
        public string Sector { get; set; }
        public string Block { get; set; }
        public string Scheme { get; set; }
        public string Area { get; set; }
        public double RatePerSqMtr { get; set; }
        public string PropertyCategory { get; set; }
        public int FloorAreaRatio { get; set; }
        public DateTime? AlottmentDate { get; set; }
        public string ResistryDate { get; set; }
        public DateTime? PossessionDate { get; set; }
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
        public int RegistrationId { get; set; }
        public string PlotNumber { get; set; }
        public string PropertyNumber { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDueDate { get; set; }
        public string Allotment_Doc { get; set; }
        public string Leasedeed_Doc { get; set; }
        public string BuildingPlan_Doc { get; set; }
        public string No_Dues_Doc { get; set; }
        public string Completion_Doc { get; set; }
        public int ConstructionPeriodAllowed { get; set; }
        public int? DepttId { get; set; }
        public bool isRIdExists { get; set; }
    }

    public class NotificationDetails
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
    }
}
