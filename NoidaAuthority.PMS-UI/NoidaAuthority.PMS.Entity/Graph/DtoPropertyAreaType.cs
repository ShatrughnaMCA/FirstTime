using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class DtoPropertyAreaType : BaseEntity
    {
        public string Areasqm { get; set; }
        public string PropertyType { get; set; }
        public int? PropertyAreaTypeCount { get; set; }
    }
}
