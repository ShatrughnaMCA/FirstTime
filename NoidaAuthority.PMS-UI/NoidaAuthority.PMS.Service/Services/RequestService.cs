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
    public class RequestService : IRequestService
    {
        private IServiceRepository _serviceRepository;

        public RequestService()
        {
            _serviceRepository = new ServiceRepository();
        }

        public DataSourceResult GetServiceRequestDataAsDataSource(DataSourceRequest request, ServiceViewModel model)
        {
            return _serviceRepository.GetServiceRequestDataAsDataSource(request, model);
        }

        public CustomerViewModel GetApplicantDetailsByRegistrationId(int? registrationId)
        {
            return _serviceRepository.GetApplicantDetailsByRegistrationId(registrationId);
        }

        public string GetFileUploadHtmlForService(int? departmentId, int? serviceId)
        {
            return _serviceRepository.GetFileUploadHtmlForService(departmentId, serviceId);
        }

        public ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<System.Web.HttpPostedFileBase> files)
        {
            return _serviceRepository.SaveServiceRequestDetail(model, files);
        }

        public List<DirectorShareholderModel> SaveDirectorOrShareholders(string directorName, decimal? share, string shareType)
        {
            return _serviceRepository.SaveDirectorOrShareholders(directorName, share, shareType);
        }


        public ServiceRequestViewModel GetServiceRequestDetailById(int? id)
        {
            return _serviceRepository.GetServiceRequestDetailById(id);
        }


        public DataSourceResult GetServiceRequestReportAsDataSource(DataSourceRequest request, ServiceViewModel model)
        {
            return _serviceRepository.GetServiceRequestReportAsDataSource(request, model);
        }


        public int GetRaisedServiceRequestStatusById(ServiceViewModel model)
        {
            return _serviceRepository.GetRaisedServiceRequestStatusById(model);
        }


        public int UpdateServiceRequestStatus(ActionViewModel model)
        {
            return _serviceRepository.UpdateServiceRequestStatus(model);
        }
    }
}
