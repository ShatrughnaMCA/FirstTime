using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Repository.Admin;
using System.Collections.Generic;

namespace NoidaAuthority.PMS.Service.Admin
{
    public class ManageRoleService : IManageRoleService
    {
        IManageRoleRepository _manageRoleRepository = null;

        public ManageRoleService()
        {
            _manageRoleRepository = new ManageRoleRepository();
        }
        public ManageRoles GetRoles()
        {
            var result = _manageRoleRepository.GetRoles();
            return result;
        }

        public int GetRoleIdForUser(string userName)
        {
            var result = _manageRoleRepository.GetRoleIDForUser(userName);
            return result;
        }

        public Role GetRoleForUser(string userName)
        {
            var result = _manageRoleRepository.GetRoleForUser(userName);
            return result;
        }

        public List<Role> GetRolesForDdl()
        {
            var result = _manageRoleRepository.GetRolesForDdl();
            return result;
        }
    }
}