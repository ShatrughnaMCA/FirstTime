using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Repository
{
    public interface IPaymentRepository
    {
        ServiceRequestViewModel GenerateChallanForPayment(ServiceRequestViewModel model);

        ServiceRequestViewModel GetCustomerDetailForOnlinePayment(ServiceRequestViewModel model);

        ServiceRequestViewModel UpdateOnlinePaymentTransaction(System.Web.Mvc.FormCollection form);

        void SaveOnlinePaymentTransaction(ServiceRequestViewModel model);

        bool IsProcessingFeePaid(ServiceRequestViewModel model);

        PaymentViewModel GetPropertyDetailForChallanPrint();

        Kendo.Mvc.UI.DataSourceResult GetBankListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request);

        Kendo.Mvc.UI.DataSourceResult GetBranchListOfBankAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model);

        Kendo.Mvc.UI.DataSourceResult GetReceiptHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request);

        Kendo.Mvc.UI.DataSourceResult GetReceiptSubHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model);

        int SaveTempChallanChargeDetail(PaymentViewModel model);

        int RemoveTempChallanChargeDetail(PaymentViewModel model);

        Kendo.Mvc.UI.DataSourceResult GetTempChallanChargesAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model);

        Kendo.Mvc.UI.DataSourceResult GetGeneratedChallanListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model);

        int CheckChallanSessionDataById(PaymentViewModel model);

        PaymentViewModel GetBankAccountDetailById(PaymentViewModel model);

        PaymentViewModel GetGeneratedChallanById(PaymentViewModel model);

        ServiceRequestViewModel GetServiceChargeForChallan(ServiceRequestViewModel model);
    }
}
