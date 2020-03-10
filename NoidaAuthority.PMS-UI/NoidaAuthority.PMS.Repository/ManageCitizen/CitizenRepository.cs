using NoidaAuthority.PMS.Entity;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entities;
using System.Web;
//using NoidaAuthority.PMS.Repository.Entities;
using System.Data.Entity.SqlServer;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Net;
using System.Configuration;
using System.IO;
using NoidaAuthority.PMS.Repository.Context;

namespace NoidaAuthority.PMS.Repository
{
    public class CitizenRepository : ICitizenRepository
    {
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

        public CitizenServiceRequest SaveServiceRequest(CitizenServiceRequest ObjServiceRequest)
        {

            var result = 0;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    if (ObjServiceRequest != null)
                    {
                        Customer_ServiceRequest ObjCustomerServiceRequest = new Customer_ServiceRequest();
                        ObjCustomerServiceRequest.Registration_No = ObjServiceRequest.Registration_No;
                        ObjCustomerServiceRequest.Property_No = ObjServiceRequest.Property_No;
                        ObjCustomerServiceRequest.DepartmentId = ObjServiceRequest.DepartmentId;
                        ObjCustomerServiceRequest.ServiceId = ObjServiceRequest.ServiceId;
                        ObjCustomerServiceRequest.Created_Date = (DateTime)System.DateTime.Now;
                        ObjCustomerServiceRequest.Created_By = ObjServiceRequest.Created_By;
                        ObjCustomerServiceRequest.Request_Status = ObjServiceRequest.Request_Status;
                        //if (ObjServiceRequest.ServiceId <= 9)
                        //{
                        //    ObjCustomerServiceRequest.Description = ObjServiceRequest.Description;
                        //}
                        ObjCustomerServiceRequest.LastDateofPayment = ObjServiceRequest.LastDateofPayment;
                        ObjCustomerServiceRequest.Comment = ObjServiceRequest.Comment;
                        ObjCustomerServiceRequest.Request_Status = 8;// Draft mode
                        ObjCustomerServiceRequest.IsActive = false;
                        ObjCustomerServiceRequest.ServiceType = ObjServiceRequest.ReqType;
                        ObjCustomerServiceRequest.MobileNumber = ObjServiceRequest.Mobile;
                        ObjCustomerServiceRequest.Email = ObjServiceRequest.Email;
                        dbContext.Customer_ServiceRequest.Add(ObjCustomerServiceRequest);


                        var chkLstMaster = dbContext.ServiceCheckList_Master.Where(c => c.dept_id == ObjServiceRequest.DepartmentId && c.service_id == ObjServiceRequest.ServiceId).ToList();

                        if (chkLstMaster != null)
                        {
                            foreach (var item in chkLstMaster)
                            {
                                ServiceRequest_Documents ObjServiceRequest_Documents = new ServiceRequest_Documents();
                                ObjServiceRequest_Documents.ServiceReq_No = ObjCustomerServiceRequest.Id;
                                ObjServiceRequest_Documents.ChkId = item.ChkId;
                                ObjServiceRequest_Documents.DocumentPath = item.ChkName.Trim();
                                ObjServiceRequest_Documents.Uploaded_Date = System.DateTime.Now;
                                ObjServiceRequest_Documents.Uploaded_By = ObjServiceRequest.Created_By;
                                dbContext.ServiceRequest_Documents.Add(ObjServiceRequest_Documents);
                            }
                        }

                        dbContext.SaveChanges();
                        ObjServiceRequest.ServiceRequestId = ObjCustomerServiceRequest.Id;
                        result = ObjCustomerServiceRequest.Id;



                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return ObjServiceRequest;
        }

        public List<ServiceRequestDocument> GetCheckListDocumentMentsByServiceId_DepartmentId(int ServiceRequestNo)
        {
            List<ServiceRequestDocument> CheckLstDocuments;

            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    CheckLstDocuments = dbContext.ServiceRequest_Documents.Where(m => m.ServiceReq_No == ServiceRequestNo).Select(d => new ServiceRequestDocument()
                    {

                        ChkDocumentId = d.SrvDocId,
                        ChkDocumentName = d.DocumentPath
                    }).ToList();

                }
                return CheckLstDocuments;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CitizenServiceRequest GetServiceRequestDetails(int ServiceRequestNo)
        {
            try
            {
                var details = new CitizenServiceRequest();
                using (var dbContext = new PIMSEntitiesContext())
                {
                    //CitizenServiceRequestObj = dbContext.Customer_ServiceRequest.Where(m => m.Id == ServiceRequestNo).Select(d => new CitizenServiceRequest()
                    //{
                    //    Registration_No = d.Registration_No,
                    //    Description = d.Description,

                    //}).FirstOrDefault();
                    details = (from det in dbContext.Customer_ServiceRequest
                               join csm in dbContext.CitizenService_Master on det.ServiceId equals csm.service_id
                               join stMaster in dbContext.StatusMasters on det.Request_Status equals stMaster.Id
                               //join app in dbContext.ApplicationDetails on det.Registration_No equals app.registrationId.ToString()
                               //join all in dbContext.AllotmentMasters on app.registrationId equals all.rid
                               //join spt in dbContext.SchemePropTrans on all.propertyId equals spt.propertyId
                               //join sm in dbContext.SectorMsts on spt.sectorId equals sm.sectorId
                               //join bm in dbContext.BlockMsts on spt.blockId equals bm.blockId
                               //where all.rid == RID && all.isActive == 1
                               where det.DepartmentId == csm.Deptt_Id && det.Id == ServiceRequestNo && csm.Status == 1
                               select new CitizenServiceRequest
                {
                    Registration_No = det.Registration_No,
                    Id = det.Id,
                    DepartmentId = det.DepartmentId.Value,
                    DepartmentName = det.DepartmentId != 1 ? (det.DepartmentId != 2 ? (det.DepartmentId != 3 ? (det.DepartmentId != 4 ? (det.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional",
                    SubDepartment = det.SubDepartment == "A" ? "Accounts" : "Property",
                    ServiceName = csm.ServiceName,
                    Status = stMaster.Status,
                    Description = det.Description,
                    ServiceFee = det.ServiceFee,
                    DuesAmnt = det.DuesAmount,
                    Comment = det.Comment,
                    PaymentStatus = (det.PaymentStatus == 1) ? Contants.yes : Contants.no,
                    ServiceRequestId = det.Id,
                    ServiceId = det.ServiceId,
                    Mobile = det.MobileNumber,
                    //Name = (from app in dbContext.ApplicationDetails
                    //        join all in dbContext.AllotmentMasters on app.registrationId equals all.rid
                    //        join spt in dbContext.SchemePropTrans on all.propertyId equals spt.propertyId
                    //        join sm in dbContext.SectorMsts on spt.sectorId equals sm.sectorId
                    //        join bm in dbContext.BlockMsts on spt.blockId equals bm.blockId
                    //        join det1 in dbContext.ServiceRequest_Documents on app.registrationId.ToString() equals det.Registration_No
                    //        where det1.ServiceReq_No == ServiceRequestNo && all.isActive == 1
                    //        select app.tFirstName + " " + app.tMiddleName + " " + app.tLastName).FirstOrDefault(),
                    //Property_No = (from app in dbContext.ApplicationDetails
                    //               join all in dbContext.AllotmentMasters on app.registrationId equals all.rid
                    //               join spt in dbContext.SchemePropTrans on all.propertyId equals spt.propertyId
                    //               join sm in dbContext.SectorMsts on spt.sectorId equals sm.sectorId
                    //               join bm in dbContext.BlockMsts on spt.blockId equals bm.blockId
                    //               join det1 in dbContext.ServiceRequest_Documents on app.registrationId.ToString() equals det.Registration_No
                    //               where det1.ServiceReq_No == ServiceRequestNo && all.isActive == 1
                    //               select sm.sectorName + "/" + bm.blockName + "-" + spt.propertyNo).FirstOrDefault()
                }).FirstOrDefault();
                    details.MortgageDetails = new MortgageModel();
                    details.transDetails = new TransferModel();
                    details.CICmodel = new CICModel();
                    details.mutationDetails = new MutationModel();
                    details.RentingModel = new RentingModel();
                    details.gdaModel = new GPAModel();
                    details.ExtensionRequest = new ExtensionRequest();
                    details.otherReq = new OtherRequests();
                    details.otherReq.ServiceId = details.ServiceId;
                }
                using (var dbContext1 = new PIMSEntitiesContext())
                {
                    //var name = string.Empty;
                    //var property = string.Empty;
                    //name = (from vw in dbContext1.view_property where vw.PRDVREGISTRATION_ID == details.Registration_No select vw.PRDVREGIST_APPLICANT_NAME).FirstOrDefault();
                    //property = (from vw in dbContext1.view_property where vw.PRDVREGISTRATION_ID == details.Registration_No select vw.SECTOR.Trim() + "/" + vw.BLOCK.Trim() + "-" + vw.PLDIPROPERTY_NO.Trim()).FirstOrDefault();
                    //if (name != null && property != null)
                    //{
                    //    details.Name = name;
                    //    details.Property_No = property;
                    //}
                    //else
                    //{
                    //    var dbContext2 = new PIMSEntitiesContext();
                    //    var service = dbContext2.Customer_ServiceRequest.Where(c => c.Id == ServiceRequestNo).FirstOrDefault();
                    //    details.Name = service.ApplicantName;
                    //    details.Property_No = service.Property_No;
                    //    details.ServiceId = 21;
                    //}
                }
                return details;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public List<DDList> GetRIDsByDeptt()
        //{
        //    try
        //    {
        //        using (var dbContext = new PIMSEntitiesContext())
        //        {
        //            //var role = (Role)HttpContext.Current.Session["Roles"];
        //            //var roleName = role.RoleName;
        //            //if(roleName.ToLower() == Contants.deptHead.ToLower())
        //            //{
        //            //var lstDeptts = (from u in dbContext.UmUserMasters
        //            //                 join d in dbContext.UmUserDepartmentTrans on u.UserRefId equals d.UserRefId
        //            //                 //where u.UserRefId == userInfo.UserID
        //            //                 select d.DepartmentId).ToList();
        //            //}
        //            var lst = (from f in dbContext.AllotmentMasters
        //                       where f.isActive == 1
        //                           //&& lstDeptts.Contains(f.departmentId) 
        //                       && f.isStatus.ToLower() == Contants.AllotmentStatus.Approved.ToString().ToLower()
        //                       orderby f.createdDate descending
        //                       select new DDList
        //                       {
        //                           id = f.rid,
        //                           text = f.rid.ToString()
        //                       }).ToList();
        //            return lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        public List<DDList> GetRIDsByDeptt(int deptt)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {

                //var lst = (from f in dbContext.view_property
                //           where f.PLDVLAND_USE_TYPE_ID == deptt.ToString()
                //           //&& lstDeptts.Contains(f.departmentId) 
                //           // && f.isStatus.ToLower() == Contants.AllotmentStatus.Approved.ToString().ToLower()
                //           //orderby f.createdDate descending
                //           select new RegList
                //           {
                //               id = f.PRDVREGISTRATION_ID,
                //               text = f.PRDVREGISTRATION_ID.ToString()
                //           }).ToList();
                var lstFin = new List<DDList>();
                //foreach (var item in lst)
                //{
                //    DDList ddobj = new DDList();
                //    int idf = 0;
                //    int.TryParse(item.id, out idf);
                //    ddobj.id = idf;
                //    ddobj.text = idf.ToString();
                //    lstFin.Add(ddobj);
                //}
                return lstFin;
            }
        }

        public bool UpdateRequestStatusByReqId(int ServiceRequestId)
        {

            var result = false;

            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    var serv = dbContext.Customer_ServiceRequest.FirstOrDefault(sr => sr.Id == ServiceRequestId);
                    if (serv != null)
                    {
                        serv.Request_Status = Contants.RequestStatusCodeInitiated;
                        if (dbContext.SaveChanges() > 0)
                            result = true;
                        
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetDetailsByRequestId(int RequestId)
        {
            string result = "";
            using (var dbContext = new PIMSEntitiesContext())
            {
                //result = dbContext.usp_GetServiceRequestDetails(RequestId).Select(x => x.ReportDetails).FirstOrDefault();
            }

            return result;

        }



        public CitizenServiceRequest SaveInternalServiceRequest(CitizenServiceRequest serviceRequest)
        {
            var result = 0;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    if (serviceRequest != null)
                    {
                        Customer_ServiceRequest citizenServiceRequest = new Customer_ServiceRequest();
                        citizenServiceRequest.Registration_No = serviceRequest.Registration_No;
                        citizenServiceRequest.Property_No = serviceRequest.Property_No;
                        citizenServiceRequest.ApplicantName = serviceRequest.Name;
                        citizenServiceRequest.RequestorName = serviceRequest.RequesterName;
                        citizenServiceRequest.ApplicantAddress = serviceRequest.ApplicantAddress;
                        citizenServiceRequest.RequestorAddress = serviceRequest.RequestorAddress;
                        citizenServiceRequest.DepartmentId = serviceRequest.DepartmentId;
                        citizenServiceRequest.SubDepartment = serviceRequest.SubDepartment;
                        citizenServiceRequest.ServiceId = serviceRequest.ServiceId;
                        citizenServiceRequest.Created_Date = (DateTime)System.DateTime.Now;
                        citizenServiceRequest.Created_By = serviceRequest.Created_By;
                        citizenServiceRequest.Request_Status = serviceRequest.Request_Status;
                        //if (serviceRequest.ServiceId <= 10)
                        //{
                        //    citizenServiceRequest.Description = serviceRequest.Description;
                        //}
                        citizenServiceRequest.LastDateofPayment = serviceRequest.LastDateofPayment;
                        citizenServiceRequest.Comment = serviceRequest.Comment;
                        citizenServiceRequest.Request_Status = 8;// Draft mode
                        citizenServiceRequest.ServiceType = serviceRequest.ReqType;
                        citizenServiceRequest.MobileNumber = serviceRequest.Mobile;
                        citizenServiceRequest.Email = serviceRequest.Email;
                        dbContext.Customer_ServiceRequest.Add(citizenServiceRequest);
                        //serviceRequest.ServiceId = 21;
                        var chkLstMaster = dbContext.ServiceCheckList_Master.Where(c => c.dept_id == serviceRequest.DepartmentId && c.service_id == serviceRequest.ServiceId).ToList();

                        if (chkLstMaster != null)
                        {
                            foreach (var item in chkLstMaster)
                            {
                                ServiceRequest_Documents ObjServiceRequest_Documents = new ServiceRequest_Documents();
                                ObjServiceRequest_Documents.ServiceReq_No = citizenServiceRequest.Id;
                                ObjServiceRequest_Documents.ChkId = item.ChkId;
                                ObjServiceRequest_Documents.DocumentPath = item.ChkName.Trim();
                                ObjServiceRequest_Documents.Uploaded_Date = System.DateTime.Now;
                                ObjServiceRequest_Documents.Uploaded_By = serviceRequest.Created_By;
                                dbContext.ServiceRequest_Documents.Add(ObjServiceRequest_Documents);
                            }
                        }

                        dbContext.SaveChanges();
                        serviceRequest.ServiceRequestId = citizenServiceRequest.Id;
                        result = citizenServiceRequest.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return serviceRequest;
        }


        public CitizenServiceRequest SaveOfflineServiceRequest(CitizenServiceRequest serviceRequest)
        {
            var result = 0;
            try
            {
                using (var dbContext = new PIMSEntitiesContext())
                {
                    if (serviceRequest != null)
                    {
                        Customer_ServiceRequest citizenServiceRequest = new Customer_ServiceRequest();
                        citizenServiceRequest.Registration_No = serviceRequest.Registration_No;
                        citizenServiceRequest.ApplicantName = serviceRequest.Name;
                        citizenServiceRequest.RequestorName = serviceRequest.RequesterName;
                        citizenServiceRequest.ApplicantAddress = serviceRequest.ApplicantAddress;
                        citizenServiceRequest.Property_No = serviceRequest.Property_No;
                        citizenServiceRequest.DepartmentId = serviceRequest.DepartmentId;
                        citizenServiceRequest.ServiceId = serviceRequest.ServiceId;
                        citizenServiceRequest.Created_Date = (DateTime)System.DateTime.Now;
                        citizenServiceRequest.Created_By = serviceRequest.Created_By;
                        citizenServiceRequest.Request_Status = serviceRequest.Request_Status;
                        //if (serviceRequest.ServiceId <= 10)
                        //{
                        //    citizenServiceRequest.Description = serviceRequest.Description;
                        //}
                        citizenServiceRequest.LastDateofPayment = serviceRequest.LastDateofPayment;
                        citizenServiceRequest.Comment = serviceRequest.Comment;
                        citizenServiceRequest.Request_Status = 8;// Draft mode
                        citizenServiceRequest.ServiceType = serviceRequest.ReqType;
                        citizenServiceRequest.MobileNumber = serviceRequest.Mobile;
                        citizenServiceRequest.Email = serviceRequest.Email;
                        citizenServiceRequest.RequestorAddress = serviceRequest.RequestorAddress;
                        citizenServiceRequest.SubDepartment = serviceRequest.SubDepartment;
                        citizenServiceRequest.IsActive = false;
                        dbContext.Customer_ServiceRequest.Add(citizenServiceRequest);
                        serviceRequest.ServiceId = 21;
                        var chkLstMaster = dbContext.ServiceCheckList_Master.Where(c => c.dept_id == serviceRequest.DepartmentId && c.service_id == serviceRequest.ServiceId).ToList();

                        if (chkLstMaster != null)
                        {
                            foreach (var item in chkLstMaster)
                            {
                                ServiceRequest_Documents ObjServiceRequest_Documents = new ServiceRequest_Documents();
                                ObjServiceRequest_Documents.ServiceReq_No = citizenServiceRequest.Id;
                                ObjServiceRequest_Documents.ChkId = item.ChkId;
                                ObjServiceRequest_Documents.DocumentPath = item.ChkName.Trim();
                                ObjServiceRequest_Documents.Uploaded_Date = System.DateTime.Now;
                                ObjServiceRequest_Documents.Uploaded_By = serviceRequest.Created_By;
                                dbContext.ServiceRequest_Documents.Add(ObjServiceRequest_Documents);
                            }
                        }

                        dbContext.SaveChanges();
                        serviceRequest.ServiceRequestId = citizenServiceRequest.Id;
                        result = citizenServiceRequest.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return serviceRequest;
        }


        public DataSourceResult GetAllServiceRequestId(DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var reqlist = (from req in dbContext.Customer_ServiceRequest
                               select new DDList
                               {
                                   id = req.Id,
                                   text = req.Id.ToString()
                               });
                return reqlist.ToDataSourceResult(request);
            }
        }

        public CitizenServiceRequest GetServiceRequestDetailByRequestId(int? requestId, string mobile)
        {
            var det = new CitizenServiceRequest();
            using (var dbContext = new PIMSEntitiesContext())
            {
                var detail = (from req in dbContext.Customer_ServiceRequest
                              where req.Id == requestId && req.MobileNumber == mobile
                              select new CitizenServiceRequest
                              {
                                  Registration_No = req.Registration_No,
                                  Property_No = req.Property_No,
                                  ApplicantName = req.ApplicantName,
                                  ApplicantAddress = req.ApplicantAddress,
                                  RequestorAddress = req.RequestorName,
                                  RequesterName = req.RequestorName,
                                  Mobile = req.MobileNumber,
                                  Comment = req.Comment,
                                  Description = req.Description
                              }).FirstOrDefault();
                if (detail == null)
                {
                    det.IsNull = true;
                }
                else
                {
                    det = detail;
                    det.IsNull = false;
                }
                return det;
            }
        }


        public bool UpdateCustomerServiceRequest(CitizenServiceRequest model)
        {
            var result = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var serv = dbContext.Customer_ServiceRequest.FirstOrDefault(sr => sr.Id == model.Id);
                if (serv != null)
                {
                    serv.Request_Status = Contants.RequestStatusCodeInitiated;
                    serv.Description = model.Description;
                    if (dbContext.SaveChanges() > 0)
                        result = true;
                }

            }
            return result;
        }


        public LetterViewModel GetLetterByBarcode(string barcode)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var rd = Convert.ToInt32(barcode);

                //var departmentList = (from dept in dbContext.UmDepartmentMasters
                //                      join udts in dbContext.UmUserDepartmentTrans on dept.DepartmentId equals udts.DepartmentId
                //                      where udts.UserRefId == userInfo.UserID
                //                      select dept.DepartmentId).ToList();

                var result = (from lettr in dbContext.Letter_History
                              join allot in dbContext.AllotmentMasters on lettr.Rid equals allot.rid
                              join propt in dbContext.SchemePropTrans on allot.propertyId equals propt.propertyId
                              join tmplt in dbContext.TemplateMasters on (lettr.Department_Id + lettr.Template_Id) equals (tmplt.departmentId + tmplt.templateId)
                              where lettr.Barcode_Val == barcode  //orderby srvc.requestNo descending
                              select new LetterViewModel
                              {
                                  Id = lettr.Id,
                                  Rid = lettr.Rid,
                                  ApplicantName = allot.ApplicationDetail.tFirstName,
                                  CorresspondentAddress = allot.ApplicationDetail.tCorrespondanceAdd,
                                  Sector = propt.SectorMst.sectorName,
                                  Block = propt.BlockMst.blockName,
                                  PlotNo = propt.propertyNo,
                                  DepartmentId = lettr.Department_Id,
                                  DepartmentName = allot.DepartmentMst.departmentName,
                                  LetterId = lettr.Template_Id,
                                  LetterType = tmplt.templateName,
                                  CreatedDate = lettr.Created_Date,
                                  LetterDate = lettr.Generate_Date,
                                  LetterContent = lettr.Template_Html
                              }).FirstOrDefault();
                return result;
            }
        }
    }
}
