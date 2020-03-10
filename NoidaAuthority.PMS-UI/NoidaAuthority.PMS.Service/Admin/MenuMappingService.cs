using System.Collections.Generic;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Repository.Admin;

namespace NoidaAuthority.PMS.Service.Admin
{
    public class MenuMappingService : IMenuMappingService
    {
        IMenuMappingRepository menuMappingRepository = null;
        public MenuMappingService()
        {
            menuMappingRepository = new MenuMappingRepository();
        }

        public List<int> GetallMenuIdsForRole(int roleId)
        {
            return menuMappingRepository.GetallMenuIdsForRole(roleId);
        }

        public List<int> GetallMenuIdsForUser(string userName)
        {
            ManageRoleService roleService = new ManageRoleService();
            int roleId = roleService.GetRoleIdForUser(userName);
            return GetallMenuIdsForRole(roleId);
        }

        public List<Menu> GetallMenusForRole(int roleId)
        {
            return menuMappingRepository.GetAllMenuForRole(roleId);
        }

        public bool SetMenuSecurityForRole(IEnumerable<Menu> menuSecurityAttributes, string roleId)
        {
            return menuMappingRepository.SetMenuSecurityForRole(menuSecurityAttributes, roleId);

        }
    }
}