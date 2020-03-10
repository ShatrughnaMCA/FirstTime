using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class FeedbackModel
    {
        public int id { get; set; }
        public string feedback { get; set; }
        public string registrationID { get; set; }
        public int status { get; set; }
        public string customerName { get; set; }
        public DateTime date { get; set; }
    }

    public class ServiceRequestsModel
    {
        public int id { get; set; }
        public string dept { get; set; }
        public string service { get; set; }
        public string custName { get; set; }
        public string regNo { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
    }

    public class RemarksModel
    {
        public int id { get; set; }
        public string regNo { get; set; }
        public string remarks { get; set; }
        public DateTime? date { get; set; }
    }
}
