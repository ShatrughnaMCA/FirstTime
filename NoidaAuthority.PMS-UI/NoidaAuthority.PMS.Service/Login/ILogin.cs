using System;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Service.Login
{
    public interface ILogin
    {
        Boolean ValidateUser(String userName, String password);

        User GetUserDetails(String email);

        Boolean ChangePassword(String email, String newPassword);

        Boolean CheckEmailAddressExists(String emailAddress);
        //added by Shatrughna kumar
        Boolean CheckEmailAddressForNAcustomer(String emailAddress);
        Boolean CheckRegistationIDforNAcustomer(String RegistrationId);
        Boolean LockUser(String emailAddress);


        int ValidateUser(UserViewModel model);

        UserViewModel GetAuthorityUserDetail(UserViewModel model);

        int LockApplicationUser(UserViewModel model);

        UserViewModel GetUserDetailsByUserName(string userName);
    }
}