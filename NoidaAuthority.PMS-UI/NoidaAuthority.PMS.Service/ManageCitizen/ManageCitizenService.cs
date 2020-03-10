using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Service
{
    public class ManageCitizenService : IManageCitizenService
    {
        ICitizenRepository citizenRepository = null;

        public ManageCitizenService()
        {
            citizenRepository = new CitizenRepository();
        }
        public CitizenServiceRequest SaveServiceRequest(CitizenServiceRequest ObjServiceRequest)
        {
            return citizenRepository.SaveServiceRequest(ObjServiceRequest);

        }
        public List<ServiceRequestDocument> GetCheckListDocumentMentsByServiceId_DepartmentId(int ServiceRequestNo)
        {
            return citizenRepository.GetCheckListDocumentMentsByServiceId_DepartmentId(ServiceRequestNo);
        }
        public CitizenServiceRequest GetServiceRequestDetails(int ServiceRequestNo)
        {
            return citizenRepository.GetServiceRequestDetails(ServiceRequestNo);
        }

        public List<DDList> GetRIDsByDeptt(int deptt)
        {
            return citizenRepository.GetRIDsByDeptt(deptt);
        }

        public bool UpdateRequestStatusByReqId(int ServiceRequestId)
        {
            return citizenRepository.UpdateRequestStatusByReqId(ServiceRequestId);
        }
        public string GetDetailsByRequestId(int RequestId)
        {
            return citizenRepository.GetDetailsByRequestId(RequestId);
        }

        public CitizenServiceRequest SaveInternalServiceRequest(CitizenServiceRequest serviceRequest)
        {
            return citizenRepository.SaveInternalServiceRequest(serviceRequest);
        }

        public CitizenServiceRequest SaveOfflineServiceRequest(CitizenServiceRequest serviceRequest)
        {
            return citizenRepository.SaveOfflineServiceRequest(serviceRequest);
        }


        public DataSourceResult GetAllServiceRequestId(DataSourceRequest request)
        {
            return citizenRepository.GetAllServiceRequestId(request);
        }

        public CitizenServiceRequest GetServiceRequestDetailByRequestId(int? requestId, string mobile)
        {
            return citizenRepository.GetServiceRequestDetailByRequestId(requestId, mobile);
        }


        public bool UpdateCustomerServiceRequest(CitizenServiceRequest model)
        {
            return citizenRepository.UpdateCustomerServiceRequest(model);
        }


        public LetterViewModel GetLetterByBarcode(string barcode)
        {
            return citizenRepository.GetLetterByBarcode(barcode);
        }
    }
}
