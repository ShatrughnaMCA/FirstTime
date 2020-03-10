using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity.NaUser
{
    public class DtoNAcustomerDetails
    {
        public System.Guid CustomerId { get; set; }
        public string RegistrationId { get; set; }
        public string AllotteeName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string CustomerIdFileName { get; set; }
        public string AuthorityLetter { get; set; }
        public string PropertyNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
