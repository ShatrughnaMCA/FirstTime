using System;
using System.Linq;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Repository.Entities;
using NoidaAuthority.PMS.Repository.Context;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Repository.Login
{
    public class LoginRepository : ILoginRepository
    {
        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUser(String userName, String password)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName && cond.Pasword == password);

                    if (user != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return flag;
        }

        /// <summary>
        /// Get user Details
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUserDetails(String userName)
        {
            User user = null;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return user;
        }


        public Boolean ChangePassword(String userName, String newPassword)
        {
            var flag = false;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName == userName);
                    if (user != null)
                    {
                        user.Password = newPassword;
                        user.PasswordChangeDate = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public Boolean CheckEmailAddressExists(String emailAddress)
       {
            var flag = false;
            //try
            //{
                using (var dbContext = new NOIDANEWEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName.ToLower() == emailAddress);
                    if (user != null)
                    {
                        flag = true;
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return flag;
        }

        //added by Shatrughna
        public Boolean CheckEmailAddressForNAcustomer(String emailAddress)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var customer = dbContext.Users.FirstOrDefault(cond => cond.UserName.ToLower() == emailAddress.ToLower() && (cond.IsRejected == false || cond.IsRejected == null));
                    if (customer != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public Boolean CheckRegistationIDforNAcustomer(String RegistrationId)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var customer = dbContext.Users.FirstOrDefault(cond => cond.PropertyId.ToLower() == RegistrationId.ToLower() && (cond.IsRejected == false || cond.IsRejected == null));
                    if (customer != null)
                    {
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public Boolean LockUser(String emailAddress)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == emailAddress);
                    if (user != null)
                    {
                        user.IsLockedOut = true;
                        dbContext.SaveChanges();
                        flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }


        public Boolean RegisterUser(NaUser customer)
        {

            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var user = new User
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName,
                        Pasword = customer.Password,
                        UserName = customer.Email,
                        PropertyId = customer.PropertyId,
                        Status = true,
                        IsLockedOut = false,
                        CreatedOn = DateTime.Now,
                        CreatedBy = customer.CreatedBy,

                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }


        public int ValidateUser(Entity.UserViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int flag = ReturnType.None;
                var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName == model.UserName && cond.Password == model.Password);
                if (user != null)
                {
                    if (user.StatusId == NAServiceStatusId.Locked) flag = ReturnType.Locked;
                    else if(user.StatusId == NAServiceStatusId.Initiated) flag = ReturnType.NotActive;
                    else if (user.StatusId == NAServiceStatusId.Rejected) flag = ReturnType.Rejected;
                    else if (user.StatusId == NAServiceStatusId.Approved && user.IsActive == true) flag = ReturnType.Success;
                }
                return flag;
            }
        }


        public UserViewModel GetAuthorityUserDetail(UserViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName == model.UserName);
                if (user != null)
                {
                    model.Id = user.Id;
                    //model.UserId = user.UserId;
                    model.RegistrationId = user.RegistrationId != null ? user.RegistrationId : ( user.PropertyId != null ? Convert.ToInt32(user.PropertyId) : 0 );
                    model.PropertyId = user.PropertyId;
                    model.UserName = user.UserName;
                    model.Password = user.Password;
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                    model.FullName = string.IsNullOrEmpty(user.LastName) ? user.FirstName : (user.FirstName + " " + user.LastName);
                    model.Sector = user.Sector;
                    model.Block = user.Block;
                    model.PlotNo = user.PlotNo;
                    model.MobileNo = user.MobileNo;
                    model.Email = user.Email;
                    model.StatusId = user.StatusId;
                    model.IsActive = user.IsActive;
                    model.IsLocked = user.IsLocked;
                    model.SecurityQuestion = user.SecurityQuestion;
                    model.SecurityAnswer = user.SecurityAnswer;
                    model.DepartmentId = user.DepartmentId;
                    model.Department = string.Empty;
                    model.RoleId = user.RoleId;
                    model.RoleName = user.RoleId == 2 ? "Allottee" : (user.RoleId == 1 ? "Admin" : "Super Admin");
                }
                return model;
            }
        }


        public int LockApplicationUser(UserViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int flag = ReturnType.None;
                var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName == model.UserName);
                if (user != null)
                {
                    user.IsLocked = true;
                    dbContext.SaveChanges();
                    flag = ReturnType.Locked;
                }
                return flag;
            }
        }


        public UserViewModel GetUserDetailsByUserName(string userName)
        {
            UserViewModel customer = new UserViewModel();
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName == userName);
                    customer.Password = user.Password;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return customer;
        }
    }
}