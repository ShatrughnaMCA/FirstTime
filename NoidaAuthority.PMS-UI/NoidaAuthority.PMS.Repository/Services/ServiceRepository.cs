using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
//using NoidaAuthority.PMS.Repository.Entities;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Repository.Context;

namespace NoidaAuthority.PMS.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public DataSourceResult GetServiceRequestDataAsDataSource(DataSourceRequest request, ServiceViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from csr in dbContext.Customer_ServiceRequest
                            join csm in dbContext.CitizenService_Master on new { y = csr.DepartmentId, x = csr.ServiceId } equals new { y = csm.Deptt_Id, x = csm.service_id }
                            join sts in dbContext.StatusMasters on csr.Request_Status equals sts.Id
                            where csm.Status == 1 //&& DepartmentList.Contains(csr.DepartmentId)
                            && (model.Id == null || csr.Id == model.Id)
                            && (model.ServiceId == null || csr.ServiceId == model.ServiceId)
                            && (model.ServiceStatusId == null || csr.Request_Status == model.ServiceStatusId)
                            && (model.DepartmentId == null || csr.DepartmentId == model.DepartmentId)
                            && (model.StartDate == null || DbFunctions.TruncateTime(csr.Created_Date) >= DbFunctions.TruncateTime(model.StartDate))
                            && (model.EndDate == null || DbFunctions.TruncateTime(csr.Created_Date) <= DbFunctions.TruncateTime(model.EndDate))
                            select new ServiceViewModel
                            {
                                Id = csr.Id,
                                RequestId = csr.Id,
                                RegistrationNo = csr.Registration_No,
                                DepartmentId = csr.DepartmentId,
                                Department = (csr.DepartmentId == null || csr.DepartmentId == 0) ? string.Empty : dbContext.DepartmentMsts.FirstOrDefault(d => d.departmentId == csr.DepartmentId).departmentName,
                                PropertyNo = csr.Property_No != null ? csr.Property_No : string.Empty,
                                RequestDate = csr.Created_Date,
                                CreatedDate = csr.Created_Date,
                                Timeline = csm.Timeline,
                                ServiceId = csr.ServiceId,
                                ServiceName = csm.ServiceName,
                                ServiceType = csr.ServiceType,
                                DuesAmount = csr.DuesAmount == null ? 0 : csr.DuesAmount,
                                Amount = csr.DuesAmount,
                                StatusId = sts.Id,
                                Status = sts.Status,
                                IsCancelled = csr.Request_Status == NAServiceStatusId.Canceled ? true : false,
                                Comment = csr.Comment,
                                MobileNo = csr.MobileNumber,
                                Applicant = csr.ApplicantName,
                                Description = csr.Description,
                                Requestor = csr.RequestorName,
                                RequestorAddress = csr.RequestorAddress,
                                SubDepartment = string.IsNullOrEmpty(csr.SubDepartment) ? SubDepartment.Property : csr.SubDepartment
                            });

                return list.ToDataSourceResult(request);
            }
        }


        public CustomerViewModel GetApplicantDetailsByRegistrationId(int? registrationId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var applicant = (from alotment in dbContext.AllotmentMasters
                                 join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                                 join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                                 where alotment.rid == registrationId && alotment.isActive == 1
                                 select new CustomerViewModel
                                 {
                                     RegistrationId = alotment.rid,
                                     DepartmentId = alotment.departmentId,
                                     Department = alotment.DepartmentMst.departmentName,
                                     SectorId = property.sectorId,
                                     Sector = property.SectorMst.sectorName,
                                     BlockId = property.blockId,
                                     Block = property.BlockMst.blockName,
                                     PlotNo = property.propertyNo,
                                     Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName,
                                     MobileNo = aplicant.tMobileNumber,
                                     Email = aplicant.tEmail,
                                     PermanentAddress = aplicant.tPermanentAdd,
                                     CorrespondAddress = aplicant.tCorrespondanceAdd
                                 }).FirstOrDefault();
                return applicant;
            }
        }

        public string GetFileUploadHtmlForService(int? departmentId, int? serviceId)
        {
            using (var context = new PIMSEntitiesContext())
            {
                var checklists = (from checklist in context.ServiceCheckList_Master
                                  where checklist.dept_id == departmentId && checklist.service_id == serviceId
                                  select new ServiceCheckListModel
                                  {
                                      Id = checklist.ChkId,
                                      DepartmentId = checklist.dept_id,
                                      Department = context.DepartmentMsts.Where(d => d.departmentId == departmentId).Select(d => d.departmentName).FirstOrDefault(),
                                      ServiceId = checklist.service_id,
                                      ChecklistRefNo = checklist.Checklist_Ref,
                                      ChecklistName = checklist.ChkName,
                                      Status = checklist.Status
                                  }).ToList();
                string divMain = "";
                if (checklists.Count != 0)
                {
                    divMain = "<div class='row  file-header'> "
                                    + "<div class='col-md-3 col-lbl-sn'><label>Serial No</label></div>"
                                    + "<div class='col-md-6 col-lbl'><label>Required File</label></div>"
                                    + "<div class='col-md-3 col-lbl-vl'><label>Select File</label></div>"
                                + "</div>";

                    int docCounter = 1;
                    foreach (var item in checklists)
                    {
                        divMain = divMain + "<div class='row  row-border row-file'> "
                                               + "<div class='col-md-3 col-lbl-sn'><label>" + docCounter + "</label></div>"
                                               + "<div class='col-md-6 col-lbl'><label>" + item.ChecklistName + "</label></div>"
                                               + "<div class='col-md-3 col-lbl-vl'><input type='file' class='single' name='files' /></div>"
                                          + "</div>";
                        docCounter++;
                    }
                    return divMain;
                }
                else return null;
                //return divMain;
            }
        }

        public List<DirectorShareholderModel> SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            try
            {
                var directors = (List<DirectorShareholderModel>)HttpContext.Current.Session["TempDirectors"];
                if (directors == null)
                {
                    directors = new List<DirectorShareholderModel>();
                    directors.Add(new DirectorShareholderModel
                    {
                        Id = 1,
                        ShareType = Convert.ToInt32(shareType),
                        ShareholderName = directorName,
                        ShareValue = share
                    });
                    HttpContext.Current.Session["TempDirectors"] = directors;
                }
                else
                {
                    var flag = directors.Count;
                    directors.Add(new DirectorShareholderModel
                    {
                        Id = flag + 1,
                        ShareType = Convert.ToInt32(shareType),
                        ShareholderName = directorName,
                        ShareValue = share
                    });
                    HttpContext.Current.Session["TempDirectors"] = directors;
                }
                return directors;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<System.Web.HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            switch (model.ServiceModel.ServiceId)
            {
                case NAServiceId.Transfer:
                    flag = SaveTransferRequestService(model, files);
                    break;
                case NAServiceId.Rent:
                    flag = SaveRentRequestService(model, files);
                    break;
                case NAServiceId.CIC:
                    flag = SaveCICRequestService(model, files);
                    break;
                case NAServiceId.Mortgage:
                    flag = SaveMortgageRequestService(model, files);
                    break;
                case NAServiceId.Extension:
                    flag = SaveExtensionRequestDetails(model, files);
                    break;
                case NAServiceId.GPA:
                    flag = SaveGPARequestService(model, files);
                    break;
                case NAServiceId.Mutation:
                    flag = SaveMutationRequestService(model, files);
                    break;
                default:
                    flag = SaveOtherRequestService(model, files);
                    break;
            }
            return model;
        }

        private int SaveOtherRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                model.Id = requestNo;
                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }

                return ReturnType.Success;
            }
        }

        private int SaveMutationRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                var exMutation = dbContext.OnlineTransferMutations.FirstOrDefault(m => m.RequestNo == requestNo);
                if (exMutation != null)
                {
                    exMutation.Rid = model.ServiceModel.RegistrationId;
                    exMutation.RequestNo = requestNo;
                    exMutation.Applicant_Name = model.ServiceModel.Applicant;
                    exMutation.Correspondance_Add = model.ServiceModel.ApplicantAddress;
                    exMutation.TransferDeed_Date = model.MutationModel.TransferdeedDate;
                    exMutation.Bahi_No = model.MutationModel.BahiNo;
                    exMutation.Bahi_Zild_No = model.MutationModel.BahiZildNo;
                    exMutation.Bahi_Page_No = model.MutationModel.BahiPageNo;
                    exMutation.Bahi_Series_No = model.MutationModel.BahiSeriesNo;
                    exMutation.Type = "M"; // NAConstant.Mutation;
                    exMutation.Modified_Date = DateTime.Now;
                    //exMutation.Modified_By = userInfo.UserID;
                    exMutation.Comment = model.ServiceModel.Description;
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;

                    var mutation = new OnlineTransferMutation();
                    mutation.Rid = model.ServiceModel.RegistrationId;
                    mutation.RequestNo = requestNo;
                    mutation.Applicant_Name = model.ServiceModel.Applicant;
                    mutation.Correspondance_Add = model.ServiceModel.ApplicantAddress;
                    mutation.TransferDeed_Date = model.MutationModel.TransferdeedDate;
                    mutation.Bahi_No = model.MutationModel.BahiNo;
                    mutation.Bahi_Zild_No = model.MutationModel.BahiZildNo;
                    mutation.Bahi_Page_No = model.MutationModel.BahiPageNo;
                    mutation.Bahi_Series_No = model.MutationModel.BahiSeriesNo;
                    mutation.Type = "M"; // NAConstant.Mutation;
                    mutation.Created_Date = DateTime.Now;
                    //mutation.Created_By = userInfo.UserID;
                    mutation.Comment = model.ServiceModel.Description;
                    dbContext.OnlineTransferMutations.Add(mutation);
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }
                return flag;
            }
        }

        private int SaveGPARequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                var exGPA = dbContext.OnlineGPAs.FirstOrDefault(g => g.RequestNo == requestNo);
                if (exGPA != null)
                {
                    exGPA.Rid = model.ServiceModel.RegistrationId;
                    exGPA.GPA_Holder_Name = model.GPAModel.GPAHolderName;
                    exGPA.GPA_Holder_Address = model.GPAModel.GPAHolderAdd;
                    exGPA.Effcetd_From = model.GPAModel.EffectiveFrom;
                    exGPA.Effected_To = model.GPAModel.EffectiveTo;
                    exGPA.Relation = model.GPAModel.RelationName;
                    exGPA.Application_Date = model.GPAModel.ApplicationDate;
                    //exGPA.Modified_By = userInfo.UserID;
                    exGPA.Modified_date = DateTime.Now;
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;
                    OnlineGPA gpa = new OnlineGPA();
                    gpa.RequestNo = requestNo;
                    gpa.Rid = model.ServiceModel.RegistrationId;
                    gpa.GPA_Holder_Name = model.GPAModel.GPAHolderName;
                    gpa.GPA_Holder_Address = model.GPAModel.GPAHolderAdd;
                    gpa.Effcetd_From = model.GPAModel.EffectiveFrom;
                    gpa.Effected_To = model.GPAModel.EffectiveTo;
                    gpa.Relation = model.GPAModel.RelationName;
                    gpa.Application_Date = model.GPAModel.ApplicationDate;
                    //gpa.Created_By = userInfo.UserID;
                    gpa.Created_Date = DateTime.Now;

                    dbContext.OnlineGPAs.Add(gpa);
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }
                return flag;
            }
        }

        private int SaveExtensionRequestDetails(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                var exExtension = dbContext.OnlineExtensionDetails.FirstOrDefault(e => e.RequestNo == requestNo);
                if (exExtension != null)
                {
                    exExtension.Rid = model.ServiceModel.RegistrationId;
                    exExtension.RequestNo = requestNo;
                    exExtension.ExtensionDueDate = model.ExtensionModel.ExtensionDueDate;
                    exExtension.ExtensionGivenDate = model.ExtensionModel.ExtensionGivenDate;
                    //exExtension.Status = NAStatusId.Initiated;
                    exExtension.IsActive = true;
                    exExtension.ModifiedDate = DateTime.Now;
                    //exExtension.ModifiedBy = userInfo.UserID;
                    exExtension.Comment = model.ServiceModel.Description;
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;
                    OnlineExtensionDetail extension = new OnlineExtensionDetail();
                    extension.Rid = model.ServiceModel.RegistrationId;
                    extension.RequestNo = requestNo;
                    extension.ExtensionDueDate = model.ExtensionModel.ExtensionDueDate;
                    extension.ExtensionGivenDate = model.ExtensionModel.ExtensionGivenDate;
                    extension.Status = NAServiceStatusId.Initiated;
                    extension.IsActive = true;
                    extension.CreatedDate = DateTime.Now;
                    //extension.CreatedBy = userInfo.UserID;
                    extension.Comment = model.ServiceModel.Description;

                    dbContext.OnlineExtensionDetails.Add(extension);
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }
                return flag;
            }
        }

        private int SaveMortgageRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                var exMortgage = dbContext.OnlineMortgageDetails.FirstOrDefault(m => m.RequestNo == requestNo);
                if (exMortgage != null)
                {
                    exMortgage.RID = model.ServiceModel.RegistrationId;
                    exMortgage.RequestNo = requestNo;
                    exMortgage.BankName = model.MortgageModel.BankName;
                    exMortgage.BranchAddress = model.MortgageModel.BranchAddress;
                    exMortgage.SanctionedAmount = model.MortgageModel.SanctionedAmount;
                    exMortgage.MortgageType = model.MortgageModel.MortgageType;
                    exMortgage.PreviousLoanNoc = model.MortgageModel.PreviousLoanNoc;
                    exMortgage.IsActive = true;
                    exMortgage.Comment = model.ServiceModel.Description;
                    exMortgage.CommentDate = DateTime.Now;
                    //exMortgage.StatusId = NAStatusId.Initiated;
                    exMortgage.ModifiedDate = DateTime.Now;
                    //exMortgage.Modifiedby = userInfo.UserID;
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;
                    var mortgage = new OnlineMortgageDetail();
                    mortgage.RID = model.ServiceModel.RegistrationId;
                    mortgage.RequestNo = requestNo;
                    mortgage.BankName = model.MortgageModel.BankName;
                    mortgage.BranchAddress = model.MortgageModel.BranchAddress;
                    mortgage.SanctionedAmount = model.MortgageModel.SanctionedAmount;
                    mortgage.MortgageType = model.MortgageModel.MortgageType;
                    mortgage.PreviousLoanNoc = model.MortgageModel.PreviousLoanNoc;
                    mortgage.IsActive = true;
                    mortgage.CreatedDate = DateTime.Now;
                    //mortgage.CreatedBy = userInfo.UserID;
                    mortgage.Comment = model.ServiceModel.Description;
                    mortgage.CommentDate = DateTime.Now;
                    mortgage.StatusId = NAServiceStatusId.Initiated;

                    dbContext.OnlineMortgageDetails.Add(mortgage);
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }

            }
            return flag;
        }

        private int SaveCICRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                model.Id = requestNo;
                if (model.CICModel.ChangeTypeId == NAServiceId.ChangeInDirector)
                {
                    var masterList = (List<DirectorShareholderModel>)HttpContext.Current.Session["TempDirectors"];
                    if (masterList != null)
                    {
                        var exDirectors = dbContext.OnlineFirmDirectorMasters.Where(f => f.RequestNo == requestNo).ToList();
                        if (exDirectors != null)
                        {
                            exDirectors.RemoveAll(r => r.RequestNo == requestNo);
                        }
                        List<OnlineFirmDirectorMaster> firmMaster = new List<OnlineFirmDirectorMaster>();
                        foreach (var master in masterList)
                        {
                            OnlineFirmDirectorMaster director = new OnlineFirmDirectorMaster();
                            director.RequestNo = requestNo;
                            director.Rid = model.ServiceModel.RegistrationId;
                            director.DirectorName = master.ShareholderName;
                            director.DirectorShare = master.ShareValue;
                            director.Type = master.ShareType;
                            director.IsActive = 1;
                            director.RequestDate = DateTime.Now;
                            director.CreatedDate = DateTime.Now;
                            //director.CreatedBy = userInfo.UserID;
                            firmMaster.Add(director);
                        }
                        dbContext.OnlineFirmDirectorMasters.AddRange(firmMaster);
                        dbContext.SaveChanges();
                        if (files != null)
                        {
                            if (files.ToList().Count > 0)
                            {
                                FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                            }
                        }
                        flag = ReturnType.Success;
                    }
                    else
                    {
                        flag = ReturnType.NotExist;
                    }
                }
                if (model.CICModel.ChangeTypeId == NAServiceId.ChangeInFirmName)
                {
                    var exFirm = dbContext.OnlineFirmRequestMasters.FirstOrDefault(o => o.RequestNo == requestNo);
                    if (exFirm != null)
                    {
                        exFirm.Rid = model.ServiceModel.RegistrationId;
                        exFirm.OldFirmName = model.CICModel.OldFirmName;
                        exFirm.NewFirmName = model.CICModel.NewFirmName;
                        exFirm.NewFirmStatus = model.CICModel.NewFirmStatusId;
                        //exFirm.Status = NAStatusId.Initiated;
                        exFirm.IsActive = 1;
                        exFirm.RequestDate = DateTime.Now;
                        exFirm.ChangeType = NAServiceId.ChangeInFirmName;
                        exFirm.ModifiedDate = DateTime.Now;
                        //exFirm.ModifiedBy = userInfo.UserID;
                        dbContext.SaveChanges();
                        flag = ReturnType.Success;
                    }
                    else
                    {
                        OnlineFirmRequestMaster firm = new OnlineFirmRequestMaster();
                        firm.RequestNo = requestNo;
                        firm.Rid = model.ServiceModel.RegistrationId;
                        //firm.OldFirmName = model.CICmodel.OldFirmName;
                        firm.NewFirmName = model.CICModel.NewFirmName;
                        //firm.OldFirmStatus = model.CICmodel.OldFirmStatus;
                        firm.NewFirmStatus = model.CICModel.NewFirmStatusId;
                        firm.Status = NAServiceStatusId.Initiated;
                        firm.IsActive = 1;
                        firm.RequestDate = DateTime.Now;
                        firm.CreatedDate = DateTime.Now;
                        firm.ChangeType = NAServiceId.ChangeInFirmName;
                        //firm.CreatedBy = userInfo.UserID;
                        dbContext.OnlineFirmRequestMasters.Add(firm);
                        dbContext.SaveChanges();
                        flag = ReturnType.Success;
                    }

                    if (files != null)
                    {
                        if (files.ToList().Count > 0)
                        {
                            FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                        }
                    }
                    //flag = ReturnType.Success;
                }
                if (model.CICModel.ChangeTypeId == NAServiceId.ChangeInProduct)
                {
                    var exProduct = dbContext.OnlineFirmRequestMasters.FirstOrDefault(e => e.RequestNo == requestNo);
                    if (exProduct != null)
                    {
                        exProduct.Rid = model.ServiceModel.RegistrationId;
                        exProduct.OldFirmProduct = model.CICModel.OldFirmProduct;
                        exProduct.NewFirmProduct = model.CICModel.NewFirmProduct;
                        exProduct.Status = NAServiceStatusId.Initiated;
                        exProduct.IsActive = 1;
                        exProduct.RequestDate = DateTime.Now;
                        exProduct.ModifiedDate = DateTime.Now;
                        //exProduct.ModifiedBy = userInfo.UserID;
                        exProduct.ChangeType = NAServiceId.ChangeInProduct;
                        dbContext.SaveChanges();
                        flag = ReturnType.Success;
                    }
                    else
                    {
                        OnlineFirmRequestMaster firm = new OnlineFirmRequestMaster();
                        firm.RequestNo = requestNo;
                        firm.Rid = model.ServiceModel.RegistrationId;
                        //firm.OldFirmProduct = model.CICmodel.OldFirmProduct;
                        firm.NewFirmProduct = model.CICModel.NewFirmProduct;
                        firm.Status = NAServiceStatusId.Initiated;
                        firm.IsActive = 1;
                        firm.RequestDate = DateTime.Now;
                        firm.CreatedDate = DateTime.Now;
                        //firm.CreatedBy = userInfo.UserID;
                        firm.ChangeType = NAServiceId.ChangeInProduct;

                        dbContext.OnlineFirmRequestMasters.Add(firm);
                        dbContext.SaveChanges();
                        flag = ReturnType.Success;
                    }

                    if (files != null)
                    {
                        if (files.ToList().Count > 0)
                        {
                            FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                        }
                    }
                    //flag = ReturnType.Success;
                }
                return flag;
            }
        }

        private int SaveRentRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                var exRent = dbContext.OnlineRentPermissionDetails.FirstOrDefault(r => r.RequestNo == requestNo);
                if (exRent != null)
                {
                    exRent.Rid = model.ServiceModel.RegistrationId;
                    exRent.TenantName = model.RentModel.TenantName;
                    exRent.TenantProject = model.RentModel.TenantProject;
                    exRent.RentDurationYears = model.RentModel.RentDuration;
                    exRent.RentingDate = model.RentModel.RentingDate;
                    exRent.IsActive = true;
                    //exRent.StatusId = NAStatusId.Initiated;
                    exRent.Comment = exRent.Comment + "</br>" + model.ServiceModel.Description;
                    exRent.CommentDate = DateTime.Now;
                    exRent.ModifiedDate = DateTime.Now;
                    //exRent.Modifiedby = userInfo.UserID;
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;
                    var rent = new OnlineRentPermissionDetail();
                    rent.Rid = model.ServiceModel.RegistrationId;
                    rent.RequestNo = requestNo;
                    rent.TenantName = model.RentModel.TenantName;
                    rent.TenantProject = model.RentModel.TenantProject;
                    rent.RentDurationYears = model.RentModel.RentDuration;
                    rent.RentingDate = model.RentModel.RentingDate;
                    rent.IsActive = true;
                    rent.CreatedDate = DateTime.Now;
                    rent.StatusId = NAServiceStatusId.Initiated;
                    rent.Comment = model.ServiceModel.Description;
                    rent.CommentDate = DateTime.Now;
                    //rent.CreatedBy = userInfo.UserID;
                    dbContext.OnlineRentPermissionDetails.Add(rent);
                    dbContext.SaveChanges();
                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }
                //var flag = SaveDocumentsForServiceRequest(model, files);

                return flag;
            }
        }

        private int SaveTransferRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                //var exTransfer = dbContext.Succ_Mut_Trans.FirstOrDefault(t => t.Request_No == requestNo);
                var exTransfer = dbContext.OnlineTransferMutations.FirstOrDefault(t => t.RequestNo == requestNo);
                if (exTransfer != null)
                {
                    exTransfer.Rid = model.ServiceModel.RegistrationId;
                    exTransfer.Type = "T"; // NAConstant.Transfer;
                    exTransfer.Transfer_Type = model.TransferModel.TransferTypeId;
                    exTransfer.Transfer_Sub_Type = model.TransferModel.TransferSubTypeId;
                    //exTransfer.Created_Date = DateTime.Now;

                    if (model.TransferModel.TypeOfTransferee == NAConstant.Company)
                    {
                        exTransfer.T_Gender = "Company";
                        exTransfer.T_Company_Name = model.TransferModel.CompanyName;
                        exTransfer.T_Signing_Authority = model.TransferModel.SigningAuthority;
                        exTransfer.T_Registered_Office = model.TransferModel.RegisteredOffice;
                    }
                    else
                    {
                        exTransfer.T_Gender = model.TransferModel.Gender;
                        exTransfer.T_First_Name = model.TransferModel.FirstName;
                        exTransfer.T_Middle_Name = model.TransferModel.MiddleName;
                        exTransfer.T_Last_Name = model.TransferModel.LastName;
                        exTransfer.T_Father_Husband_Name = model.TransferModel.ApplicantMaster;
                        exTransfer.T_Mother_Name = model.TransferModel.MotherName;
                    }
                    exTransfer.T_Email = model.TransferModel.Email;
                    exTransfer.T_Mobile = model.TransferModel.Mobile;
                    exTransfer.T_Correspondence_Add = model.TransferModel.CorrespondenceAdd;
                    exTransfer.T_Permanent_Add = model.TransferModel.PermanentAdd;
                    exTransfer.T_Occupation_Id = model.TransferModel.OccupationId;
                    exTransfer.T_Pan = model.TransferModel.PAN;
                    exTransfer.Applicant_Name = model.ServiceModel.Applicant;
                    exTransfer.Correspondance_Add = model.ServiceModel.ApplicantAddress;
                    //exTransfer.Modified_By = userInfo.UserID;
                    exTransfer.Modified_Date = DateTime.Now;
                    dbContext.SaveChanges();

                    flag = ReturnType.Success;
                }
                else
                {
                    model.Id = requestNo;
                    var transfer = new OnlineTransferMutation();
                    transfer.Rid = model.ServiceModel.RegistrationId;
                    transfer.RequestNo = requestNo;
                    //transfer.Transfer_Date = transDate;
                    transfer.Type = "T"; // NAConstant.Transfer;
                    transfer.Transfer_Type = model.TransferModel.TransferTypeId;
                    transfer.Transfer_Sub_Type = model.TransferModel.TransferSubTypeId;
                    transfer.Created_Date = DateTime.Now;

                    if (model.TransferModel.TypeOfTransferee == NAConstant.Company)
                    {
                        transfer.T_Gender = "Company";
                        transfer.T_Signing_Authority = model.TransferModel.SigningAuthority;
                        transfer.T_Registered_Office = model.TransferModel.RegisteredOffice;
                    }
                    else
                    {
                        transfer.T_Gender = model.TransferModel.Gender;
                        transfer.T_First_Name = model.TransferModel.FirstName;
                        transfer.T_Middle_Name = model.TransferModel.MiddleName;
                        transfer.T_Last_Name = model.TransferModel.LastName;
                        transfer.T_Father_Husband_Name = model.TransferModel.ApplicantMaster;
                        transfer.T_Mother_Name = model.TransferModel.MotherName;
                    }
                    transfer.T_Email = model.TransferModel.Email;
                    transfer.T_Mobile = model.TransferModel.Mobile;
                    transfer.T_Correspondence_Add = model.TransferModel.CorrespondenceAdd;
                    transfer.T_Permanent_Add = model.TransferModel.PermanentAdd;
                    transfer.T_Occupation_Id = model.TransferModel.OccupationId;
                    transfer.T_Pan = model.TransferModel.PAN;
                    //transfer.RequestNo = model.ServiceRequestId;
                    transfer.Applicant_Name = model.ServiceModel.Applicant;
                    transfer.Correspondance_Add = model.ServiceModel.ApplicantAddress;
                    //transfer.Created_By = userInfo.UserID;
                    transfer.Created_Date = DateTime.Now;
                    dbContext.OnlineTransferMutations.Add(transfer);
                    dbContext.SaveChanges();

                    flag = ReturnType.Success;
                }

                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {

                        FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), requestNo);
                    }
                }

                return flag;
            }
        }

        private int SaveCustomerServiceRequestDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (model.Id != null && model.Id > 0)
                {
                    var exService = dbContext.Customer_ServiceRequest.FirstOrDefault(c => c.Id == model.Id);
                    if (exService != null)
                    {
                        exService.Registration_No = model.ServiceModel.RegistrationId.ToString();
                        exService.Property_No = !string.IsNullOrEmpty(model.ServiceModel.PropertyNo) ? model.ServiceModel.PropertyNo : model.ServiceModel.Sector + "/" + model.ServiceModel.Block + "-" + model.ServiceModel.PlotNo;
                        exService.ServiceId = model.ServiceModel.ServiceId;
                        exService.ServiceType = model.ServiceModel.RegistrationType == NAConstant.PradhikaranDiwas ? NAConstant.SDService : NAConstant.JSKService;
                        exService.DepartmentId = model.ServiceModel.DepartmentId;
                        //exService.SubDepartment = model.ServiceModel.SubDepartment;
                        exService.SubDepartment = model.ServiceModel.ServiceId == NAServiceId.NDC || model.ServiceModel.ServiceId == NAServiceId.DuesCalculation ? "Account" : "Property";
                        exService.ApplicantName = model.ServiceModel.Applicant;
                        exService.SectorName = model.ServiceModel.Sector;
                        exService.BlockName = model.ServiceModel.Block;
                        exService.PlotNo = model.ServiceModel.PlotNo;
                        exService.MobileNumber = model.ServiceModel.MobileNo;
                        exService.Email = model.ServiceModel.Email;
                        exService.ApplicantAddress = model.ServiceModel.ApplicantAddress;
                        exService.Description = model.ServiceModel.Description;
                        exService.IsActive = true;
                        //exService.Request_Status = NAStatusId.Initiated;
                        exService.RequestorName = model.ServiceModel.Requestor != null ? model.ServiceModel.Requestor : model.ServiceModel.Applicant;
                        exService.RequestorAddress = model.ServiceModel.RequestorAddress != null ? model.ServiceModel.RequestorAddress : model.ServiceModel.ApplicantAddress;
                        exService.Modified_Date = DateTime.Now;
                        //exService.Modified_By = userInfo.UserID;
                        dbContext.SaveChanges();

                        return exService.Id;
                    }
                    else return (int)model.Id;
                }
                else
                {
                    Customer_ServiceRequest request = new Customer_ServiceRequest();
                    request.Registration_No = model.ServiceModel.RegistrationId.ToString();
                    request.Property_No = model.ServiceModel.Sector + "/" + model.ServiceModel.Block + "-" + model.ServiceModel.PlotNo;
                    request.ServiceId = model.ServiceModel.ServiceId;
                    request.ServiceType = model.ServiceModel.RegistrationType == NAConstant.PradhikaranDiwas ? NAConstant.SDService : NAConstant.JSKService;
                    request.DepartmentId = model.ServiceModel.DepartmentId;
                    //request.SubDepartment = model.ServiceModel.SubDepartment;
                    request.SubDepartment = model.ServiceModel.ServiceId == NAServiceId.NDC || model.ServiceModel.ServiceId == NAServiceId.DuesCalculation ? "Account" : "Property";
                    request.ApplicantName = model.ServiceModel.Applicant;
                    request.SectorName = model.ServiceModel.Sector;
                    request.BlockName = model.ServiceModel.Block;
                    request.PlotNo = model.ServiceModel.PlotNo;
                    request.MobileNumber = model.ServiceModel.MobileNo;
                    request.Email = model.ServiceModel.Email;
                    request.ApplicantAddress = model.ServiceModel.ApplicantAddress;
                    request.Description = model.ServiceModel.Description;
                    request.IsActive = true;
                    request.Request_Status = NAServiceStatusId.Initiated;
                    request.RequestorName = model.ServiceModel.Requestor != null ? model.ServiceModel.Requestor : model.ServiceModel.Applicant;
                    request.RequestorAddress = model.ServiceModel.RequestorAddress != null ? model.ServiceModel.RequestorAddress : model.ServiceModel.ApplicantAddress;
                    request.Created_Date = DateTime.Now;
                    request.Created_By = 0;
                    request.RequestThrough = "Online";
                    //request.Created_By = userInfo.UserID;
                    dbContext.Customer_ServiceRequest.Add(request);
                    dbContext.SaveChanges();

                    model.Id = request.Id;
                    model.RegistrationId = model.ServiceModel.RegistrationId;
                    model.ServiceModel.Id = request.Id;
                    model.ServiceModel.PropertyNo = request.Property_No;
                    model.ServiceModel.PropertyNo = request.Property_No;
                    model.ServiceModel.CreatedDate = DateTime.Now;

                    //string message = string.Format(NAMessages.OnlineServiceRequest, request.Id);
                    //ApplicationHelper.SendSMS(request.MobileNumber, message);
                    //if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);

                    //int requestNo = (from req in dbContext.Customer_ServiceRequest where req.Registration_No == model.RegistrationId.ToString() && req.Description == model.Description && req.Request_Status == StatusType.Initiated && req.IsActive == true select req.Id).FirstOrDefault();
                    return request.Id;
                }
            }
        }

        private int SaveDocumentsForServiceRequest(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            if (files != null)
            {
                if (files.ToList().Count > 0)
                {
                    FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), (int)model.Id);
                }
            }
            return flag;
        }


        public ServiceRequestViewModel GetServiceRequestDetailById(int? id)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                ServiceRequestViewModel model = new ServiceRequestViewModel();

                var service = (from csr in dbContext.Customer_ServiceRequest
                               where csr.Id == id && csr.IsActive == true
                               select new ServiceViewModel
                               {
                                   Id = csr.Id,
                                   RequestId = csr.Id,
                                   RegistrationNo = csr.Registration_No,
                                   Applicant = csr.ApplicantName,
                                   ApplicantAddress = csr.ApplicantAddress,
                                   Requestor = csr.RequestorName,
                                   RequestorAddress = csr.RequestorAddress,
                                   PropertyNo = csr.Property_No,
                                   MobileNo = csr.MobileNumber,
                                   Email = csr.Email,
                                   ServiceId = csr.ServiceId,
                                   ServiceName = dbContext.CitizenService_Master.FirstOrDefault(x => x.Deptt_Id == csr.DepartmentId && x.service_id == csr.ServiceId && x.Status == 1).ServiceName,
                                   DepartmentId = csr.DepartmentId,
                                   Department = dbContext.DepartmentMsts.FirstOrDefault(x => x.departmentId == csr.DepartmentId && x.IsActive == true).departmentName,
                                   SubDepartment = string.IsNullOrEmpty(csr.SubDepartment) ? SubDepartment.Property : csr.SubDepartment,
                                   SubDepartmentId = string.IsNullOrEmpty(csr.SubDepartment) ? 1 : dbContext.SubDepartmentMsts.FirstOrDefault(x => x.departmentId == csr.DepartmentId && x.SubdepartmentName == csr.SubDepartment && x.IsActive == true).SubdepartmentId,
                                   Description = csr.Description,
                                   RegistrationType = csr.ServiceType == NAConstant.SDService ? NAConstant.PradhikaranDiwas : (string.IsNullOrEmpty(csr.Registration_No) ? NAConstant.UnRegistered : NAConstant.Registered),
                                   ServiceStatusId = csr.Request_Status,
                                   RequestStatus = dbContext.StatusMasters.FirstOrDefault(m => m.Id == csr.Request_Status && m.IsActive == true).Status,
                                   Comment = csr.Comment
                               }).FirstOrDefault();

                if (!string.IsNullOrEmpty(service.RegistrationNo)) service.RegistrationId = Convert.ToInt32(service.RegistrationNo);

                //if (!string.IsNullOrEmpty(service.Property_No))
                //{
                //    var property = service.Property_No.Split('/');
                //    if (property != null)
                //    {
                //        service.Sector = property[0];
                //        var blck = property[1].Split('-');
                //        if (blck != null)
                //        {
                //            service.Block = blck[0];
                //            service.PlotNo = blck[1];
                //        }
                //    }
                //}
                model.Id = service.Id;
                model.RegistrationId = !string.IsNullOrEmpty(service.RegistrationNo) ? Convert.ToInt32(service.RegistrationNo) : 0;
                model.OnlineRequestId = service.Id;
                model.ServiceModel = service;

                // get detail by service id
                switch (service.ServiceId)
                {
                    case NAServiceId.Transfer:
                        model = GetTransferRequestServiceDetail(model);
                        break;
                    case NAServiceId.Rent:
                        model = GetRentRequestServiceDetail(model);
                        break;
                    case NAServiceId.CIC:
                        model = GetCICRequestServiceDetail(model);
                        break;
                    case NAServiceId.Mortgage:
                        model = GetMortgageRequestServiceDetail(model);
                        break;
                    case NAServiceId.Extension:
                        model = GetExtensionRequestDetails(model);
                        break;
                    case NAServiceId.GPA:
                        model = GetGPARequestServiceDetail(model);
                        break;
                    case NAServiceId.Mutation:
                        model = GetMutationRequestServiceDetail(model);
                        break;
                    default:
                        //model = model;
                        break;
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetTransferRequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var transfer = dbContext.OnlineTransferMutations.Where(m => m.RequestNo == model.Id && m.Type == "T").FirstOrDefault();
                if (transfer != null)
                {
                    TransferViewModel tmodel = new TransferViewModel
                    {
                        TransferTypeId = transfer.Transfer_Type.Value,
                        TransferType = dbContext.Transfer_Type.Where(t => t.Id == transfer.Transfer_Type).Select(t => t.type).FirstOrDefault(),
                        TransferSubTypeId = transfer.Transfer_Sub_Type.Value,
                        TransferSubType = dbContext.Transfer_Type.Where(t => t.Id == transfer.Transfer_Sub_Type).Select(t => t.type).FirstOrDefault(),
                        Gender = (transfer.T_Gender == NAConstant.Male || transfer.T_Gender == NAConstant.Female) ? NAConstant.Individual : NAConstant.Company,
                        ApplicantType = transfer.T_Gender,
                        FirstName = transfer.T_First_Name,
                        MiddleName = transfer.T_Middle_Name,
                        LastName = transfer.T_Middle_Name,
                        Applicant = transfer.T_Gender == NAConstant.Company ? transfer.T_Company_Name : transfer.T_First_Name + " " + (string.IsNullOrEmpty(transfer.T_Middle_Name) ? string.Empty : transfer.T_Middle_Name + " ") + transfer.T_Last_Name,
                        CompanyName = transfer.T_Company_Name,
                        SigningAuthority = transfer.T_Signing_Authority,
                        RegisteredOffice = transfer.T_Registered_Office,
                        ApplicantMaster = transfer.T_Father_Husband_Name,
                        MotherName = transfer.T_Mother_Name,
                        PermanentAdd = transfer.T_Permanent_Add,
                        CorrespondenceAdd = transfer.T_Correspondence_Add,
                        PAN = transfer.T_Pan,
                        OccupationId = transfer.T_Occupation_Id,
                        Occupation = dbContext.OccupationMsts.Where(m => m.occupationId == transfer.T_Occupation_Id).Select(o => o.occupation).FirstOrDefault(),
                        Mobile = transfer.T_Mobile,
                        Email = transfer.T_Email,
                        TypeOfTransferee = (transfer.T_Gender == NAConstant.Male || transfer.T_Gender == NAConstant.Female) ? NAConstant.Individual : NAConstant.Company
                    };

                    model.TransferModel = tmodel;
                }
                else
                {
                    model.TransferModel = new TransferViewModel();
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetRentRequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var rents = dbContext.OnlineRentPermissionDetails.Where(r => r.RequestNo == model.Id).FirstOrDefault();
                if (rents != null)
                {
                    RentingViewModel rent = new RentingViewModel
                    {
                        TenantName = rents.TenantName,
                        TenantProject = rents.TenantProject,
                        RentDuration = rents.RentDurationYears,
                        RentingDate = rents.RentingDate
                    };
                    model.RentModel = rent;
                    //return model;
                }
                else
                {
                    model.RentModel = new RentingViewModel();
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetCICRequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var directors = dbContext.OnlineFirmDirectorMasters.Where(m => m.RequestNo == model.Id && m.IsActive == 1).ToList();
                var firms = dbContext.OnlineFirmRequestMasters.Where(m => m.RequestNo == model.Id && m.IsActive == 1).FirstOrDefault();
                CICViewModel cic = new CICViewModel();
                if (directors != null)
                {
                    List<DirectorShareholderModel> dlist = new List<DirectorShareholderModel>();
                    foreach (var drtor in directors)
                    {
                        dlist.Add(new DirectorShareholderModel
                        {
                            ShareType = drtor.Type,
                            ShareholderName = drtor.DirectorName,
                            ShareValue = drtor.DirectorShare
                        });
                    }
                    //model.CICModel.DirectorsModel = dlist;
                    cic.ChangeTypeId = NAServiceId.ChangeInDirector;
                    model.CICModel = cic;
                    return model;
                }
                if (firms != null && firms.ChangeType == NAServiceId.ChangeInFirmName)
                {
                    cic.OldFirmName = firms.OldFirmName;
                    cic.NewFirmName = firms.NewFirmName;
                    cic.ChangeTypeId = NAServiceId.ChangeInFirmName;
                    model.CICModel = cic;
                    //return model;
                }
                if (firms != null && firms.ChangeType == NAServiceId.ChangeInProduct)
                {
                    cic.OldFirmProduct = firms.OldFirmProduct;
                    cic.NewFirmProduct = firms.NewFirmProduct;
                    cic.ChangeTypeId = NAServiceId.ChangeInProduct;
                    model.CICModel = cic;
                    //return model;
                }
                else
                {
                    model.CICModel = cic;
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetMortgageRequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = dbContext.OnlineMortgageDetails.Where(m => m.RequestNo == model.Id).FirstOrDefault();
                if (data != null)
                {
                    MortgageViewModel mortgage = new MortgageViewModel
                    {
                        SanctionedAmount = data.SanctionedAmount,
                        BankName = data.BankName,
                        BranchAddress = data.BranchAddress,
                        MortgageType = data.MortgageType,
                        PreviousLoanNoc = data.PreviousLoanNoc
                    };
                    model.MortgageModel = mortgage;
                    //return model;
                }
                else
                {
                    model.MortgageModel = new MortgageViewModel();
                }

                return model;
            }
        }

        private ServiceRequestViewModel GetExtensionRequestDetails(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = dbContext.OnlineExtensionDetails.Where(m => m.RequestNo == model.Id).FirstOrDefault();
                if (data != null)
                {
                    ExtensionViewModel extension = new ExtensionViewModel
                    {
                        ExtensionDueDate = data.ExtensionDueDate,
                        ExtensionGivenDate = data.ExtensionGivenDate
                    };
                    model.ExtensionModel = extension;
                    //return model;
                }
                else
                {
                    model.ExtensionModel = new ExtensionViewModel();
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetGPARequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = dbContext.OnlineGPAs.Where(m => m.RequestNo == model.Id).FirstOrDefault();
                if (data != null)
                {
                    GPAViewModel gpa = new GPAViewModel
                    {
                        Id = data.GpaSid,
                        GPAId = data.GpaSid,
                        EffectiveFrom = data.Effcetd_From,
                        EffectiveTo = data.Effected_To,
                        RelationName = data.Relation,
                        ApplicationDate = data.Application_Date,
                        GPAHolderName = data.GPA_Holder_Name,
                        GPAHolderAdd = data.GPA_Holder_Address,
                        IsGPARegistered = data.Registered,
                        GPARegisteredNo = data.Registration_No,
                        IsGPAActive = data.Is_Active
                    };
                    model.GPAModel = gpa;
                    //return model;
                }
                else
                {
                    model.GPAModel = new GPAViewModel();
                }
                return model;
            }
        }

        private ServiceRequestViewModel GetMutationRequestServiceDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = dbContext.OnlineTransferMutations.Where(m => m.RequestNo == model.Id).FirstOrDefault();
                if (data != null && data.Type == NAConstant.Mutation)
                {
                    TransferViewModel mutation = new TransferViewModel
                    {
                        TransferdeedDate = data.TransferDeed_Date,
                        BahiNo = data.Bahi_No,
                        BahiZildNo = data.Bahi_Zild_No,
                        BahiPageNo = data.Bahi_Page_No,
                        BahiSeriesNo = data.Bahi_Series_No
                    };
                    model.MutationModel = mutation;
                    //return model;
                }
                else
                {
                    model.MutationModel = new TransferViewModel();
                }

                return model;
            }
        }


        public DataSourceResult GetServiceRequestReportAsDataSource(DataSourceRequest request, ServiceViewModel model)
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

                var list = (from csr in dbContext.Customer_ServiceRequest
                            join csm in dbContext.CitizenService_Master on new { y = csr.DepartmentId, x = csr.ServiceId } equals new { y = csm.Deptt_Id, x = csm.service_id }
                            join sts in dbContext.StatusMasters on csr.Request_Status equals sts.Id
                            where csm.Status == 1 //&& DepartmentList.Contains(csr.DepartmentId)
                            && (model.Id == null || csr.Id == model.Id)
                            && (model.ServiceId == null || csr.ServiceId == model.ServiceId)
                            && (model.ServiceStatusId == null || csr.Request_Status == model.ServiceStatusId)
                            && (model.DepartmentId == null || csr.DepartmentId == model.DepartmentId)
                            && (model.StartDate == null || DbFunctions.TruncateTime(csr.Created_Date) >= DbFunctions.TruncateTime(model.StartDate))
                            && (model.EndDate == null || DbFunctions.TruncateTime(csr.Created_Date) <= DbFunctions.TruncateTime(model.EndDate))
                            select new ServiceViewModel
                            {
                                Id = csr.Id,
                                RequestId = csr.Id,
                                RegistrationNo = csr.Registration_No,
                                DepartmentId = csr.DepartmentId,
                                Department = (csr.DepartmentId == null || csr.DepartmentId == 0) ? string.Empty : dbContext.DepartmentMsts.FirstOrDefault(d => d.departmentId == csr.DepartmentId).departmentName,
                                PropertyNo = csr.Property_No != null ? csr.Property_No : string.Empty,
                                RequestDate = csr.Created_Date,
                                CreatedDate = csr.Created_Date,
                                Timeline = csm.Timeline,
                                ServiceId = csr.ServiceId,
                                ServiceName = csm.ServiceName,
                                ServiceType = csr.ServiceType,
                                DuesAmount = csr.DuesAmount == null ? 0 : csr.DuesAmount,
                                Amount = csr.DuesAmount,
                                StatusId = sts.Id,
                                Status = sts.Status,
                                IsCancelled = csr.Request_Status == NAServiceStatusId.Canceled ? true : false,
                                Comment = csr.Comment,
                                MobileNo = csr.MobileNumber,
                                Applicant = csr.ApplicantName,
                                Description = csr.Description,
                                Requestor = csr.RequestorName,
                                RequestorAddress = csr.RequestorAddress,
                                SubDepartment = string.IsNullOrEmpty(csr.SubDepartment) ? SubDepartment.Property : csr.SubDepartment
                            });
                return list.ToDataSourceResult(request);
            }
        }


        public int GetRaisedServiceRequestStatusById(ServiceViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                List<int?> statuslist = new List<int?> { 4,5,6,8,10,14,18 };
                int flag = ReturnType.None;
                var service = dbContext.Customer_ServiceRequest.FirstOrDefault(f => f.Registration_No == model.RegistrationId.ToString() && f.ServiceId == model.ServiceId && f.IsActive == true && statuslist.Contains(f.Request_Status));
                if (service != null) flag = ReturnType.Exist;
                return flag;
            }
        }


        public int UpdateServiceRequestStatus(ActionViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int flag = ReturnType.None;
                var service = dbContext.Customer_ServiceRequest.FirstOrDefault(f => f.Id==model.Id && f.RequestThrough=="Online");
                var status = dbContext.Customer_ServiceStatusTrans.Where(c => c.RequestRefId == model.Id).OrderByDescending(c => c.Id).FirstOrDefault();
                if (model.ActionType == "Resubmit")
                {
                    if (service != null)
                    {
                        service.Request_Status = (service.Request_Status==NAStatusId.Objection && model.StatusId==NAStatusId.Resubmitted) ? NAStatusId.Resubmitted : service.Request_Status;
                        service.Comment = string.IsNullOrEmpty(model.Message) ? string.Empty : service.Comment + "\n" + model.Message + " Regards, Allottee";
                        service.Modified_By = 0;
                        service.Modified_Date = DateTime.Now;
                        dbContext.SaveChanges();
                        if (status != null)
                        {
                            status.StatusId = model.StatusId;
                            status.ModifiedBy = Convert.ToInt32(service.Registration_No);
                            status.ModifiedDate = DateTime.Now;
                            status.Remarks = status.Remarks + "\n" + model.Message + " by \n" + "Allottee" + " Date:" + DateTime.Now + ".";
                            dbContext.SaveChanges();
                        }
                        flag = ReturnType.Success;
                    }
                }
                else
                {
                    if (service != null && service.Request_Status == NAStatusId.Initiated)
                    {
                        service.Request_Status = NAStatusId.Withdraw;
                        service.Modified_By = 0;
                        service.Modified_Date = DateTime.Now;
                        dbContext.SaveChanges();
                        flag = ReturnType.Success;
                    }
                }
                
                return flag;
            }
        }
    }
}
