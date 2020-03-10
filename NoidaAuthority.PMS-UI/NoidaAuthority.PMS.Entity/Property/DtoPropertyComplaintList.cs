using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPropertyComplaintList
    {
        public double? Complaint_No { get; set; }
        public string Nature_Subject { get; set; }
        public string Department { get; set; }
        public string PropertyNo { get; set; }
        public string Complaint_Location { get; set; }
        public string Citizen_Name { get; set; }
        public double? Citizen_Mobile { get; set; }
        public DateTime? Submission_Date { get; set; }
        public DateTime? Exp_Delivery_Date { get; set; }
        public string Disposal_Status { get; set; }
        public string Citizen_Verification_Status { get; set; }
        public int? RegistrationId { get; set; }
    }
}
