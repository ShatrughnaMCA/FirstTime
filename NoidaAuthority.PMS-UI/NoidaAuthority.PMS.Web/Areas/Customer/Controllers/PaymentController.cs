using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Common;
using System.Configuration;
using Kendo.Mvc.UI;

namespace NoidaAuthority.PMS.Web.Areas.Customer.Controllers
{
    public class PaymentController : Controller
    {
        private IPaymentService _paymentService;
        private ICommonService _commonService;

        public PaymentController()
        {
            _paymentService = new PaymentService();
            _commonService = new CommonService();
        }

        //public ActionResult Index(int? rid, int? id)
        //{
        //    if (rid != null && rid > 0 && id != null && id > 0)
        //    {
        //        ServiceRequestViewModel data = _paymentService.GenerateChallanForPayment(new ServiceRequestViewModel { RegistrationId = rid, OnlineRequestId = id });
        //        return View(data);
        //    }
        //    else
        //    {
        //        return RedirectToAction("ServiceRequest", "Property", new { area = "Customer" });
        //    }
        //}

        public ActionResult Index(int? rid, int? id, string actionflag)
        {
            if (rid != null && rid > 0 && id != null && id > 0)
            {
                //ServiceRequestViewModel data = _paymentService.GenerateChallanForPayment(new ServiceRequestViewModel { RegistrationId = rid, OnlineRequestId = id, ActionType = "ServiceCharge" });
                //ServiceRequestViewModel data = _paymentService.GenerateChallanForPayment(new ServiceRequestViewModel { RegistrationId = rid, OnlineRequestId = id, ActionType = actionflag });
                ServiceRequestViewModel data = _paymentService.GetServiceChargeForChallan(new ServiceRequestViewModel { RegistrationId = rid, OnlineRequestId = id, ActionType = actionflag });
                return View(data);
            }
            else
            {
                return RedirectToAction("ServiceRequest", "Property", new { area = "Customer" });
            }
        }

        //public ActionResult Receipt(FormCollection form)
        //{
        //    var OnlinePaymentModel = new ServiceRequestViewModel();
        //    try
        //    {
        //        //if (form != null)
        //        //{
        //        //    OnlinePaymentModel = _paymentService.UpdateOnlinePaymentTransaction(form);
        //        //}
        //        //string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
        //        string HashSequence = ConfigurationManager.AppSettings["hashSequence"];

        //        if (form != null && form["status"].ToString() == "success" && !string.IsNullOrEmpty(form["bank_ref_num"]))
        //        {
        //            string[] paramArr = HashSequence.Split('|');
        //            Array.Reverse(paramArr);
        //            string paramSequence = ConfigurationManager.AppSettings["HDFC_SALT"] + "|" + form["status"].ToString();
        //            foreach (string param in paramArr)
        //            {
        //                paramSequence += "|";
        //                paramSequence = paramSequence + (form[param] != null ? form[param] : "");
        //            }

        //            string hashSequence = PaymentGateway.GenerateSHA512HashCode(paramSequence).ToLower();
        //            if (hashSequence != form["hash"])
        //            {
        //                TempData["MismatchTraxaction"] = "Transaction key did not match";
        //                return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //            }
        //            else
        //            {
        //                OnlinePaymentModel = _paymentService.UpdateOnlinePaymentTransaction(form);
        //                TempData["Success"] = "Payment success";
        //                return View(OnlinePaymentModel);
        //            }
        //        }
        //        else
        //        {
        //            TempData["FailedTraxaction"] = "Payment request failed";
        //            return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorInTraxaction"] = ex.Message;
        //        return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //    }
        //    //return View(OnlinePaymentModel);
        //}

        public ActionResult Receipt(FormCollection form)
        {
            var ViewModel = new ServiceRequestViewModel();
            if (form != null)
            {
                ViewModel = _paymentService.UpdateOnlinePaymentTransaction(form);
                if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Success)
                {
                    TempData["SuccessTraxaction"] = "Payment successfully completed.";
                    return View(ViewModel);
                }
                else if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Failed)
                {
                    TempData["FailedTraxaction"] = "Payment request failed";
                    return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
                }
                else if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Mismatch)
                {
                    TempData["MismatchTraxaction"] = "Transaction key did not match";
                    return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
                }
                else
                {
                    TempData["ErrorInTraxaction"] = "Error in Transaction";
                    return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
                }
            }
            else
            {
                TempData["ErrorInTraxaction"] = "Error in Transaction";
                return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
            }
        }

        //public ActionResult Result(FormCollection form)
        //{
        //    if (form != null)
        //    {
                
        //    }


        //    return View();

        //    //var ViewModel = new ServiceRequestViewModel();
        //    //if (form != null)
        //    //{
        //    //    ViewModel = _paymentService.UpdateOnlinePaymentTransaction(form);
        //    //    if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Success)
        //    //    {
        //    //        TempData["SuccessTraxaction"] = "Payment successfully completed.";
        //    //        return View(ViewModel);
        //    //    }
        //    //    else if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Failed)
        //    //    {
        //    //        TempData["FailedTraxaction"] = "Payment request failed";
        //    //        return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //    //    }
        //    //    else if (ViewModel.OnlinePaymentModel.ReturnTypeId == ReturnType.Mismatch)
        //    //    {
        //    //        TempData["MismatchTraxaction"] = "Transaction key did not match";
        //    //        return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //    //    }
        //    //    else
        //    //    {
        //    //        TempData["ErrorInTraxaction"] = "Error in Transaction";
        //    //        return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    TempData["ErrorInTraxaction"] = "Error in Transaction";
        //    //    return RedirectToAction("Index", "Payment", new { area = "Customer", rid = form["udf2"], id = form["udf3"] });
        //    //}
        //}

        public ActionResult Challan()
        {
            PaymentViewModel challan = _paymentService.GetPropertyDetailForChallanPrint();
            return View(challan);
        }

        public JsonResult GetBankListAsDataSource([DataSourceRequest]DataSourceRequest request)
        {
            var list = _paymentService.GetBankListAsDataSource(request);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchListOfBankAsDataSource([DataSourceRequest]DataSourceRequest request, DropdownViewModel model)
        {
            var list = _paymentService.GetBranchListOfBankAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankAccountDetailById(PaymentViewModel model)
        {
            PaymentViewModel list = _paymentService.GetBankAccountDetailById(model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountHeadList([DataSourceRequest]DataSourceRequest request)
        {
            //var list = _paymentEngine.GetAccountHeadList(request);
            var list = _paymentService.GetReceiptHeadListAsDataSource(request);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountSubHeadList([DataSourceRequest]DataSourceRequest request, DropdownViewModel model)
        {
            var list = _paymentService.GetReceiptSubHeadListAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTempChallanChargeDetail(PaymentViewModel model)
        {
            int flag = _paymentService.SaveTempChallanChargeDetail(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTempChallanChargeDetail(PaymentViewModel model)
        {
            int flag = _paymentService.RemoveTempChallanChargeDetail(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckChallanSessionDataById(PaymentViewModel model)
        {
            int flag = _paymentService.CheckChallanSessionDataById(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTempChallanChargesAsDataSource([DataSourceRequest]DataSourceRequest request, PaymentViewModel model)
        {
            var list = _paymentService.GetTempChallanChargesAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGeneratedChallanListAsDataSource([DataSourceRequest]DataSourceRequest request, PaymentViewModel model)
        {
            var list = _paymentService.GetGeneratedChallanListAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GenerateChallanForPayment(ServiceRequestViewModel model)
        {
            var challan = _paymentService.GenerateChallanForPayment(model);
            return Json(challan, JsonRequestBehavior.AllowGet);
        }

        public void PayOnlineForServices(ServiceRequestViewModel model)
        {
            //ServiceRequestViewModel payment = _paymentService.SaveOnlinePaymentTransaction(model);
            _paymentService.SaveOnlinePaymentTransaction(model);
        }

        public JsonResult IsProcessingFeePaid(ServiceRequestViewModel model)
        {
            bool flag = _paymentService.IsProcessingFeePaid(model);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGeneratedChallanById(PaymentViewModel model)
        {
            var list = _paymentService.GetGeneratedChallanById(model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankDetailAsDataSource([DataSourceRequest]DataSourceRequest request, DropdownViewModel model)
        {
            var list = _commonService.GetBankDetailAsDataSource(request, model);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}