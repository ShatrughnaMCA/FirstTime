using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPropertyLocation
    {
        public int PropertyID { get; set; }
        public string Sector { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public int RegistrationId { get; set; }
    }
}
