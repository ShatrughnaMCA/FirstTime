using System;
using System.Collections.Generic;
using System.Linq;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Repository.Entities;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Common;
using System.Text;
using NoidaAuthority.PMS.Entity;
//using NoidaAuthority.PMS.Repository.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Configuration;
using System.IO;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Repository.Context;

namespace NoidaAuthority.PMS.Repository.ManageUsers
{
    public class ManageUsersRepository : IManageUsersRepository
    {
        public IEnumerable<UserModel> GetUsers()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
               var result = (from lstUsers in dbContext.CustomerMsts.ToList()
                          join lstRoles in dbContext.UmRoleMasters.ToList() on lstUsers.RoleId equals lstRoles.RoleId
                          select new UserModel
                          {
                              //UserId = lstUsers.UserId,
                              UserName = lstUsers.UserName,
                              Pasword = lstUsers.Password,
                              FirstName = lstUsers.FirstName,
                              LastName = lstUsers.LastName,
                              MobileNo = lstUsers.MobileNo,
                              Status = lstUsers.IsActive,
                              PropertyId = lstUsers.PropertyId,
                              IsLockedOut = lstUsers.IsLocked.Value,
                              CreatedOn = lstUsers.CreatedDate.Value,
                              //CreatedBy = lstUsers.CreatedBy,
                              //ModifiedBy = lstUsers.ModifiedBy,
                              ModifiedOn = lstUsers.ModifiedDate,
                              RoleId = lstUsers.RoleId,
                              RoleName = lstRoles.RoleName,
                              LastPasswordChangeDate = lstUsers.PasswordChangeDate,
                              FullName = lstUsers.FirstName + " " + lstUsers.LastName,
                              CustomerIdFileName = lstUsers.IdFileName == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + lstUsers.PropertyId + "/" + lstUsers.IdFileName,
                              CustomerIdFileType = lstUsers.IdFileType,
                              AuthorityLetter = lstUsers.PropertyFileName == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + lstUsers.PropertyId + "/" + lstUsers.PropertyFileName,
                              AuthorityLetterType = lstUsers.PropertyFileType,
                              DeptId = lstUsers.DepartmentId,
                              IsRejected = lstUsers.StatusId == NAServiceStatusId.Rejected ? true : false,
                              Remarks = lstUsers.Remarks,
                              IsFirstTimeActivated = lstUsers.IsFirstTimeActivated,
                              UserEmail = lstUsers.Email
                          }).OrderBy(x => x.UserName).ToList();
               return result;
            }           
        }

        public Boolean DeactivateUser(string email)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var existingUser = dbContext.CustomerMsts.Where(us => us.UserName.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.IsActive = false;
                    dbContext.SaveChanges();
                    if (existingUser.Email != null)
                    {
                        EmailHelper emailHelper = new EmailHelper();
                        emailHelper.Send(existingUser.Email, Contants.deactivatedMailSubject, "Dear User,<br><br>Your account has been deactivated. <br> Regards, http://mynoida.in");
                    }
                    //Send SMS
                    var msg = "Dear User, Your account has been deactivated. Regards, http://mynoida.in";
                    //mobileNo = "9891140331";//Hard coded for Testing purposes
                    SMSSend(existingUser.MobileNo, msg);
                    flag = true;
                }
            }
            return flag;
        }

        public Boolean RejectCustomer(string email, string mobileNo, string remarks)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var existingUser = dbContext.CustomerMsts.Where(us => us.UserName.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
                if (existingUser != null)
                {
                    existingUser.StatusId = NAServiceStatusId.Rejected;
                    existingUser.Remarks = remarks;
                    dbContext.SaveChanges();
                    if (existingUser.Email != null)
                    {
                        EmailHelper emailHelper = new EmailHelper();
                        emailHelper.Send(existingUser.Email, Contants.rejectedMailSubject, "Dear User,<br><br>Due to " + remarks + " Your Application has been rejected. <br> Regards, http://mynoida.in");
                    }
                    //Send SMS
                    var msg = "Dear User, Due to " + remarks + " Your Application has been rejected. Regards, http://mynoida.in";
                    //mobileNo = "9891140331";//Hard coded for Testing purposes
                    SMSSend(mobileNo, msg);
                    flag = true;
                }
            }
            return flag;
        }



        private string CreatePassword()
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int length = 8;
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public NaUser GetUserById(Guid uId)
        {
            var result = new NaUser();
            //try
            //{
            //    using (var dbContext = new PIMSEntitiesContext())
            //    {
            //        var user = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserId == uId);

            //        if (user != null)
            //        {
            //            result = new NaUser
            //            {
            //                UserId = user.UserId,
            //                FirstName = user.FirstName,
            //                LastName = user.LastName,
            //                Password = user.Pasword,
            //                Email = user.UserName,
            //                Status = user.Status.Value,
            //                PropertyId = user.PropertyId,
            //                IsLockedOut = user.IsLockedOut,
            //                CreatedOn = user.CreatedOn,
            //                CreatedBy = user.CreatedBy,
            //                RoleId = user.RoleId,
            //                MobileNo = user.MobileNo

            //            };
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return result;
        }

        public bool AddEditUser(NaUser customer)
        {
            var flag = false;
            var user = new CustomerMst();
            var isNew = false;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    if (customer.UserId == new Guid()) // in case of new user
                    {
                        //if (customer.City != null && customer.State != 0)
                        //    customer.RoleId = ConstantEntities.CustomerRoleId;
                        //else

                        //    customer.RoleId = ConstantEntities.SpecialAdminRoleId;
                        customer.Status = true;//customer.Status;
                        customer.CreatedOn = DateTime.Now;
                        customer.UserId = Guid.NewGuid();


                        //customer.Email = customer.Email;
                        // customer.Pasword = customer.Pasword;

                        isNew = true;
                    }
                    else // in case of edit user
                    {
                        //user = dbContext.CustomerMsts.FirstOrDefault(us => us.UserId == customer.UserId);
                        customer.ModifiedOn = DateTime.Now;
                        customer.ModifiedBy = customer.CreatedBy;
                        customer.CreatedOn = user.CreatedDate.Value;
                        //customer.CreatedBy = user.UserName;
                    }

                    //user = MapCustomerToUser(customer, user);

                    if (isNew) // in case of new user
                    {
                        //Updated changes for bug #5985
                        // Restrict adding duplicate users
                        var objCustomer = dbContext.CustomerMsts.FirstOrDefault(cond => cond.UserName.Trim().ToLower() == customer.Email.Trim().ToLower());
                        if (objCustomer == null)
                            dbContext.CustomerMsts.Add(user);
                    }
                    dbContext.SaveChanges();
                    flag = true;

                }


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }




        //added by Shatrughna
        //update registered customer
        public bool AddEditNAcustomer(NoidaAuthority.PMS.Entity.NaUser.NAcustomer customer)
        {
            var flag = false;
            //NoidaAuthority.PMS.Repository.Entities.NAcustomer ctxCustomer = new NoidaAuthority.PMS.Repository.Entities.NAcustomer();
            //User ctxCustomer = new User();
            CustomerMst ctxCustomer = new CustomerMst();
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    var existingCustomer = dbContext.CustomerMsts.FirstOrDefault(us => us.PropertyId == customer.RegistrationId);
                    if (existingCustomer != null)
                    {
                        existingCustomer.ModifiedDate = DateTime.Now;
                        //existingCustomer.ModifiedBy = customer.CreatedBy;
                        //existingCustomer.Email = customer.Email;
                        existingCustomer.MobileNo = customer.MobileNo;

                        existingCustomer.PropertyId = customer.RegistrationId;
                        existingCustomer.FirstName = customer.FirstName;
                        existingCustomer.LastName = customer.LastName;
                        //ctxCustomer.LastName = existingCustomer.LastName;
                        //ctxCustomer.IsLockedOut = existingCustomer.IsLockedOut;
                        existingCustomer.CreatedBy = ctxCustomer.UserName;
                        existingCustomer.UserName = customer.UserNameEmail;

                        //existingCustomer.EmailId = customer.Email;
                        //ctxCustomer.Pasword = "password123";

                        //creating password
                        var newPassword = "password123";
                        existingCustomer.Password = newPassword.ToMD5HashForPassword();

                        existingCustomer.RoleId = 2;
                        //ctxCustomer.Email = customer.Email;
                        existingCustomer.IdFileName = customer.CustomerIdFileName;
                        existingCustomer.IdFileType = customer.CustomerIdFiletype;
                        existingCustomer.PropertyFileName = customer.AuthorityLetter;
                        existingCustomer.PropertyFileType = customer.AuthorityLetterType;
                        //ctxCustomer.CreatedBy = customer.CreatedBy;
                        //ctxCustomer.PropertyNo = customer.PropertyNo;
                        existingCustomer.IsActive = false;
                        existingCustomer.Sector = customer.Sector;
                        existingCustomer.Block = customer.Block;
                        existingCustomer.PlotNo = customer.PropertyNo;
                        //existingCustomer.IsRejected = false;
                        existingCustomer.Remarks = "";
                        existingCustomer.IsFirstTimeActivated = false;
                        existingCustomer.Email = customer.Email;
                        existingCustomer.SecurityQuestion = customer.SecurityQuestion;
                        existingCustomer.SecurityAnswer = customer.SecurityAnswer;
                    }
                    else
                    {
                        //ctxCustomer.UserId = Guid.NewGuid();
                        ctxCustomer.PropertyId = customer.RegistrationId;
                        ctxCustomer.FirstName = customer.FirstName;
                        ctxCustomer.LastName = customer.LastName;
                        //ctxCustomer.LastName = existingCustomer.LastName;
                        //ctxCustomer.IsLockedOut = existingCustomer.IsLockedOut;
                        ctxCustomer.CreatedDate = DateTime.Now;
                        ctxCustomer.CreatedBy = ctxCustomer.UserName;
                        ctxCustomer.UserName = customer.UserNameEmail;
                        //ctxCustomer.Pasword = "password123";

                        //creating password
                        var newPassword = "password123";
                        ctxCustomer.Password = newPassword.ToMD5HashForPassword();

                        ctxCustomer.RoleId = 2;
                        //ctxCustomer.Email = customer.Email;
                        ctxCustomer.MobileNo = customer.MobileNo;
                        ctxCustomer.IdFileName = customer.CustomerIdFileName;
                        ctxCustomer.IdFileType = customer.CustomerIdFiletype;
                        ctxCustomer.PropertyFileName = customer.AuthorityLetter;
                        ctxCustomer.PropertyFileType = customer.AuthorityLetterType;
                        //ctxCustomer.CreatedBy = customer.CreatedBy;
                        //ctxCustomer.CreatedOn = DateTime.Now;
                        //ctxCustomer.PropertyNo = customer.PropertyNo;
                        ctxCustomer.StatusId = NAServiceStatusId.Initiated;
                        ctxCustomer.Sector = customer.Sector;
                        ctxCustomer.Block = customer.Block;
                        ctxCustomer.PlotNo = customer.PropertyNo;
                        //ctxCustomer.IsRejected = false;
                        ctxCustomer.Remarks = "";
                        ctxCustomer.IsFirstTimeActivated = false;
                        ctxCustomer.Email = customer.Email;
                        ctxCustomer.SecurityQuestion = customer.SecurityQuestion;
                        ctxCustomer.SecurityAnswer = customer.SecurityAnswer;
                        dbContext.CustomerMsts.Add(ctxCustomer);
                        //var msg = "Your Registration Process has been completed successfully. Thanks for registering with us. Now Authority will verify your detail and you will get the confirmation on your registered Email Id and Mobile Number.";
                        var msg1 = "Dear User, Registration Process has been completed successfully.";
                        if (customer.Email != null)
                        {
                            EmailHelper emailHelper = new EmailHelper();
                            emailHelper.Send(customer.Email, "Registration Completed", msg1 + "Regards, http://mynoida.in");
                        }
                        //Send SMS
                        SMSSend(customer.MobileNo, msg1);
                        //Sending mail to From Address on new Registration, as asked by Vishal Shukla to do so
                        var emailAdd = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                        var msg2 = "Hi,<br><br>A new User has registered with resgistration no. " + ctxCustomer.PropertyId + " . Please check and reply.";
                        if (!string.IsNullOrEmpty(emailAdd))
                        {
                            EmailHelper emailHelper = new EmailHelper();
                            emailHelper.Send(emailAdd, "Registration Completed", msg2 + "<br><br>Regards, http://mynoida.in");
                        }
                    }
                    dbContext.SaveChanges();
                    flag = true;

                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        private User MapCustomerToUser(NaUser customer, User user)
        {
            // var user =  new User();

            user.FirstName = customer.FirstName;
            user.LastName = customer.LastName;
            user.Pasword = customer.Password != null ? customer.Password : user.Pasword;// for edit mode
            user.UserName = customer.Email != null ? customer.Email : user.UserName;// for edit mode
            user.UserId = customer.UserId;
            user.PropertyId = customer.PropertyId;
            user.Status = customer.Status;
            user.MobileNo = customer.MobileNo;

            user.IsLockedOut = customer.IsLockedOut;
            user.CreatedOn = customer.CreatedOn != null ? customer.CreatedOn : DateTime.Now;
            user.CreatedBy = customer.CreatedBy != null ? customer.CreatedBy : user.CreatedBy;// for edit mode
            user.RoleId = customer.RoleId != null ? customer.RoleId : user.RoleId;// for edit mode
            user.DeptId = customer.DeptId;

            return user;
        }


        public Boolean sendOTPtoUser(string mobNo, int r)
        {
            var result = false;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    //var phoneNo = dbContext.view_property.Where(us => us.PRDVREGISTRATION_ID.ToLower() == id.ToLower()).Select(us => us.mobile_no).FirstOrDefault();
                    //phoneNo = "8010079321";//Hard coded for Testing purposes
                    if (mobNo != null)
                    {
                        var msgs = "Dear User, Your OTP is " + r + " DO NOT disclose this OTP to anyone. This is for online use by you only. Regards, http://mynoida.in";
                        //var msg = "Dear User, Your OTP is " + r + " DO NOT disclose this to anyone by any means.";
                        SMSSend(mobNo, msgs);
                        result = true;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private void SMSSend(string mobileNo, string msg)
        {
            WebClient client = new WebClient();
            string baseurl = ConfigurationManager.AppSettings["SMSsend"].ToString() + ConfigurationManager.AppSettings["SMSUsername"].ToString() + "&password=" + ConfigurationManager.AppSettings["SMSPassword"].ToString() + "&sendername=" + ConfigurationManager.AppSettings["SMSSenderID"].ToString() + "&mobileno=" + mobileNo + "&message=" + msg;
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
        }

        public Boolean ResetPassword(string emailID)
        {
            var flag = true;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var existingUser = dbContext.CustomerMsts.Where(us => us.UserName.ToLower() == emailID.ToLower() && us.IsActive == true).FirstOrDefault();
                if (existingUser != null)
                {
                    var newPassword = CreatePassword();
                    existingUser.Password = newPassword.ToMD5HashForPassword();
                    dbContext.SaveChanges();
                    flag = true;
                    //Send Email
                    if (existingUser.Email != null && existingUser.Email != "")
                    {
                        var body = "Dear User,<br><br>You have successfully changed your password. Your new password is " + newPassword + ". DO NOT disclose this to anyone by any means.<br>Regards, http://mynoida.in";
                        EmailHelper emailHelper = new EmailHelper();
                        emailHelper.Send(existingUser.Email, "New Generated Password", body);
                    }
                    var msg = "Dear User, You have successfully changed your password. Your new password is " + newPassword + ". Regards, http://mynoida.in";
                    //SMSSend(existingUser.MobileNo, msg);
                    ApplicationHelper.SendSMS(existingUser.MobileNo, msg);
                }
                else
                {
                    flag = false; //Case when Email ID does not exist in our DB (User does not exist or is not Active).
                }
            }
            return flag;
        }


        //Lock/UnLock Customer
        public Boolean LockUnLockCustomer(string email)
        {
            Boolean IsLockedStatus = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var user = dbContext.CustomerMsts.Where(us => us.UserName.ToLower().Trim() == email.ToLower().Trim()).FirstOrDefault();
                if (user != null)
                {
                    user.IsLocked = user.IsLocked != true ? true : false;
                    IsLockedStatus = user.IsLocked.Value;
                    dbContext.SaveChanges();
                }
            }
            return IsLockedStatus;
        }

        public int SaveNoidaEmployee(UserViewModel model)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                CustomerMst user = new CustomerMst();
                //user.UserId = Guid.NewGuid();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.MobileNo = model.MobileNo;
                user.Password = model.Password.ToMD5HashForPassword();
                user.Email = model.Email;
                user.RoleId = model.RoleId;
                user.DepartmentId = model.DepartmentId;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = user.UserName;
                user.UserName = model.UserName;

                dbContext.CustomerMsts.Add(user);
                dbContext.SaveChanges();
                flag = ReturnType.Saved;
            }
            return flag;
        }

        public int IsUserNameExist(string userName)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var IsExist = dbContext.CustomerMsts.Where(us => us.UserName.ToLower().Trim() == userName.ToLower().Trim()).FirstOrDefault();
                if (IsExist != null)
                {
                    flag = ReturnType.Exist;
                }
                else
                {
                    flag = ReturnType.NotExist;
                }
            }
            return flag;
        }

        public int SaveNACustomerDetails(UserViewModel model, HttpPostedFileBase fileI, HttpPostedFileBase fileII)
        {
            var flag = ReturnType.None;
            CustomerMst user = new CustomerMst();
            try
            {
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId);
                }
                if (fileI != null)
                {
                    var documentPath = HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileI.FileName;
                    var extension = Path.GetExtension(documentPath);
                    model.CustomerIdName = model.CustomerIdType + extension;
                }
                if (fileII != null)
                {
                    var documentPath = HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileII.FileName;
                    var extension = Path.GetExtension(documentPath);
                    model.NALetterName = model.NALetterType + extension;
                } 
                using (var dbContext = new PIMSEntitiesContext())
                {
                    //user.UserId = Guid.NewGuid();
                    user.PropertyId = model.RegistrationId.ToString();
                    user.RegistrationId = model.RegistrationId;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.UserName = model.UserName;
                    var newPassword = "password123";
                    user.Password = newPassword.ToMD5HashForPassword();
                    user.DepartmentId = model.DepartmentId;
                    user.RoleId = NAConstant.CustomerRoleId;
                    user.Sector = model.Sector;
                    user.Block = model.Block;
                    user.PlotNo = model.PlotNo;
                    user.Email = model.Email;
                    user.MobileNo = model.MobileNo;
                    user.SecurityQuestion = model.SecurityQuestion;
                    user.SecurityAnswer = model.SecurityAnswer;
                    user.IdFileName = fileI != null ? model.CustomerIdName : string.Empty; // model.CustomerIdName;
                    user.IdFileType = model.CustomerIdType;
                    user.PropertyFileName = fileII != null ? model.NALetterName : string.Empty; // model.NALetterName;
                    user.PropertyFileType = model.NALetterType;
                    user.CreatedDate = DateTime.Now;
                    user.CreatedBy = user.UserName;
                    user.StatusId = NAServiceStatusId.Initiated;
                    user.IsActive = false;
                    user.Remarks = "";
                    user.IsFirstTimeActivated = false;
                    dbContext.CustomerMsts.Add(user);
                    dbContext.SaveChanges();

                    {
                        if (fileI != null)
                        {
                            fileI.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + model.CustomerIdName);
                        }
                        if (fileII != null)
                        {
                            fileII.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + model.NALetterName);
                        }                            
                    }

                    //if (!Directory.Exists(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId))
                    //{
                    //    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId);
                    //    if (fileI != null)
                    //        fileI.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileI.FileName);
                    //    if (fileII != null)
                    //        fileII.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileII.FileName);
                    //}
                    //else
                    //{
                    //    if (fileI != null)
                    //        fileI.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileI.FileName);
                    //    if (fileII != null)
                    //        fileII.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fileII.FileName);
                    //}

                    flag = ReturnType.Saved;

                    //var msg = "Your Registration Process has been completed successfully. Thanks for registering with us. Now Authority will verify your detail and you will get the confirmation on your registered Email Id and Mobile Number.";
                    var msg1 = "Dear User, Registration Process has been completed successfully.";
                    if (model.MobileNo != null)
                    {
                        ApplicationHelper.SendSMS(model.MobileNo, msg1);
                    }
                    if (model.Email != null)
                    {
                        ApplicationHelper.SendEmail(model.Email, "Registration Completed", msg1 + "Regards, http://mynoida.in");
                    }
                    

                    var emailAdd = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
                    if (!string.IsNullOrEmpty(emailAdd))
                    {
                        var msg2 = "Hi,<br><br>A new User has registered with resgistration no. " + user.PropertyId + " . Please check and reply.";
                        ApplicationHelper.SendEmail(emailAdd, "Registration Completed", msg2 + "<br><br>Regards, http://mynoida.in");
                    }                   
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return flag;
        }

        public PropertyViewModel ValidateRegistrationId(PropertyViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var user = dbContext.CustomerMsts.FirstOrDefault(u => u.UserName == model.RegistrationId.ToString());
                if (user == null)
                {
                    var alotment = dbContext.AllotmentMasters.FirstOrDefault(c => c.rid == model.RegistrationId);
                    if (alotment != null)
                    {
                        var property = (from allotment in dbContext.AllotmentMasters
                                        join properties in dbContext.SchemePropTrans on allotment.propertyId equals properties.propertyId
                                        where allotment.rid==model.RegistrationId
                                        select properties).FirstOrDefault();
                        model.SectorId = property.sectorId;
                        model.SectorName = property.SectorMst.sectorName;
                        model.BlockId = property.blockId;
                        model.BlockName = property.BlockMst.blockName;
                        model.PlotNo = property.propertyNo;
                        model.ActionTypeId = ReturnType.Allotted;
                        model.DepartmentId = property.departmentId;
                    }
                    else
                    {
                        //var sqlConnection = NADbConnection.GetPISConnection();
                        //string sqlQuery = "Select * from PropertyMaster where PRDVREGISTRATION_ID="+model.RegistrationId;
                        //SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
                        model.ActionTypeId = ReturnType.NotAllotted;
                    }
                }
                else
                {
                    model.ActionTypeId = ReturnType.Exist;
                }
                return model;
            }
        }


        public DataSourceResult GetRegisteredEmployeeList(DataSourceRequest request, UserViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var result = (from user in dbContext.CustomerMsts
                              //join role in dbContext.UmRoleMasters on user.RoleId equals role.RoleId
                              where user.RoleId != NAConstant.CustomerRoleId && (model.UserName == null || user.UserName == model.UserName)
                              select new UserViewModel
                              {
                                  //UserId = user.UserId,
                                  UserName = user.UserName,
                                  Password = user.Password,
                                  DepartmentId = user.DepartmentId,
                                  FirstName = user.FirstName,
                                  LastName = user.LastName,
                                  FullName = user.FirstName + " " + user.LastName,
                                  MobileNo = user.MobileNo,
                                  Email = user.Email,
                                  RoleId = user.RoleId,
                                  RoleName = user.RoleId == 0 ? "Super Admin" : (user.RoleId == 1 ? "Admin" : (user.RoleId == 2 ? "Allottee" : "Other")),
                                  StatusId = user.StatusId,
                                  StatusName = dbContext.StatusMasters.FirstOrDefault(m => m.Id == user.StatusId).Status,
                                  IsActive = user.IsActive,
                                  PropertyId = user.PropertyId,
                                  IsLocked = user.IsLocked,
                                  IsRejected = user.StatusId == NAServiceStatusId.Rejected ? true : false,
                                  IsFirstTimeActivated = user.IsFirstTimeActivated,
                                  CreatedOn = user.CreatedDate,
                                  //CreatedBy = user.CreatedBy,
                                  //ModifiedBy = user.ModifiedBy,
                                  ModifiedOn = user.ModifiedDate,                                 
                                  LastPasswordChangeDate = user.PasswordChangeDate,                                
                                  CustomerIdName = user.IdFileName,// == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + user.PropertyId + "/" + user.CustomerIdFileName,
                                  CustomerIdType = user.IdFileType,
                                  NALetterName = user.PropertyFileName,// == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + user.PropertyId + "/" + user.CustomerLetterFileName,
                                  NALetterType = user.PropertyFileType,
                                  SecurityQuestion = user.SecurityQuestion,
                                  SecurityAnswer = user.SecurityAnswer,
                                  Remarks = user.Remarks
                              });
                return result.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetRegisteredCustomerList(DataSourceRequest request, UserViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                string documentPath = ConfigurationManager.AppSettings["DocumentFilesPath"].ToString();
                var list = (from user in dbContext.CustomerMsts
                            //join role in dbContext.UmRoleMasters on user.RoleId equals role.RoleId
                            where user.RoleId == NAConstant.CustomerRoleId && (model.UserName == null || user.UserName == model.UserName)
                            select new UserViewModel
                            {
                                //UserId = user.UserId,
                                PropertyId = user.PropertyId,
                                DepartmentId = user.DepartmentId,
                                UserName = user.UserName,
                                Password = user.Password,
                                RoleId = user.RoleId,
                                RoleName = user.RoleId == 0 ? "Super Admin" : (user.RoleId == 1 ? "Admin" : (user.RoleId==2 ? "Allottee" : "Other")),
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                MobileNo = user.MobileNo,
                                Email = user.Email,
                                LastPasswordChangeDate = user.PasswordChangeDate,
                                FullName = user.FirstName + " " + user.LastName,

                                StatusId = user.StatusId,
                                StatusName=dbContext.StatusMasters.FirstOrDefault(m=>m.Id==user.StatusId).Status,
                                IsActive = user.IsActive,
                                IsLocked = user.IsLocked,
                                IsRejected = user.StatusId == NAServiceStatusId.Rejected ? true : false,
                                IsFirstTimeActivated = user.IsFirstTimeActivated,

                                CreatedOn = user.CreatedDate,
                                //CreatedBy = user.CreatedBy,
                                //ModifiedBy = user.ModifiedBy,
                                ModifiedOn = user.ModifiedDate,

                                CustomerIdName = user.IdFileName,// == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + user.PropertyId + "/" + user.CustomerIdFileName,
                                CustomerIdType = user.IdFileType,
                                CustomerIdPath = documentPath + user.PropertyId + "/"+ user.IdFileName,
                                NALetterName = user.PropertyFileName,// == null ? string.Empty : ConfigurationManager.AppSettings["DocumentFilesPath"].ToString() + user.PropertyId + "/" + user.CustomerLetterFileName,
                                NALetterType = user.PropertyFileType,
                                NALetterPath = documentPath + user.PropertyId + "/" + user.PropertyFileName,
                                SecurityQuestion = user.SecurityQuestion,
                                SecurityAnswer = user.SecurityAnswer,
                                Remarks = user.Remarks
                            });
                return list.ToDataSourceResult(request);
            }
        }

        public int TakeActionOnUserByType(ActionViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int flag = ReturnType.None;
                var user = dbContext.CustomerMsts.Where(u => u.UserName == model.UserName).FirstOrDefault();
                if (model.ActionType == NAConstant.LockUnlock)
                {
                    user.IsLocked = user.IsLocked != true ? true : false;
                    dbContext.SaveChanges();
                    flag = user.IsLocked == true ? ReturnType.Locked : ReturnType.Unlock;
                }
                else if (model.ActionType == NAConstant.ActiveDeactive)
                {
                    user.IsActive = user.IsActive != true ? true : false;
                    if (user.IsActive == false)
                    {
                        user.StatusId = NAServiceStatusId.Canceled;
                    }
                    else
                    {
                        user.StatusId = NAServiceStatusId.Approved;
                    }
                    dbContext.SaveChanges();
                    flag = user.IsActive == true ? ReturnType.Active : ReturnType.NotActive;
                }
                else if (model.ActionType == NAConstant.Reject)
                {
                    user.StatusId = NAServiceStatusId.Rejected;
                    user.IsActive = false;
                    user.Remarks = model.Message;
                    dbContext.SaveChanges();
                    flag = ReturnType.Rejected;
                }
                return flag;
            }
        }

        public int SendPassword(ActionViewModel model)
        {
            int flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var newPassword = ApplicationHelper.CreatePassword();
                var ExUser = dbContext.CustomerMsts.Where(us => us.UserName == model.UserName).FirstOrDefault();
                if (ExUser != null)
                {
                    ExUser.Password = newPassword.ToMD5HashForPassword();
                    ExUser.IsActive = true;
                    ExUser.StatusId = 1;
                    if (ExUser.IsFirstTimeActivated == false || ExUser.IsFirstTimeActivated == null)
                    {
                        ExUser.IsFirstTimeActivated = true;
                    }
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;

                    //Send SMS
                    if (ExUser.MobileNo != null && ExUser.MobileNo != "")
                    {
                        var msg = "Dear User, You are successfully registered with mynoida.in. Your user name is " + ExUser.UserName + " and password is " + newPassword + " Regards, http://mynoida.in";
                        ApplicationHelper.SendSMS(ExUser.MobileNo, msg);
                    }
                    //Send Email
                    //var body = "Hi,<br><br>Your password for Noida Authority PMS is ";
                    var body = "Dear User, You are successfully registered with mynoida.in. Your user name is " + ExUser.UserName + " and password is " + newPassword + " Regards, http://mynoida.in";
                    if (ExUser.Email != "" && ExUser.Email != null)
                    {
                        ApplicationHelper.SendEmail(ExUser.Email, "Generated Password", body);
                    }              
                }  
            }
            return flag;
        }
    }
}