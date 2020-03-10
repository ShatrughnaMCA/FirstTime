using System;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Repository.Login
{
    public interface ILoginRepository
    {
        Boolean ValidateUser(String userName, String password);

        User GetUserDetails(String userName);

        Boolean ChangePassword(String email, String newPassword);

        Boolean CheckEmailAddressExists(String emailAddress);
        Boolean CheckEmailAddressForNAcustomer(String emailAddress);
        Boolean CheckRegistationIDforNAcustomer(String RegistrationId);
        Boolean LockUser(String emailAddress);




        int ValidateUser(Entity.UserViewModel model);

        UserViewModel GetAuthorityUserDetail(UserViewModel model);

        int LockApplicationUser(UserViewModel model);

        UserViewModel GetUserDetailsByUserName(string userName);
    }
}