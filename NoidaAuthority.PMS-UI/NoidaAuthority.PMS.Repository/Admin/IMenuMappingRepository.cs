using System;
using System.Collections.Generic;
using NoidaAuthority.PMS.Entities;

namespace NoidaAuthority.PMS.Repository.Admin
{
    public interface IMenuMappingRepository
    {
        List<int> GetallMenuIdsForRole(Int32 roleId);
        List<Menu> GetAllMenuForRole(int roleId);
        bool SetMenuSecurityForRole(IEnumerable<Menu> menuSecurityAttributes, string roleId);
    }
}