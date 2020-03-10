using System;
using System.Collections.Generic;
using NoidaAuthority.PMS.Entities;

namespace NoidaAuthority.PMS.Service.Admin
{
    public interface IMenuMappingService
    {
        List<int> GetallMenuIdsForRole(Int32 roleId);
        List<int> GetallMenuIdsForUser(string userName);
        List<Menu> GetallMenusForRole(int roleId);
        bool SetMenuSecurityForRole(IEnumerable<Menu> menuSecurityAttributes, string roleId); 
    }
}