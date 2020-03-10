using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository.Context;
using NoidaAuthority.PMS.Common;
using System.Web;
using System.Configuration;
using com.fss.plugin.bob;

namespace NoidaAuthority.PMS.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public string GenerateChallanForPaymentII(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var challanIdPK = 0;
                List<PaymentViewModel> tempDataList = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModel"];
                if (tempDataList != null && tempDataList.FirstOrDefault().RegistrationId == model.RegistrationId)
                {
                    Challan_Master challanMaster = new Challan_Master();
                    challanMaster.Rid = model.RegistrationId;
                    challanMaster.Department_Id = model.PaymentModel.DepartmentId;
                    challanMaster.Sector_Id = model.PaymentModel.SectorId;
                    challanMaster.Block_Id = model.PaymentModel.BlockId;
                    challanMaster.Plot_No = model.PaymentModel.PlotNo;
                    challanMaster.Allottee = model.PaymentModel.Applicant;
                    challanMaster.Address = model.PaymentModel.CorresspondentAddress;
                    challanMaster.Mobile_No = model.PaymentModel.MobileNo;
                    challanMaster.Email = model.PaymentModel.Email;
                    challanMaster.Bank_Id = model.PaymentModel.BankId;
                    challanMaster.Branch_Id = model.PaymentModel.BranchId;
                    challanMaster.Account_Number = model.PaymentModel.AccountNo;
                    challanMaster.Created_Date = DateTime.Now;
                    challanMaster.Generate_Date = DateTime.Now;
                    challanMaster.Created_By = model.RegistrationId; //userInfo.UserID;
                    challanMaster.Is_Active = true;
                    challanMaster.Is_Verified = false;
                    //challanMaster.ServiceRequestNo = model.ServiceRequestId;
                    challanMaster.PAN = (model.PaymentModel.FilterType == "PAN" && model.PaymentModel.FilterType != null) ? model.PaymentModel.ReferenceNo : null;
                    challanMaster.GST_No = (model.PaymentModel.FilterType == "GST" && model.PaymentModel.FilterType != null) ? model.PaymentModel.ReferenceNo : null;
                    dbContext.Challan_Master.Add(challanMaster);
                    dbContext.SaveChanges();
                    //var challanIdPK = dbContext.Challan_Master.Max(m => m.Id);
                    var challanId = challanMaster.Id;
                    model.PaymentModel.Id = challanMaster.Id;
                    model.PaymentModel.ChallanId = challanMaster.Challan_Id;
                    foreach (var data in tempDataList)
                    {
                        Challan_Trans trans = new Challan_Trans();
                        trans.Rid = model.RegistrationId;
                        trans.Challan_Master_Id = model.Id;
                        //trans.Challan_Master_Id = Convert.ToInt32(model.ChallanId);
                        trans.Head_Id = data.AccountHeadId;
                        trans.Subhead_Id = data.AccountSubHeadId;
                        trans.Amount = data.Amount;
                        trans.Is_Active = true;
                        trans.Created_By = model.RegistrationId; //userInfo.UserID;
                        trans.Created_Date = DateTime.Now;
                        dbContext.Challan_Trans.Add(trans);
                        dbContext.SaveChanges();
                    }

                    string challanTemplate = string.Empty;
                    string bankCopy = string.Empty;
                    string allotteeCopy = string.Empty;
                    string authorityCopy = string.Empty;
                    //var header = GetBankChallanHeaderTemplate(challan);
                    var challanDetail = GetChallanChargesTable(model.PaymentModel);
                    var applicantDetail = GetApplicantTable(model.PaymentModel);
                    string content = string.Empty;

                    content = "<table style='width:100%;border-top:1px solid black;border-bottom:1px solid black;'><tr><td style='width:50%;border-right: 1px solid black;'> " + challanDetail + "</td><td style='width:50%;'>" + applicantDetail + " </td></tr></table>";
                    var footer = GetChallanFooterTemplate(model.PaymentModel);

                    bankCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanBankCopy) + content + footer;
                    authorityCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanAuthorityCopy) + content + footer;
                    allotteeCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanAllotteeCopy) + content + footer;

                    challanTemplate = bankCopy + authorityCopy + allotteeCopy;
                    challanMaster.Content = challanTemplate;
                    dbContext.SaveChanges();
                    return challanTemplate;
                }
                else
                {
                    return null;// Constants.Mismatch;// null;
                }
            }
        }

        private string GetChallanChargesTable(PaymentViewModel model)
        {
            List<PaymentViewModel> tempDataList = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
            string content = string.Empty;
            int count = 1;
            decimal? total = 0;
            content = "<Table style='width:100%;'><tr style=''><td class='tr-no'>SI No</td><td class='td-head'>Account Head</td><td class='td-amount' style='text-align:right;'>Amount</td>";
            foreach (var data in tempDataList)
            {
                content = content + "<tr  style=''><td class='i-no'>" + count + "</td><td class='td-head'>" + data.AccountSubHead + "</td><td class='td-amount' style='text-align:right;'>" + data.Amount + "</td></tr>";
                count++;
                total = total + data.Amount;
            }
            content = content + "<tr  style=''><td class='i-no'></td><td class='td-head' style='text-align:right;'>Total: </td><td class='td-amount' style='text-align:right;'>" + total + "</td></tr>";
            content = content + "</Table>";
            HttpContext.Current.Session["TempChallanModelII"] = null;
            return content;
        }

        private string GetProcessingChargeTable(PaymentViewModel model)
        {
            string content = string.Empty;
            int count = 1;
            decimal? total = model.Amount + model.GSTAmount + model.GSTAmount;
            content = "<Table style='width:100%;'><tr style=''><td class='tr-no'>SI No</td><td class='td-head'>Account Head</td><td class='td-amount' style='text-align:right;'>Amount</td></tr>";
            //content = content + "<tr  style=''><td class='i-no'>" + count + "</td><td class='td-head'>" + model.AccountSubHead + "</td><td class='td-amount' style='text-align:right;'>" + model.Amount + "</td></tr>";
            content = content + "<tr  style=''><td class='i-no'>" + 1 + "</td><td class='td-head'>" + "Processing Charge" + "</td><td class='td-amount' style='text-align:right;'>" + model.Amount + "</td></tr>";
            content = content + "<tr  style=''><td class='i-no'>" + 2 + "</td><td class='td-head'>" + "CGST" + "</td><td class='td-amount' style='text-align:right;'>" + model.GSTAmount + "</td></tr>";
            content = content + "<tr  style=''><td class='i-no'>" + 3 + "</td><td class='td-head'>" + "SGST" + "</td><td class='td-amount' style='text-align:right;'>" + model.GSTAmount + "</td></tr>";
            content = content + "<tr  style=''><td class='i-no'></td><td class='td-head' style='text-align:right;'>Total: </td><td class='td-amount' style='text-align:right;'>" + total + "</td></tr>";
            content = content + "</Table>";
            return content;
        }

        private string GetRequiredServiceChargeTable(PaymentViewModel model)
        {
            string content = string.Empty;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var chargelist = dbContext.Customer_ServiceRequestCharges.Where(c => c.ServiceRequestRefId == model.OnlineRequestId).ToList();
                decimal? total = 0;
                if (chargelist != null && chargelist.Count > 0)
                {
                    int count = 1;
                    content = "<Table style='width:100%;'><tr style=''><td class='tr-no'>SI No</td><td class='td-head'>Account Head</td><td class='td-amount' style='text-align:right;'>Amount</td></tr>";
                    foreach (var item in chargelist)
                    {
                        var accounthead = dbContext.RECEIPT_SUB_HEAD.Where(r => r.RECEIPT_CODE == item.AccountHeadId && r.RECEIPT_SUBHEAD_ID == item.AccountSubHeadId && r.STATUS == 1).FirstOrDefault().RECEIPT_SUB_HEAD1;
                        content = content + "<tr  style=''><td class='i-no'>" + count + "</td><td class='td-head'>" + accounthead + "</td><td class='td-amount' style='text-align:right;'>" + item.Amount + "</td></tr>";
                        count++;
                        total = total + item.Amount;
                    }
                }
                //content = content + "<tr  style=''><td class='i-no'>" + count + "</td><td class='td-head'>" + model.AccountSubHead + "</td><td class='td-amount' style='text-align:right;'>" + model.Amount + "</td></tr>";
                content = content + "<tr  style=''><td class='i-no'></td><td class='td-head' style='text-align:right;'>Total: </td><td class='td-amount' style='text-align:right;'>" + total + "</td></tr>";
                content = content + "</Table>";
            }
           
            return content;
        }

        private string GetApplicantTable(PaymentViewModel model)
        {
            string content = string.Empty;
            string gst = model.FilterType == "GST" ? model.ReferenceNo : model.GSTNo;
            string pan = model.FilterType == "PAN" ? model.ReferenceNo : model.PAN;
            content = "<Table style='width:100%;'>";
            content = content + "<tr  style=''><td colspan='2'>Property Location: <b> Sector-" + model.SectorName + ", Block-" + model.BlockName + ", Plot/Flat No-" + model.PlotNo + " </b></td></tr>";
            content = content + "<tr  style=''><td colspan='2'>Applicant: <b>" + model.Applicant + "</b></td></tr>";
            content = content + "<tr  style=''><td colspan='2'>GST No.:<b>" + gst + "</b> </td></tr>";
            content = content + "<tr  style=''><td colspan='2'>PAN No.:<b>" + pan + "</b> </td></tr>";
            content = content + "<tr  style=''><td colspan='2'>E-mail: <b>" + model.Email + "</b></td></tr>";
            content = content + "<tr  style=''><td colspan='2'>Mobile No: <b>" + model.MobileNo + "</b></td></tr>";
            content = content + "<tr  style=''><td colspan='2'>Address: <b>" + model.CorresspondentAddress + "</b></td></tr>";
            content = content + "</Table>";
            return content;
        }

        private string GetChallanHeaderTemplate(PaymentViewModel model, string HeaderName)
        {
            var today = DateTime.Now.ToString("dd/MMM/yyyy");
            string content = "<div id='authorityChallan' style='page-break-before:auto;overflow:auto'><Table style='width:100%;'><tr><td style='width:25%;'><b>" + model.BankName + "</b></td><td colspan='2'><center>Date:<b>" + today + "</b></center></td><td style='text-align:right;width:25%;'><b>" + HeaderName + "</b></td></tr>" +
            "<tr><td colspan='4'><center><h4 class='h4-noida'><b>NEW OKHLA INDUSTRIAL DEVELOPMENT AUTHORITY</b></h4></center></td></tr>" +
                //"<tr><td colspan='4'><center><h4 class='h4-dept' style='padding-top:0px;padding-bottom:0px;'><b>" + model.Department + "</b></h4></center></td></tr>" +
            "<tr><td colspan='4'><center><h4 style='padding-top:0px;'><b>GST No.: " + NAConstant.NoidaAuthorityGSTNo + "</b></h4></center></td></tr>" +
            "<tr><td colspan='2'>Registration Id: <b>" + model.RegistrationId + "</b></td><td colspan='2' style='text-align:right;'>Challan No.:<b>" + model.ChallanId + "</b></td></tr>" +
            "<tr><td colspan='2'>Account No.: <b>" + model.AccountNo + "</b></td><td colspan='2'  style='text-align:right;'>Property Type: <b>" + model.Department + "</b></td></tr>" +
            "<tr><td colspan='2'>IFSC Code: <b>" + model.IFSCCode + "</b></td></tr></Table>";
            return content;
        }

        private string GetChallanFooterTemplate(PaymentViewModel model)
        {
            string content = "<Table><tr style='padding-top:15px;'><td colspan='3'><p>Please find enclosed herewith Draft/Pay order No./Cash____________  Dated____________ for Rs.____________Drawn On" +
                        "____________  against above mentioned account head the payment of property Allotted / Lease / Sublease /Rent or any charges to me by NOIDA Authority." +
                    "</p></td></tr>" +
                    "<tr><td colspan='3' style='height:6px;'></td></tr>" +
                    "<tr><td><b>Authorised Signatory</b></td><td></td><td style='text-align:right;'><b>Depositor Signature</b></td></tr>" +

                    "<tr><td colspan='3'><table style='width:100%;border:1px solid;'><tr><td colspan=9 ><center style='border-bottom:1px solid black;'><b> Details of Notes </b></center></td></tr>" +
                    "<tr class='tr-notetype' style='border-bottom:1px solid black;padding-bottom:2px'><td style='border-right:1px solid black;'><center>2000*</center></td><td style='border-right:1px solid black;'><center>500*</center></td><td style='border-right:1px solid black;'><center>200*</center></td><td style='border-right:1px solid black;'><center>100*</center></td><td style='border-right:1px solid black;'><center>50*</center></td><td  style='border-right:1px solid black;'><center>20*</center></td><td style='border-right:1px solid black;'><center>10*</center></td><td style='border-right:1px solid black;'><center>5*</center></td><td>Total</td></tr>" +
                    "<tr style='padding-top:2px;padding-bottom:2px'><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td style='border-right:1px solid black;'>&nbsp;</td><td>&nbsp;</td></tr></table></td></tr></Table></div>" +
                    "<tr><p style='font-size:11px;'>Note: </p>" +
                        "<p style='font-size:11px;'>(1)Payment alone will not accrue any right to allottee.</p>" +
                        "<p style='font-size:11px;'>(2)Notwithstanding any request of the allotee the payment, made by the allotee shall be first adjusted towards the interest due, if any, and the balance shall be adjusted towards the annual leaserent and the installment respectively.</p></tr>" +
                //"<p style='font-size:11px;'>(3)IFSC Code: "+model.IFSCCode+"</p>" +
                        "<p style='font-size:11px;'>(3)<b>Allottee will pay GST by Reverse Charge Mechanism against Property. Authority's GST No: 09AAALN0120A1ZV</b></p></tr><hr style='display:block; margin-left:auto;margin-right:auto;margin-top:0;margin-bottom:0;border-style=inset;border-width;1px;page-break-after:always;'><br/>";

            return content;
        }

        public ServiceRequestViewModel GenerateChallanForPayment(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                HttpContext.Current.Session["OnlineRequestId"] = model.OnlineRequestId;
                int? challanId = 0;
                DateTime? createdDate = DateTime.Now;
                var service = dbContext.Customer_ServiceRequest.Where(r => r.Id == model.OnlineRequestId).FirstOrDefault();
                var onlinePayment = new OnlinePaymentViewModel();
                onlinePayment.RegistrationId = model.RegistrationId;
                onlinePayment.OnlineRequestId = service.Id;
                onlinePayment.Applicant = service.ApplicantName; //data.Allottee;
                onlinePayment.MobileNo = service.MobileNumber;
                onlinePayment.Email = service.Email;
                onlinePayment.AddressI = service.ApplicantAddress;

                onlinePayment.ServiceId = service.ServiceId;
                onlinePayment.ServiceName = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).ServiceName;
                onlinePayment.ProductInfo = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).ServiceName;
                
                if (model.ActionType == NAPayment.ServiceCharge)
                {
                    var exChallan = SaveChallanForPayment(model);  //save data to generate challan
                    challanId = exChallan.PaymentModel.Id;
                    createdDate = exChallan.PaymentModel.CreatedDate;
                    service.PaymentStatus = 0;
                    dbContext.SaveChanges();
                    onlinePayment.ChallanId = challanId;
                    onlinePayment.ChallanDate = createdDate;
                }
                else
                {
                    var processingFee = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).Amount;
                    if (processingFee != null && processingFee > 0)
                    {
                        var exChallan = SaveChallanForPayment(model);  //save data to generate challan
                        challanId = exChallan.PaymentModel.Id;
                        createdDate = exChallan.PaymentModel.CreatedDate;
                        service.PaymentStatus = 0;
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        model.PaymentModel = model.PaymentModel == null ? new PaymentViewModel() : model.PaymentModel;
                        model.PaymentModel.HTMLContent = string.Empty;
                        model.PaymentModel.IsChallanExist = false;
                    }
                    onlinePayment.ChallanId = (processingFee != null && processingFee > 0) ? challanId : null;
                    onlinePayment.ChallanDate = (processingFee != null && processingFee > 0) ? createdDate : null;
                }
                
                if (model.ActionType == NAPayment.ServiceCharge)
                {
                    var charges = dbContext.Customer_ServiceRequestCharges.Where(c => c.ServiceRequestRefId == model.OnlineRequestId).ToList();
                    charges.Sum(s => s.Amount);
                    onlinePayment.Amount = charges.Sum(s => s.Amount);
                    onlinePayment.TransactionId = service.Id.ToString() + model.RegistrationId.ToString();
                    onlinePayment.AccountHead = "Service Charge";
                    onlinePayment.AccountSubHead = "Service Charge";
                }
                else
                {
                    var charges = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                    var gst = charges != null ? (charges * (decimal?)0.18) : 0;
                    //onlinePayment.Amount = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                    onlinePayment.Amount = charges + Math.Round(gst.Value, 2);
                    onlinePayment.GSTAmount = gst;
                    onlinePayment.TransactionId = service.Id.ToString() + model.RegistrationId.ToString();
                    onlinePayment.AccountHead = "Processing Charge";
                    onlinePayment.AccountSubHead = "Processing Charge";
                }

                model.OnlinePaymentModel = onlinePayment;
                model.DepartmentId = service.DepartmentId;
                model.ServiceId = service.ServiceId;
                return model;
            }
        }

        private ServiceRequestViewModel SaveChallanForPayment(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var exChallan = dbContext.Challan_Master.FirstOrDefault(c => c.ServiceRequestNo == model.OnlineRequestId && c.Bank_Id == model.PaymentModel.BankId && c.Branch_Id==model.PaymentModel.BranchId && c.Is_Active==true);
                if (exChallan == null)
                {
                    var detail = GetCustomerDetailForOnlinePayment(model);
                    var data = new Challan_Master
                    {
                        Rid = detail.ServiceModel.RegistrationId,
                        Department_Id = detail.ServiceModel.DepartmentId,
                        Sector_Id = detail.ServiceModel.SectorId,
                        Block_Id = detail.ServiceModel.BlockId,
                        Plot_No = detail.ServiceModel.PropertyNo,
                        Allottee = detail.ServiceModel.Applicant,
                        Address = detail.ServiceModel.ApplicantAddress,
                        Mobile_No = detail.ServiceModel.MobileNo,
                        Email = detail.ServiceModel.Email,
                        Bank_Id = detail.PaymentModel.BankId, //67 - HDFC Bank
                        Branch_Id = detail.PaymentModel.BranchId, //81 - Sector 18 Noida
                        Account_Number = detail.PaymentModel.AccountNo, //"15921450000040",
                        Created_By = model.RegistrationId,
                        Created_Date = DateTime.Now,
                        Generate_Date = DateTime.Now,
                        Is_Active = true,
                        Is_Verified = false,
                        //PAN = detail.ServiceModel.PAN,
                        ServiceRequestNo = model.OnlineRequestId
                    };

                    dbContext.Challan_Master.Add(data);
                    dbContext.SaveChanges();

                    var challanId = data.Id;
                    var challan_unique_Id = data.Challan_Id;

                    var service = dbContext.Customer_ServiceRequest.Where(r => r.Id == model.OnlineRequestId).FirstOrDefault();
                    List<Challan_Trans> translist = new List<Challan_Trans>();
                    decimal? totalAmount = 0;
                    if (model.ActionType == NAPayment.ServiceCharge)
                    {
                        var chargelist = dbContext.Customer_ServiceRequestCharges.Where(r => r.ServiceRequestRefId == model.OnlineRequestId && r.IsActive==true).ToList();
                        if (chargelist != null && chargelist.Count > 0)
                        {
                            foreach (var item in chargelist)
                            {
                                Challan_Trans trans = new Challan_Trans();
                                trans.Rid = model.RegistrationId;
                                trans.Challan_Master_Id = challanId;
                                trans.Head_Id = item.AccountHeadId;
                                trans.Subhead_Id = item.AccountSubHeadId;
                                trans.Amount = item.Amount;
                                totalAmount = totalAmount + item.Amount;
                                trans.Is_Active = true;
                                trans.Created_By = model.RegistrationId;
                                trans.Created_Date = DateTime.Now;
                                translist.Add(trans);
                            }
                        }
                    }
                    else
                    {
                        var charges = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                        var gst = charges != null ? (charges * (decimal?)0.09) : 0;

                        for (int i = 1; i <= 3; i++)
                        {
                            Challan_Trans trans = new Challan_Trans();
                            trans.Rid = model.RegistrationId;
                            trans.Challan_Master_Id = challanId;
                            //trans.Challan_Master_Id = Convert.ToInt32(model.ChallanId);
                            trans.Head_Id = i == 1 ? 0 : 148;  //1-Processing charge, 148-GST
                            trans.Subhead_Id = i == 1 ? 3 : (i == 2 ? 289 : 290); //3-Processing charge, 289-CGST, 290-SGST
                            trans.Amount = i == 1 ? charges : Math.Round(gst.Value, 2);
                            //trans.Amount = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                            trans.Is_Active = true;
                            trans.Created_By = model.RegistrationId; //userInfo.UserID;
                            trans.Created_Date = DateTime.Now;
                            translist.Add(trans);
                        }
                    }
                    
                    dbContext.Challan_Trans.AddRange(translist);
                    dbContext.SaveChanges();

                    if (model.PaymentModel == null) model.PaymentModel = new PaymentViewModel();

                    model.PaymentModel.OnlineRequestId = model.OnlineRequestId;
                    model.PaymentModel.Department = dbContext.DepartmentMsts.FirstOrDefault(d => d.departmentId == data.Department_Id).departmentName;
                    model.PaymentModel.RegistrationId = model.RegistrationId;
                    model.PaymentModel.Id = challanId;
                    model.PaymentModel.ChallanId = challan_unique_Id; //challanId.ToString();
                    //model.PaymentModel.BankName = "HDFC Bank";
                    //model.PaymentModel.AccountNo = dbContext.BranchMsts.Where(b => b.bankId == 67 && b.branchId == 81 && b.IsActive == true).Select(s => s.accountNumber).FirstOrDefault();
                    //model.PaymentModel.IFSCCode = dbContext.BranchMsts.Where(b => b.bankId == 67 && b.branchId == 81 && b.IsActive == true).Select(s => s.IFSCcode).FirstOrDefault();
                    model.PaymentModel.Applicant = service.ApplicantName;
                    model.PaymentModel.ApplicantAddress = service.ApplicantAddress;
                    model.PaymentModel.CorresspondentAddress = service.ApplicantAddress;
                    model.PaymentModel.SectorName = service.SectorName;
                    model.PaymentModel.BlockName = service.BlockName;
                    model.PaymentModel.PlotNo = service.PlotNo;
                    model.PaymentModel.MobileNo = service.MobileNumber;
                    model.PaymentModel.Email = service.Email;
                    model.PaymentModel.ApplicantMaster = detail.ServiceModel.ApplicantMaster; //data.Address;
                    model.PaymentModel.PAN = detail.ServiceModel.PAN; //data.PAN;
                    model.PaymentModel.GSTNo = detail.ServiceModel.GSTNo;
                    model.PaymentModel.AadharNo = detail.ServiceModel.AadharNo;

                    if (model.ActionType != NAPayment.ServiceCharge)
                    {
                        model.PaymentModel.Amount = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                        model.PaymentModel.AccountSubHead = "Processing Charge";
                        var cgst = model.PaymentModel.Amount != null ? (model.PaymentModel.Amount * (decimal?)0.09) : 0;
                        model.PaymentModel.GSTAmount = Math.Round(cgst.Value, 2);
                    }

                    //model.PaymentModel = challandata;

                    string challanTemplate = string.Empty;
                    string bankCopy = string.Empty;
                    string allotteeCopy = string.Empty;
                    string authorityCopy = string.Empty;
                    var challanDetail = string.Empty; //GetProcessingChargeTable(challandata);
                    if (model.ActionType == NAPayment.ServiceCharge)
                    {
                        //challanDetail = GetRequiredServiceChargeTable(challandata);
                        challanDetail = GetRequiredServiceChargeTable(model.PaymentModel);
                    }
                    else
                    {
                        //challanDetail = GetProcessingChargeTable(challandata);
                        challanDetail = GetProcessingChargeTable(model.PaymentModel);
                    }
                   
                    //var applicantDetail = GetApplicantTable(challandata);
                    var applicantDetail = GetApplicantTable(model.PaymentModel);
                    string content = string.Empty;

                    content = "<table style='width:100%;border-top:1px solid black;border-bottom:1px solid black;'><tr><td style='width:50%;border-right: 1px solid black;'> " + challanDetail + "</td><td style='width:50%;'>" + applicantDetail + " </td></tr></table>";
                    var footer = GetChallanFooterTemplate(model.PaymentModel);

                    bankCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanBankCopy) + content + footer;
                    authorityCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanAuthorityCopy) + content + footer;
                    allotteeCopy = GetChallanHeaderTemplate(model.PaymentModel, NAConstant.ChallanAllotteeCopy) + content + footer;

                    challanTemplate = bankCopy + authorityCopy + allotteeCopy;
                    data.Content = challanTemplate;
                    service.ChallanId = challanId; // update challan id in customer service request
                    //service.ServiceFee = charges + Math.Round(gst.Value, 2);
                    dbContext.SaveChanges();
                    model.PaymentModel.HTMLContent = challanTemplate;
                    model.PaymentModel.IsChallanExist = true;
                }
                else
                {
                    model.PaymentModel = model.PaymentModel == null ? new PaymentViewModel() : model.PaymentModel;
                    model.PaymentModel.Id = exChallan.Id;
                    model.PaymentModel.ChallanId = exChallan.Challan_Id;
                    model.PaymentModel.Applicant = exChallan.Allottee;
                    model.PaymentModel.CreatedDate = exChallan.Created_Date;
                    model.PaymentModel.HTMLContent = exChallan.Content;
                    model.PaymentModel.IsChallanExist = true;
                    model.PaymentModel.BankId = exChallan.Bank_Id;
                }
                return model;
            }
        }


        public ServiceRequestViewModel GetCustomerDetailForOnlinePayment(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //ServiceRequestViewModel model = new ServiceRequestViewModel();
                var data = (from alotment in dbContext.AllotmentMasters
                            join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                            join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                            where alotment.rid == model.RegistrationId
                            select new ServiceViewModel
                            {
                                Id = alotment.rid,
                                RegistrationId = aplicant.registrationId,
                                PropertyId = property.propertyId,
                                SchemeId = alotment.schemeId,
                                SchemeName = alotment.SchemeMst.schemeName,
                                DepartmentId = alotment.departmentId,
                                Department = alotment.DepartmentMst.departmentName,
                                SectorId = property.sectorId,
                                Sector = property.SectorMst.sectorName,
                                BlockId = property.blockId,
                                Block = property.BlockMst.blockName,
                                PlotNo = property.propertyNo,
                                PropertyNo = property.SectorMst.sectorName + "/" + property.BlockMst.blockName + "-" + property.propertyNo,
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName,
                                ApplicantMaster = aplicant.tGender == NAConstant.Company ? aplicant.tSigningAuthority : aplicant.tFatherHusbandName,
                                ApplicantAddress = aplicant.tCorrespondanceAdd,
                                MobileNo = aplicant.tMobileNumber,
                                Email = aplicant.tEmail,
                                PAN = aplicant.tPan,
                                GSTNo = aplicant.tGSTNo,
                                AadharNo = aplicant.tAadhaarNo
                            }).FirstOrDefault();
                var request = dbContext.Customer_ServiceRequest.Where(c => c.Id == model.OnlineRequestId).FirstOrDefault();
                if (request != null)
                {
                    var onlinePayment = new OnlinePaymentViewModel();
                    onlinePayment.RegistrationId = model.RegistrationId;
                    onlinePayment.Applicant = request.ApplicantName;
                    onlinePayment.MobileNo = request.MobileNumber;
                    onlinePayment.Email = request.Email;
                    onlinePayment.Amount = request.DuesAmount;
                    onlinePayment.ServiceId = request.ServiceId;
                    onlinePayment.OnlineRequestId = request.Id;

                    model.OnlinePaymentModel = onlinePayment;
                }
                model.ServiceModel = data;
                return model;
            }
        }

        //public ServiceRequestViewModel UpdateOnlinePaymentTransaction(System.Web.Mvc.FormCollection form)
        //{
        //    using (var dbContext = new PIMSEntitiesContext())
        //    {
        //        var id = form["udf1"]; // TransactionId
        //        var rid = form["udf2"]; // RegistrationId
        //        var requestId = form["udf3"]; // OnlineRequestId
        //        var challanId = form["udf4"]; // ChallanId
        //        var reqid = HttpContext.Current.Session["OnlineRequestId"] != null ? (int?)HttpContext.Current.Session["OnlineRequestId"] : null;
        //        var TransactionModel = new OnlinePaymentViewModel();
        //        var transaction = dbContext.OnlineApplicationDetails_trans.Where(m => m.txnid == id).FirstOrDefault();
        //        if (transaction != null)
        //        {
        //            transaction.TranStatus = (form["status"].ToString() == "success" && !string.IsNullOrEmpty(form["bank_ref_num"])) ? NAStatusId.Success : NAStatusId.Failed;
        //            transaction.status = form["status"].ToString() == "success" ? 1 : 0;
        //            transaction.error = form["error"];
        //            transaction.error_Message = form["error_Message"];
        //            transaction.TrKey = form["txnid"].ToString();
        //            transaction.mode = form["mode"].ToString();
        //            transaction.mihpayid = form["mihpayid"];
        //            transaction.productinfo = form["productinfo"];
        //            transaction.EntryDate = DateTime.Now;
        //            transaction.payment_source = form["payment_source"];
        //            transaction.PG_Type = form["PG_Type"];
        //            transaction.bank_ref_num = form["bank_ref_num"];
        //            transaction.bankcode = form["bankcode"];
        //            transaction.name_on_card = form["name_on_card"];
        //            transaction.cardnum = form["cardnum"];
        //            //transaction.cardhash = form["cardhash"];
        //            transaction.issuing_bank = form["issuing_bank"];
        //            transaction.card_type = form["card_type"];
        //            transaction.udf1 = form["udf1"];
        //            transaction.udf2 = form["udf2"];
        //            transaction.udf3 = form["udf3"];
        //            transaction.udf4 = form["udf4"];
        //            transaction.ChallanId = form["udf4"];
        //            dbContext.SaveChanges();

        //            var request = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == transaction.ServiceRefId);
        //            if (form["status"].ToString() == "success")
        //            {
        //                request.PaymentStatus = 1;
        //                dbContext.SaveChanges();
        //                string message = string.Format(NAResources.OnlineProcessingFee, form["productinfo"], transaction.ServiceRefId, transaction.Amount);
        //                if (!string.IsNullOrEmpty(request.MobileNumber)) ApplicationHelper.SendSMS(request.MobileNumber, message);
        //                if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);
        //            }
        //            else
        //            {
        //                request.PaymentStatus = 0;
        //                dbContext.SaveChanges();
        //            }
        //        }


        //        var ServiceDetailModel = new ServiceRequestViewModel();
        //        //var ServiceRequest = GetServiceRequestDetailByRequestId(reqid);
        //        //ServiceDetailModel.ServiceModel = ServiceRequest;
        //        var serviceModel = GetServiceRequestDetailByRequestId(Convert.ToInt32(requestId));
        //        serviceModel.TransactionId = form["txnid"].ToString();
        //        serviceModel.Status = form["status"].ToString();
        //        ServiceDetailModel.ServiceModel = serviceModel;

        //        var paydetail = dbContext.OnlineApplicationDetails_trans.FirstOrDefault(c => c.ServiceRefId.ToString() == requestId.ToString());
        //        TransactionModel.Amount = paydetail.Amount;
        //        TransactionModel.NameOnCard = paydetail.name_on_card;
        //        TransactionModel.CardNumber = paydetail.cardnum;
        //        TransactionModel.BankReferenceNo = paydetail.bank_ref_num;
        //        TransactionModel.Mode = paydetail.mode;
        //        TransactionModel.TransactionStatus = form["status"].ToString();
        //        TransactionModel.EntryDate = paydetail.EntryDate;
        //        TransactionModel.TransactionId = paydetail.txnid;
        //        ServiceDetailModel.OnlinePaymentModel = TransactionModel;
        //        return ServiceDetailModel;
        //    }
        //}

        public ServiceRequestViewModel UpdateOnlinePaymentTransaction(System.Web.Mvc.FormCollection form)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (HttpContext.Current.Session["OnlineBankId"] != null)
                {
                    var bankId = (int)HttpContext.Current.Session["OnlineBankId"];
                    var bob = UpdateOnlinePaymentBankOfBaroda(form);
                    HttpContext.Current.Session["OnlineBankId"] = null;
                    return bob;
                }
                else
                {
                    //string hash_seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";
                    string HashSequence = ConfigurationManager.AppSettings["hashSequence"];
                    var ServiceDetailModel = new ServiceRequestViewModel();
                    var TransactionModel = new OnlinePaymentViewModel();
                    if (form != null && form["status"].ToString() == "success")
                    {
                        var id = form["udf1"]; // TransactionId
                        var rid = form["udf2"]; // RegistrationId
                        var requestId = form["udf3"]; // OnlineRequestId
                        var challanId = form["udf4"]; // ChallanId
                        var serviceRequestId = Convert.ToInt32(form["udf3"]);//requestId
                        var paidAmount = Convert.ToDecimal(form["amount"]);

                        var serviceRequest = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == serviceRequestId);
                        //var processingCharge = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == serviceRequest.DepartmentId && c.service_id == serviceRequest.ServiceId && c.Status == 1).Amount;

                        string[] paramArr = HashSequence.Split('|');
                        Array.Reverse(paramArr);
                        string paramSequence = ConfigurationManager.AppSettings["HDFC_SALT"] + "|" + form["status"].ToString();
                        foreach (string param in paramArr)
                        {
                            paramSequence += "|";
                            paramSequence = paramSequence + (form[param] != null ? form[param] : "");
                        }
                        //var sent_hash = HttpContext.Current.Session["hash_sequence"].ToString();
                        string hashSequence = PaymentGateway.GenerateSHA512HashCode(paramSequence).ToLower();
                        if (paidAmount == serviceRequest.ServiceFee && hashSequence == form["hash"])
                        {
                            var transaction = dbContext.OnlineApplicationDetails_trans.Where(m => m.txnid == id).FirstOrDefault();
                            if (transaction != null)
                            {
                                //transaction.TranStatus = (form["status"].ToString() == "success" && !string.IsNullOrEmpty(form["bank_ref_num"])) ? NAStatusId.Success : NAStatusId.Failed;
                                transaction.TranStatus = form["status"].ToString() == "success" ? NAStatusId.Success : NAStatusId.Failed;
                                transaction.status = form["status"].ToString() == "success" ? 1 : 0;
                                transaction.error = form["error"];
                                transaction.error_Message = form["error_Message"];
                                transaction.TrKey = form["txnid"].ToString();
                                transaction.mode = form["mode"].ToString();
                                transaction.mihpayid = form["mihpayid"];
                                transaction.productinfo = form["productinfo"];
                                transaction.EntryDate = DateTime.Now;
                                transaction.payment_source = form["payment_source"];
                                transaction.PG_Type = form["PG_Type"];
                                transaction.bank_ref_num = form["bank_ref_num"];
                                transaction.bankcode = form["bankcode"];
                                transaction.name_on_card = form["name_on_card"];
                                transaction.cardnum = form["cardnum"];
                                //transaction.cardhash = form["cardhash"];
                                transaction.issuing_bank = form["issuing_bank"];
                                transaction.card_type = form["card_type"];
                                transaction.udf1 = form["udf1"];
                                transaction.udf2 = form["udf2"];
                                transaction.udf3 = form["udf3"];
                                transaction.udf4 = form["udf4"];
                                transaction.ChallanId = form["udf4"];
                                dbContext.SaveChanges();

                                var request = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == transaction.ServiceRefId);
                                if (form["status"].ToString() == "success")
                                {
                                    request.PaymentStatus = 1;
                                    dbContext.SaveChanges();
                                    var challan = dbContext.Challan_Master.FirstOrDefault(c => c.Id.ToString() == challanId);
                                    if (challan != null) challan.Is_Verified = true;
                                    dbContext.SaveChanges();
                                    TransactionModel.ReturnTypeId = ReturnType.Success; //success
                                    TransactionModel.Status = form["status"].ToString();
                                    string message = string.Format(NAResources.OnlineProcessingFee, form["productinfo"], transaction.ServiceRefId, transaction.Amount);
                                    if (!string.IsNullOrEmpty(request.MobileNumber)) ApplicationHelper.SendSMS(request.MobileNumber, message);
                                    if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);
                                    //form = null;
                                }
                                else
                                {
                                    request.PaymentStatus = 0;
                                    dbContext.SaveChanges();
                                    TransactionModel.ReturnTypeId = ReturnType.Failed; //failed
                                }

                                var serviceModel = GetServiceRequestDetailByRequestId(Convert.ToInt32(requestId));
                                serviceModel.TransactionId = form["txnid"].ToString();
                                serviceModel.Status = form["status"].ToString();
                                ServiceDetailModel.ServiceModel = serviceModel;

                                //var paydetail = dbContext.OnlineApplicationDetails_trans.FirstOrDefault(c => c.ServiceRefId.ToString() == requestId.ToString());
                                var paydetail = dbContext.OnlineApplicationDetails_trans.FirstOrDefault(c => c.txnid == id);
                                TransactionModel.Amount = paydetail.Amount;
                                TransactionModel.NameOnCard = paydetail.name_on_card;
                                TransactionModel.CardNumber = paydetail.cardnum;
                                TransactionModel.BankReferenceNo = paydetail.bank_ref_num;
                                TransactionModel.Mode = paydetail.mode;
                                TransactionModel.TransactionStatus = form["status"].ToString();
                                TransactionModel.EntryDate = paydetail.EntryDate;
                                TransactionModel.TransactionId = paydetail.txnid;
                                //return ServiceDetailModel;
                            }
                            else
                            {
                                TransactionModel.ReturnTypeId = ReturnType.Mismatch; //mismatch
                            }
                        }
                        else
                        {
                            TransactionModel.ReturnTypeId = ReturnType.Mismatch; //mismatch
                        }
                    }
                    else
                    {
                        TransactionModel.ReturnTypeId = ReturnType.Failed; // failed
                    }

                    ServiceDetailModel.OnlinePaymentModel = TransactionModel;
                    return ServiceDetailModel;
                }
            }
        }

        public ServiceRequestViewModel UpdateOnlinePaymentBankOfBaroda(System.Web.Mvc.FormCollection form)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var ServiceDetailModel = new ServiceRequestViewModel();
                var TransactionModel = new OnlinePaymentViewModel();

                string resourcePath = ConfigurationManager.AppSettings["BOB_RESOURCE_PATH"];
                string recieptURL = ConfigurationManager.AppSettings["BOB_RETURN_URL"];
                string errorURL = ConfigurationManager.AppSettings["BOB_RETURN_URL"];
                string aliasName = ConfigurationManager.AppSettings["BOB_ALIAS_NAME"];

                iPayPipe pipe = new iPayPipe();
                pipe.setAlias(aliasName);
                pipe.setResourcePath(resourcePath);
                pipe.setKeystorePath(resourcePath);

                if (form != null)
                {
                    if (!String.IsNullOrEmpty(form["trandata"]))
                    {
                        int a = pipe.parseEncryptedRequest(form["trandata"].ToString());
                        var id = pipe.getUdf12(); // TransactionId
                        var rid = pipe.getUdf20(); // RegistrationId
                        var requestId = pipe.getUdf22();  // OnlineRequestId
                        var challanId = pipe.getUdf19(); // ChallanId 
                        var serviceRequestId = string.IsNullOrEmpty(requestId) ? 0 : Convert.ToInt32(requestId);//requestId
                        var paidAmount = Convert.ToDecimal(pipe.getAmt());
                        if (a == 0)
                        {
                            string errorText = !String.IsNullOrEmpty(HttpContext.Current.Request.Form["ErrorText"]) ? HttpContext.Current.Request.Form["ErrorText"] : !String.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("ErrorText")) ? HttpContext.Current.Request.QueryString.Get("ErrorText") : "";
                            var result = pipe.getResult(); // for success get as "CAPTURED"
                            
                            var transaction = dbContext.OnlineApplicationDetails_trans.Where(m => m.txnid == id).FirstOrDefault();
                            if (transaction != null)
                            {
                                //transaction.TranStatus = (form["status"].ToString() == "success" && !string.IsNullOrEmpty(form["bank_ref_num"])) ? NAStatusId.Success : NAStatusId.Failed;
                                transaction.TranStatus = a == 0 ? NAStatusId.Success : NAStatusId.Failed;
                                transaction.status = a == 0 ? 1 : 0;
                                transaction.error = errorText;// form["error"];
                                transaction.error_Message = errorText; // form["error_Message"];
                                //transaction.TrKey = pipe.getTransId();  //form["txnid"].ToString();
                                transaction.mode = "Online"; //pipe.getPmntmode(); //form["mode"].ToString();
                                transaction.mihpayid = pipe.getPaymentId(); //form["mihpayid"];
                                //transaction.productinfo = form["productinfo"];
                                transaction.EntryDate = DateTime.Now;
                                transaction.payment_source = pipe.getType(); //form["payment_source"];
                                transaction.PG_Type = "BOB"; //pipe.getPmntmode();  //form["PG_Type"];
                                transaction.bank_ref_num = pipe.getRef();  //form["bank_ref_num"];
                                transaction.bankcode = pipe.getRef(); //form["bankcode"];
                                transaction.name_on_card = pipe.getMember(); //form["name_on_card"];
                                transaction.cardnum = pipe.getCard(); //form["cardnum"];
                                //transaction.cardhash = form["cardhash"];
                                transaction.issuing_bank = "Bank Of Baroda"; //form["issuing_bank"];
                                transaction.card_type = pipe.getType(); //form["card_type"];
                                transaction.udf1 = pipe.getTransId();
                                transaction.udf2 = pipe.getTrackId();
                                transaction.ChallanId = pipe.getUdf19(); 
                                dbContext.SaveChanges();

                                var request = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == transaction.ServiceRefId);
                                if (a == 0)
                                {
                                    request.PaymentStatus = 1;
                                    //dbContext.SaveChanges();
                                    var challan = dbContext.Challan_Master.FirstOrDefault(c => c.Id.ToString() == challanId);
                                    if (challan != null) challan.Is_Verified = true;
                                    dbContext.SaveChanges();
                                    TransactionModel.ReturnTypeId = ReturnType.Success; //success
                                    TransactionModel.Status = "success";
                                    string message = string.Format(NAResources.OnlineProcessingFee, "Service Charge", transaction.ServiceRefId, transaction.Amount);
                                    if (!string.IsNullOrEmpty(request.MobileNumber)) ApplicationHelper.SendSMS(request.MobileNumber, message);
                                    if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);
                                    //form = null;
                                }
                                else
                                {
                                    request.PaymentStatus = 0;
                                    dbContext.SaveChanges();
                                    TransactionModel.ReturnTypeId = ReturnType.Failed; //failed
                                }

                                var serviceModel = GetServiceRequestDetailByRequestId(Convert.ToInt32(requestId));
                                serviceModel.TransactionId = pipe.getTransId(); //form["txnid"].ToString();
                                serviceModel.Status = a == 0 ? "Success" : "Failed"; //form["status"].ToString();
                                ServiceDetailModel.ServiceModel = serviceModel;

                                //var paydetail = dbContext.OnlineApplicationDetails_trans.FirstOrDefault(c => c.ServiceRefId.ToString() == requestId.ToString());
                                var paydetail = dbContext.OnlineApplicationDetails_trans.FirstOrDefault(c => c.txnid == id);
                                TransactionModel.Amount = paydetail.Amount;
                                TransactionModel.NameOnCard = paydetail.name_on_card;
                                TransactionModel.CardNumber = paydetail.cardnum;
                                TransactionModel.BankReferenceNo = paydetail.bank_ref_num;
                                TransactionModel.Mode = paydetail.mode;
                                TransactionModel.TransactionStatus = a == 0 ? "Success" : "Failed"; //form["status"].ToString();
                                TransactionModel.EntryDate = paydetail.EntryDate;
                                TransactionModel.TransactionId = paydetail.txnid;
                                //return ServiceDetailModel;
                            }
                            else
                            {
                                TransactionModel.ReturnTypeId = ReturnType.Mismatch; //mismatch
                            }
                        }
                    }
                    else
                    {
                        TransactionModel.ReturnTypeId = ReturnType.Failed; // failed
                    }
                }
                else
                {
                    TransactionModel.ReturnTypeId = ReturnType.Failed; // failed
                }

                ServiceDetailModel.OnlinePaymentModel = TransactionModel;
                return ServiceDetailModel;
            }
        }

        //public ServiceRequestViewModel UpdateOnlinePaymentTransactionII(System.Web.Mvc.FormCollection form)
        //{
        //    using (var dbContext = new PIMSEntitiesContext())
        //    {
        //        var id = form["udf1"];
        //        var paymenttype = form["udf2"];
        //        var reqid = HttpContext.Current.Session["OnlineRequestId"] != null ? (int?)HttpContext.Current.Session["OnlineRequestId"] : null;
        //        //var TransactionModel = new OnlinePaymentViewModel();
        //        var transaction = dbContext.OnlinePaymentDetailMsts.Where(m => m.TransactionId == id).FirstOrDefault();

        //        transaction.TransactionStatusId = form["status"].ToString() == "success" ? NAStatusId.Success : NAStatusId.Failed;
        //        transaction.TransactionStatus = form["status"].ToString();
        //        transaction.StatusId = form["status"].ToString() == "success" ? 1 : 0;
        //        transaction.Error = form["error"].ToString();
        //        transaction.ErrorMessage = form["error_Message"].ToString();
        //        transaction.TransactionKey = form["txnid"].ToString();
        //        transaction.Mode = form["mode"].ToString();
        //        transaction.MIHPayId = form["mihpayid"].ToString();
        //        transaction.ProductInfo = form["productinfo"].ToString();
        //        transaction.EntryDate = DateTime.Now;
        //        transaction.PaymentSource = form["payment_source"].ToString();
        //        transaction.PaymentGatewayType = form["PG_Type"].ToString();
        //        transaction.BankRefNo = form["bank_ref_num"].ToString();
        //        transaction.BankCode = form["bankcode"].ToString();
        //        transaction.NameOnCard = form["name_on_card"].ToString();
        //        transaction.CardNo = form["cardnum"].ToString();
        //        transaction.CardHash = form["cardhash"].ToString();
        //        transaction.CardIssuanceBank = form["issuing_bank"].ToString();
        //        transaction.CardType = form["card_type"].ToString();
        //        dbContext.SaveChanges();

        //        if (form["status"].ToString() == "success")
        //        {
        //            var request = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == transaction.ServiceRefId);
        //            string message = string.Format(NAResources.OnlineProcessingFee, form["productinfo"], transaction.ServiceRefId, transaction.Amount);
        //            if (!string.IsNullOrEmpty(request.MobileNumber)) ApplicationHelper.SendSMS(request.MobileNumber, message);
        //            if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);
        //        }

        //        var ServiceRequest = GetServiceRequestDetailByRequestId(reqid);
        //        ServiceRequest.TransactionId = transaction.TransactionId;
        //        var ServiceDetailModel = new ServiceRequestViewModel();
        //        ServiceDetailModel.ServiceModel = ServiceRequest;
        //        //ServiceDetailModel.OnlinePaymentModel = TransactionModel;
        //        return ServiceDetailModel;
        //    }
        //}

        //public void SaveOnlinePaymentTransactionII(ServiceRequestViewModel model)
        //{
        //    using (var dbContext = new PIMSEntitiesContext())
        //    {
        //        //<add key="hashSequence" value="key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"/>
        //        string transid = string.Empty;
        //        var transactionId = dbContext.OnlinePaymentDetailMsts.Where(f => f.ServiceRefId == model.OnlineRequestId).OrderByDescending(x => x.Id).Select(x => x.TransactionId).FirstOrDefault();
        //        if (transactionId == null)
        //        {
        //            transid = model.OnlinePaymentModel.TransactionId + "-" + 1;
        //        }
        //        else
        //        {
        //            var temp = transactionId.Split('-').Last();
        //            int newTxId = Convert.ToInt32(temp) + 1;
        //            transid = model.OnlinePaymentModel.TransactionId + "-" + newTxId;
        //        }

        //        OnlinePaymentDetailMst transaction = new OnlinePaymentDetailMst
        //        {
        //            ServiceRefId = model.OnlinePaymentModel.OnlineRequestId,
        //            RegistrationId = model.OnlinePaymentModel.RegistrationId,
        //            ServiceId = model.OnlinePaymentModel.ServiceId,
        //            Applicant = model.OnlinePaymentModel.Applicant,
        //            MobileNo = model.OnlinePaymentModel.MobileNo,
        //            Email = model.OnlinePaymentModel.Email,
        //            OnlineRequestId = model.OnlineRequestId,
        //            PaymentTypeId = NAConstant.OnlinePayment,
        //            TransactionId = transid,
        //            Amount = model.OnlinePaymentModel.Amount,
        //            ProductInfo = model.OnlinePaymentModel.ServiceName,
        //            BankId = model.OnlinePaymentModel.BankId,
        //            Mode = "Online",
        //            ServiceTypeId = NAConstant.OnlinePayment,
        //            GatewayName = "PAYU",
        //            StatusId = 0,
        //            EntryDate = DateTime.Now
        //        };

        //        dbContext.OnlinePaymentDetailMsts.Add(transaction);
        //        dbContext.SaveChanges();

        //        var service = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == model.OnlinePaymentModel.OnlineRequestId);
        //        service.ChallanId = model.OnlinePaymentModel.ChallanId;
        //        service.ServiceFee = model.OnlinePaymentModel.Amount;
        //        dbContext.SaveChanges();

        //        model.OnlinePaymentModel.TransactionId = transid;

        //        PaymentGateway gateway = new PaymentGateway();
        //        if (model.OnlinePaymentModel.BankId == 1)//indusind
        //        {
        //            gateway.PayOnline(model.OnlinePaymentModel);
        //        }
        //        if (model.OnlinePaymentModel.BankId == 2) //hdfc
        //        {
        //            gateway.PayOnlineHDFC(model.OnlinePaymentModel);
        //        }
        //    }
        //}

        public void SaveOnlinePaymentTransaction(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //<add key="hashSequence" value="key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"/>
                string transid = string.Empty;
                //bool flag = false;
                var exTransaction = dbContext.OnlineApplicationDetails_trans.Where(f => f.ServiceRefId == model.OnlineRequestId).OrderByDescending(x => x.AutoID).FirstOrDefault();
                if (exTransaction == null)
                {
                    //transid = model.OnlineRequestId.ToString() + model.RegistrationId.ToString() + "-1";
                    transid = model.OnlinePaymentModel.TransactionId + "-1";
                }
                else
                {
                    var temp = exTransaction.txnid.Split('-').Last();
                    int newTxId = Convert.ToInt32(temp) + 1;
                    //transid = model.OnlineRequestId.ToString() + model.RegistrationId.ToString() + "-" + newTxId;
                    transid = model.OnlinePaymentModel.TransactionId + "-" + newTxId;
                }
                var transactionKey = ApplicationHelper.GenerateTransactionId();
                model.OnlinePaymentModel.TransactionKey = transactionKey;

                var service = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == model.OnlinePaymentModel.OnlineRequestId);
                var processingCharge = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).Amount;
                var gst = processingCharge != null ? processingCharge * (decimal?)0.18 : 0;
                processingCharge = processingCharge + Math.Round(gst.Value, 2);
                OnlineApplicationDetails_trans transaction = new OnlineApplicationDetails_trans
                {
                    ServiceRefId = model.OnlineRequestId,
                    txnid = transid,
                    TrKey = model.OnlinePaymentModel.TransactionKey,
                    //Amount = model.OnlinePaymentModel.Amount,
                    Amount = processingCharge,
                    productinfo = model.OnlinePaymentModel.ServiceName,
                    mode = "Online",
                    ServiceType = NAConstant.OnlinePayment,
                    GetwayName = "PAYU",
                    status = 0,
                    EntryDate = DateTime.Now,
                    RegistrationId = model.OnlinePaymentModel.RegistrationId,
                    OnlineRequestId = model.OnlinePaymentModel.OnlineRequestId,
                    ChallanId = model.OnlinePaymentModel.ChallanId.ToString()
                };
                dbContext.OnlineApplicationDetails_trans.Add(transaction);
                dbContext.SaveChanges();

                service.ChallanId = model.OnlinePaymentModel.ChallanId;
                //service.ServiceFee = model.OnlinePaymentModel.Amount;
                service.ServiceFee = processingCharge;
                dbContext.SaveChanges();

                model.OnlinePaymentModel.TransactionId = transid;
                model.OnlinePaymentModel.Amount = processingCharge;

                PaymentGateway gateway = new PaymentGateway();
                if (model.OnlinePaymentModel.BankId == 1)//indusind
                {
                    gateway.PayOnline(model.OnlinePaymentModel);
                }
                if (model.OnlinePaymentModel.BankId == 67) //hdfc
                {
                    gateway.PayOnlineHDFC(model.OnlinePaymentModel);
                    //gateway.PayOnlineHDFCII(model.OnlinePaymentModel);
                }
                if (model.OnlinePaymentModel.BankId == 96) //Bank of Baroda
                {
                    HttpContext.Current.Session["OnlineBankId"] = 96;
                    gateway.PayBankOfBaroda(model.OnlinePaymentModel);
                }
                else
                {
                    HttpContext.Current.Session["OnlineBankId"] = null;
                }
            }
        }

        private OnlinePaymentViewModel GetOnlinePaymentTransactionDetailById(string transactionId, string paymentType)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var PaymentModel = new OnlinePaymentViewModel();
                if (transactionId != null)
                {
                    PaymentModel = (from transaction in dbContext.OnlineApplicationDetails_trans
                                    join request in dbContext.Customer_ServiceRequest on transaction.ServiceRefId equals request.Id
                                    where transaction.txnid == transactionId
                                    select new OnlinePaymentViewModel
                                    {
                                        OnlineRequestId = transaction.ServiceRefId,
                                        RegistrationId = transaction.RegistrationId,
                                        //ChallanId = transaction.ChallanId,
                                        TransactionKey = transaction.TrKey,
                                        TransactionId = transaction.txnid,
                                        Amount = transaction.Amount,
                                        ProductInfo = transaction.productinfo,
                                        Mode = transaction.mode,
                                        TransactionStatusId = transaction.TranStatus,
                                        Discount = transaction.discount,
                                        StatusId = transaction.status,
                                        EntryDate = transaction.EntryDate,
                                        Applicant = request.ApplicantName,
                                        MobileNo = request.MobileNumber,
                                        Email = request.Email,
                                        Status = transaction.TranStatus == NAStatusId.Success ? "Success" : "Failure",
                                        PaymentSource = transaction.payment_source,
                                        BankReferenceNo = transaction.bank_ref_num,
                                        BankCode = transaction.bankcode,
                                        Error = transaction.error,
                                        ErrorMessage = transaction.error_Message,
                                        NameOnCard = transaction.name_on_card,
                                        CardNumber = transaction.cardnum,
                                        CardHash = transaction.cardhash,
                                        IssuingBank = transaction.issuing_bank,
                                        CardType = transaction.card_type,
                                        Mihpayid = transaction.mihpayid
                                    }).FirstOrDefault();
                }
                return PaymentModel;
            }
        }

        private ServiceViewModel GetServiceRequestDetailByRequestId(int? requestId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from request in dbContext.Customer_ServiceRequest
                            join dprtmnt in dbContext.DepartmentMsts on request.DepartmentId equals dprtmnt.departmentId
                            join service in dbContext.CitizenService_Master on new { x = request.DepartmentId, y = request.ServiceId } equals new { x = service.Deptt_Id, y = service.service_id }
                            where request.Id == requestId && service.Status == 1
                            select new ServiceViewModel
                            {
                                Id = request.Id,
                                RequestId = request.Id,
                                //RegistrationId = request.Registration_No,
                                RegistrationNo = request.Registration_No,
                                DepartmentId = request.DepartmentId,
                                Department = dprtmnt.departmentName,
                                SubDepartment = request.SubDepartment,
                                ServiceId = request.ServiceId,
                                ServiceName = service.ServiceName,
                                //SectorId = property.sectorId,
                                Sector = request.SectorName,
                                //BlockId = property.blockId,
                                Block = request.BlockName,
                                PlotNo = request.PlotNo,
                                PropertyNo = request.Property_No,
                                Applicant = request.ApplicantName,
                                ApplicantAddress = request.ApplicantAddress,
                                MobileNo = request.MobileNumber,
                                Email = request.Email,
                                Description = request.Description,
                                CreatedDate = request.Created_Date
                            }).FirstOrDefault();
                return data;
            }
        }

        public bool IsProcessingFeePaid(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var flag = false;
                if (model.ActionType == "ServiceCharge")
                {
                    var challan = dbContext.Challan_Master.FirstOrDefault(c=>c.Id==model.ChallanId);
                    if (challan != null && challan.Is_Verified == true)
                    {
                        flag = true;
                    }
                }
                else
                {
                    var exTransaction = dbContext.OnlineApplicationDetails_trans.Where(f => f.ServiceRefId == model.OnlineRequestId).OrderByDescending(x => x.AutoID).FirstOrDefault();
                    if (exTransaction != null && exTransaction.TranStatus == 1) { 
                        flag = true; 
                    }
                }
                
                return flag;
            }
        }

        public PaymentViewModel GetPropertyDetailForChallanPrint()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var rid = (int)HttpContext.Current.Session["RegistrationId"];
                var data = (from alotment in dbContext.AllotmentMasters
                            join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                            join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                            where alotment.rid == rid
                            select new PaymentViewModel
                            {
                                Id = alotment.rid,
                                RegistrationId = aplicant.registrationId,
                                //PropertyId = property.propertyId,
                                //SchemeId = alotment.schemeId,
                                //SchemeName = alotment.SchemeMst.schemeName,
                                DepartmentId = alotment.departmentId,
                                Department = alotment.DepartmentMst.departmentName,
                                SectorId = property.sectorId,
                                SectorName = property.SectorMst.sectorName,
                                BlockId = property.blockId,
                                BlockName = property.BlockMst.blockName,
                                PlotNo = property.propertyNo,
                                PropertyNo = property.SectorMst.sectorName + "/" + property.BlockMst.blockName + "-" + property.propertyNo,
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName,
                                ApplicantMaster = aplicant.tGender == NAConstant.Company ? aplicant.tSigningAuthority : aplicant.tFatherHusbandName,
                                ApplicantAddress = aplicant.tCorrespondanceAdd,
                                MobileNo = aplicant.tMobileNumber,
                                Email = aplicant.tEmail
                            }).FirstOrDefault();
                //var request = dbContext.Customer_ServiceRequest.Where(c => c.Id == model.OnlineRequestId).FirstOrDefault();
                //if (request != null)
                //{
                //    var onlinePayment = new OnlinePaymentViewModel();
                //    onlinePayment.RegistrationId = model.RegistrationId;
                //    onlinePayment.Applicant = request.ApplicantName;
                //    onlinePayment.MobileNo = request.MobileNumber;
                //    onlinePayment.Email = request.Email;
                //    onlinePayment.Amount = request.DuesAmount;
                //    onlinePayment.ServiceId = request.ServiceId;
                //    onlinePayment.OnlineRequestId = request.Id;

                //    model.OnlinePaymentModel = onlinePayment;
                //}
                //model.ServiceModel = data;
                return data;
            }
        }

        public Kendo.Mvc.UI.DataSourceResult GetBankListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from bank in dbContext.BankMsts
                            where bank.IsActive == true
                            select new DropdownViewModel
                            {
                                Text = bank.bankName,
                                Id = bank.bankId
                            });
                return list.ToDataSourceResult(request);
            }
        }

        public Kendo.Mvc.UI.DataSourceResult GetBranchListOfBankAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from branch in dbContext.BranchMsts
                            where branch.bankId == model.BankId && branch.IsActive == true
                            select new DropdownViewModel
                            {
                                Text = branch.branchName,
                                Id = branch.branchId
                            });
                return list.ToDataSourceResult(request);
            }
        }

        public Kendo.Mvc.UI.DataSourceResult GetReceiptHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from receipt in dbContext.RECIEPT_HEAD
                            where receipt.STATUS == 1
                            select new DropdownViewModel
                            {
                                Text = receipt.RECIEPT_HEAD_NAME,
                                Id = receipt.RECIEPT_CODE
                            });
                return list.ToDataSourceResult(request);
            }
        }

        public Kendo.Mvc.UI.DataSourceResult GetReceiptSubHeadListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from receipt in dbContext.RECEIPT_SUB_HEAD
                            where receipt.RECEIPT_CODE == model.ReceiptHeadId && receipt.STATUS == 1
                            select new DropdownViewModel
                            {
                                Text = receipt.RECEIPT_SUB_HEAD1,
                                Id = receipt.RECEIPT_SUBHEAD_ID
                            });
                //request.Filters.RemoveAt(0);
                return list.ToDataSourceResult(request);
            }
        }

        public int SaveTempChallanChargeDetail(PaymentViewModel model)
        {
            int flag = ReturnType.None;
            List<PaymentViewModel> dataList = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
            if (dataList == null || dataList.Count == 0)
            {
                List<PaymentViewModel> modelList = new List<PaymentViewModel>();
                model.Id = 1;
                modelList.Add(model);
                HttpContext.Current.Session["TempChallanModelII"] = modelList;
                flag = ReturnType.Saved;
            }
            else
            {
                int id = dataList.Count;
                model.Id = id + 1;
                dataList.Add(model);
                HttpContext.Current.Session["TempChallanModelII"] = dataList;
                return flag = ReturnType.Saved;
            }
            return flag;
        }

        public int RemoveTempChallanChargeDetail(PaymentViewModel model)
        {
            var flag = ReturnType.None;
            List<PaymentViewModel> dataList = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
            if (dataList != null)
            {
                if (dataList.Count > 0)
                {
                    var rmodel = dataList.Where(x => x.AccountHead == model.AccountHead && x.AccountSubHead == model.AccountSubHead && x.Amount == model.Amount).FirstOrDefault();
                    dataList.Remove(rmodel);
                    flag = ReturnType.Removed;
                }
            }
            return flag;
        }

        public Kendo.Mvc.UI.DataSourceResult GetTempChallanChargesAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model)
        {
            List<PaymentViewModel> tempdata = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
            if (tempdata != null && tempdata.Count > 0)
            {
                for (int id = 0; id < tempdata.Count; id++) tempdata[id].Id = id;
            }
            return tempdata != null ? tempdata.ToDataSourceResult(request) : null;
        }

        public Kendo.Mvc.UI.DataSourceResult GetGeneratedChallanListAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, PaymentViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from challan in dbContext.Challan_Master
                            join bank in dbContext.BankMsts on challan.Bank_Id equals bank.bankId
                            where (model.Id == null || challan.Id == model.Id)
                            && (model.IsActive == null || challan.Is_Active == model.IsActive)
                            select new PaymentViewModel
                            {
                                Id = challan.Id,
                                ChallanId = challan.Challan_Id,
                                RegistrationId = challan.Rid,
                                DepartmentId = challan.Department_Id,
                                Department = challan.Department_Id != null ? dbContext.DepartmentMsts.FirstOrDefault(c => c.departmentId == challan.Department_Id).departmentName : string.Empty,
                                SectorId = challan.Sector_Id,
                                SectorName = challan.Sector_Id != null ? dbContext.SectorMsts.FirstOrDefault(s => s.sectorId == challan.Sector_Id).sectorName : string.Empty,
                                BlockId = challan.Block_Id,
                                BlockName = challan.Block_Id != null ? dbContext.BlockMsts.FirstOrDefault(b => b.blockId == challan.Block_Id).blockName : string.Empty,
                                PlotNo = challan.Plot_No,
                                Applicant = (challan.Allottee == null && challan.Rid != null) ? dbContext.ApplicationDetails.FirstOrDefault(a => a.registrationId == challan.Rid).tFirstName : challan.Allottee,
                                CorresspondentAddress = challan.Address,
                                //Applicant = aplicant.tGender == Constants.Company ? (string.IsNullOrEmpty(aplicant.T_Company_Name) ? aplicant.tFirstName : aplicant.T_Company_Name) : (aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName),
                                MobileNo = challan.Mobile_No,
                                PropertyNo = (challan.Sector_Id != null ? dbContext.SectorMsts.FirstOrDefault(s => s.sectorId == challan.Sector_Id).sectorName : string.Empty) + "/" + (challan.Block_Id != null ? dbContext.BlockMsts.FirstOrDefault(b => b.blockId == challan.Block_Id).blockName : string.Empty) + "-" + challan.Plot_No,
                                //PropertyNo = property.SectorMst.sectorName + "/" + property.blockId == null ? Constants.NA : property.BlockMst.blockName + "-" + property.propertyNo,
                                //GeneratedDate = challan.Generate_Date,
                                ChallanDate = challan.Created_Date,
                                HTMLContent = challan.Content,
                                IsActive = challan.Is_Active,
                                IsCancelled = challan.Is_Active == true ? false : true,
                                IsVerified = challan.Is_Verified,
                                Status = challan.Is_Active == false ? "Canceled" : (challan.Is_Verified == true ? "Verified" : "Not Verified"),
                                OnlineRequestId = challan.ServiceRequestNo,
                                BankId = challan.Bank_Id,
                                BankName = challan.Bank_Id == null ? string.Empty : dbContext.BankMsts.FirstOrDefault(b => b.bankId == challan.Bank_Id).bankName
                            });
                return list.ToDataSourceResult(request);
            }
        }


        public int CheckChallanSessionDataById(PaymentViewModel model)
        {
            int flag = ReturnType.None;
            List<PaymentViewModel> tempdata = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
            if (tempdata == null || tempdata.Count == 0)
            {
                return flag;
            }
            else
            {
                if (tempdata.FirstOrDefault().RegistrationId == model.RegistrationId)
                {
                    return flag = ReturnType.Exist;
                }
                else
                {
                    return flag = ReturnType.NotExist;
                }
            }
        }

        public PaymentViewModel GetBankAccountDetailById(PaymentViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (model.FilterType == "Bank")
                {
                    var data = (from bank in dbContext.BankMsts
                                where bank.bankId == model.BankId
                                select new PaymentViewModel
                                {
                                    FilterType = "Bank",
                                    BankId = bank.bankId,
                                    BankName = bank.bankName
                                }).FirstOrDefault();
                    return data;
                }
                else if (model.FilterType == "Branch")
                {
                    var data = (from branch in dbContext.BranchMsts
                                where branch.bankId == model.BankId && branch.branchId == model.BranchId
                                select new PaymentViewModel
                                {
                                    FilterType = "Branch",
                                    BankId = branch.bankId,
                                    BranchId = branch.branchId,
                                    BranchName = branch.branchName,
                                    AccountNo = branch.accountNumber,
                                    IFSCCode = branch.IFSCcode
                                }).FirstOrDefault();
                    return data;
                }
                else if (model.FilterType == "AccountNo")
                {
                    var data = (from branch in dbContext.BranchMsts
                                where branch.bankId == model.BankId && branch.branchId == model.BranchId
                                select new PaymentViewModel
                                {
                                    FilterType = "AccountNo",
                                    BankId = branch.bankId,
                                    BranchId = branch.branchId,
                                    BranchName = branch.branchName,
                                    AccountNo = branch.accountNumber,
                                    IFSCCode = branch.IFSCcode
                                }).FirstOrDefault();
                    return data;
                }
                else
                {
                    return null;
                }
            }
        }


        public PaymentViewModel GetGeneratedChallanById(PaymentViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var challanIdPK = 0;
                List<PaymentViewModel> tempDataList = (List<PaymentViewModel>)HttpContext.Current.Session["TempChallanModelII"];
                if (tempDataList != null && tempDataList.FirstOrDefault().RegistrationId == model.RegistrationId)
                {
                    Challan_Master challanMaster = new Challan_Master();
                    challanMaster.Rid = model.RegistrationId;
                    challanMaster.Department_Id = model.DepartmentId;
                    challanMaster.Sector_Id = model.SectorId;
                    challanMaster.Block_Id = model.BlockId;
                    challanMaster.Plot_No = model.PlotNo;
                    challanMaster.Allottee = model.Applicant;
                    challanMaster.Address = model.CorresspondentAddress;
                    challanMaster.Mobile_No = model.MobileNo;
                    challanMaster.Email = model.Email;
                    challanMaster.Bank_Id = model.BankId;
                    challanMaster.Branch_Id = model.BranchId;
                    challanMaster.Account_Number = model.AccountNo;
                    challanMaster.Created_Date = DateTime.Now;
                    challanMaster.Generate_Date = DateTime.Now;
                    challanMaster.Created_By = model.RegistrationId; //userInfo.UserID;
                    challanMaster.Is_Active = true;
                    challanMaster.Is_Verified = false;
                    //challanMaster.ServiceRequestNo = model.ServiceRequestId;
                    challanMaster.PAN = (model.FilterType == "PAN" && model.FilterType != null) ? model.ReferenceNo : null;
                    challanMaster.GST_No = (model.FilterType == "GST" && model.FilterType != null) ? model.ReferenceNo : null;
                    dbContext.Challan_Master.Add(challanMaster);
                    dbContext.SaveChanges();
                    //var challanIdPK = dbContext.Challan_Master.Max(m => m.Id);
                    var challanId = challanMaster.Id;
                    model.Id = challanMaster.Id;
                    model.ChallanId = challanMaster.Challan_Id;
                    foreach (var data in tempDataList)
                    {
                        Challan_Trans trans = new Challan_Trans();
                        trans.Rid = model.RegistrationId;
                        trans.Challan_Master_Id = model.Id;
                        //trans.Challan_Master_Id = Convert.ToInt32(model.ChallanId);
                        trans.Head_Id = data.AccountHeadId;
                        trans.Subhead_Id = data.AccountSubHeadId;
                        trans.Amount = data.Amount;
                        trans.Is_Active = true;
                        trans.Created_By = model.RegistrationId; //userInfo.UserID;
                        trans.Created_Date = DateTime.Now;
                        dbContext.Challan_Trans.Add(trans);
                        dbContext.SaveChanges();
                    }

                    string challanTemplate = string.Empty;
                    string bankCopy = string.Empty;
                    string allotteeCopy = string.Empty;
                    string authorityCopy = string.Empty;
                    //var header = GetBankChallanHeaderTemplate(challan);
                    var challanDetail = GetChallanChargesTable(model);
                    var applicantDetail = GetApplicantTable(model);
                    string content = string.Empty;

                    content = "<table style='width:100%;border-top:1px solid black;border-bottom:1px solid black;'><tr><td style='width:50%;border-right: 1px solid black;'> " + challanDetail + "</td><td style='width:50%;'>" + applicantDetail + " </td></tr></table>";
                    var footer = GetChallanFooterTemplate(model);

                    bankCopy = GetChallanHeaderTemplate(model, NAConstant.ChallanBankCopy) + content + footer;
                    authorityCopy = GetChallanHeaderTemplate(model, NAConstant.ChallanAuthorityCopy) + content + footer;
                    allotteeCopy = GetChallanHeaderTemplate(model, NAConstant.ChallanAllotteeCopy) + content + footer;

                    challanTemplate = bankCopy + authorityCopy + allotteeCopy;
                    challanMaster.Content = challanTemplate;
                    dbContext.SaveChanges();

                    model.HTMLContent = challanTemplate;
                    model.ReturnTypeId = ReturnType.Success;
                    return model;
                }
                else
                {
                    model.ReturnTypeId = ReturnType.NotExist;
                    return model;
                }
            }
        }


        public ServiceRequestViewModel GetServiceChargeForChallan(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                HttpContext.Current.Session["OnlineRequestId"] = model.OnlineRequestId;
                
                DateTime? createdDate = DateTime.Now;
                var service = dbContext.Customer_ServiceRequest.Where(r => r.Id == model.OnlineRequestId).FirstOrDefault();
                var onlinePayment = new OnlinePaymentViewModel();
                onlinePayment.RegistrationId = model.RegistrationId;
                onlinePayment.OnlineRequestId = service.Id;
                onlinePayment.Applicant = service.ApplicantName; //data.Allottee;
                onlinePayment.MobileNo = service.MobileNumber;
                onlinePayment.Email = service.Email;
                onlinePayment.AddressI = service.ApplicantAddress;

                onlinePayment.ServiceId = service.ServiceId;
                onlinePayment.ServiceName = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).ServiceName;
                onlinePayment.ProductInfo = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == service.DepartmentId && c.service_id == service.ServiceId && c.Status == 1).ServiceName;

                if (model.ActionType == NAPayment.ServiceCharge)
                {
                    var charges = dbContext.Customer_ServiceRequestCharges.Where(c => c.ServiceRequestRefId == model.OnlineRequestId).ToList();
                    charges.Sum(s => s.Amount);
                    onlinePayment.Amount = charges.Sum(s => s.Amount);
                    onlinePayment.TransactionId = service.Id.ToString() + model.RegistrationId.ToString();
                    onlinePayment.AccountHead = "Service Charge";
                    onlinePayment.AccountSubHead = "Service Charge";
                }
                else
                {
                    var charges = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                    var gst = charges != null ? (charges * (decimal?)0.18) : 0;
                    //onlinePayment.Amount = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == service.DepartmentId && x.service_id == service.ServiceId && x.Status == 1).Amount;
                    onlinePayment.Amount = charges + Math.Round(gst.Value, 2);
                    onlinePayment.GSTAmount = gst;
                    onlinePayment.TransactionId = service.Id.ToString() + model.RegistrationId.ToString();
                    onlinePayment.AccountHead = "Processing Charge";
                    onlinePayment.AccountSubHead = "Processing Charge";
                }
                if (model.PaymentModel != null)
                {
                    if (model.OnlineRequestId != null && model.PaymentModel.BankId != null && model.PaymentModel.BranchId != null)
                    {
                        var exChallan = dbContext.Challan_Master.FirstOrDefault(c => c.ServiceRequestNo == model.OnlineRequestId && c.Bank_Id == model.PaymentModel.BankId && c.Branch_Id == model.PaymentModel.BranchId && c.Is_Active == true);
                        if (exChallan != null)
                        {
                            onlinePayment.BankId = exChallan.Bank_Id;
                            onlinePayment.BranchId = exChallan.Branch_Id;
                            onlinePayment.AccountNo = exChallan.Account_Number;
                            onlinePayment.HtmlContent = exChallan.Content;
                        }
                    }
                }
                
                model.OnlinePaymentModel = onlinePayment;
                model.DepartmentId = service.DepartmentId;
                model.ServiceId = service.ServiceId;
                return model;
            }
        }
    }
}
