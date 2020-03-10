using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
   public class PropertyDocument :BaseEntity
    {
        public string Name { get; set; }
        public string DocumentType { get; set; }
        public double? PropertyId { get; set; }
        public string Url { get; set; }
        public int? RegistrationId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
    }
}
