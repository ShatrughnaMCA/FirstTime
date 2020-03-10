using System;
using NoidaAuthority.PMS.Repository.Login;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Service.Login
{
    public class LoginService : ILogin
    {
        private ILoginRepository loginRepo = null;
        public LoginService()
        {
            loginRepo = new LoginRepository();
            //SendRelianceEmail sendReliance = new SendRelianceEmail();
            // sendReliance.SendEmail("smangla@svam.com",string.Empty,"THis is Test Email","this is Test  body");
        }

        public bool ValidateUser(string userName, string password)
        {
            return loginRepo.ValidateUser(userName, password);
        }

        public Entities.User GetUserDetails(string email)
        {
            return loginRepo.GetUserDetails(email);
        }


        public Boolean ChangePassword(String userEmail, String newPassword)
        {
            return loginRepo.ChangePassword(userEmail, newPassword);
        }

        public Boolean CheckEmailAddressExists(String emailAddress)
        {
            return loginRepo.CheckEmailAddressExists(emailAddress);
        }

        //added by Shatrughna kumar
        public Boolean CheckEmailAddressForNAcustomer(String emailAddress)
        {
            return loginRepo.CheckEmailAddressForNAcustomer(emailAddress);
        }

        public Boolean CheckRegistationIDforNAcustomer(String RegistrationId)
        {
            return loginRepo.CheckRegistationIDforNAcustomer(RegistrationId);
        }

        public Boolean LockUser(String emailAddress)
        {
            return loginRepo.LockUser(emailAddress);
        }


        public int ValidateUser(Entity.UserViewModel model)
        {
            return loginRepo.ValidateUser(model);
        }


        public UserViewModel GetAuthorityUserDetail(Entity.UserViewModel model)
        {
            return loginRepo.GetAuthorityUserDetail(model);
        }


        public int LockApplicationUser(UserViewModel model)
        {
            return loginRepo.LockApplicationUser(model);
        }


        public UserViewModel GetUserDetailsByUserName(string userName)
        {
            return loginRepo.GetUserDetailsByUserName(userName);
        }
    }
}