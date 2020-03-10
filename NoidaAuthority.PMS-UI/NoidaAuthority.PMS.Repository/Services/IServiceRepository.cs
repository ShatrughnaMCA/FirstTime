using Kendo.Mvc.UI;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Repository
{
    public interface IServiceRepository
    {
        DataSourceResult GetServiceRequestDataAsDataSource(DataSourceRequest request, ServiceViewModel model);

        CustomerViewModel GetApplicantDetailsByRegistrationId(int? registrationId);

        string GetFileUploadHtmlForService(int? departmentId, int? serviceId);

        ServiceRequestViewModel SaveServiceRequestDetail(ServiceRequestViewModel model, IEnumerable<System.Web.HttpPostedFileBase> files);

        List<DirectorShareholderModel> SaveDirectorOrShareholders(string directorName, decimal? share, string shareType);

        ServiceRequestViewModel GetServiceRequestDetailById(int? id);

        DataSourceResult GetServiceRequestReportAsDataSource(DataSourceRequest request, ServiceViewModel model);

        int GetRaisedServiceRequestStatusById(ServiceViewModel model);

        int UpdateServiceRequestStatus(ActionViewModel model);
    }
}
