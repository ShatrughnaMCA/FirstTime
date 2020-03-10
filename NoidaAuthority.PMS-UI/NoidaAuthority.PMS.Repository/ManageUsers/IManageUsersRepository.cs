using System;
using System.Collections.Generic;
using NoidaAuthority.PMS.Entities;
using NoidaAuthority.PMS.Entity.NaUser;
using NoidaAuthority.PMS.Entity;
using System.Web;
using Kendo.Mvc.UI;

namespace NoidaAuthority.PMS.Repository.ManageUsers
{
    public interface IManageUsersRepository
    {
        IEnumerable<UserModel> GetUsers();
        NaUser GetUserById(Guid uId);
        Boolean AddEditUser(NaUser customer);
        //added by Shatrughna
        Boolean AddEditNAcustomer(NAcustomer customer);
      
        Boolean sendOTPtoUser(string id, int r);
        Boolean DeactivateUser(string email);
        Boolean RejectCustomer(string email, string mobileNo, string remarks);
        Boolean ResetPassword(string emailID);
        Boolean LockUnLockCustomer(string email);

        int SaveNoidaEmployee(Entity.UserViewModel model);

        int IsUserNameExist(string userName);

        int SaveNACustomerDetails(Entity.UserViewModel model, HttpPostedFileBase fileI, HttpPostedFileBase fileII);

        PropertyViewModel ValidateRegistrationId(PropertyViewModel model);

        DataSourceResult GetRegisteredCustomerList(DataSourceRequest request, UserViewModel model);

        int TakeActionOnUserByType(ActionViewModel model);

        int SendPassword(ActionViewModel model);

        DataSourceResult GetRegisteredEmployeeList(DataSourceRequest request, UserViewModel model);
    }
}