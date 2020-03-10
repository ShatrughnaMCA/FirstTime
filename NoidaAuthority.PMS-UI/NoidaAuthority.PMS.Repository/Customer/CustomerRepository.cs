using Dapper;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Common.Extensions;
using NoidaAuthority.PMS.Common.Helpers;
using NoidaAuthority.PMS.Entity;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
//using NoidaAuthority.PMS.Repository.Common;
using NoidaAuthority.PMS.Repository.Entities;
using System.Data.Entity;
using NoidaAuthority.PMS.Repository.Context;


namespace NoidaAuthority.PMS.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
       
        public CustomerRepository()
        {
            
        }


        public ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext()){
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
                var processingfee = dbContext.CitizenService_Master.FirstOrDefault(c => c.Deptt_Id == model.ServiceModel.DepartmentId && c.service_id == model.ServiceModel.ServiceId && c.Status == 1).Amount;
                if (processingfee != null && processingfee > 0)
                {
                    model.ServiceModel.IsProcessingFeePaid = true;
                }
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
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
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
                mutation.Created_By = model.RegistrationId; // userInfo.UserID;
                mutation.Comment = model.ServiceModel.Description;
                dbContext.OnlineTransferMutations.Add(mutation);
                dbContext.SaveChanges();
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

        private int SaveGPARequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
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
                gpa.Created_By = model.RegistrationId; // userInfo.UserID;
                gpa.Created_Date = DateTime.Now;

                dbContext.OnlineGPAs.Add(gpa);
                dbContext.SaveChanges();

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

        private int SaveExtensionRequestDetails(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                model.Id = requestNo;
                OnlineExtensionDetail extension = new OnlineExtensionDetail();
                extension.Rid = model.ServiceModel.RegistrationId;
                extension.RequestNo = requestNo;
                extension.ExtensionDueDate = model.ExtensionModel.ExtensionDueDate;
                extension.ExtensionGivenDate = model.ExtensionModel.ExtensionGivenDate;
                extension.Status = NAServiceStatusId.Initiated;
                extension.IsActive = true;
                extension.CreatedDate = DateTime.Now;
                extension.CreatedBy = model.RegistrationId; // userInfo.UserID;
                extension.Comment = model.ServiceModel.Description;

                dbContext.OnlineExtensionDetails.Add(extension);
                dbContext.SaveChanges();
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

        private int SaveMortgageRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
            {
            var flag = ReturnType.None;
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
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
                mortgage.CreatedBy = model.RegistrationId; // userInfo.UserID;
                mortgage.Comment = model.ServiceModel.Description;
                mortgage.CommentDate = DateTime.Now;
                mortgage.StatusId = NAServiceStatusId.Initiated;

                dbContext.OnlineMortgageDetails.Add(mortgage);
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
                            director.CreatedBy = model.RegistrationId; // userInfo.UserID;
                            firmMaster.Add(director);
                        }
                        dbContext.OnlineFirmDirectorMasters.AddRange(firmMaster);
                        //dbContext.OnlineFirmDirectorMasters.Add(dataResult);                    
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
                        flag = ReturnType.Failed;
                    }
                }
                if (model.CICModel.ChangeTypeId == NAServiceId.ChangeInFirmName)
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
                    firm.CreatedBy = model.RegistrationId; // userInfo.UserID;
                    dbContext.OnlineFirmRequestMasters.Add(firm);
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
                if (model.CICModel.ChangeTypeId == NAServiceId.ChangeInProduct)
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
                    firm.CreatedBy = model.RegistrationId; // userInfo.UserID;
                    firm.ChangeType = NAServiceId.ChangeInProduct;

                    dbContext.OnlineFirmRequestMasters.Add(firm);
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
                return flag;
            }
        }

        private int SaveRentRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                //model.ServiceRequestId = requestNo;
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
                rent.CreatedBy = model.RegistrationId; // userInfo.UserID;
                dbContext.OnlineRentPermissionDetails.Add(rent);
                dbContext.SaveChanges();

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

        private int SaveTransferRequestService(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                int requestNo = SaveCustomerServiceRequestDetail(model);
                //model.ServiceRequestId = requestNo;
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
                transfer.Created_By = model.RegistrationId; // userInfo.UserID;
                transfer.Created_Date = DateTime.Now;
                dbContext.OnlineTransferMutations.Add(transfer);
                dbContext.SaveChanges();

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

        private int SaveCustomerServiceRequestDetail(ServiceRequestViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                NoidaAuthority.PMS.Repository.Context.Customer_ServiceRequest request = new NoidaAuthority.PMS.Repository.Context.Customer_ServiceRequest();
                request.Registration_No = model.RegistrationId.ToString();
                request.Property_No = model.ServiceModel.Sector + "/" + model.ServiceModel.Block + "-" + model.ServiceModel.PlotNo;
                request.SectorName = model.ServiceModel.Sector;
                request.BlockName = model.ServiceModel.Block;
                request.PlotNo = model.ServiceModel.PlotNo;
                request.ServiceId = model.ServiceModel.ServiceId;
                //request.ServiceType = model.ServiceModel.RegistrationType == NAConstant.PradhikaranDiwas ? NAConstant.SDService : NAConstant.CustomerService;
                request.ServiceType = NAConstant.CustomerService;
                request.DepartmentId = model.ServiceModel.DepartmentId;
                request.SubDepartment = model.ServiceModel.SubDepartment;
                request.ApplicantName = model.ServiceModel.Applicant;
                request.MobileNumber = model.ServiceModel.MobileNo;
                request.Email = model.ServiceModel.Email;
                request.ApplicantAddress = model.ServiceModel.ApplicantAddress;
                request.Description = model.ServiceModel.Description;
                request.IsActive = true;
                request.Request_Status = NAServiceStatusId.Initiated;
                request.RequestorName = model.ServiceModel.Requestor != null ? model.ServiceModel.Requestor : model.ServiceModel.Applicant;
                request.RequestorAddress = model.ServiceModel.RequestorAddress != null ? model.ServiceModel.RequestorAddress : model.ServiceModel.ApplicantAddress;
                request.Created_Date = DateTime.Now;
                request.Created_By = model.RegistrationId; // userInfo.UserID;
                request.RequestThrough = "Online";
                //request.PaymentStatus = 0;
                dbContext.Customer_ServiceRequest.Add(request);
                dbContext.SaveChanges();

                string message = string.Format(NAResources.OnlineServiceRequest, request.Id);
                if (!string.IsNullOrEmpty(request.MobileNumber)) ApplicationHelper.SendSMS(request.MobileNumber, message);               
                if (!string.IsNullOrEmpty(request.Email)) ApplicationHelper.SendEmail(request.Email, "Online Request", message);

                //int requestNo = (from req in dbContext.Customer_ServiceRequest where req.Registration_No == model.RegistrationId.ToString() && req.Description == model.Description && req.Request_Status == StatusType.Initiated && req.IsActive == true select req.Id).FirstOrDefault();
                model.ServiceModel.Id = request.Id;
                model.ServiceModel.RequestId = request.Id;
                model.ServiceModel.CreatedDate = DateTime.Now;
                return request.Id;
            }
        }


        public ServiceRequestViewModel GetServiceRequestDetailById(int? id)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                ServiceRequestViewModel model = new ServiceRequestViewModel();

                var service = (from csr in dbContext.Customer_ServiceRequest
                               where csr.Id == id //&& csr.IsActive == true
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
                                   PaymentStatusId = csr.PaymentStatus,
                                   PaymentStatus = csr.PaymentStatus == 1 ? "Paid" : (csr.PaymentStatus == 2 ? "Failed" : (csr.PaymentStatus == 0 ? "Not Paid" : "Not Required")),
                                   Comment = csr.Comment,
                                   CreatedDate = csr.Created_Date,
                                   RequestorId =csr.Created_By,
                                   RequestThrough = csr.RequestThrough,
                                   ValidatorId = csr.ValidatorId,
                                   Validator = csr.ValidatorId != null ? dbContext.UmUserMasters.FirstOrDefault(c=>c.UserRefId==csr.ValidatorId).FirstName : null,
                                   ApproverId = csr.ApproverId,
                                   Approver = csr.ApproverId != null ? dbContext.UmUserMasters.FirstOrDefault(c=>c.UserRefId==csr.ApproverId).FirstName : null
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
                model.RegistrationId = service.RegistrationNo != null ? Convert.ToInt32(service.RegistrationNo) : 0;
                model.OnlineRequestId = service.Id;
                model.ServiceModel = service;

                //for required document
                var requireddocs = dbContext.Customer_ServiceRequestDocument.Where(c => c.ServiceRequestRefId == id).ToList();
                //if (requireddocs != null && requireddocs.Count > 0)
                //{
                    model.ServiceModel.FileUploadHtml = GetRequiredFileUploadHtmlByServiceRequestId(id);
                //}

                var servicecharge = dbContext.Customer_ServiceRequestCharges.Where(c => c.ServiceRequestRefId == id).ToList();
                if (servicecharge != null && servicecharge.Count > 0)
                {
                    model.ServiceModel.ChargeDetailHtml = GetChargesRequiredAsHtmlContentByServiceRequestId(id);
                }

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
                        //Gender = (transfer.T_Gender == NAConstant.Male || transfer.T_Gender == NAConstant.Female) ? NAConstant.Individual : NAConstant.Company,
                        Gender=transfer.T_Gender,
                        FirstName = transfer.T_First_Name,
                        MiddleName = transfer.T_Middle_Name,
                        LastName = transfer.T_Middle_Name,
                        Applicant = transfer.T_First_Name + " " + transfer.T_Middle_Name + " " + transfer.T_Last_Name,
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
                    return model;
                }
                else
                {
                    return model;
                }
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
                    return model;
                }
                if (firms != null && firms.ChangeType == NAServiceId.ChangeInFirmName)
                {
                    cic.OldFirmName = firms.OldFirmName;
                    cic.NewFirmName = firms.NewFirmName;

                    model.CICModel = cic;
                    return model;
                }
                if (firms != null && firms.ChangeType == NAServiceId.ChangeInProduct)
                {
                    cic.OldFirmProduct = firms.OldFirmProduct;
                    cic.NewFirmProduct = firms.NewFirmProduct;
                    model.CICModel = cic;
                    return model;
                }
                else
                {
                    return model;
                }
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
                    return model;
                }
                else
                {
                    return model;
                }
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
                    return model;
                }
                else
                {
                    return model;
                }
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
                    return model;
                }
                else
                {
                    return model;
                }
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
                    return model;
                }
                else
                {
                    return model;
                }
            }
        }


        public DataSourceResult GetPaymentReceiptScheduleListById(DataSourceRequest request, PaymentViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //string Rid = Convert.ToString(rid);
                //var data = (from payreceipt in dbContext.ViewReceiptMasters
                //            join receipthead in dbContext.RECIEPT_HEAD on payreceipt.RECEIPT_HEAD_ID equals receipthead.RECIEPT_CODE
                //            join receiptsubhead in dbContext.RECEIPT_SUB_HEAD on payreceipt.RECEIPT_SUBHEAD_ID equals receiptsubhead.RECEIPT_SUBHEAD_ID
                //            where payreceipt.RID_NO.Equals(Rid)
                //            select new PaymentViewModel
                //            {
                //                Id = 1,
                //                ReceiptId = payreceipt.RECEIPT_ID,
                //                ReceiptHeadName = receipthead.RECIEPT_HEAD_NAME,
                //                ReceiptSubHeadName = receiptsubhead.RECEIPT_SUB_HEAD1,
                //                ChallanId = payreceipt.CHALLAN_ID,
                //                DepositDate = payreceipt.DEPOSIT_DATE,
                //                PaidAmount = payreceipt.AMOUNT_PAID,
                //                RegistrationId = rid,
                //                RegistrationNo = rid.ToString()
                //            });
                var data = (from receipt in dbContext.RECEIPT_DETAIL_MASTER
                            join recptTrans in dbContext.RECEIPT_AMOUNT_TRANS on receipt.RECEIPT_ID equals recptTrans.RECEIPT_ID
                            join head in dbContext.RECIEPT_HEAD on recptTrans.RECEIPT_HEAD_ID equals head.RECIEPT_CODE
                            join subhead in dbContext.RECEIPT_SUB_HEAD on recptTrans.RECEIPT_SUBHEAD_ID equals subhead.RECEIPT_SUBHEAD_ID
                            where receipt.RID_NO == model.RegistrationId.ToString() && (model.ReceiptId == null || receipt.RECEIPT_ID == model.ReceiptId)
                            select new PaymentViewModel
                            {
                                Id = 1,
                                ReceiptId = receipt.RECEIPT_ID,
                                ReceiptHeadName = head.RECIEPT_HEAD_NAME,
                                ReceiptSubHeadName = subhead.RECEIPT_SUB_HEAD1,
                                ChallanId = receipt.CHALLAN_ID,
                                DepositDate = receipt.DEPOSIT_DATE,
                                EntryDate = recptTrans.ENTRY_DATE,
                                PaidAmount = recptTrans.AMOUNT_PAID,
                                RegistrationId = model.RegistrationId,
                                RegistrationNo = receipt.RID_NO,
                                DepositorName = receipt.DEPOSETER_NAME
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetPaymentScheduleDataListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from payschtans in dbContext.PaymentScheduleTrans
                            where payschtans.Rid == rid && payschtans.IsActive == true
                            select new PaymentViewModel
                            {
                                Id = payschtans.Id,
                                RegistrationId = payschtans.Rid,
                                RegistrationNo = payschtans.Rid.ToString(),
                                InstallmentNo = payschtans.InstallmentNo,
                                InstallmentDueDate = payschtans.InstallmentDueDate,
                                InstallmentAmount = payschtans.InstallmentAmount,
                                PrincipalAmount = payschtans.BalanceAmount,
                                InstallmentInterest = payschtans.InterestAmount,
                                BalanceAmount = payschtans.InstallmentAmount + payschtans.InterestAmount
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetPaymentRescheduledListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from pay in dbContext.PaymentScheduleMasters
                            join payTrans in dbContext.PaymentScheduleTrans on pay.ScheduleId equals payTrans.ScheduleId
                            where pay.Rid == rid && pay.ScheduleType == NAConstant.Reschedule && pay.IsActive == true && payTrans.IsActive == true
                            select new PaymentViewModel
                            {
                                Id = payTrans.Id,
                                RegistrationId = payTrans.Rid,
                                RegistrationNo = payTrans.Rid.ToString(),
                                InstallmentNo = payTrans.InstallmentNo,
                                InstallmentDueDate = payTrans.InstallmentDueDate,
                                InstallmentAmount = payTrans.InstallmentAmount,
                                PrincipalAmount = payTrans.BalanceAmount,
                                InstallmentInterest = payTrans.InterestAmount,
                                BalanceAmount = payTrans.InstallmentAmount + payTrans.InterestAmount
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetPaymentLedgerDataListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var paymentLedger = dbContext.SpAccountLedger(rid).ToList();
                List<PaymentViewModel> lstPay = new List<PaymentViewModel>();
                if (paymentLedger != null)
                {
                    foreach (var item in paymentLedger)
                    {
                        var lstPayLed = new PaymentViewModel
                        {
                            Id = item.Id,
                            RegistrationId = item.Rid,
                            RegistrationNo = item.Rid.ToString(),
                            ReceiptSubHeadName = item.RECEIPT_SUB_HEAD,
                            EntryDate = item.Entry_Date,
                            DebitAmount = item.Debit_Amount,
                            CreditAmount = item.Credit_Amount,
                            BalanceAmount = item.Balance_Amount
                        };
                        lstPay.Add(lstPayLed);
                    }
                }
                //var payment = new PaymentViewModel
                //{
                //    Id = 1,
                //    RegistrationId = 10000013,
                //    RegistrationNo = "10000013",
                //    ReceiptSubHeadName = "Premium",
                //    EntryDate = DateTime.Now,
                //    DebitAmount = 50000,
                //    CreditAmount = 500000,
                //    BalanceAmount = 450000
                //};
                //lstPay.Add(payment);
                return lstPay.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetServiceHistoryDataListById(DataSourceRequest request, ServiceViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from requests in dbContext.Customer_ServiceRequest
                            join service in dbContext.CitizenService_Master on new { x1 = requests.DepartmentId, x2 = requests.ServiceId } equals new { x1 = service.Deptt_Id, x2 = service.service_id }
                            where requests.Registration_No == model.RegistrationId.ToString() && service.Status == NAConstant.Active
                            && (model.Id == null || requests.Id == model.Id)
                            select new ServiceViewModel
                            {
                                Id = requests.Id,
                                RequestId = requests.Id,
                                RegistrationId = model.RegistrationId,
                                RegistrationNo = requests.Registration_No,
                                ServiceId = requests.ServiceId,
                                ServiceName = service.ServiceName,
                                DepartmentId = requests.DepartmentId,
                                Department = requests.DepartmentId == null ? string.Empty : dbContext.DepartmentMsts.FirstOrDefault(d => d.departmentId == requests.DepartmentId).departmentName,
                                SubDepartment = (requests.SubDepartment == "A" || requests.SubDepartment == "Account") ? "Account" : "Property",
                                Requestor = requests.RequestorName,
                                RequestorAddress = requests.RequestorAddress,
                                Description = requests.Description,
                                Comment = requests.Comment,
                                MobileNo = requests.MobileNumber,
                                Email = requests.Email,
                                CreatedDate = requests.Created_Date,
                                StatusId = requests.Request_Status,
                                Status = dbContext.StatusMasters.Where(s => s.Id == requests.Request_Status).Select(s => s.Status).FirstOrDefault(),
                                RequestThrough = requests.RequestThrough,
                                ISUploaded = requests.IsUploadedLetter,
                                PaymentStatusId = requests.PaymentStatus,
                                IsProcessingFeePaid = (requests.PaymentStatus == 1) ? true : false,
                                ObjectionStatus = requests.ObjectionStatus,
                                DocumentStatus = requests.DocumentStatus
                            });
                return data.ToDataSourceResult(request);
            }
        }

        //save registered customer
        //public int RegisterCustomerDetails(NACustomer customer, IEnumerable<HttpPostedFileBase> files)
        //{
        //    var flag = ReturnType.None;
        //    User ctxCustomer = new User();
        //    try
        //    {
        //        using (var dbContext = new NOIDANEWEntities())
        //        {
        //            if (files != null && files.Count() > 0)
        //            {
        //                var fileList = files.ToList();
        //                customer.CustomerIdFileName = fileList[0] != null ? fileList[0].FileName : string.Empty;
        //                customer.AuthorityLetter = fileList[1] != null ? fileList[1].FileName : string.Empty;
        //            }

        //            //var existingCustomer = dbContext.Users.FirstOrDefault(us => us.PropertyId == customer.RegistrationId);
        //            var existingCustomer = dbContext.Users.FirstOrDefault(us => us.UserName == customer.RegistrationId.ToString());
        //            if (existingCustomer != null)
        //            {
        //                //creating password
        //                //var newPassword = "password123";
        //                //existingCustomer.Pasword = newPassword.ToMD5HashForPasswordPIS();

        //                existingCustomer.RoleId = 2;
        //                existingCustomer.DeptId = customer.DepartmentId;
        //                existingCustomer.Sector = customer.Sector;
        //                existingCustomer.Block = customer.Block;
        //                existingCustomer.Property = customer.PlotNo;
        //                existingCustomer.ModifiedOn = DateTime.Now;
        //                existingCustomer.MobileNo = customer.MobileNo;
        //                existingCustomer.UserEmail = customer.Email;
        //                existingCustomer.PropertyId = customer.PropertyId.ToString();
        //                existingCustomer.FirstName = customer.CustomerName;
        //                existingCustomer.CustomerIdFileName = customer.CustomerIdFileName;
        //                existingCustomer.CustomerIdFileType = customer.CustomerIdFiletype;
        //                existingCustomer.CustomerLetterFileName = customer.AuthorityLetter;
        //                existingCustomer.CustomerLetterType = customer.AuthorityLetterType;
        //                existingCustomer.Question = customer.SecurityQuestion;
        //                existingCustomer.Answer = customer.SecurityAnswer;
        //                existingCustomer.IsRejected = false;
        //                existingCustomer.Status = false;
        //                existingCustomer.IsFirstTimeActivated = false;
        //            }
        //            else
        //            {
        //                ctxCustomer.UserId = Guid.NewGuid();
        //                ctxCustomer.UserName = customer.RegistrationId.ToString();
        //                ctxCustomer.FirstName = customer.CustomerName;
        //                ctxCustomer.DeptId = customer.DepartmentId;
        //                ctxCustomer.PropertyId = customer.PropertyId.ToString();
        //                ctxCustomer.RoleId = 2;
        //                ctxCustomer.Sector = customer.Sector;
        //                ctxCustomer.Block = customer.Block;
        //                ctxCustomer.Property = customer.PlotNo;
        //                ctxCustomer.MobileNo = customer.MobileNo;
        //                ctxCustomer.UserEmail = customer.Email;
        //                ctxCustomer.CreatedOn = DateTime.Now;
        //                ctxCustomer.CreatedBy = ctxCustomer.UserId;
        //                ctxCustomer.CustomerIdFileName = customer.CustomerIdFileName;
        //                ctxCustomer.CustomerIdFileType = customer.CustomerIdFiletype;
        //                ctxCustomer.CustomerLetterFileName = customer.AuthorityLetter;
        //                ctxCustomer.CustomerLetterType = customer.AuthorityLetterType;
        //                ctxCustomer.Question = customer.SecurityQuestion;
        //                ctxCustomer.Answer = customer.SecurityAnswer;
        //                ctxCustomer.Status = false;
        //                ctxCustomer.IsRejected = false;
        //                ctxCustomer.IsFirstTimeActivated = false;
        //                //creating password
        //                var newPassword = "password123";
        //                ctxCustomer.Pasword = newPassword.ToMD5HashForPasswordPIS();

        //                dbContext.Users.Add(ctxCustomer);

        //                var msg1 = NAMessages.PIS_registration_1;
        //                if (customer.Email != null)
        //                {
        //                    EmailHelper emailHelper = new EmailHelper();
        //                    emailHelper.Send(customer.Email, "Registration Completed", msg1 + "Regards, http://mynoida.in");
        //                }
        //                //Send SMS
        //                ApplicationHelper.SendSMS(customer.MobileNo, msg1);
        //                //Sending mail to From Address on new Registration, as asked by Vishal Shukla to do so
        //                var emailAdd = System.Configuration.ConfigurationManager.AppSettings["SmtpFromAddress"];
        //                var msg2 = string.Format(NAMessages.PIS_registration_2, ctxCustomer.PropertyId);
        //                if (!string.IsNullOrEmpty(emailAdd))
        //                {
        //                    EmailHelper emailHelper = new EmailHelper();
        //                    emailHelper.Send(emailAdd, "Registration Completed", msg2 + "<br><br>Regards, http://mynoida.in");
        //                }
        //            }
        //            dbContext.SaveChanges();
        //            var dflag = SaveCustomerDocument(customer, files);
        //            flag = ReturnType.Saved;
        //        }
        //    }
        //    catch (System.Data.Entity.Validation.DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return flag;
        //}

        //private int SaveCustomerDocument(NACustomer model, IEnumerable<HttpPostedFileBase> files)
        //{
        //    int flag = ReturnType.None;
        //    if (files != null && files.Count() > 0)
        //    {
        //        var directoryPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["FilePath"]).ToString() + model.RegistrationId;
        //        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        //        foreach (var fyl in files)
        //        {
        //            if (fyl != null)
        //            {
        //                fyl.SaveAs(HttpContext.Current.Server.MapPath((ConfigurationManager.AppSettings["FilePath"]) + model.RegistrationId).ToString() + "/" + fyl.FileName);
        //            }
        //        }
        //        flag = ReturnType.Saved;
        //    }
        //    return flag;
        //}

        //public PropertyDetail GetPropertyDetails(DtoPropertyFilter objPropertyFilter, int inumber)
        //{
        //    try
        //    {
        //        using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
        //        {
        //            return connection.Query<PropertyDetail>("sp_GetPropertyDetailsByRegistrationId", new { RegistrationId = objPropertyFilter.RegistrationId, OperationType = inumber }, commandType: System.Data.CommandType.StoredProcedure).SingleOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //}


        public DataSourceResult GetTransferHistoryDataListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from transfer in dbContext.Succ_Mut_Trans
                            where transfer.Rid == rid
                            select new TransferViewModel
                            {
                                Id = transfer.Request_No,
                                RequestNo = transfer.Request_No,
                                RegistrationId = transfer.Rid,
                                Applicant = transfer.Applicant_Name,
                                ApplicantAddress = transfer.Correspondance_Add,
                                Transferee = transfer.T_Gender == "Company" ? transfer.T_Company_Name : transfer.T_First_Name + " " + transfer.T_Middle_Name + " " + transfer.T_Last_Name,
                                TransfereeAddress = transfer.T_Correspondence_Add,
                                TransferType = transfer.Type == "M" ? "Mutation" : "Transfer",
                                TransferDate = transfer.Transfer_Date,
                                PropertyNo = null,
                                TransferStatus = dbContext.StatusMasters.Where(s => s.Id == transfer.Status).Select(s => s.Status).FirstOrDefault()
                            });
                return data.ToDataSourceResult(request);
            }
        }


        public DataSourceResult GetMortgageHistoryDataListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from mortgage in dbContext.MortgageDetails
                            join aplicant in dbContext.ApplicationDetails on mortgage.RID equals aplicant.registrationId
                            where mortgage.RID == rid
                            select new MortgageViewModel
                            {
                                Id = mortgage.RequestNo,
                                RegistrationId = mortgage.RID,
                                MortgageDate = mortgage.MortgageDate,
                                MortgageType = mortgage.MortgageType == "2" ? "Collateral" : "Normal",
                                BankName = mortgage.BankName,
                                BranchAddress = mortgage.BranchAddress,
                                MortgageStatus = mortgage.StatusMaster.Status,
                                CreatedDate = mortgage.CreatedDate,
                                ApproveDate = mortgage.ApproveDate,
                                ValidUpto = mortgage.ValidUpto,
                                ProcessingFee = mortgage.ProcessingFee,
                                SanctionedAmount = mortgage.SanctionedAmount,
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + aplicant.tMiddleName + " " + aplicant.tLastName
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetExtensionHistoryDataListById(DataSourceRequest request, int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from extension in dbContext.Extension_Details
                            where extension.Rid == rid
                            select new ExtensionViewModel
                            {
                                Id = extension.Id,
                                RegistrationId = extension.Rid,
                                CompletionDueDate = extension.Completion_DueDate,
                                ExtensionDueDate = extension.Extension_Due_Date,
                                ExtensionGivenDate = extension.Extension_Given_Date,
                                ExtensionCharge = extension.Extension_Charge,
                                ApprovedDate = extension.Approved_Date,
                                Comment = null,
                                ExtensionStatus = dbContext.StatusMasters.Where(s => s.Id == extension.Status).Select(s => s.Status).FirstOrDefault()
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public IEnumerable<DtoLegalHistory> GetLegalHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter)
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

        public IEnumerable<DtoJalDetailsPaymentHistory> GetJalDetailsPaymentHistoryByRegistrationId(DtoPropertyFilter objPropertyFilter)
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
                                    + "<div class='col-md-3 col-lbl'><label>Serial No</label></div>"
                                    + "<div class='col-md-6 col-lbl'><label>Required File</label></div>"
                                    + "<div class='col-md-3 col-lbl-vl'><label>Select File</label></div>"
                                + "</div>";

                    int docCounter = 1;
                    foreach (var item in checklists)
                    {
                        divMain = divMain + "<div class='row  row-border row-file'> "
                                               + "<div class='col-md-3 col-lbl'><label>" + docCounter + "</label></div>"
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


        public PropertyViewModel GetPropertyDetailByRegistrationId(int rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from alotment in dbContext.AllotmentMasters
                            join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                            join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                            where alotment.rid == rid
                            select new PropertyViewModel
                            {
                                Id = alotment.rid,
                                RegistrationId = aplicant.registrationId,
                                SchemeId = alotment.schemeId,
                                SchemeName = alotment.SchemeMst.schemeName,
                                DepartmentId = alotment.departmentId,
                                Department = alotment.DepartmentMst.departmentName,
                                SectorId = property.sectorId,
                                SectorName = property.SectorMst.sectorName,
                                BlockId = property.blockId,
                                BlockName = property.BlockMst.blockName,
                                PlotNo = property.propertyNo,
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + aplicant.tMiddleName + " " + aplicant.tLastName,
                                ApplicantMaster = aplicant.tGender == NAConstant.Company ? aplicant.tSigningAuthority : aplicant.tFatherHusbandName,
                                ApplicantAddress = aplicant.tCorrespondanceAdd,
                                MobileNo = aplicant.tMobileNumber,
                                Email = aplicant.tEmail
                            }).FirstOrDefault();
                return data;
            }
        }


        public ServiceRequestViewModel GetPropertyDetailForServiceRequestById(int rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                ServiceRequestViewModel model = new ServiceRequestViewModel();
                var data = (from alotment in dbContext.AllotmentMasters
                            join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                            join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                            where alotment.rid == rid
                            select new ServiceViewModel
                            {
                                Id = alotment.rid,
                                RegistrationId = aplicant.registrationId,
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
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + aplicant.tMiddleName + " " + aplicant.tLastName,
                                ApplicantMaster = aplicant.tGender == NAConstant.Company ? aplicant.tSigningAuthority : aplicant.tFatherHusbandName,
                                ApplicantAddress = aplicant.tCorrespondanceAdd,
                                MobileNo = aplicant.tMobileNumber,
                                Email = aplicant.tEmail
                            }).FirstOrDefault();

                model.RegistrationId = rid;
                model.ServiceModel = data;
                return model;
            }
        }


        public DataSourceResult GetDirectorShareholderDataList(DataSourceRequest request)
        {
            try
            {
                var directors = (List<DirectorShareholderModel>)HttpContext.Current.Session["TempDirectors"];
                if (directors != null)
                {
                    return directors.ToDataSourceResult(request);
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            var rflag = ReturnType.None;
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
                    rflag = ReturnType.Updated;
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
                    rflag = ReturnType.Saved;
                }
                return rflag;
            }
            catch (Exception e)
            {
                return rflag = ReturnType.Failed;
            }
        }

        public int RemoveDirectorShareholderFromList(int id)
        {
            int flag = ReturnType.None;
            try
            {
                var directors = (List<DirectorShareholderModel>)HttpContext.Current.Session["TempDirectors"];
                if (directors != null)
                {
                    directors.RemoveAt(id);
                    flag = ReturnType.Success;
                }
                else
                    flag = ReturnType.NotExist;

                return flag;
            }
            catch (Exception e)
            {
                return flag = ReturnType.Failed;
            }
        }

        #region Login Repo Methods
        /// <summary>
        /// provide user details after logged on
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        //public User GetUserDetails(string userName)
        //{
        //    User userMaster = null;
        //    using (var dbContext = new NOIDANEWEntities())
        //    {
        //        userMaster = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName);

        //    }
        //    return userMaster;
        //}

        /// <summary>
        /// all details of logged user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        //public UserViewModel GetLoginUserDetails(string userName)
        //{
        //    using (var dbContext = new NOIDANEWEntities())
        //    {
        //        var data = (from user in dbContext.Users
        //                    where user.UserName == userName
        //                    select new UserViewModel
        //                    {
        //                        UserId = user.UserId,
        //                        UserName = user.UserName,
        //                        FirstName = user.FirstName,
        //                        LastName = user.LastName,
        //                        Applicant = user.FirstName + (string.IsNullOrEmpty(user.LastName) ? string.Empty : " " + user.LastName),
        //                        Sector = user.Sector,
        //                        Block = string.IsNullOrEmpty(user.Block) ? "NA" : user.Block,
        //                        PlotNo = user.Property,
        //                        RoleId = user.RoleId,
        //                        RoleName = user.RoleId == null ? string.Empty : dbContext.Roles.FirstOrDefault(r => r.RoleId == user.RoleId).RoleName,
        //                        PropertyId = user.PropertyId,
        //                        MobileNo = user.MobileNo,
        //                        Email = user.UserEmail,
        //                        DepartmentId = user.DeptId,
        //                        Department = (user.DeptId == null || user.DeptId <= 0) ? string.Empty : dbContext.LookupDepartments.FirstOrDefault(r => r.Id == user.DeptId).NAME,
        //                        CustomerIdFileType = user.CustomerIdFileType,
        //                        AuthorityLetterType = user.CustomerLetterType,
        //                        Status = user.Status,
        //                        IsActive = (user.Status == null || user.Status == false) ? false : true,
        //                        IsLocked = (user.IsLockedOut == null || user.IsLockedOut == false) ? false : true,
        //                        IsRejected = (user.IsRejected == null || user.IsRejected == false) ? false : true,
        //                        IsFirstTimeActivated = (user.IsFirstTimeActivated == null || user.IsFirstTimeActivated == false) ? false : true,
        //                        Remarks = user.Remarks,
        //                        SecurityQuestion = user.Question,
        //                        SecurityAnswer = user.Answer,
        //                    }).FirstOrDefault();
        //        return data;
        //    }
        //}

        /// <summary>
        /// user is locked after trying 5 times 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        public bool LockUser(string userName)
        {
            var flag = false;
            using (var dbContext = new NOIDANEWEntities())
            {
                var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName);
                if (user != null)
                {
                    user.IsLockedOut = false;
                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// validate user name and password at login time
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>

        public bool ValidateUser(string userName, string password)
        {
            var flag = false;
            var encryptedPassword = password.ToMD5HashForPasswordPIS();
            using (var dbContext = new NOIDANEWEntities())
            {
                var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName && cond.Pasword == encryptedPassword);
                if (user != null)
                {
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// change password for logged user verify email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string email, string newPassword)
        {
            var flag = false;
            using (var dbContext = new NOIDANEWEntities())
            {
                var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == email);
                if (user != null)
                {
                    user.Pasword = newPassword.ToMD5HashForPasswordPIS();
                    user.ModifiedOn = DateTime.Now;
                    //user.ModifiedBy = "1";// userInfo.UserID.ToString();
                    dbContext.SaveChanges();
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// change password for logged user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>

        public bool ChangePassword(string userName, string email, string newPassword)
        {
            var flag = false;
            using (var dbContext = new NOIDANEWEntities())
            {
                var user = dbContext.Users.FirstOrDefault(cond => cond.UserName == userName && cond.UserEmail == email);
                if (user != null)
                {
                    user.Pasword = newPassword.ToMD5HashForPasswordPIS();
                    user.ModifiedOn = DateTime.Now;
                    dbContext.SaveChanges();
                    if (user.UserEmail != null)
                    {
                        var body = "Dear User,<br><br>You have successfully changed your password. Your new password is " + newPassword + ". </br></br>Regards, </br>http://mynoida.in";
                        EmailHelper emailHelper = new EmailHelper();
                        emailHelper.Send(user.UserEmail, "You have successfully changed your password.", body);
                    }

                    if (user.MobileNo != null)
                    {
                        //var msg = string.Format(NAMessages.PasswordChange, newPassword);
                        //SendSMS(user.MobileNo, msg);
                    }
                    flag = true;
                }
            }
            return flag;
        }

        public int GetRoleIDForUser(string username)
        {
            int roleID = 0;
            if (string.IsNullOrEmpty(username))
            {
                return 0;
            }
            try
            {

                using (var dbContext = new NOIDANEWEntities())
                {
                    //User user = null;

                    var role = (from users in dbContext.Users
                                join selRole in dbContext.Roles on users.RoleId equals selRole.RoleId
                                where users.UserName == username
                                select selRole.RoleId).FirstOrDefault();

                    roleID = role;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roleID;
        }

        //public NA.PMS.Model.Entities.Role GetRoleForUser(string userName)
        //{
        //    return GetRoleById(GetRoleIDForUser(userName));
        //}
        //public NA.PMS.Model.Entities.Role GetRoleById(int id)
        //{
        //    var result = new NA.PMS.Model.Entities.Role();
        //    try
        //    {

        //        using (var dbContext = new NOIDANEWEntities())
        //        {

        //            var role = (from NA.PMS.Model.Entities.Role rl in dbContext.Roles.ToList()
        //                        where rl.RoleId == id
        //                        select new NA.PMS.Model.Entities.Role
        //                        {
        //                            RoleName = rl.RoleName,
        //                            RoleId = rl.RoleId,
        //                            CreatedBy = rl.CreatedBy,
        //                            CreatedOn = rl.CreatedOn,
        //                            ModifiedBy = rl.ModifiedBy,
        //                            ModifiedOn = rl.ModifiedOn,
        //                        }).FirstOrDefault();

        //            result = role;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        public IList<DtoList> LookupPropertyType()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetLookupPropertyType", new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }
        #endregion

        //public UserViewModel GetCustomerDetailById(int id)
        //{
        //    using (var dbContext = new NOIDANEWEntities())
        //    {
        //        var data = (from user in dbContext.Users
        //                    where user.UserName == id.ToString()
        //                    select new UserViewModel
        //                    {
        //                        UserId = user.UserId,
        //                        UserName = user.UserName,
        //                        FirstName = user.FirstName,
        //                        LastName = user.LastName,
        //                        Applicant = user.FirstName + (string.IsNullOrEmpty(user.LastName) ? string.Empty : " " + user.LastName),
        //                        Sector = user.Sector,
        //                        Block = user.Block,
        //                        PlotNo = user.Property,
        //                        RoleId = user.RoleId,
        //                        RoleName = user.RoleId == null ? string.Empty : dbContext.Roles.FirstOrDefault(r => r.RoleId == user.RoleId).RoleName,
        //                        PropertyId = user.PropertyId,
        //                        MobileNo = user.MobileNo,
        //                        Email = user.UserEmail,
        //                        DepartmentId = user.DeptId,
        //                        Department = (user.DeptId == null || user.DeptId <= 0) ? string.Empty : dbContext.LookupDepartments.FirstOrDefault(r => r.Id == user.DeptId).NAME,
        //                        CustomerIdFileType = user.CustomerIdFileType,
        //                        AuthorityLetterType = user.CustomerLetterType,
        //                        Status = user.Status,
        //                        IsActive = user.Status,
        //                        IsLocked = user.IsLockedOut,
        //                        IsRejected = user.IsRejected,
        //                        IsFirstTimeActivated = user.IsFirstTimeActivated,
        //                        Remarks = user.Remarks,
        //                        SecurityQuestion = user.Question,
        //                        SecurityAnswer = user.Answer,
        //                    }).FirstOrDefault();
        //        return data;
        //    }
        //}


        public DataSourceResult GetJalPaymentDataList(DataSourceRequest request, JalViewModel model)
        {
            return null;
        }

        public DataSourceResult GetLitigationDataList(DataSourceRequest request, LitigationViewModel model)
        {
            return null;
        }


        public CustomerDetailViewModel GetCustomerDetails(int rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                CustomerDetailViewModel model = new CustomerDetailViewModel();
                var customer = (from alotment in dbContext.AllotmentMasters
                                join aplicant in dbContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                                where alotment.rid == rid
                                select new CustomerViewModel
                                {
                                    Id = alotment.rid,
                                    RegistrationId = alotment.rid,
                                    PropertyId = alotment.propertyId,
                                    DepartmentId = alotment.departmentId,
                                    Department = alotment.DepartmentMst.departmentName,
                                    Applicant = aplicant.tGender == NAConstant.Company ? (string.IsNullOrEmpty(aplicant.T_Company_Name) ? aplicant.tFirstName : aplicant.T_Company_Name) : aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName,
                                    ApplicantMaster = aplicant.tGender == NAConstant.Company ? aplicant.tSigningAuthority : aplicant.tFatherHusbandName,
                                    MotherName = aplicant.tMotherName,
                                    ApplicantType = aplicant.tGender,
                                    CorrespondAddress = aplicant.tCorrespondanceAdd,
                                    PermanentAddress = aplicant.tPermanentAdd,
                                    MobileNo = aplicant.tMobileNumber,
                                    Email = aplicant.tEmail,
                                    PanNo = aplicant.tPan,
                                    DateOfBirth = aplicant.tDateOfBirth,
                                    MaritalStatus = aplicant.tMarritalStatus,
                                    Religion = aplicant.religionId == null ? string.Empty : dbContext.ReligionMsts.FirstOrDefault(r => r.religionId == aplicant.religionId).religion,
                                    Category = aplicant.quotaId == null ? string.Empty : aplicant.QuotaMst.quotaName,
                                    Occupation = aplicant.tOccupationId == null ? string.Empty : dbContext.OccupationMsts.FirstOrDefault(o => o.occupationId == aplicant.tOccupationId).occupation,
                                    AnnualIncome = aplicant.tAnnualIncome
                                }).FirstOrDefault();
                model.CustomerModel = customer;
                model.PropertyModel = GetCustomerPropertyDetail(customer.RegistrationId);
                //if (customer != null)
                //{
                //    model.CustomerModel = customer;
                //    model.PropertyModel = GetCustomerPropertyDetail(customer.RegistrationId);
                //}
                //else
                //{
                //    model.CustomerModel = new CustomerViewModel();
                //    model.CustomerModel.ReturnTypeId = ReturnType.NotExist;
                //}
                return model;
            }
        }

        private PropertyViewModel GetCustomerPropertyDetail(int? rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var details = (from alotment in dbContext.AllotmentMasters
                               join property in dbContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                               where alotment.rid == rid
                               select new PropertyViewModel
                               {
                                   RegistrationId = alotment.rid,
                                   PropertyId = alotment.propertyId,
                                   SchemeId = alotment.schemeId,
                                   SchemeName = alotment.SchemeMst.schemeName,
                                   DepartmentId = alotment.departmentId,
                                   Department = alotment.DepartmentMst.departmentName,
                                   SectorId = property.sectorId,
                                   SectorName = property.SectorMst.sectorName,
                                   BlockId = property.blockId,
                                   BlockName = property.blockId == null ? "NA" : property.BlockMst.blockName,
                                   PlotNo = property.propertyNo,
                                   FloorAreaId = property.floorId,
                                   FloorArea = property.FloorMst.floorName,
                                   PropertyTypeId = property.propertyTypeId,
                                   PropertyType = property.PropertyTypeMst.propertyTypeName,
                                   LandRate = property.landRatePerSqmt,
                                   ActualArea = property.actualArea,
                                   CoveredArea = property.coveredArea,
                                   TotalArea = property.totalArea,
                                   PropertyCost = property.propertyCost,
                                   TotalPropertyCost = property.totalPropertyCost,
                                   CivilCost = property.civilCost,
                                   AllotmentMoney = property.allotmentMoney,
                                   Registry = property.Registry,
                                   AllotmentDate = alotment.allotmentDate
                               }).FirstOrDefault();
                var possession = dbContext.PossessionDetails.FirstOrDefault(p => p.Rid == details.RegistrationId);
                if (possession != null) details.PossessionDate = possession.PossessionDate;
                var registry = dbContext.RegistryDetails.FirstOrDefault(r => r.Rid == details.RegistrationId);
                if (registry != null) details.RegistryDate = registry.RegistryDoneDate;
                var fuctional = dbContext.FunctionalDetails.FirstOrDefault(f => f.Rid == details.RegistrationId && f.IsActive == true);
                if (fuctional != null) details.FunctionalDate = fuctional.FunctionalDate;
                var completion = dbContext.Completion_Details.FirstOrDefault(c => c.Rid == details.RegistrationId);
                if (completion != null) details.CompletionDate = completion.Completion_Execution_date;
                return details;
            }
        }


        public DataSourceResult GetScannedDocumentListById(DataSourceRequest request, DocumentViewModel model)
        {
            FtpHandler ftp = new FtpHandler();
            string path = ftp.GetDocumentPath(model.RegistrationId, string.Empty, true);//System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + rid;
            //List<string> oldFiles = ftp.DirSearch(path);
            //foreach (string file in oldFiles)
            //{
            //    string str = file;
            //    if (str.Contains(" "))
            //    {
            //        str = str.Replace(" ", "");
            //        ftp.RenameFiles(file, str, path);
            //    }
            //}
            List<string> allFiles = ftp.DirSearch(path);
            List<DocumentViewModel> documentList = new List<DocumentViewModel>();
            int count = 1;
            foreach (string file in allFiles)
            {
                string str = file;
                string str1 = string.Empty;
                if (file.Contains(" "))
                {
                    str = file.Replace(" ", "");
                }
                if ((str.Split('-')).Length > 1)
                {
                    str1 = str.Substring(0, str.Length - 4);
                    str1 = str1.Substring(9, str1.Length - 9);
                }
                DocumentViewModel document = new DocumentViewModel();
                document.DocumentPath = ftp.GetDocumentPath(model.RegistrationId, file, false);
                document.DocumentName = !(string.IsNullOrEmpty(str1)) ? (!(string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[str1])) ? System.Configuration.ConfigurationManager.AppSettings[str1] : "Other Documents") : "Other Documents";
                document.RegistrationId = model.RegistrationId;
                document.SerialNo = count;
                count++;
                documentList.Add(document);
            }
            return documentList.ToDataSourceResult(request);
        }

        public DataSourceResult GetGeneratedDocumentListById(DataSourceRequest request, DocumentViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from letter in dbContext.Letter_History
                            join template in dbContext.TemplateMasters on new { x = letter.Department_Id, y = letter.Template_Id } equals new { x = template.departmentId, y = template.templateId }
                            where (letter.Template_Html != null && letter.Template_Html != "")
                            && letter.Rid == model.RegistrationId
                            select new DocumentViewModel
                            {
                                Id = letter.Id,
                                RegistrationId = letter.Rid,
                                DepartmentId = letter.Department_Id,
                                Department = letter.DepartmentMst.departmentName,
                                DocumentContent = letter.Template_Html,
                                Template = template.templateName,
                                DocumentName = template.templateName,
                                Barcode = letter.Barcode_Val
                            });
                return list.ToDataSourceResult(request);
            }
        }


        public DataSourceResult GetRegistrationIdListAsDataSource(DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var ridlist = (from alotment in dbContext.AllotmentMasters
                               where alotment.isActive == 1
                               select new DropdownViewModel
                               {
                                   Id = alotment.rid,
                                   Text = alotment.rid.ToString()
                               });
                return ridlist.ToDataSourceResult(request);
            }
        }


        public CustomerViewModel ValidateCustomerRegistration(CustomerViewModel model)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                var exuser = dbContext.Users.FirstOrDefault(u => u.UserName == model.RegistrationId.ToString());
                if (exuser == null)
                {
                    var pimsContext = new PIMSEntitiesContext();
                    var data = pimsContext.AllotmentMasters.FirstOrDefault(c => c.rid == model.RegistrationId && c.isActive == NAConstant.Active);
                    if (data != null)
                    {
                        var customer = (from alotment in pimsContext.AllotmentMasters
                                        join aplicant in pimsContext.ApplicationDetails on alotment.rid equals aplicant.registrationId
                                        join property in pimsContext.SchemePropTrans on alotment.propertyId equals property.propertyId
                                        where alotment.rid == model.RegistrationId
                                        select new CustomerViewModel
                                        {
                                            RegistrationId = alotment.rid,
                                            DepartmentId = alotment.departmentId,
                                            Department = alotment.DepartmentMst.departmentName,
                                            PropertyId = alotment.propertyId,
                                            Applicant = aplicant.tGender == NAConstant.Company ? (string.IsNullOrEmpty(aplicant.T_Company_Name) ? aplicant.tFirstName : aplicant.T_Company_Name) : (aplicant.tFirstName + " " + (string.IsNullOrEmpty(aplicant.tMiddleName) ? string.Empty : aplicant.tMiddleName + " ") + aplicant.tLastName),
                                            MobileNo = aplicant.tMobileNumber,
                                            Email = aplicant.tEmail,
                                            SectorId = property.sectorId,
                                            Sector = property.SectorMst.sectorName,
                                            BlockId = property.blockId,
                                            Block = (property.blockId == null || property.blockId == 0) ? "NA" : property.BlockMst.blockName,
                                            PlotNo = property.propertyNo
                                        }).FirstOrDefault();
                        return customer;
                    }
                    else
                    {
                        model.IsExist = false;
                        return model;
                    }
                }
                else
                {
                    model.IsActive = false;
                    return model;
                }
            }

        }


        public DataSourceResult GetMultiplePaymentStatusList(PaymentViewModel model,DataSourceRequest request)
        {
            using (var dbcontext = new PIMSEntitiesContext())
            {
                //bool IsOTLRP = model.PremiumPaidStatus == NoidaAuthority.PMS.Common.NDCOptions.Id_Yes ? true : false;

                if (model.ActionType == "InstallmentDues")
                {
                    var list = (from instlmnt in dbcontext.InstallmentDuesPayments
                                join alotment in dbcontext.AllotmentMasters on instlmnt.RegistrationId equals alotment.rid
                                join property in dbcontext.SchemePropTrans on alotment.propertyId equals property.propertyId
                                where (model.DepartmentId == null || alotment.departmentId == model.DepartmentId)
                                  && (model.IsOneTimeLeaseRentPaid == null || instlmnt.IsOneTimeLeasePaid == model.IsOneTimeLeaseRentPaid)
                                  && (model.IsTotalInstallmentPaid == null || instlmnt.IsTotalPremiumPaid == model.IsTotalInstallmentPaid)
                                select new PaymentViewModel
                                {
                                    Id = instlmnt.Id,
                                    RegistrationId = instlmnt.RegistrationId,
                                    RegistrationNo=instlmnt.RegistrationId.ToString(),
                                    DuesAmount = instlmnt.DuesAmount,
                                    GSTAmount = instlmnt.GstAmount,
                                    BalanceAmount = instlmnt.BalanceAmount,
                                    BalanceUptoDate = instlmnt.BalanceUptoDate,
                                    SectorName = property.SectorMst.sectorName,
                                    BlockName = property.BlockMst.blockName,
                                    PlotNo = property.propertyNo,
                                    DuesUptoDate = instlmnt.DuesUptoDate,
                                    LeaseRentPremium = instlmnt.BalanceAmount,
                                    LeaseRentDues = null
                                    
                                });
                    return list.ToDataSourceResult(request);
                }
                else
                {
                    var list = (from leaserent in dbcontext.LeaseRentPayments
                                join alotment in dbcontext.AllotmentMasters on leaserent.RegistrationId equals alotment.rid
                                join property in dbcontext.SchemePropTrans on alotment.propertyId equals property.propertyId
                                where (model.DepartmentId == null || alotment.departmentId == model.DepartmentId)
                                && (model.RegistrationId == null || leaserent.RegistrationId == model.RegistrationId)
                                 && (model.IsOneTimeLeaseRentPaid == null || leaserent.IsOneTimeLeasePaid == model.IsOneTimeLeaseRentPaid)
                                 && (model.IsTotalInstallmentPaid == null || leaserent.IsTotalInstallmentPaid == model.IsTotalInstallmentPaid)
                                select new PaymentViewModel
                                {
                                    Id = leaserent.Id,
                                    LeaseRentId = leaserent.Id,
                                    RegistrationId = leaserent.RegistrationId,
                                    RegistrationNo=leaserent.RegistrationId.ToString(),
                                    PropertyNo = property.SectorMst.sectorName + "/" + property.BlockMst.blockName + "/" + property.propertyNo,
                                    LeaseDeedDate = leaserent.LeaseDeedDate,
                                    LeaseRentPremium = leaserent.PremiumLeaseRent,
                                    PenalInterest = leaserent.PanelInterest,
                                    PaidUptoDate = leaserent.PremiumPaidUptoDate,
                                    PremiumPaidDuration = leaserent.PremiumPaidDuration,
                                    RevisedDate = leaserent.RevisedPremiumDate,
                                    RevisedPremium = leaserent.RevisedRate,
                                    BalanceAmount = leaserent.BalanceAmount,
                                    BalanceUptoDate = leaserent.BalanceUptoDate,
                                    BalanceInterest = leaserent.BalanceInterest,
                                    GST = leaserent.GST,
                                    TotalBalance = leaserent.BalanceAmount != null ? leaserent.BalanceAmount : 0 + leaserent.BalanceInterest != null ? leaserent.BalanceInterest : 0 + leaserent.GST != null ? leaserent.GST : 0,
                                    IsOneTimeLeaseRentPaid = leaserent.IsOneTimeLeasePaid,
                                    OneTimePaidStatus = leaserent.IsOneTimeLeasePaid == true ? NoidaAuthority.PMS.Common.NAConstant.Yes_Y : NoidaAuthority.PMS.Common.NAConstant.No_N,
                                    PremiumPaidStatus = leaserent.IsTotalPremiumPaid == true ? NoidaAuthority.PMS.Common.NAConstant.Yes_Y : NoidaAuthority.PMS.Common.NAConstant.No_N,
                                    NDCDate = leaserent.NDCDate,
                                    NDCStatus = leaserent.NDCStatus,
                                    ChallanDate = leaserent.ChallanDate,
                                    StatusId = leaserent.StatusId,
                                    Status = leaserent.StatusId > 0 ? (from status in dbcontext.StatusMasters where status.Id == leaserent.StatusId select status.Status).FirstOrDefault() : string.Empty,
                                    IsActive = leaserent.IsActive,
                                    SectorName = property.SectorMst.sectorName,
                                    BlockName = property.BlockMst.blockName,
                                    PlotNo = property.propertyNo,
                                    LeaseRentDues = leaserent.LeaseRentDues,//for lease rent dues
                                    DuesUptoDate = leaserent.DuesUptoDate,
                                    IsDuesUptoDate = (leaserent.DuesUptoDate == null || DbFunctions.TruncateTime(leaserent.DuesUptoDate) < DbFunctions.TruncateTime(DateTime.Now)) ? false : true
                                });
                    var datalist = list.ToDataSourceResult(request);
                    return datalist;
                }
            }
        }


        public DataSourceResult GetInstallmentPaymentDues(PaymentViewModel model, DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from detail in dbContext.InstallmentDuesPayments
                            //join aplicant in dbContext.ApplicationDetails on detail.Rid equals aplicant.registrationId
                            where detail.RegistrationId == model.RegistrationId
                            select new PaymentViewModel
                            {
                                Id = detail.Id,
                                RegistrationId = detail.RegistrationId,
                                RegistrationNo=detail.RegistrationId.ToString(),
                                DepartmentId = detail.DepartmentId,
                                PropertyId = detail.PropertyId.ToString(),
                                StartDate = detail.InstallmentStartDate,
                                EndDate = detail.InstallmentEndDate,
                                PenalInterest = detail.PenalInterest,
                                GST = detail.GST,
                                NDCDate = detail.NDCDate,
                                NDCStatus = detail.NDCStatus,
                                DuesAmount = detail.DuesAmount,
                                GSTAmount = detail.GstAmount,
                                DuesUptoDate = detail.DuesUptoDate,
                                BalanceInterest = detail.BalanceInterest,
                                BalanceAmount = detail.BalanceAmount,
                                BalanceUptoDate = detail.BalanceUptoDate,
                                PaymentMode = detail.PaymentMode,
                                TransactionId = detail.TransactionId,
                                IsOneTimeLeaseRentPaid = detail.IsOneTimeLeasePaid,
                                //IsTotalPremiumPaid = detail.IsTotalPremiumPaid,
                                //IsInstallmentDeposited = detail.IsOneTimeInstallmentPaid,
                                IsActive = detail.IsActive,
                                //TotalIstallmentStatus = (detail.IsTotalPremiumPaid == null || detail.IsTotalPremiumPaid == true) ? "Paid" : "Not Paid",
                                //OneTimeLeaseRentStatus = (detail.IsOneTimeLeasePaid == null || detail.IsOneTimeLeasePaid == true) ? "Paid" : "Not Paid",
                                ActionType = null
                            });
                return list.ToDataSourceResult(request);
            }
        }


        public DataSourceResult GetNDCDetails(PaymentViewModel model, DataSourceRequest request)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                //var list = (from preNdc in dbContext.PRE_FULL_PAYMENT_NDC
                //                     where preNdc.RegistrationId == model.RegistrationId //&& preNdc.IsActive == true && preNdc.Status == "1"
                //                     select new PaymentViewModel
                //                     {
                //                         RegistrationId=preNdc.RegistrationId,
                //                         RegistrationNo=preNdc.RegistrationId.ToString(),
                //                         NDCDate = preNdc.NDCDate,
                //                         IsTotalPremiumPaid = !string.IsNullOrEmpty(preNdc.TotalPaidPream) ? preNdc.TotalPaidPream == NAConstant.Yes_Y? NAConstant.Yes:NAConstant.No: string.Empty,
                //                         IsOneTimeLeasePaid = !string.IsNullOrEmpty(preNdc.OneTimeLease) ?(preNdc.OneTimeLease == NAConstant.Yes_Y ? NAConstant.Yes : NAConstant.No ): string.Empty,
                //                         LeaseRentUpto = preNdc.LeaseRentUpto
                //                     });
                var list = (from preNdc in dbContext.LeaseRentPayments
                            where preNdc.RegistrationId == model.RegistrationId //&& preNdc.IsActive == true && preNdc.Status == "1"
                            select new PaymentViewModel
                            {
                                RegistrationId = preNdc.RegistrationId,
                                RegistrationNo = preNdc.RegistrationId.ToString(),
                                NDCDate = preNdc.NDCDate,
                                IsTotalPremiumPaid = !string.IsNullOrEmpty(preNdc.IsTotalPremiumPaid.ToString()) ? preNdc.IsTotalPremiumPaid == true ? NAConstant.Yes : NAConstant.No : string.Empty,
                                IsOneTimeLeasePaid = !string.IsNullOrEmpty(preNdc.IsOneTimeLeasePaid.ToString()) ? (preNdc.IsOneTimeLeasePaid == true ? NAConstant.Yes : NAConstant.No) : string.Empty,
                                LeaseRentUpto = preNdc.PremiumPaidDuration
                            });
                return list.ToDataSourceResult(request);
            }
        }


        public DataSourceResult GetJalPaymentPISDataList(DataSourceRequest request, JalViewModel model)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    var data = connection.Query<DtoJalDetailsPaymentHistory>("sp_GetJalDetailsHistoryByRegistrationId", new { RegistrationId = model.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return data.ToDataSourceResult(request);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSourceResult GetLitigationPISDataList(DataSourceRequest request, LitigationViewModel model)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    var data= connection.Query<DtoLegalHistory>("sp_GetLegalHistoryByRegistrationId", new { RegistrationId = model.RegistrationId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    return data.ToDataSourceResult(request);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataSourceResult GetNoticesAndRemarksAsDataSource(DataSourceRequest request, CustomerViewModel model)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                var lst = (from prop in dbContext.Property_Remarks
                           join view in dbContext.view_property on prop.Registration_No equals view.PRDVREGISTRATION_ID
                           where prop.Registration_No == model.RegistrationId.ToString()
                           select new RemarksModel
                           {
                               id = prop.Id,
                               regNo = prop.Registration_No,
                               date = prop.Created_Date,
                               remarks = prop.Remarks,
                           }).ToList();
                return lst.ToDataSourceResult(request);
                //return lst;
            }
        }

        public List<DDList> GetAllBanks()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from prop in dbContext.BankMsts
                           where prop.IsActive == true
                           select new DDList
                           {
                               id = prop.bankId,
                               text = prop.bankName
                           }).ToList();
                var otherBank = new DDList();
                otherBank.id = -1;
                otherBank.text = NAConstant.Other;
                lst.Add(otherBank);
                return lst;
            }
        }


        public List<DDList> GetAllBranchs(int bankId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from prop in dbContext.BranchMsts
                           where prop.IsActive == true && prop.bankId == bankId
                           select new DDList
                           {
                               id = prop.branchId,
                               text = prop.branchName
                           }).ToList();
                if (bankId != -1)
                {
                    var otherBranch = new DDList();
                    otherBranch.id = -1;
                    otherBranch.text = NAConstant.Other;
                    lst.Add(otherBranch);
                }
                return lst;
            }
        }


        public string GetAccountNumber(int bankId, int branchId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var existingRequest = (from m in dbContext.BranchMsts where m.bankId == bankId && m.branchId == branchId && m.IsActive == true select m.accountNumber).FirstOrDefault();
                return existingRequest;
            }
        }


        public List<DDList> GetAllAccountHead()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {

                var lst = (from transTy in dbContext.RECIEPT_HEAD
                           where transTy.STATUS == 1
                           select new DDList
                           {
                               text = transTy.RECIEPT_HEAD_NAME,
                               id = transTy.RECIEPT_CODE
                           }).ToList();
                return lst;
            }
        }


        public List<DDList> GetAccountSubHead(int AccountHeadId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from subhead in dbContext.RECEIPT_SUB_HEAD
                           where subhead.STATUS == 1 && subhead.RECEIPT_CODE == AccountHeadId
                           select new DDList
                           {
                               text = subhead.RECEIPT_SUB_HEAD1,
                               id = subhead.RECEIPT_SUBHEAD_ID
                           }).ToList();
                return lst;
            }
        }


        public int SaveCreateChallan(int rId, int AccountHeadId, int AccountSubHeadId, decimal? Amount)
        {
            int flag = 0;
            List<PaymentViewModel> data = (List<PaymentViewModel>)HttpContext.Current.Session["TempModel"];
            if (data == null)
            {
                List<PaymentViewModel> modelList = new List<PaymentViewModel>();
                PaymentViewModel model = new PaymentViewModel();
                model.RegistrationId = rId;
                model.AccountHeadId = AccountHeadId;
                model.AccountSubHeadId = AccountSubHeadId;
                model.Amount = Amount.Value;
                modelList.Add(model);
                HttpContext.Current.Session["TempModel"] = modelList;
            }
            else
            {
                int count = 0;
                foreach (var models in data)
                {
                    if (models.RegistrationId != rId)
                    {
                        data = null;
                        List<PaymentViewModel> modelList = new List<PaymentViewModel>();
                        PaymentViewModel model = new PaymentViewModel();
                        model.RegistrationId = rId;
                        model.AccountHeadId = AccountHeadId;
                        model.AccountSubHeadId = AccountSubHeadId;
                        model.Amount = Amount.Value;
                        modelList.Add(model);
                        HttpContext.Current.Session["TempModel"] = modelList;

                        return flag = 2;
                    }
                    else
                    {
                        count++;
                    }
                }
                
                PaymentViewModel objmodel = new PaymentViewModel();
                objmodel.RegistrationId = rId;
                objmodel.AccountHeadId = AccountHeadId;
                objmodel.AccountSubHeadId = AccountSubHeadId;
                objmodel.Amount = Amount.Value;
                data.Add(objmodel);
                HttpContext.Current.Session["TempModel"] = data;
            }            
            return flag;
        }


        //public List<PaymentViewModel> GetGeneratedChallanDetails(int rid)
        //{
        //    List<PaymentViewModel> data = (List<PaymentViewModel>)HttpContext.Current.Session["TempModel"];
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var dbContext = new PIMSEntitiesContext();
        //        foreach (var model in data)
        //        {
        //            model.AccountHead = dbContext.RECIEPT_HEAD.Where(h => h.RECIEPT_CODE == model.AccountHeadId).Select(h => h.RECIEPT_HEAD_NAME).FirstOrDefault();
        //            model.AccountSubHead = dbContext.RECEIPT_SUB_HEAD.Where(s => s.RECEIPT_SUBHEAD_ID == model.AccountSubHeadId).Select(s => s.RECEIPT_SUB_HEAD1).FirstOrDefault();
        //        }
        //        return data;
        //    }
        //}


        public DataSourceResult GetTransferHistoryByIdAsDataSource(DataSourceRequest request, TransferViewModel model)
        {
            //using (var dbcontext = new PIMSEntitiesContext())
            using (var dbContext = new PIMSEntitiesContext())
            {
                List<TransferViewModel> transferList = new List<TransferViewModel>();
                if (model.ActionType == "Transfer")
                {
                    var data = (from transfer in dbContext.Succ_Mut_Trans
                                where transfer.Rid == model.RegistrationId
                                select new TransferViewModel
                                {
                                    Id = transfer.Request_No,
                                    RequestNo = transfer.Request_No,
                                    RegistrationId = transfer.Rid,
                                    Applicant = transfer.Applicant_Name,
                                    ApplicantAddress = transfer.Correspondance_Add,
                                    Transferee = transfer.T_Gender == "Company" ? transfer.T_Company_Name : transfer.T_First_Name + " " + transfer.T_Middle_Name + " " + transfer.T_Last_Name,
                                    TransfereeAddress = transfer.T_Correspondence_Add,
                                    TransferType = transfer.Type == "M" ? "Mutation" : "Transfer",
                                    TransferDate = transfer.Transfer_Date,
                                    PropertyNo = null,
                                    IsActive = transfer.Is_Active,
                                    TransferStatus = dbContext.StatusMasters.Where(s => s.Id == transfer.Status).Select(s => s.Status).FirstOrDefault()
                                }).ToList();
                    transferList.AddRange(data);
                }
                if (model.ActionType == "GPA")
                {
                    var data = (from gpa in dbContext.GPAs
                                where gpa.Rid == model.RegistrationId
                                select new TransferViewModel
                                {
                                    Id = gpa.Id,
                                    RegistrationId = gpa.Rid,
                                    GPAHolderName = gpa.GPA_Holder_Name,
                                    GPAHolderAddress = gpa.GPA_Holder_Address,
                                    GPAEffectiveFrom = gpa.Effcetd_From,
                                    GPAEffectiveTo = gpa.Effected_To,
                                    IsActive = gpa.Is_Active,
                                    IsGPAExecuted = gpa.Registered,
                                    Relation = gpa.Relation,
                                    ApprovedDate = gpa.Acceptance_date
                                });
                    transferList.AddRange(data);
                }
                if (model.ActionType == "Nominee")
                {
                    var data = (from nominee in dbContext.Nominee_Details
                                where nominee.Rid == model.RegistrationId
                                select new TransferViewModel
                                {
                                    Id = nominee.Id,
                                    RegistrationId = nominee.Rid,
                                    Nominee = nominee.Nominee_Name,
                                    ApprovedDate = nominee.Nomination_Date,
                                    IsActive = (nominee.Is_Active == null || nominee.Is_Active == 0) ? false : true
                                });
                    transferList.AddRange(data);
                }
                return transferList.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetMortgageHistoryByIdAsDataSource(DataSourceRequest request, MortgageViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from mortgage in dbContext.MortgageDetails
                            join aplicant in dbContext.ApplicationDetails on mortgage.RID equals aplicant.registrationId
                            where mortgage.RID == model.RegistrationId
                            select new MortgageViewModel
                            {
                                Id = mortgage.RequestNo,
                                RegistrationId = mortgage.RID,
                                MortgageDate = mortgage.MortgageDate,
                                MortgageType = mortgage.MortgageType == "2" ? "Collateral" : "Normal",
                                BankName = mortgage.BankName,
                                BranchAddress = mortgage.BranchAddress,
                                MortgageStatus = mortgage.StatusMaster.Status,
                                CreatedDate = mortgage.CreatedDate,
                                ApproveDate = mortgage.ApproveDate,
                                ValidUpto = mortgage.ValidUpto,
                                ProcessingFee = mortgage.ProcessingFee,
                                SanctionedAmount = mortgage.SanctionedAmount,
                                Applicant = aplicant.tGender == NAConstant.Company ? aplicant.T_Company_Name : aplicant.tFirstName + " " + aplicant.tMiddleName + " " + aplicant.tLastName
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetExtensionHistoryByIdAsDataSource(DataSourceRequest request, ExtensionViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from extension in dbContext.Extension_Details
                            where extension.Rid == model.RegistrationId
                            select new ExtensionViewModel
                            {
                                Id = extension.Id,
                                RegistrationId = extension.Rid,
                                CompletionDueDate = extension.Completion_DueDate,
                                ExtensionDueDate = extension.Extension_Due_Date,
                                ExtensionGivenDate = extension.Extension_Given_Date,
                                ExtensionCharge = extension.Extension_Charge,
                                ApprovedDate = extension.Approved_Date,
                                Comment = null,
                                ExtensionStatus = dbContext.StatusMasters.Where(s => s.Id == extension.Status).Select(s => s.Status).FirstOrDefault()
                            });
                return data.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetRentingHistoryByIdAsDataSource(DataSourceRequest request, RentingViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var requests = (from rent in dbContext.RentPermissionDetails
                                join alot in dbContext.AllotmentMasters on rent.Rid equals alot.rid
                                where rent.Rid == model.RegistrationId
                                select new RentingViewModel
                                {
                                    Id = rent.RequestNo,
                                    RegistrationId = rent.Rid.Value,
                                    //SchemeName = alot.SchemeMst.schemeName,
                                    //Department = alot.DepartmentMst.departmentName,
                                    Applicant = alot.ApplicationDetail.tFirstName + " " + alot.ApplicationDetail.tMiddleName + " " + alot.ApplicationDetail.tLastName,
                                    TenantName = rent.TenantName,
                                    RequestDate = rent.RentingDate.Value,
                                    RentStatus = rent.StatusMaster.Status
                                });
                return requests.ToDataSourceResult(request);
            }
        }

        public DataSourceResult GetCICHistoryByIdAsDataSource(DataSourceRequest request, CICViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (model.ActionType == "Directors")
                {
                    var list = (from masters in dbContext.Director_Request_Master
                                join director in dbContext.Firm_Director_Master on masters.Rid equals director.Rid
                                where masters.Rid == model.RegistrationId
                                select new CICViewModel
                                {
                                    Id = masters.Id,
                                    RegistrationId = masters.Rid,
                                    RequestDate = masters.Request_Date,
                                    ApprovedDate = masters.Approved_date,
                                    IsCICActive = masters.Is_Active,
                                    CICCharge = masters.CIC_Charge,
                                    DirectorName = director.Director_Name,
                                    DirectorShare = director.Director_Share,
                                    DirectorId = director.Director_Id,
                                    TypeId = director.Type,
                                    TypeName = director.Type != null ? dbContext.Common_Config.FirstOrDefault(d => d.Id == director.Type && d.Category == "Director").Name : string.Empty,
                                    OnlineRequestNo = masters.OnlineRequestNo
                                });
                    return list != null ? list.ToDataSourceResult(request) : null;
                }
                if (model.ActionType == "FirmName")
                {
                    var list = (from firm in dbContext.Firm_Master
                                where firm.Rid == model.RegistrationId
                                select new CICViewModel
                                {
                                    Id = firm.Id,
                                    OldFirmName = firm.Old_Firm_Name,
                                    NewFirmName = firm.New_Firm_Name,
                                    OldFirmStatus = firm.Old_Firm_Status != null ? dbContext.Common_Config.FirstOrDefault(d => d.Id == firm.Old_Firm_Status && d.Category == "Firm Status").Name : string.Empty,
                                    NewFirmStatus = firm.Old_Firm_Status != null ? dbContext.Common_Config.FirstOrDefault(d => d.Id == firm.New_Firm_Status && d.Category == "Firm Status").Name : string.Empty,
                                    IsCICActive = firm.Is_Active,
                                    ChangeTypeId = firm.Change_Type,
                                    ApprovedDate = firm.Approved_date,
                                    RequestDate = firm.Request_Date
                                });
                    return list != null ? list.ToDataSourceResult(request) : null;
                }
                else
                {
                    return null;
                }
            }
        }

        public DataSourceResult GetFunctionalHistoryByIdAsDataSource(DataSourceRequest request, FunctionalViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from data in dbContext.FunctionalDetails
                            where data.Rid == model.RegistrationId
                            select new FunctionalViewModel
                            {
                                Id = data.RequestNo,
                                RequestNo = data.RequestNo,
                                RegistrationId = data.Rid,
                                PropertyNo = data.PropertyNumber,
                                IsFunctional = data.Functional,
                                FunctionalDueDate = data.FunctionalDueDate,
                                FunctionalDate = data.FunctionalDate,
                                FunctionalCharge = data.FunctionalCharge,
                                IsMeterSealed = data.MeterSeallingDocFlag,
                                IsAffidavit = data.AffidavitFlag,
                                IsRegistered = data.RegistrationCertiFlag,
                                IsNOCAccount = data.NOCAccountFlag,
                                IsActive = data.IsActive,
                                Comment = data.Comment
                            });
                return list != null ? list.ToDataSourceResult(request) : null;
            }
        }

        public DataSourceResult GetServiceRequestUploadedDocumentsById(DataSourceRequest request, ServiceViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                List<DocumentViewModel> documentList = new List<DocumentViewModel>();
                FtpHandler ftpHandler = new FtpHandler();
                var service = dbContext.Customer_ServiceRequest.FirstOrDefault(f => f.Id == model.RequestId);
                if (service != null)
                {
                    if (service.Registration_No != null)
                    {
                        documentList = ftpHandler.GetServiceRequestUploadedDocumentsById((int)model.RegistrationId, (int)model.RequestId);
                    }
                }
                return documentList.ToDataSourceResult(request);
            }
        }


        public ServiceRequestViewModel ReSubmitServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<HttpPostedFileBase> files)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var service = dbContext.Customer_ServiceRequest.Where(r => r.Id == model.ServiceModel.RequestId).FirstOrDefault();
                var status = dbContext.Customer_ServiceStatusTrans.Where(c => c.RequestRefId == model.ServiceModel.RequestId).OrderByDescending(c => c.Id).FirstOrDefault();
                service.Request_Status = NAStatusId.Resubmitted;
                //service.ObjectionStatus = NAStatusId.Resubmitted;
                service.Modified_Date = DateTime.Now;
                service.Modified_By = model.RegistrationId;
                if (status != null)
                {
                    status.StatusId = NAStatusId.Resubmitted;
                    status.ModifiedBy = model.RegistrationId;
                    status.ModifiedDate = DateTime.Now;
                    status.Remarks = status.Remarks + "\n" + "Resubmitted by \n" + model.RegistrationId +" " + " Date:" + DateTime.Now + ".";
                }
                dbContext.SaveChanges();
                var flag = false;
                model.Id = model.ServiceModel.RequestId;
                if (files != null)
                {
                    if (files.ToList().Count > 0)
                    {
                        flag = FtpHandler.UploadFiles(files, model.ServiceModel.RegistrationId.ToString(), model.ServiceModel.RequestId.Value);
                        model.ServiceModel.ActionTypeId = flag == true ? ReturnType.Success : ReturnType.Failed;
                    }
                }
                return model;
            }
        }


        public PaymentViewModel GetNDCDetailsByRid(int rid)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                PaymentViewModel PreFullPaymentNdc = new PaymentViewModel();
                var leaserentndc = dbContext.LeaseRentPayments.FirstOrDefault(l => l.RegistrationId == rid);
                if (leaserentndc != null)
                {
                    PreFullPaymentNdc.IsOneTimeLeaseRentPaid = leaserentndc.IsOneTimeLeasePaid;
                    PreFullPaymentNdc.IsTotalInstallmentPaid = leaserentndc.IsTotalPremiumPaid;
                    PreFullPaymentNdc.NDCDate = leaserentndc.NDCDate;
                    PreFullPaymentNdc.PremiumPaidDuration = leaserentndc.PremiumPaidDuration;
                }
                return PreFullPaymentNdc;
            } 
        }


        public string GetUploadedDocumentByServiceId(int Id, int Rid, string ActionType)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var file =string.Empty;
                FtpHandler ftpHandler = new FtpHandler();
                var service = dbContext.Customer_ServiceRequest.FirstOrDefault(f => f.Id == Id);
                if (service != null)
                {
                    if (service.Registration_No != null)
                    {
                        if (ActionType == "Complete")
                        {
                            file = ftpHandler.GetUploadedDocumentByServiceId(Rid, Id);
                        }
                        else if (ActionType == "Objection")
                        {
                            file = ftpHandler.GetObjectionDocumentByServiceId(Rid, Id);
                        }
                        else
                        {
                            file = null;
                        }
                    }
                }
                return file;
            }
        }

        public string GetRequiredFileUploadHtmlByServiceRequestId(int? requestId)
        {
            using (var context = new PIMSEntitiesContext())
            {
                string divMain = "";
                divMain = "<div class='row  file-header'> "
                                        + "<div class='col-md-3 col-lbl'><label>Serial No</label></div>"
                                        + "<div class='col-md-6 col-lbl'><label>Required File</label></div>"
                                        + "<div class='col-md-3 col-lbl-vl'><label>Select File</label></div>"
                                    + "</div>";
                var service = context.Customer_ServiceRequest.FirstOrDefault(c => c.Id == requestId);
                var requireDocs = context.Customer_ServiceRequestDocument.Where(d => d.ServiceRequestRefId == requestId).ToList();
                if (service != null && requireDocs != null && requireDocs.Count() > 0)
                {
                    var checklists = (from docs in context.Customer_ServiceRequestDocument
                                      join checklist in context.ServiceCheckList_Master on new { x = docs.DepartmentId, y = docs.DocumentRefId } equals new { x = checklist.dept_id, y = checklist.Checklist_Ref } //docs.DepartmentId equals checklist.dept_id
                                      //where checklist.dept_id == service.DepartmentId && checklist.service_id == requestId
                                      where docs.ServiceRequestRefId == requestId
                                      select new ServiceCheckListModel
                                      {
                                          Id = checklist.ChkId,
                                          DepartmentId = checklist.dept_id,
                                          Department = context.DepartmentMsts.Where(d => d.departmentId == docs.DepartmentId).Select(d => d.departmentName).FirstOrDefault(),
                                          ServiceId = checklist.service_id,
                                          ChecklistRefNo = checklist.Checklist_Ref,
                                          ChecklistName = checklist.ChkName,
                                          Status = checklist.Status
                                      }).ToList();

                    if (checklists.Count != 0)
                    {

                        int docCounter = 1;
                        foreach (var item in checklists)
                        {
                            divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'><label>" + docCounter + "</label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>" + item.ChecklistName + "</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='file' class='single' name='files' /></div>"
                                              + "</div>";
                            docCounter++;
                        }
                        divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'><label>" + docCounter + "</label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>" + "Other" + "</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='file' class='single' name='files' /></div>"
                                              + "</div>";

                        divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'></div>"
                                                   + "<div class='col-md-6 col-lbl'></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='button' id='btn-upload-objection-file' class='k-button' value='Uploda Document' /></div>"
                                              + "</div>";
                        return divMain;
                    }
                    else
                    {
                        divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'><label>" + 1 + "</label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>" + "Other" + "</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='file' class='single' name='files' /></div>"
                                              + "</div>";

                        divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'></div>"
                                                   + "<div class='col-md-6 col-lbl'></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='button' id='btn-upload-objection-file' class='k-button' value='Uploda Document' /></div>"
                                              + "</div>";
                        return divMain;
                    }
                }
                else {
                    divMain = divMain + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'><label>" + 1 + "</label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>" + "Other" + "</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><input type='file' class='single' name='files' /></div>"
                                              + "</div>";

                    divMain = divMain + "<div class='row  row-border row-file'> "
                                               + "<div class='col-md-3 col-lbl'></div>"
                                               + "<div class='col-md-6 col-lbl'></div>"
                                               + "<div class='col-md-3 col-lbl-vl'><input type='button' id='btn-upload-objection-file' class='k-button' value='Uploda Document' /></div>"
                                          + "</div>";
                    return divMain; 
                }
            }
        }

        public string GetChargesRequiredAsHtmlContentByServiceRequestId(int? requestId)
        {
            using (var context = new PIMSEntitiesContext())
            {
                var service = context.Customer_ServiceRequest.FirstOrDefault(c => c.Id == requestId);
                var chargelist = context.Customer_ServiceRequestCharges.Where(d => d.ServiceRequestRefId == requestId).ToList();
                if (service != null && chargelist != null && chargelist.Count() > 0)
                {
                    var list = (from charge in context.Customer_ServiceRequestCharges
                                join head in context.RECIEPT_HEAD on charge.AccountHeadId equals head.RECIEPT_CODE
                                join subhd in context.RECEIPT_SUB_HEAD on charge.AccountSubHeadId equals subhd.RECEIPT_SUBHEAD_ID
                                where charge.ServiceRequestRefId == requestId && head.STATUS == 1 && head.STATUS == 1
                                select new PaymentViewModel
                                {
                                    Id = charge.Id,
                                    OnlineRequestId = charge.ServiceRequestRefId,
                                    AccountHeadId = charge.AccountHeadId,
                                    AccountHead = head.RECIEPT_HEAD_NAME,
                                    AccountSubHeadId = charge.AccountSubHeadId,
                                    AccountSubHead = subhd.RECEIPT_SUB_HEAD1,
                                    Amount = charge.Amount
                                }).ToList();
                    string htmlcontent = string.Empty;
                    if (list != null && list.Count != 0)
                    {
                        htmlcontent = "<div class='row  file-header'> "
                                        + "<div class='col-md-3 col-lbl'><label>Serial No</label></div>"
                                        + "<div class='col-md-6 col-lbl'><label>Accounts Head</label></div>"
                                        + "<div class='col-md-3 col-lbl-vl'><label>Amount</label></div>"
                                    + "</div>";

                        int counter = 1;
                        decimal? totalamount = 0;
                        foreach (var item in list)
                        {
                            htmlcontent = htmlcontent + "<div class='row  row-border row-file'> "
                                                   + "<div class='col-md-3 col-lbl'><label>" + counter + "</label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>" + item.AccountSubHead + "</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><label>" + item.Amount + "</div>"
                                              + "</div>";
                            totalamount = totalamount + item.Amount;
                            counter++;
                        }
                        htmlcontent = htmlcontent + "<div class='row  row-border'> "
                                                   + "<div class='col-md-3 col-lbl'><label></label></div>"
                                                   + "<div class='col-md-6 col-lbl'><label>Total Amount</label></div>"
                                                   + "<div class='col-md-3 col-lbl-vl'><label>" + totalamount + "</label><span class='pay-charge-online hyperlink'>Pay Now</span></div>"
                                              + "</div>";
                        return htmlcontent;
                    }
                    else return null;
                }
                else return null;
            }
        }
    }
}
