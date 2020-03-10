using System;
using System.Collections.Generic;
using System.Linq;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Repository.Entities;

namespace NoidaAuthority.PMS.Repository.Admin
{
    public class MenuMappingRepository : IMenuMappingRepository
    {
        public List<Menu> GetAllMenuForRole(int roleId)
        {
            var menuSecurityAttributes = new List<Menu>();
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {

                    var allmenus = dbContext.Menus.ToList();
                    var rolemenuMapping = dbContext.Role_Menu_Mapping.Where(map => map.RoleId == roleId).ToList();
                    foreach (Menu menues in allmenus)
                    {
                        var menuSecurity = new Menu();
                        menuSecurity = menues;
                        foreach (var menuMapping in rolemenuMapping)
                        {
                            if (Convert.ToInt16(menuMapping.MenuId) == menues.MenuId)
                            {
                                menuSecurity.IsSelected = true;
                            }
                        }
                        menuSecurityAttributes.Add(menuSecurity);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return menuSecurityAttributes;
        }


        public bool SetMenuSecurityForRole(IEnumerable<Menu> menuSecurityAttributes, string roleId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    if (!String.IsNullOrEmpty(roleId))
                    {
                        var intRoleId = Convert.ToInt32(roleId);
                        var userMenuMapping = dbContext.Role_Menu_Mapping.Where(cond => cond.RoleId == intRoleId).ToList();
                        var menusToAdd = menuSecurityAttributes.Where(cond => cond.IsSelected == true).ToList();
                        foreach (var usermenuToDelete in userMenuMapping)
                        {
                            try
                            {
                                dbContext.Role_Menu_Mapping.Remove(usermenuToDelete);
                                dbContext.SaveChanges();
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                        foreach (var usermenuToAdd in menusToAdd)
                        {
                            var roleLevelMenu = new Role_Menu_Mapping();
                            roleLevelMenu.MenuId = usermenuToAdd.MenuId;
                            roleLevelMenu.RoleId = intRoleId;
                            roleLevelMenu.CreatedOn = DateTime.Now;

                            dbContext.Role_Menu_Mapping.Add(roleLevelMenu);
                            dbContext.SaveChanges();
                        }
                    }

                    flag = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return flag;
        }
        public List<int> GetallMenuIdsForRole(Int32 roleId)
        {
            try
            {
                var allMenuIdsForRole = new List<int>();
                using (var dbContext = new NOIDANEWEntities())
                {
                    allMenuIdsForRole = (from data in dbContext.Role_Menu_Mapping where data.RoleId == roleId select data.MenuId).ToList();
                }
                return allMenuIdsForRole;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}

