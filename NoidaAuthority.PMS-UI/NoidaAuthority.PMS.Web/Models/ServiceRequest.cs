using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoidaAuthority.PMS.Web.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public string Registration_No { get; set; }
        public string Property_No { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> ServiceId { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<int> Created_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<int> Request_Status { get; set; }
        public string Description { get; set; }
    }
}