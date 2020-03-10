using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using NoidaAuthority.PMS.Common.EmailTemplate;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Repository.ManageUsers;
using System.Web.Hosting;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Entity;
using System.Web;
using Kendo.Mvc.UI;

namespace NoidaAuthority.PMS.Service.ManageUsers
{
    public class ManageUserService : IManageUserService
    {
        IManageUsersRepository usersRepository = null;

        /// <summary>
        /// Cunstructor
        /// </summary>
        public ManageUserService()
        {
            usersRepository = new ManageUsersRepository();
        }

        /// <summary>
        /// usersRepository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<UserModel> GetUsers()
        {
            return usersRepository.GetUsers();
        }

        /// <summary>
        /// GetUserById
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public NaUser GetUserById(Guid uId)
        {
            return usersRepository.GetUserById(uId);
        }

        public Boolean DeactivateUser(string email)
        {
            return usersRepository.DeactivateUser(email);
        }

        public Boolean RejectCustomer(string email, string mobileNo, string remarks)
        {
            return usersRepository.RejectCustomer(email, mobileNo, remarks);
        }

        

        public Boolean ResetPassword(string emailID)
        {
            return usersRepository.ResetPassword(emailID);
        }

        /// <summary>
        /// AddEditUser
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="isWelcomeEmail">Id parameter is Set to True in that case Welcome email will be sent in cae of Customer</param>
        /// <returns></returns>
        public bool AddEditUser(NaUser customer)
        {
            var result = false;

            try
            {

                if (customer.UserId == new Guid())
                {

                    if (!string.IsNullOrEmpty(customer.Password))
                    {
                        var passwd = customer.Password;
                        customer.Password = customer.Password.ToMD5HashForPassword();
                        customer.IsLockedOut = false;
                        //add user
                        result = usersRepository.AddEditUser(customer);

                        //Email will be sent  later once user click on Send Welcome email From edit customer screen 

                        //if (result && customer.RoleId != ConstantEntities.CustomerRoleId)
                        {
                            var emailLink = ConfigurationSettings.AppSettings["NewUserEmailPath"].ToString();
                            Hashtable templateVars = new Hashtable();
                            templateVars.Add("LoginURL", emailLink);
                            templateVars.Add("UserName", customer.Email);
                            templateVars.Add("Password", passwd);
                            Parser parser = null;
                            parser = new Parser(HostingEnvironment.MapPath("~/MailTemplates/RegisterAdmin.html"), templateVars);

                            var emailBody = parser.Parse();
                            EmailHelper emailHelper = new EmailHelper();
                            emailHelper.Send("cspl7970@gmail.com", "test", emailBody);


                            // string.Format(ConstantEntities.NewCustomerEmailBody, emailLink, customer.Email, pasword);
                            //var mailer = new SendRelianceEmail();

                            //send email with email queqe
                            //mailer.SendEmail(customer.Email, string.Empty, ConstantEntities.NewCustomerEmailSubject, emailBody);
                        }
                    }
                    else
                    {
                        throw new Exception(message: " User not Created");
                    }
                }
                {
                    result = usersRepository.AddEditUser(customer);
                }

                //}
            }

            catch (Exception ex)
            {

                throw;
            }
            return result;
        }

        //added by Shatrughna
        //update registered customer details 
        public bool AddEditNAcustomer(NAcustomer customer)
        {
            var result = false;
            try
            {
                result = usersRepository.AddEditNAcustomer(customer);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public bool sendOTPtoUser(string id, int r)
        {
            var result = false;
            try
            {
                result = usersRepository.sendOTPtoUser(id, r);
            }
            catch (Exception ex)
            {
                throw;
            }
            return result;
        }

        public Boolean LockUnLockCustomer(string email)
        {
            return usersRepository.LockUnLockCustomer(email);
        }



        public int SaveNoidaEmployee(Entity.UserViewModel model)
        {
            return usersRepository.SaveNoidaEmployee(model);
        }


        public int IsUserNameExist(string userName)
        {
            return usersRepository.IsUserNameExist(userName);
        }


        public int SaveNACustomerDetails(Entity.UserViewModel model, HttpPostedFileBase fileI, HttpPostedFileBase fileII)
        {
            return usersRepository.SaveNACustomerDetails(model,fileI,fileII);
        }


        public PropertyViewModel ValidateRegistrationId(PropertyViewModel model)
        {
            return usersRepository.ValidateRegistrationId(model);
        }


        public DataSourceResult GetRegisteredCustomerList(DataSourceRequest request, UserViewModel model)
        {
            return usersRepository.GetRegisteredCustomerList(request, model);
        }


        public int TakeActionOnUserByType(ActionViewModel model)
        {
            return usersRepository.TakeActionOnUserByType(model);
        }

        public int SendPassword(ActionViewModel model)
        {
            return usersRepository.SendPassword(model);
        }

        public DataSourceResult GetRegisteredEmployeeList(DataSourceRequest request, UserViewModel model)
        {
            return usersRepository.GetRegisteredEmployeeList(request,model);
        }
    }
}