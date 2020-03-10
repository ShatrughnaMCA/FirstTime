using NoidaAuthority.PMS.Entities;
using System.Collections.Generic;

namespace NoidaAuthority.PMS.Service.Admin
{
    public interface IManageRoleService
    {
        ManageRoles GetRoles();
        int GetRoleIdForUser(string roleName);
        Role GetRoleForUser(string userName);
        List<Role> GetRolesForDdl();
    }
}