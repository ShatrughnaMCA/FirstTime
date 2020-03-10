using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Repository.Entities;

namespace NoidaAuthority.PMS.Repository.Admin
{
    public class ManageRoleRepository : IManageRoleRepository
    {
        public ManageRoles GetRoles()
        {
            var result = new ManageRoles();
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {

                    var roles = (from Role rl in dbContext.Roles.ToList()
                                 select new Role
                                 {
                                     RoleName = rl.RoleName,
                                     RoleId = rl.RoleId,
                                     CreatedBy = rl.CreatedBy,
                                     CreatedOn = rl.CreatedOn,
                                     ModifiedBy = rl.ModifiedBy,
                                     ModifiedOn = rl.ModifiedOn,
                                 }).ToList();

                    result.RoleList = roles;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public List<Role> GetRolesForDdl()
        {
            var roles = new List<Role>();
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {

                    roles = (from Role rl in dbContext.Roles.ToList()
                             select new Role
                             {
                                 RoleName = rl.RoleName,
                                 RoleId = rl.RoleId,
                                 CreatedBy = rl.CreatedBy,
                                 CreatedOn = rl.CreatedOn,
                                 ModifiedBy = rl.ModifiedBy,
                                 ModifiedOn = rl.ModifiedOn,
                             }).ToList();


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            roles = roles.Where(x => x.RoleName.ToLower() != "customer" && x.RoleName.ToLower() != "back office").ToList();
            return roles;
        }

        public Role GetRoleById(int id)
        {
            var result = new Role();
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {

                    var role = (from Role rl in dbContext.Roles.ToList()
                                where rl.RoleId == id
                                select new Role
                                {
                                    RoleName = rl.RoleName,
                                    RoleId = rl.RoleId,
                                    CreatedBy = rl.CreatedBy,
                                    CreatedOn = rl.CreatedOn,
                                    ModifiedBy = rl.ModifiedBy,
                                    ModifiedOn = rl.ModifiedOn,
                                }).FirstOrDefault();

                    result = role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Role GetRoleByName(string roleName)
        {
            var result = new Role();
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {

                    var role = (from Role rl in dbContext.Roles.ToList()
                                where String.Equals(rl.RoleName, roleName, StringComparison.CurrentCultureIgnoreCase)
                                select new Role
                                {
                                    RoleName = rl.RoleName,
                                    RoleId = rl.RoleId,
                                    CreatedBy = rl.CreatedBy,
                                    CreatedOn = rl.CreatedOn,
                                    ModifiedBy = rl.ModifiedBy,
                                    ModifiedOn = rl.ModifiedOn,
                                }).FirstOrDefault();

                    result = role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public int GetRoleIDForUser(string username)
        {
            int roleID = 0;
            if (string.IsNullOrEmpty(username))
            {
                return 0;
            }
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {
                    User user = null;

                    var role = (from users in dbContext.Users
                                join selRole in dbContext.Roles on users.RoleId equals selRole.RoleId
                                where users.UserName == username
                                select selRole.RoleId).FirstOrDefault();

                    roleID = role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roleID;
        }

        public Role GetRoleForUser(string userName)
        {
            return GetRoleById(GetRoleIDForUser(userName));
        }
    }
}
