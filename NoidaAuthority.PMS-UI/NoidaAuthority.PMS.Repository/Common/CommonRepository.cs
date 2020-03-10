using Dapper;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
//using NoidaAuthority.PMS.Repository.Common;
using NoidaAuthority.PMS.Repository.Entities;
//using NoidaAuthority.PMS.Repository.Entities.Citizen;
using System.Resources;
using System.Collections;
using System.Globalization;
using NoidaAuthority.PMS.Repository.Context;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;


namespace NoidaAuthority.PMS.Repository
{
    public class CommonRepository : ICommonRepository
    {
        public IList<DtoList> LookupPropertyType()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetLookupPropertyType", new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DtoList> LookupSector(int propertyID)
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return
                        connection.Query<DtoList>("dbo.sp_GetLookupSector", new { propertyID },
                            commandType: System.Data.CommandType.StoredProcedure)
                            .ToList();
                }
            }


            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DtoList> GetLegalDDL()
        {
            try
            {
                var list = new List<DtoList>();
                var rec = new DtoList();
                var rec1 = new DtoList();
                var rec2 = new DtoList();
                rec.label = "Supreme Court";
                rec.value = "1";
                rec1.label = "High Court";
                rec1.value = "2";
                rec2.label = "District Court";
                rec2.value = "3";
                list.Add(rec2);
                list.Add(rec1);
                list.Add(rec);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<DtoList> LookupArea()
        {
            try
            {
                using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
                {
                    return
                        connection.Query<DtoList>("dbo.sp_GetLookupArea", new { },
                            commandType: System.Data.CommandType.StoredProcedure)
                            .ToList();
                }
            }


            catch (Exception ex)
            {
                return null;
            }
        }

        public List<DtoList> GetRevYears()
        {
            try
            {
                var list = new List<DtoList>();
                var begYear = "2000";
                for (int i = Convert.ToInt32(begYear); i <= DateTime.Today.Year; i++)
                {
                    var rec = new DtoList();
                    rec.value = i.ToString();
                    rec.label = i.ToString() + "-" + (i + 1).ToString();
                    list.Add(rec);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IList<DtoList> LookupBlock(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetLookupBlock", new { sectorId = objPropertyFilter.Sector },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DtoList> LookupAlotmentStatus()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetLookupAlotmentStatus", new { },
                        commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DtoList> AutoFillPropertyNo(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetAutoPropertyNoList",
                        new { SearchType = objPropertyFilter.SearchType, PropertyNumber = objPropertyFilter.PropertyNo, PropertyTypeId = objPropertyFilter.PropertyTypeId, Sector = objPropertyFilter.Sector, Block = objPropertyFilter.Block }, commandType: System.Data.CommandType.StoredProcedure)
                        .ToList();
            }
        }
        public IList<DtoList> AutoFillAlloteName(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return
                    connection.Query<DtoList>("dbo.sp_GetAutoAlloteName",
                        new { SearchType = objPropertyFilter.SearchType, PropertyTypeId = objPropertyFilter.PropertyTypeId, Sector = objPropertyFilter.Sector, Block = objPropertyFilter.Block, AlloteName = objPropertyFilter.AlloteName }, commandType: System.Data.CommandType.StoredProcedure)
                        .ToList();
            }

        }
        public IList<DtoList> AutoFillPlotNumber(DtoPropertyFilter objPropertyFilter)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                return
                    connection.Query<DtoList>("dbo.sp_GetAutoPlotNumber",
                        new { SearchType = objPropertyFilter.SearchType, PropertyTypeId = objPropertyFilter.PropertyTypeId, Sector = objPropertyFilter.Sector, Block = objPropertyFilter.Block, PlotNumber = objPropertyFilter.PlotNumber }, commandType: System.Data.CommandType.StoredProcedure)
                        .ToList();
            }
        }

        public PropertyCount GetPropertyCount()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {

                return connection.Query<PropertyCount>("dbo.sp_GetAllPropertyCount", commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
            }
        }


        public List<DtoList> GetAllServiceData(int deptt)
        {
            var lst = new List<DtoList>();
            using (var dbContext = new PIMSEntitiesContext())
            {
                lst = (from csm in dbContext.CitizenService_Master
                       where csm.Deptt_Id == deptt && csm.Status == 1
                       select new DtoList
                       {
                           label = csm.ServiceName,
                           value = csm.service_id.ToString() // changed from id to serviceid
                       }).ToList();
            }
            return lst;
        }

        public List<DDList> YesNoDDL()
        {
            var lst = new List<DDList>();
            var opt1 = new DDList();
            opt1.text = Contants.yes;
            opt1.id = NoidaAuthority.PMS.Common.Contants.MortPrevLoan.yes;
            var opt2 = new DDList();
            opt2.text = Contants.no;
            opt2.id = NoidaAuthority.PMS.Common.Contants.MortPrevLoan.no;
            lst.Add(opt1);
            lst.Add(opt2);
            return lst;
        }

        public DtoPropertyDetails GetDepttByRID(int RID)
        {
            using (var dbContext = new NOIDANEWEntities())
            {
                //var depttId = (from all in dbContext.AllotmentMasters where all.rid == RID && all.isActive == 1 select all.departmentId).FirstOrDefault();
                //return depttId.Value;
                //var details = (from app in dbContext.ApplicationDetails
                //               join all in dbContext.AllotmentMasters on app.registrationId equals all.rid
                //               join spt in dbContext.SchemePropTrans on all.propertyId equals spt.propertyId
                //               join sm in dbContext.SectorMsts on spt.sectorId equals sm.sectorId
                //               join bm in dbContext.BlockMsts on spt.blockId equals bm.blockId
                //               where all.rid == RID && all.isActive == 1
                //               select new DtoPropertyDetails
                //               {
                //                   //Name = app.tGender == "Company" ? app.tco
                //                   Name = app.tFirstName + " " + app.tMiddleName + " " + app.tLastName,
                //                   PropertyNumber = sm.sectorName + "/" + bm.blockName + "-" + spt.propertyNo,
                //                   CustomerAddress = app.tPermanentAdd,
                //                   Mobile = app.tMobileNumber,
                //                   EmailId = app.tEmail,
                //                   DepttId = app.departmentId
                //               }).FirstOrDefault();
                //return details;
                var details = new DtoPropertyDetails();
                details.isRIdExists = false;
                var det = (from vw in dbContext.view_property
                           where vw.PRDVREGISTRATION_ID == RID.ToString()
                           select vw).FirstOrDefault();
                if (det != null)
                {
                    details.Name = det.PRDVREGIST_APPLICANT_NAME;
                    details.PropertyNumber = det.SECTOR + "/" + det.BLOCK + "-" + det.PLDIPROPERTY_NO;
                    details.CustomerAddress = det.PRDVREGIST_APPLICANT_ADDRESS;
                    details.Mobile = det.mobile_no;
                    details.isRIdExists = true;
                    details.DepttId = Convert.ToInt32(det.PLDVLAND_USE_TYPE_ID);
                }
                //details = (from vw in dbContext.view_property
                //               where vw.PRDVREGISTRATION_ID == RID.ToString()
                //               select new DtoPropertyDetails
                //               {
                //                   Name = vw.PRDVREGIST_APPLICANT_NAME,
                //                   PropertyNumber = vw.PLDIPROPERTY_NO,
                //                   CustomerAddress = vw.PRDVREGIST_APPLICANT_ADDRESS,
                //                   Mobile = vw.mobile_no,                                   
                //                   //EmailId = vw
                //               }).FirstOrDefault();
                //if()
                return details;
            }
        }


        public List<DDList> GetDepartmentList()
        {
            List<DDList> deptList = new List<DDList>();
            using (var dbContext = new PIMSEntitiesContext())
            {
                deptList = (from dept in dbContext.DepartmentMsts
                            select new DDList
                            {
                                id = dept.departmentId,
                                text = dept.departmentName
                            }).ToList();
            }
            return deptList;
        }



        public List<DDList> GetServicesList(int? departmentId)
        {
            List<DDList> serviceList = new List<DDList>();
            using (var dbContext = new PIMSEntitiesContext())
            {
                serviceList = (from dept in dbContext.CitizenService_Master
                                where dept.Deptt_Id == departmentId
                                select new DDList
                                {
                                    id = dept.service_id.Value,
                                    text = dept.ServiceName
                                }).ToList();
            }
            return serviceList;
        }

        // added by Shatrughna for customer

        public List<DropdownViewModel> GetServiceListByDepartment(int departmentId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (departmentId == NADepartmentId.Industrial)
                {
                    List<int?> NICServiceIdList = new List<int?>() { 1, 2, 6, 10, 12, 13, 19, 21, 22, 25 };
                    var lst = (from service in dbContext.CitizenService_Master
                               where service.Deptt_Id == departmentId && service.Status == 1 && !NICServiceIdList.Contains(service.service_id)
                               select new DropdownViewModel
                               {
                                   Id = service.service_id.Value,
                                   ServiceId = service.service_id,
                                   Text = service.ServiceName
                               }).ToList();
                    return lst;
                }
                else
                {
                    var lst = (from service in dbContext.CitizenService_Master
                               where service.Deptt_Id == departmentId && service.Status == 1 && service.service_id != 21 && service.service_id != 22 && service.service_id != 25
                               select new DropdownViewModel
                               {
                                   Id = service.service_id.Value,
                                   ServiceId = service.service_id,
                                   Text = service.ServiceName
                               }).ToList();
                    return lst;
                } 
            }
        }

        public List<DropdownViewModel> GetSubDepartmentList(int departmentId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from service in dbContext.SubDepartmentMsts
                           where service.departmentId == departmentId && service.IsActive == true
                           select new DropdownViewModel
                           {
                               Id = service.SubdepartmentId,
                               Text = service.SubdepartmentName
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetTransferTypeList()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from transTy in dbContext.Transfer_Type
                           where transTy.Parent_Id == null && transTy.Is_Active == true
                           select new DropdownViewModel
                           {
                               Text = transTy.type,
                               Id = transTy.Id
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetTransferSubTypeList(int transferTypeId)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from transTy in dbContext.Transfer_Type
                           where transTy.Parent_Id == transferTypeId && transTy.Is_Active == true
                           select new DropdownViewModel
                           {
                               Text = transTy.type,
                               Id = transTy.Id
                           }).ToList();
                return lst;
            }
        }


        public List<DropdownViewModel> GetCICRequestTypeList()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from cfg in dbContext.Common_Config
                           where cfg.Is_Active == 1 && cfg.Category.ToLower().Equals(NACategoryType.CIC.ToLower())
                           select new DropdownViewModel
                           {
                               Id = cfg.Id,
                               Text = cfg.Name
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetCompanyMemberTypeList()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from cfg in dbContext.Common_Config
                           where cfg.Is_Active == 1 && cfg.Category.ToLower().Equals(NACategoryType.Director.ToLower())
                           select new DropdownViewModel
                           {
                               Id = cfg.Id,
                               Text = cfg.Name
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetFirmStatusList()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from cfg in dbContext.Common_Config
                           where cfg.Is_Active == 1 && cfg.Category.ToLower().Equals(NACategoryType.FirmStatus.ToLower())
                           select new DropdownViewModel
                           {
                               Id = cfg.Id,
                               Text = cfg.Name
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetGenderList()
        {
            List<DropdownViewModel> genderList = new List<DropdownViewModel>();
            genderList.Add(new DropdownViewModel { Text = NAConstant.Male });
            genderList.Add(new DropdownViewModel { Text = NAConstant.Female });
            return genderList;
        }

        public List<DropdownViewModel> GetOccupationList()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var lst = (from f in dbContext.OccupationMsts
                           select new DropdownViewModel
                           {
                               Id = f.occupationId,
                               Text = f.occupation
                           }).ToList();
                return lst;
            }
        }

        public List<DropdownViewModel> GetMortgageTypeList()
        {
            List<DropdownViewModel> mortgageType = new List<DropdownViewModel>();
            foreach (int value in Enum.GetValues(typeof(NoidaAuthority.PMS.Common.Contants.MortgageType)))
            {
                mortgageType.Add(new DropdownViewModel
                {
                    Text = Enum.GetName(typeof(NoidaAuthority.PMS.Common.Contants.MortgageType), value),
                    Id = value
                });
            }
            return mortgageType;
        }

        public List<DropdownViewModel> GetGPAStatusList()
        {
            List<DropdownViewModel> gender = new List<DropdownViewModel>();
            foreach (int value in Enum.GetValues(typeof(EnumStatusType)))
            {
                gender.Add(new DropdownViewModel
                {
                    Text = Enum.GetName(typeof(EnumStatusType), value),
                    Id = value
                });
            }
            return gender;
        }


        public List<DropdownViewModel> GetNOCStatusList()
        {
            List<DropdownViewModel> statusList = new List<DropdownViewModel>();
            foreach (int value in Enum.GetValues(typeof(NoidaAuthority.PMS.Common.Contants.MortgagePrevLoan)))
            {
                statusList.Add(new DropdownViewModel
                {
                    Text = Enum.GetName(typeof(NoidaAuthority.PMS.Common.Contants.MortgagePrevLoan), value),
                    Id = value
                });
            }
            return statusList;
        }


        public List<DropdownViewModel> GetDepartmentListII()
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var list = (from dept in dbContext.DepartmentMsts
                            select new DropdownViewModel
                            {
                                Id = dept.departmentId,
                                Text = dept.departmentName
                            }).ToList();
                return list;
            }            
        }


        public List<DropdownViewModel> GetSecurityQuestionList()
        {
            List<DropdownViewModel> messageList = new List<DropdownViewModel>();
            ResourceSet resourceSet = SecurityQuestion.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                var message = new DropdownViewModel();
                message.Text = entry.Key.ToString();
                message.Value = (string)entry.Value;
                messageList.Add(message);
            }
            return messageList;
        }


        public List<DropdownViewModel> GetCustomerIdList()
        {
            List<DropdownViewModel> IdList = new List<DropdownViewModel>();
            IdList.Add(new DropdownViewModel { Text = "PANCard", Value = "PanCard" });
            IdList.Add(new DropdownViewModel { Text = "AadhaarCard", Value = "AadhaarCard" });
            return IdList;
        }


        public List<DropdownViewModel> GetLetterTypeList()
        {
            List<DropdownViewModel> letterTypeList = new List<DropdownViewModel>();
            letterTypeList.Add(new DropdownViewModel { Text = "AllotmentLetter", Value = "AllotmentLetter" });
            letterTypeList.Add(new DropdownViewModel { Text = "TransferLetter", Value = "TransferLetter" });
            return letterTypeList;
        }


        public List<DropdownViewModel> GetRoleTypeList()
        {
            List<DropdownViewModel> roleTypeList = new List<DropdownViewModel>();
            roleTypeList.Add(new DropdownViewModel { Text = "Admin", Id= 1});
            roleTypeList.Add(new DropdownViewModel { Text = "Customer", Id = 2});
            roleTypeList.Add(new DropdownViewModel { Text = "Department Head", Id = 3 });
            roleTypeList.Add(new DropdownViewModel { Text = "Back Office", Id = 4 });
            roleTypeList.Add(new DropdownViewModel { Text = "JSK", Id = 5 });
            return roleTypeList;
        }


        public List<DropdownViewModel> GetMiscellaneousTypeList(DropdownViewModel model)
        {
            List<DropdownViewModel> list = new List<DropdownViewModel>();
            if (model.FilterType == NAConstant.IdType)
            {
                list.Add(new DropdownViewModel { Text = "Pan Card", Value = "PanCard"});
                list.Add(new DropdownViewModel { Text = "Aadhar Card", Value = "AadharCard" });
                //list.Add(new DropdownViewModel { Text = "Driving License", Value = "Driving License" });
            }
            else if (model.FilterType == NAConstant.LetterType)
            {
                list.Add(new DropdownViewModel { Text = "Allotment Letter", Value = "AllotmentLetter" });
                list.Add(new DropdownViewModel { Text = "Transfer Letter", Value = "TransferLetter" });
            }
            return list;
        }


        public ActionViewModel SendMessageOnMobileAndEmail(ActionViewModel model)
        {
            if (model.ActionType == NAConstant.OTP)
            {
                int otp = ApplicationHelper.GenerateOTP();
                model.OTP = otp;
                var msgs = "Dear User, Your OTP is " + otp + " DO NOT disclose this OTP to anyone. This is for online use by you only. Regards, http://mynoida.in";
                model.ReturnTypeId = ApplicationHelper.SendSMS(model.MobileNo, msgs);
            }
            return model;
        }


        public List<DropdownViewModel> GetSectorList(DropdownViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from sector in dbContext.SectorMsts
                            select new DropdownViewModel
                            {
                                Id = sector.sectorId,
                                Text = sector.sectorName,
                                Value = sector.sectorName,
                            }).ToList();
                return data;
            }
        }

        public List<DropdownViewModel> GetBlockList(DropdownViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                var data = (from block in dbContext.BlockMsts
                            select new DropdownViewModel
                            {
                                Id = block.blockId,
                                Text = block.blockName,
                                Value = block.blockName,
                            }).ToList();
                return data;
            }
        }


        public IList<DropdownViewModel> GetPropertyType()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DropdownViewModel>("dbo.sp_GetLookupPropertyType", new { }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DropdownViewModel> GetPropertyTypeII()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DropdownViewModel>("dbo.sp_GetLookupPropertyType", new { }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DropdownViewModel> GetAreaTypeII()
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DropdownViewModel>("dbo.sp_GetLookupArea", new { }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DropdownViewModel> GetSectorListII(int propertyId)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DropdownViewModel>("dbo.sp_GetLookupSector", new { propertyId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }

        public IList<DropdownViewModel> GetBlockListII(PropertyFilterViewModel model)
        {
            using (System.Data.IDbConnection connection = NADbConnection.GetPISConnection())
            {
                return connection.Query<DropdownViewModel>("dbo.sp_GetLookupBlock", new { sectorId = model.Sector }, commandType: System.Data.CommandType.StoredProcedure).ToList();
            }
        }
        
        public List<DropdownViewModel> GetServicesListII()
        {
            throw new NotImplementedException();
        }


        public DataSourceResult GetBankDetailAsDataSource(Kendo.Mvc.UI.DataSourceRequest request, DropdownViewModel model)
        {
            using (var dbContext = new PIMSEntitiesContext())
            {
                if (model.FilterType == "Bank")
                {
                    var data = (from bank in dbContext.BankMsts
                                where bank.IsActive == true && (model.BankId == null || bank.bankId == model.BankId)
                                select new DropdownViewModel
                                {
                                    FilterType = "Bank",
                                    Id = bank.bankId,
                                    Text = bank.bankName,
                                    BankId = bank.bankId,
                                    BankName = bank.bankName
                                });
                    return data.ToDataSourceResult(request);
                }
                else if (model.FilterType == "Branch")
                {
                    var data = (from branch in dbContext.BranchMsts
                                where branch.IsActive == true 
                                && branch.bankId == model.BankId 
                                && (model.BranchId == null || branch.branchId == model.BranchId)
                                select new DropdownViewModel
                                {
                                    FilterType = "Branch",
                                    Id = branch.branchId,
                                    Text = branch.branchName,
                                    BankId = branch.bankId,
                                    BranchId = branch.branchId,
                                    BranchName = branch.branchName,
                                    AccountNo = branch.accountNumber,
                                    IFSCCode = branch.IFSCcode
                                });
                    request.Filters.RemoveAt(0);
                    return data.ToDataSourceResult(request);
                }
                else if (model.FilterType == "AccountNo")
                {
                    var data = (from branch in dbContext.BranchMsts
                                where branch.IsActive == true
                                && (model.BankId == null || branch.bankId == model.BankId)
                                && (model.BranchId == null || branch.branchId == model.BranchId)
                                && (model.AccountNo == null || branch.accountNumber == model.AccountNo)
                                select new DropdownViewModel
                                {
                                    FilterType = "AccountNo",
                                    Id = branch.branchId,
                                    Text = branch.accountNumber,
                                    BankId = branch.bankId,
                                    BranchId = branch.branchId,
                                    BranchName = branch.branchName,
                                    AccountNo = branch.accountNumber,
                                    IFSCCode = branch.IFSCcode
                                });
                    if(request.Filters != null) request.Filters.RemoveAt(0);
                    return data.ToDataSourceResult(request);
                }
                else if (model.FilterType == "OnlineBank")
                {
                    var bankList = new List<int?> { 67, 96};
                    var data = (from bank in dbContext.BankMsts
                                where bank.IsActive == true && bankList.Contains(bank.bankId)
                                && (model.BankId == null || bank.bankId == model.BankId)
                                select new DropdownViewModel
                                {
                                    FilterType = "Bank",
                                    Id = bank.bankId,
                                    Text = bank.bankName,
                                    BankId = bank.bankId,
                                    BankName = bank.bankName
                                });
                    return data.ToDataSourceResult(request);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}