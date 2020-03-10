using NoidaAuthority.PMS.Entities;
using System.Collections.Generic;

namespace NoidaAuthority.PMS.Repository.Admin
{
    public interface IManageRoleRepository
    {
        ManageRoles GetRoles();
        Role GetRoleById(int id);
        Role GetRoleByName(string roleName);
        int GetRoleIDForUser(string userName);
        Role GetRoleForUser(string userName);
        List<Role> GetRolesForDdl();
    }
}