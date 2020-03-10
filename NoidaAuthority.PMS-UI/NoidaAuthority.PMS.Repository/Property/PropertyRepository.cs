using Dapper;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
//using NoidaAuthority.PMS.Repository.Common;
using NoidaAuthority.PMS.Repository.Entities;
using NoidaAuthority.PMS.Entities;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using System.Web.Mvc;
using Kendo.Mvc;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using NoidaAuthority.PMS.Repository.Context;


namespace NoidaAuthority.PMS.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        public IEnumerable<DtoProperty> GetPropertyList(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DtoProperty>("sp_GetPropertyList", new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, AlloteName = objPropertyFilter.AlloteName, AreaId = objPropertyFilter.AreaId, SearchType = objPropertyFilter.SearchType, PlotNumber = objPropertyFilter.PlotNumber, RegistrationId = objPropertyFilter.RegistrationId, PageIndex = 1, PageSize = 40 }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public DataSourceResult GetPropertyList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                var dt = connection.Query<DtoProperty>("sp_GetPropertyList", new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, AlloteName = objPropertyFilter.AlloteName, AreaId = objPropertyFilter.AreaId, SearchType = objPropertyFilter.SearchType, PlotNumber = objPropertyFilter.PlotNumber, RegistrationId = objPropertyFilter.RegistrationId, PageIndex = request.Page, PageSize = request.PageSize }, commandType: System.Data.CommandType.StoredProcedure);
                foreach (var varItem in dt)
                {
                    varItem.Block = varItem.Block.Trim();
                }
                request.Page = 1;
                var dtrest = dt.ToDataSourceResult(request);
                dtrest.Total = dt.Select(x => x.TotalCount).FirstOrDefault();
                return dtrest;
            }
        }

        //public DataSourceResult GetPropertyList_New(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        //{
        //    using (var dbContext = new NOIDANEWEntities())
        //    {
        //        //var dt = new DtoProperty();
        //        var dt = (from vp in dbContext.view_property
        //              where vp.PLDVLAND_USE_TYPE_ID.Trim() == objPropertyFilter.PropertyTypeId.Trim() && vp.SECTOR.Trim() == objPropertyFilter.Sector.Trim() && vp.BLOCK.Trim() == objPropertyFilter.Block.Trim() && vp.PLDIPROPERTY_NO.Contains(objPropertyFilter.PropertyNo) && vp.PRDVREGIST_APPLICANT_NAME.Contains(objPropertyFilter.AlloteName) && vp.AreaId == objPropertyFilter.AreaId && vp.PlotNumber == objPropertyFilter.PlotNumber && vp.PRDVREGISTRATION_ID == objPropertyFilter.RegistrationId.ToString()
        //              select new DtoProperty
        //              {
        //                  PropertyNo = vp.SECTOR
        //              });
        //    }
        //}

        public DtoPropertyDetails GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyDetails>("sp_GetPropertyDetailsByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId, OperationType = inumber }, commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public NotificationDetails GetMobileEmailByReqId(int reqId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var det = (from cust in dbContext.Customer_ServiceRequest
                           where cust.Id == reqId
                           select new NotificationDetails
                           {
                               Mobile = cust.MobileNumber,
                               Email = cust.Email
                           }).FirstOrDefault();
                return det;
            }
        }

        public DtoPropertyDetails GetPropertyDetailsByRequestId(int RequestId)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyDetails>("sp_GetPropertyDetailsByServiceId", new { ServiceId = RequestId, OperationType = 2 }, commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }


        public IEnumerable<DocumentDetail> GetPropertyAttachmentDetails(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DocumentDetail>("sp_GetAllDocumentDetailByRegID", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoPropertyPaymentHistory> GetPaymentHistoryByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyPaymentHistory>("sp_GetPropertyPaymentHistoryByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoPropertyPaymentHistory> GetPaymentHistoryCustomerByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyPaymentHistory>("sp_GetPropertyPaymentHistoryCustomerByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<DtoPropertyPaymentHistory> GetPaymentList(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyPaymentHistory>("sp_GetPaymentList", new { Year = objPropertyFilter.Year, ProperyTypeId = objPropertyFilter.PropertyTypeId, Sector = objPropertyFilter.Sector }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoJalDetailsPaymentHistory>("sp_GetJalDetailsHistoryByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoTransferHistory> GetTransferHistoryByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoTransferHistory>("sp_GetTransferHistoryByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoLegalHistory> GetLegalHistoryByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoLegalHistory>("sp_GetLegalHistoryByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoPaymentSchedule> GetPaymentScheduleByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPaymentSchedule>("sp_GetPaymentScheduleByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId, Flag = "DETAIL" }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public IEnumerable<DtoPaymentDetails> GetPaymentDetails(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPaymentDetails>("sp_GetPropertyPaymentHistoryByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public DtoSchedulePaymentSummary GetPaymentScheduleSummaryByPropertyId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoSchedulePaymentSummary>("sp_GetPaymentScheduleByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId, Flag = "SUMMARY" }, commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DtoSectorLocation GetSectorLocation(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoSectorLocation>("dbo.sp_GetSectorLocation", new { Sector = objPropertyFilter.Sector }, commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<DtoPropertyLocation> GetPropertyLocations(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyLocation>("dbo.sp_GetPropertyLocation", new { Sector = objPropertyFilter.Sector, RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<DtoDepartmentPaymentDetails> GetDepartmentPaymentDetails(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoDepartmentPaymentDetails>("dbo.sp_GetPropertyPaymentDetails", new { Sector = objPropertyFilter.Sector, propertyTypeId = objPropertyFilter.PropertyTypeId, paymentStatus = objPropertyFilter.PaymentStatus, block = objPropertyFilter.Block, _SelectedQuarter = objPropertyFilter.SelectedQuarter }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<DtoLegalwithFarmerDetails> GetLegalwithFarmerDetails(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoLegalwithFarmerDetails>("dbo.sp_GetLegalwithFarmerDetails", new { _LegalTypeID = objPropertyFilter.LegalTypeID }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintList(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyComplaintList>("dbo.sp_PropertyComplaintList", new { StatusId = objPropertyFilter.ComplaintStatusId, Sector = objPropertyFilter.Sector }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public IEnumerable<DtoPropertyComplaintList> GetPropertyComplaintListByRegistrationId(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPropertyComplaintList>("dbo.sp_PropertyComplaintListByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DtoPaymentPayStatus UpdatePaymentPayStatus(DtoPropertyFilter objPropertyFilter)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return connection.Query<DtoPaymentPayStatus>("dbo.UpdatePayment", new { Id = objPropertyFilter.PaymentScheduleId, UserName = objPropertyFilter.Username }, commandType: System.Data.CommandType.StoredProcedure).First();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool AddPropertyRemark(string registrationId, string remark)
        {
            var flag = false;
            try
            {
                using (var dbContext = new NOIDANEWEntities())
                {
                    var newRemark = new Property_Remarks();
                    newRemark.Registration_No = registrationId;
                    newRemark.Remarks = remark;
                    newRemark.Created_Date = DateTime.Now;
                    dbContext.Property_Remarks.Add(newRemark);
                    dbContext.SaveChanges();
                    flag = true;
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<RemarksModel> Remarks_Read(string id)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                var lst = (from prop in dbContext.Property_Remarks
                           join view in dbContext.view_property on prop.Registration_No equals view.PRDVREGISTRATION_ID
                           where prop.Registration_No == id
                           select new RemarksModel
                           {
                               id = prop.Id,
                               regNo = prop.Registration_No,
                               date = prop.Created_Date,
                               remarks = prop.Remarks,
                           }).ToList();
                //var data = lst.ToDataSourceResult(req);
                return lst;
            }
        }

        //public List<PropertyRemark> GetPropertyRemark(string registrationId)
        //{
        //    List<PropertyRemark> listRemarks = null;
        //    try
        //    {
        //        using (var dbContext = new NOIDANEWEntities())
        //        {
        //            listRemarks = dbContext.PropertyRemarks.Where(t => t.RegistrationId == registrationId).ToList();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        listRemarks = new List<PropertyRemark>();
        //    }
        //    return listRemarks;
        //}

        public Kendo.Mvc.UI.DataSourceResult GetPropertyListByID(DtoPropertyFilter objPropertyFilter, Kendo.Mvc.UI.DataSourceRequest request)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                Kendo.Mvc.UI.DataSourceResult aa = new Kendo.Mvc.UI.DataSourceResult();
                return aa;
                //return connection.Query<DtoProperty>("sp_GetPropertyList", new { ProperyTypeId = objPropertyFilter.PropertyTypeId, SectorId = objPropertyFilter.Sector, Block = objPropertyFilter.Block, propertyNo = objPropertyFilter.PropertyNo, AlloteName = objPropertyFilter.AlloteName, AreaId = objPropertyFilter.AreaId, SearchType = objPropertyFilter.SearchType, PlotNumber = objPropertyFilter.PlotNumber, RegistrationId = objPropertyFilter.RegistrationId }, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public bool SaveFeedback(string feedback, string regNo)
        {
            var flag = false;
            using (var dbContext = new NOIDANEWEntities())
            {
                var newFeedback = new Customer_Feedback();
                newFeedback.Feedback = feedback;
                newFeedback.Created_Date = DateTime.Now;
                newFeedback.Registration_No = regNo;
                dbContext.Customer_Feedback.Add(newFeedback);
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public DataSourceResult Feedbacks_Read(DataSourceRequest request)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                var lst = (from feed in dbContext.Customer_Feedback
                           join view in dbContext.view_property on feed.Registration_No equals view.PRDVREGISTRATION_ID
                           select new FeedbackModel
                           {
                               id = feed.Id,
                               registrationID = feed.Registration_No,
                               date = feed.Created_Date.Value,
                               feedback = feed.Feedback,
                               customerName = view.PRDVREGIST_APPLICANT_NAME
                           });
                var data = lst.ToDataSourceResult(request);
                return data;
            }
        }

        public bool SaveServiceReq(int deptVal, int serviceVal, string description, string regNo)
        {
            var flag = false;
            using (var dbContext = new NOIDANEWEntities())
            {
                var serviceRequest = new NoidaAuthority.PMS.Repository.Entities.Customer_ServiceRequest();
                serviceRequest.Registration_No = regNo;
                serviceRequest.DepartmentId = deptVal;
                serviceRequest.ServiceId = serviceVal;
                serviceRequest.Description = description;
                serviceRequest.Created_Date = DateTime.Now;
                dbContext.Customer_ServiceRequest.Add(serviceRequest);
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public DataSourceResult SR_Read(DataSourceRequest request)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                var lst = (from csr in dbContext.Customer_ServiceRequest
                           join view in dbContext.view_property on csr.Registration_No equals view.PRDVREGISTRATION_ID
                           //join ld in dbContext.LookupDepartments on csr.DepartmentId equals ld.Id
                           join csm in dbContext.CitizenService_Master on csr.ServiceId equals csm.Id
                           select new ServiceRequestsModel
                           {
                               id = csr.Id,
                               regNo = csr.Registration_No,
                               date = csr.Created_Date.Value,
                               description = csr.Description,
                               custName = view.PRDVREGIST_APPLICANT_NAME,
                               service = csm.ServiceName,
                               //dept = ld.NAME
                               dept = csr.DepartmentId != 1 ? (csr.DepartmentId != 2 ? (csr.DepartmentId != 3 ? (csr.DepartmentId != 4 ? (csr.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional"
                           });
                var data = lst.ToDataSourceResult(request);
                return data;
            }
        }


        public NoidaServiceRequestModel GetPropertyDetailForServiceRequest(int id)
        {
            throw new NotImplementedException();
        }

        public DataSourceResult GetCitizenRequests(DataSourceRequest req)
        {
            var rid = string.Empty;
            if (HttpContext.Current.Session["RegistrationID"] != null)
            {
                rid = (string)HttpContext.Current.Session["RegistrationID"];
            }

            //For sorting Kendo DataSourceResult
            if (req.Sorts.Count == 0)
            {
                req.Sorts.Add(new SortDescriptor("RefNo",System.ComponentModel.ListSortDirection.Descending));
            }

            using (var dbContext = new PIMSEntitiesContext())
            {
                //var serviceRequests = (from serv in dbContext.View_Service_report
                //                       where serv.Registration_No == rid
                //                       select new CitizenRequestsModel
                //                                    {
                //                                        RId = serv.Registration_No,
                //                                        RefNo = serv.requestNo,
                //                                        ReqDate = serv.Created_Date,
                //                                        //SLA = 12,
                //                                        ServiceName = serv.ServiceName,
                //                                        TotalAmount = serv.DuesAmount == null ? 0 : serv.DuesAmount,
                //                                        ChallanId = serv.ChallanId,
                //                                        DuesAmount = serv.DuesAmount,
                //                                        ServiceFee = serv.ServiceFee == null ? 0 : serv.ServiceFee
                //                                    }
                //                          );
                //return serviceRequests.ToDataSourceResult(req);
                return null;
            }
        }

        public RequestDetails GetRequestDetailsById(int id)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var details = (from det in dbContext.Customer_ServiceRequest
                               join csm in dbContext.CitizenService_Master on det.ServiceId equals csm.service_id
                               //join stMaster in dbContext.StatusMasters on det.Request_Status equals stMaster.Id
                               where det.DepartmentId == csm.Deptt_Id && det.Id == id
                               select new RequestDetails
                               {
                                   RId = det.Registration_No,
                                   Id = id,
                                   ServiceName = csm.ServiceName,
                                   //ReqStatus = stMaster.Status,
                                   Description = det.Description,
                                   //ServiceFee = det.ServiceFee,
                                   //DuesAmnt = det.DuesAmount,
                                   //Comment = det.Comment,
                                   //PaymentStatus = (det.PaymentStatus == 1) ? Constants.yes : Constants.no
                               }).FirstOrDefault();
                return details;
            }
        }

        public List<CitizenPropertyDocument> GetDocumentDetails(DataSourceRequest req, int rid, int id)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from docs in dbContext.ServiceRequest_Documents
                            join det in dbContext.Customer_ServiceRequest on docs.ServiceReq_No equals det.Id
                            join chk in dbContext.ServiceCheckList_Master on docs.ChkId equals chk.ChkId
                            where det.Registration_No == rid.ToString()
                            select new CitizenPropertyDocument
                            {
                                //RID = rid,
                                //DocumentPath = docs.DocumentPath
                                RID = rid,
                                DocumentName = docs.DocumentPath,
                                DocumentType = chk.ChkName
                            });
                return data.ToList();
            }
        }

        public List<DDList> GetMortgageType()
        {
            List<DDList> mortgageType = new List<DDList>();
            foreach (int value in Enum.GetValues(typeof(NoidaAuthority.PMS.Common.Contants.MortgageType)))
            {
                mortgageType.Add(new DDList
                {
                    text = Enum.GetName(typeof(NoidaAuthority.PMS.Common.Contants.MortgageType), value),
                    id = value
                });
            }
            return mortgageType;
        }

        public List<DDList> GetMortgagePrevLoan()
        {
            List<DDList> mortgagePrevLoan = new List<DDList>();
            foreach (int value in Enum.GetValues(typeof(NoidaAuthority.PMS.Common.Contants.MortgagePrevLoan)))
            {
                mortgagePrevLoan.Add(new DDList
                {
                    text = Enum.GetName(typeof(NoidaAuthority.PMS.Common.Contants.MortgagePrevLoan), value),
                    id = value
                });
            }
            return mortgagePrevLoan;
        }

        public bool SaveTransferRequest(int transType, int transSubType, string firstName, string middleName, string lastName, string relativeName, string motherName, string mobileNo, string email, string correspondenceAdd, string permanentAdd, string PAN, int? occupation, int rId, int serviceId, string gender, string cmpnyName, string signAuth, string RO, int reqID, string desc)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var newTransRequest = new OnlineTransferMutation();
                newTransRequest.Rid = rId;
                //newTransRequest.Property_Id = 
                //newTransRequest.Transfer_Date = transDate;
                newTransRequest.Type = Contants.transfer;
                newTransRequest.Transfer_Type = transType;
                newTransRequest.Transfer_Sub_Type = transSubType;
                newTransRequest.Created_Date = DateTime.Now;
                if (gender == Contants.select)
                {
                    newTransRequest.T_Gender = "Company";
                }
                else
                    newTransRequest.T_Gender = gender;
                newTransRequest.T_First_Name = firstName;
                newTransRequest.T_Middle_Name = middleName;
                newTransRequest.T_Last_Name = lastName;
                newTransRequest.T_Father_Husband_Name = relativeName;
                newTransRequest.T_Mother_Name = motherName;
                newTransRequest.T_Email = email;
                newTransRequest.T_Mobile = mobileNo;
                newTransRequest.T_Correspondence_Add = correspondenceAdd;
                newTransRequest.T_Permanent_Add = permanentAdd;
                newTransRequest.T_Occupation_Id = occupation;
                newTransRequest.T_Pan = PAN;
                newTransRequest.T_Company_Name = cmpnyName;
                newTransRequest.T_Signing_Authority = signAuth;
                newTransRequest.T_Registered_Office = RO;
                newTransRequest.RequestNo = serviceId;
                dbContext.OnlineTransferMutations.Add(newTransRequest);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == reqID select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public bool SaveOtherRequest(string desc, int reqID)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == reqID select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.Description = desc;
                    existingRequest.IsActive = true;
                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            return flag;
        }

        public bool SaveMutationRequest(DateTime transDeedDate, string bahiNo, string bahiZildNo, string bahiPgNo, string SINo, int rId, int serviceId, string desc)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var newMutReq = new OnlineTransferMutation();
                newMutReq.Rid = rId;
                newMutReq.RequestNo = serviceId;
                //newMutReq.Mutation_Date = mutDate;
                newMutReq.TransferDeed_Date = transDeedDate;
                newMutReq.Bahi_No = bahiNo;
                newMutReq.Bahi_Zild_No = bahiZildNo;
                newMutReq.Bahi_Page_No = bahiPgNo;
                newMutReq.Bahi_Series_No = SINo;
                newMutReq.Type = Contants.mut;
                newMutReq.Created_Date = DateTime.Now;
                dbContext.OnlineTransferMutations.Add(newMutReq);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == serviceId select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        //registrationno is Service Request No.
        public bool SaveMortgageReq(int rid, int registrationno, string BankName, string BranchAddress, decimal SanctionedAmount, string MortgageType, short PreviousLoanNoc, string desc)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var serviceRequest = new OnlineMortgageDetail();
                serviceRequest.RID = rid;
                serviceRequest.RequestNo = registrationno;
                serviceRequest.BankName = BankName;
                serviceRequest.BranchAddress = BranchAddress;
                serviceRequest.SanctionedAmount = SanctionedAmount;
                serviceRequest.MortgageType = MortgageType;
                serviceRequest.PreviousLoanNoc = PreviousLoanNoc;
                serviceRequest.IsActive = true;
                serviceRequest.CreatedDate = DateTime.Now;
                dbContext.OnlineMortgageDetails.Add(serviceRequest);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == registrationno select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public bool SavegpaRequest(GPAModel objgpaModel)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                OnlineGPA objgpa = new OnlineGPA();
                objgpa.RequestNo = objgpaModel.RequestNo;
                objgpa.Rid = objgpaModel.RId;
                objgpa.GPA_Holder_Name = objgpaModel.GPAHolderName;
                objgpa.GPA_Holder_Address = objgpaModel.GPAHolderAdd;
                objgpa.Effcetd_From = objgpaModel.EffectiveFrom;
                objgpa.Effected_To = objgpaModel.EffectiveTo;
                objgpa.Created_By = 1;
                objgpa.Created_Date = DateTime.Now;
                objgpa.Relation = objgpaModel.RelationName;
                objgpa.Application_Date = objgpaModel.ApplicationDate;
                dbContext.OnlineGPAs.Add(objgpa);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == objgpaModel.RequestNo select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = objgpaModel.Desc;
                }
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }

        public List<DDList> GetOccupation()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from f in dbContext.OccupationMsts
                           select new DDList
                           {
                               id = f.occupationId,
                               text = f.occupation
                           }).ToList();
                return lst;
            }
        }

        public List<DDList> GetGender()
        {
            List<DDList> gender = new List<DDList>();
            //foreach (int value in Enum.GetValues(typeof(NoidaAuthority.PMS.Common.Contants.Gender)))
            //{
            //    gender.Add(new DDList
            //    {
            //        text = Enum.GetName(typeof(NoidaAuthority.PMS.Common.Contants.Gender), value),
            //        id = value
            //    });
            //}
            var obj1 = new DDList();
            var obj2 = new DDList();
            obj1.id = Convert.ToInt32(Contants.Gender.Male);
            obj1.text = Contants.male;
            obj2.id = Convert.ToInt32(Contants.Gender.Female);
            obj2.text = Contants.female;
            gender.Add(obj1);
            gender.Add(obj2);
            return gender;
        }

        public List<DDList> GetTransferTypes()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from transTy in dbContext.Transfer_Type
                           where transTy.Parent_Id == null && transTy.Is_Active == true
                           select new DDList
                           {
                               text = transTy.type,
                               id = transTy.Id
                           }).ToList();
                return lst;
            }
        }

        public List<DDList> GetTransferSubTypes(int TransferType)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from transTy in dbContext.Transfer_Type
                           where transTy.Parent_Id == TransferType && transTy.Is_Active == true
                           select new DDList
                           {
                               text = transTy.type,
                               id = transTy.Id
                           }).ToList();
                return lst;
            }
        }

        public List<DDList> BindDDL(string type)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from ddlBind in dbContext.Common_Config
                           where ddlBind.Is_Active == 1 && ddlBind.Category.ToLower().Equals(type.ToLower())
                           select new DDList
                           {
                               id = ddlBind.Id,
                               text = ddlBind.Name
                           }).ToList();
                return lst;
            }
        }

        public DataSourceResult GetDirectorDetails(DataSourceRequest req, int rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int i = 0;
                var lstDirectorDetails = (from rd in dbContext.OnlineFirmDirectorMasters
                                          join ty in dbContext.Common_Config on rd.Type equals ty.Id
                                          where rd.IsActive == 1 && rd.Rid == rid
                                          select new CICModel
                                          {
                                              Director_Id = rd.CidSid,
                                              Director_Name = rd.DirectorName,
                                              Director_Share = rd.DirectorShare,
                                              TypeName = ty.Name,
                                              SNo = 0
                                          }).ToList();
                foreach (var item in lstDirectorDetails)
                {
                    i = i + 1;
                    item.SNo = i;
                }
                return lstDirectorDetails.ToDataSourceResult(req);
            }
        }

        public bool SaveCIC(int rid, int registrationno, decimal directorShare, int changeType, decimal cicCharge, string directorName, string desc, int type)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var dataResult = new OnlineFirmDirectorMaster
                {
                    Rid = rid,
                    RequestNo = registrationno,
                    DirectorName = directorName,
                    DirectorShare = directorShare,
                    Type = type,
                    IsActive = 1,
                    RequestDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                dbContext.OnlineFirmDirectorMasters.Add(dataResult);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == registrationno select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                return true;
            }
        }

        public int SaveCICFirmName(int rid, int registrationno, int newFirmStatus, string newFirmName, string desc, decimal ciccharge)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var bflag = 0;
                var lstFirstName = new OnlineFirmRequestMaster
                {
                    Rid = rid,
                    RequestNo = registrationno,
                    NewFirmStatus = newFirmStatus,
                    OldFirmStatus = newFirmStatus,
                    NewFirmName = newFirmName,
                    OldFirmName = newFirmName,
                    Status = Contants.RequestInitiated,
                    IsActive = 1,
                    RequestDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CicCharge = ciccharge,
                    ChangeType = Contants.ChangeInFirmName
                };
                dbContext.OnlineFirmRequestMasters.Add(lstFirstName);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == registrationno select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                bflag = 1;
                return bflag;
            }
        }

        public int SaveForFirmProduct(int rid, int registrationno, string newFirmProduct, string desc, decimal ciccharge = 0)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var bflag = 0;
                var lstFirstName = new OnlineFirmRequestMaster
                {
                    Rid = rid,
                    RequestNo = registrationno,
                    NewFirmProduct = newFirmProduct,
                    OldFirmProduct = newFirmProduct,
                    Status = Contants.RequestInitiated,
                    IsActive = 1,
                    RequestDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    CicCharge = ciccharge,
                    ChangeType = Contants.ChangeInProduct
                };
                dbContext.OnlineFirmRequestMasters.Add(lstFirstName);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == registrationno select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                bflag = 1;
                return bflag;
            }
        }

        public bool RemoveRecord(int dirID)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var bflag = false;
                var dResult = dbContext.OnlineFirmDirectorMasters.FirstOrDefault(m => m.CidSid == dirID && m.IsActive == 1);
                if (dResult != null)
                {
                    dResult.IsActive = 0;
                    dResult.ModifiedDate = DateTime.Now;
                    dbContext.SaveChanges();
                    bflag = true;
                }
                return bflag;
            }
        }


        public bool SaveRentServiceReq(int rid, int serviceId, string tenantName, string tenantProject, decimal rentingCharge, DateTime rentingDate, int rentDuration, string desc)
        {
            var flag = false;
            using (var dbContext = new PIMSEntitiesContext())
            {
                var serviceRequest = new OnlineRentPermissionDetail();
                serviceRequest.Rid = rid;
                serviceRequest.RequestNo = serviceId;
                serviceRequest.TenantName = tenantName;
                serviceRequest.TenantProject = tenantProject;
                //serviceRequest.RentingCharge = rentingCharge;
                serviceRequest.RentDurationYears = rentDuration;
                serviceRequest.RentingDate = rentingDate;
                serviceRequest.IsActive = true;
                serviceRequest.CreatedDate = DateTime.Now;
                dbContext.OnlineRentPermissionDetails.Add(serviceRequest);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == serviceId select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                flag = true;
            }
            return flag;
        }


        public object DownloadChallan(int challanId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var challan = dbContext.Challan_Master.Where(c => c.Challan_Id == challanId.ToString()).Select(s => s.Content);
                return challan;
            }
        }


        public int SaveExtensionRequest(int rid, int requestId, DateTime extensionDueDate, DateTime extensionGivenDate, string desc)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var bflag = 0;
                var lstFirstName = new OnlineExtensionDetail
                {
                    Rid = rid,
                    RequestNo = requestId,
                    ExtensionDueDate = extensionDueDate,
                    ExtensionGivenDate = extensionGivenDate,
                    Status = Contants.RequestInitiated,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                };
                dbContext.OnlineExtensionDetails.Add(lstFirstName);
                var existingRequest = (from serv in dbContext.Customer_ServiceRequest where serv.Id == requestId select serv).FirstOrDefault();
                if (existingRequest != null)
                {
                    existingRequest.IsActive = true;
                    existingRequest.Description = desc;
                }
                dbContext.SaveChanges();
                bflag = 1;
                return bflag;
            }
        }

        //public DataSourceResult GetServiceRequestReport(DataSourceRequest request)
        //{
        //    var requests;
        //    var prop;
        //    var lst;
        //    using (var dbContext = new NOIDANEWEntities())
        //    {
        //         lst = (from csr in dbContext.Customer_ServiceRequest
        //                   join view in dbContext.view_property on csr.Registration_No equals view.PRDVREGISTRATION_ID
        //                   join csm in dbContext.CitizenService_Master on csr.ServiceId equals csm.Id
        //                   select new ServiceReportModel
        //                   {
        //                       Id = csr.Id,
        //                       RegistrationNo = csr.Registration_No,
        //                       CreatedDate = csr.Created_Date.Value,
        //                       Description = csr.Description,
        //                       ApplicantName = view.PRDVREGIST_APPLICANT_NAME,
        //                       Sector = view.SECTOR,
        //                       Block = view.BLOCK,
        //                       PropertyNo = view.PLDIPROPERTY_NO,
        //                       ServiceName = csm.ServiceName,
        //                       //Eamil = csr.
        //                       DepartmentName = csr.DepartmentId != 1 ? (csr.DepartmentId != 2 ? (csr.DepartmentId != 3 ? (csr.DepartmentId != 4 ? (csr.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional"
        //                   });
        //        var data = lst.ToDataSourceResult(request);
        //        return data;
        //    }
        //}

        public DataSourceResult GetServiceRequestReport(DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var lst = (from csr in dbContext.View_Service_report
                //           select new ServiceReportModel
                //           {
                //               Id = csr.requestNo,
                //               RegistrationNo = csr.Registration_No,
                //               CreatedDate = csr.Created_Date.Value,
                //               Description = csr.Description,
                //               ApplicantName = csr.PRDVREGIST_APPLICANT_NAME,
                //               //RequestorName = csr.RequestorName,
                //               Sector = csr.SECTOR,
                //               Block = csr.BLOCK,
                //               PropertyNo = csr.PLDIPROPERTY_NO,
                //               ServiceName = csr.ServiceName,
                //               Email = csr.Email,
                //               MobileNo = csr.MobileNumber,
                //               DepartmentName = csr.DepartmentId != 1 ? (csr.DepartmentId != 2 ? (csr.DepartmentId != 3 ? (csr.DepartmentId != 4 ? (csr.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional",
                //               Status = (from st in dbContext.StatusMasters join vw in dbContext.View_Service_report on st.Id equals vw.Request_Status select st.Status).FirstOrDefault()
                //           }).Distinct();
                ////lst.GroupBy(g => g.RegistrationNo).Select(s => s.First());
                //var data = lst.ToDataSourceResult(request);
                //return data;
                return null;
            }
        }

        public DataSourceResult GetReportingService(DataSourceRequest request)
        {
            using (var context = new PIMSEntitiesContext())
            {
                var scheduleDetail =
                      context.Database.SqlQuery<ServiceReportingModel>(
                          "SELECT a.Id AS requestNo, a.Registration_No,b.PRDVREGIST_APPLICANT_NAME, b.sector,b.Block,b.Pldiproperty_no , " +
                          " a.DepartmentId, a.ServiceId, a.Description,a.Created_Date " +
                          " FROM  NoidaUat.dbo.Customer_ServiceRequest a, NoidaDashboardDev.dbo.view_property b " +
                          " where a.Registration_No=b.prdvregistration_id order by    a.Id ");

                var data = scheduleDetail.ToDataSourceResult(request);
                return data;
            }
        }

        public DataSourceResult GetServiceRequestReport(DataSourceRequest request, int? departmentId, DateTime? fromDate, DateTime? toDate, int? serviceId, string registrationId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var lst = (from csr in dbContext.View_Service_report
                //           where (departmentId == null || departmentId == 0 || csr.DepartmentId == departmentId)
                //                && (serviceId == null || serviceId == 0 || csr.ServiceId == serviceId)
                //                && (fromDate == null || DbFunctions.TruncateTime(csr.Created_Date) >= DbFunctions.TruncateTime(fromDate))
                //                && (toDate == null || DbFunctions.TruncateTime(csr.Created_Date) <= DbFunctions.TruncateTime(toDate))
                //                && (registrationId == null || registrationId == "" || csr.Registration_No == registrationId)
                //           select new ServiceReportModel
                //           {
                //               Id = csr.requestNo,
                //               RegistrationNo = csr.Registration_No,
                //               CreatedDate = csr.Created_Date.Value,
                //               Description = csr.Description,
                //               ApplicantName = csr.PRDVREGIST_APPLICANT_NAME,
                //               Sector = csr.SECTOR,
                //               Block = csr.BLOCK,
                //               PropertyNo = csr.PLDIPROPERTY_NO,
                //               ServiceName = csr.ServiceName,
                //               Email = csr.Email,
                //               MobileNo = csr.MobileNumber,
                //               DepartmentName = csr.DepartmentId != 1 ? (csr.DepartmentId != 2 ? (csr.DepartmentId != 3 ? (csr.DepartmentId != 4 ? (csr.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional",
                //               Status = (from st in dbContext.StatusMasters join vw in dbContext.View_Service_report on st.Id equals vw.Request_Status select st.Status).FirstOrDefault(),
                //               SubDepartment = !string.IsNullOrEmpty(csr.SubDepartment) ? (csr.SubDepartment == "P" ? "Property" : "Account") : string.Empty
                //           }).Distinct();
                //var data = lst.ToDataSourceResult(request);
                //return data;
                return null;
            }
        }

        public DataSourceResult GetPropertyDetailsForExtension(DataSourceRequest request, int departmentId, int ServiceId)
        {
            throw new NotImplementedException();
        }

        public DataSourceResult GetPropertySearchList(DtoPropertyFilter objPropertyFilter, DataSourceRequest request)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                string prId = objPropertyFilter.PropertyId > 0 ? Convert.ToString(objPropertyFilter.PropertyId) : null;
                string sec = objPropertyFilter.Sector != "0" ? objPropertyFilter.Sector : null;
                string block = objPropertyFilter.Block != "0" ? objPropertyFilter.Block : null;
                var PropertySearch = (from View_pr in dbContext.view_property
                                      where (prId == null || View_pr.PLDVLAND_USE_TYPE_ID == prId)
                                      && (sec == null || View_pr.SECTOR == sec)
                                      && (block == null || View_pr.BLOCK == block)
                                      && (objPropertyFilter.PlotNumber == null || View_pr.PLDIPROPERTY_NO.Contains(objPropertyFilter.PlotNumber))
                                      select new DtoProperty
                                      {
                                          RID = View_pr.PRDVREGISTRATION_ID,
                                          Block = (View_pr.BLOCK).Trim(),
                                          PropertyNo = View_pr.SECTOR != string.Empty ? (View_pr.SECTOR).Trim() + "/" + (View_pr.BLOCK).Trim() + "-" + (View_pr.PLDIPROPERTY_NO).Trim() : string.Empty,
                                          PropertyType = View_pr.PropertyType,
                                          Sector = View_pr.SECTOR,
                                          PlotNumber = (View_pr.PLDIPROPERTY_NO).Trim(),
                                          AreaSqm = View_pr.PLDRACTUAL_LAND_AREA,
                                          ApplicantName = View_pr.PRDVREGIST_APPLICANT_NAME
                                      });

                var data = PropertySearch.ToDataSourceResult(request);
                return data;
            }
        }

        public DataSourceResult GetServiceRequestForUnRegisteredProperty(DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from csr in dbContext.Customer_ServiceRequest
                           join csm in dbContext.CitizenService_Master on new { X1 = csr.DepartmentId, Y1 = csr.ServiceId } equals new { X1 = csm.Deptt_Id, Y1 = csm.service_id }
                           where (csr.Registration_No == null || csr.Registration_No == "")
                           select new ServiceReportModel
                           {
                               Id = csr.Id,
                               PropertyNo = csr.Property_No,
                               ApplicantName = csr.ApplicantName,
                               ApplicantAddress = csr.ApplicantAddress,
                               RequestorName = csr.RequestorName,
                               RequestorAddress = csr.RequestorAddress,
                               MobileNo = csr.MobileNumber,
                               CreatedDate = csr.Created_Date.Value,
                               Description = csr.Description,
                               ServiceName = csm.ServiceName,
                               DepartmentName = csr.DepartmentId != 1 ? (csr.DepartmentId != 2 ? (csr.DepartmentId != 3 ? (csr.DepartmentId != 4 ? (csr.DepartmentId != 5 ? "Group Housing" : "Housing") : "Industrial") : "Residential") : "Commercial") : "Institutional"
                           });
                //lst.Distinct();
                var data = lst.ToDataSourceResult(request);
                return data;
            }
        }
    }
}
