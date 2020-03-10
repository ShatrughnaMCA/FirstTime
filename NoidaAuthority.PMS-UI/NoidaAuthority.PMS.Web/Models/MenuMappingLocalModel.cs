using NoidaAuthority.PMS.Entities;
using System.Collections.Generic;

namespace NoidaAuthority.PMS.Web.Models
{
    public class MenuMappingLocalModel
    {
        public string RoleId { get; set; }
        public List<Menu> AllMenus { get; set; }
        public string Menus { get; set; }
    }
}