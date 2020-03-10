using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoidaAuthority.PMS.Service
{
    public interface IPaymentService
    {
        ServiceRequestViewModel GenerateChallanForPayment(ServiceRequestViewModel model);

        ServiceRequestViewModel GetCustomerDetailForOnlinePayment(ServiceRequestViewModel model);

        ServiceRequestViewModel UpdateOnlinePaymentTransaction(FormCollection form);

        void SaveOnlinePaymentTransaction(ServiceRequestViewModel model);

        bool IsProcessingFeePaid(ServiceRequestViewModel model);

        PaymentViewModel GetPropertyDetailForChallanPrint();

        DataSourceResult GetBankListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request);

        DataSourceResult GetBranchListOfBankAsDataSource(DataSourceRequest request, DropdownViewModel model);

        DataSourceResult GetReceiptHeadListAsDataSource(DataSourceRequest request);

        DataSourceResult GetReceiptSubHeadListAsDataSource(DataSourceRequest request, DropdownViewModel model);

        int SaveTempChallanChargeDetail(PaymentViewModel model);

        int RemoveTempChallanChargeDetail(PaymentViewModel model);

        DataSourceResult GetTempChallanChargesAsDataSource(DataSourceRequest request, PaymentViewModel model);

        DataSourceResult GetGeneratedChallanListAsDataSource(DataSourceRequest request, PaymentViewModel model);

        int CheckChallanSessionDataById(PaymentViewModel model);

        PaymentViewModel GetBankAccountDetailById(PaymentViewModel model);

        PaymentViewModel GetGeneratedChallanById(PaymentViewModel model);

        ServiceRequestViewModel GetServiceChargeForChallan(ServiceRequestViewModel model);
    }
}
