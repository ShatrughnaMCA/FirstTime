using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;

namespace NoidaAuthority.PMS.Service
{
    public class PaymentService : IPaymentService
    {
        private IPaymentRepository _paymentRepository;
        public PaymentService()
        {
            _paymentRepository = new PaymentRepository();
        }

        public ServiceRequestViewModel GenerateChallanForPayment(ServiceRequestViewModel model)
        {
            return _paymentRepository.GenerateChallanForPayment(model);
        }


        public ServiceRequestViewModel GetCustomerDetailForOnlinePayment(ServiceRequestViewModel model)
        {
            return _paymentRepository.GetCustomerDetailForOnlinePayment(model);
        }


        public ServiceRequestViewModel UpdateOnlinePaymentTransaction(System.Web.Mvc.FormCollection form)
        {
            return _paymentRepository.UpdateOnlinePaymentTransaction(form);
        }


        public void SaveOnlinePaymentTransaction(ServiceRequestViewModel model)
        {
            _paymentRepository.SaveOnlinePaymentTransaction(model);
        }


        public bool IsProcessingFeePaid(ServiceRequestViewModel model)
        {
            return _paymentRepository.IsProcessingFeePaid(model);
        }


        public PaymentViewModel GetPropertyDetailForChallanPrint()
        {
            return _paymentRepository.GetPropertyDetailForChallanPrint();
        }

        public Kendo.Mvc.UI.DataSourceResult GetBankListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request)
        {
            return _paymentRepository.GetBankListAsDataSource(request);
        }

        public Kendo.Mvc.UI.DataSourceResult GetBranchListOfBankAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            return _paymentRepository.GetBranchListOfBankAsDataSource(request, model);
        }

        public Kendo.Mvc.UI.DataSourceResult GetReceiptHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request)
        {
            return _paymentRepository.GetReceiptHeadListAsDataSource(request);
        }

        public Kendo.Mvc.UI.DataSourceResult GetReceiptSubHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            return _paymentRepository.GetReceiptSubHeadListAsDataSource(request, model);
        }

        public int SaveTempChallanChargeDetail(PaymentViewModel model)
        {
            return _paymentRepository.SaveTempChallanChargeDetail(model);
        }

        public int RemoveTempChallanChargeDetail(PaymentViewModel model)
        {
            return _paymentRepository.RemoveTempChallanChargeDetail(model);
        }

        public Kendo.Mvc.UI.DataSourceResult GetTempChallanChargesAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model)
        {
            return _paymentRepository.GetTempChallanChargesAsDataSource(request, model);
        }

        public Kendo.Mvc.UI.DataSourceResult GetGeneratedChallanListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model)
        {
            return _paymentRepository.GetGeneratedChallanListAsDataSource(request, model);
        }


        public int CheckChallanSessionDataById(PaymentViewModel model)
        {
            return _paymentRepository.CheckChallanSessionDataById(model);
        }

        public PaymentViewModel GetBankAccountDetailById(PaymentViewModel model)
        {
            return _paymentRepository.GetBankAccountDetailById(model);
        }


        public PaymentViewModel GetGeneratedChallanById(PaymentViewModel model)
        {
            return _paymentRepository.GetGeneratedChallanById(model);
        }


        public ServiceRequestViewModel GetServiceChargeForChallan(ServiceRequestViewModel model)
        {
            return _paymentRepository.GetServiceChargeForChallan(model);
        }
    }
}
