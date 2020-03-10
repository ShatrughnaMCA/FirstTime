using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Repository
{
    public interface ICitizenRepository
    {
        CitizenServiceRequest SaveServiceRequest(CitizenServiceRequest ObjServiceRequest);
        List<ServiceRequestDocument> GetCheckListDocumentMentsByServiceId_DepartmentId(int ServiceRequestNo);
        CitizenServiceRequest GetServiceRequestDetails(int ServiceRequestNo);
        bool UpdateRequestStatusByReqId(int ServiceRequestId);
        string GetDetailsByRequestId(int RequestId);
        List<DDList> GetRIDsByDeptt(int deptt);


        CitizenServiceRequest SaveInternalServiceRequest(CitizenServiceRequest serviceRequest);

        CitizenServiceRequest SaveOfflineServiceRequest(CitizenServiceRequest serviceRequest);

        DataSourceResult GetAllServiceRequestId(DataSourceRequest request);

        CitizenServiceRequest GetServiceRequestDetailByRequestId(int? requestId, string mobile);

        bool UpdateCustomerServiceRequest(CitizenServiceRequest model);

        LetterViewModel GetLetterByBarcode(string barcode);
    }
}
