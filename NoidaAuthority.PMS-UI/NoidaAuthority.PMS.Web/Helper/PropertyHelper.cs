using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace NoidaAuthority.PMS.Web.Helper
{
    public static class PropertyHelper
    {
        public static PropertyFilterViewModel FilterProperty(HttpRequestMessage Request)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            PropertyFilterViewModel model = new PropertyFilterViewModel();
            if (nvc["pt"] != null)
                model.PropertyType = nvc["pt"];
            if (nvc["pa"] != null)
                model.AreaId = Convert.ToInt32(nvc["pa"]);
            if (nvc["ps"] != null)
                model.AllotmentStatusId = Convert.ToInt32(nvc["ps"]);
            if (nvc["yr"] != null)
                model.Year = Convert.ToInt32(nvc["yr"]);
            if (nvc["sc"] != null)
                model.Sector = nvc["sc"];
            if (nvc["bk"] != null)
                model.Block = nvc["bk"];
            if (nvc["pn"] != null)
                model.PropertyNo = nvc["pn"];
            if (nvc["an"] != null)
                model.AllotteeName = nvc["an"];
            if (nvc["pln"] != null)
                model.PlotNo = nvc["pln"];
            if (nvc["id"] != null)
            {
                int _RegistrationId = 0;
                if (int.TryParse(nvc["id"], out _RegistrationId))
                    model.RegistrationId = _RegistrationId;
            }
            return model;
        }

        public static DtoPropertyFilter CreatePropertyFilter(HttpRequestMessage Request)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            DtoPropertyFilter PropertyFilterObj = new DtoPropertyFilter();
            if (nvc["pt"] != null)
                PropertyFilterObj.PropertyTypeId = nvc["pt"];
            if (nvc["pa"] != null)
                PropertyFilterObj.AreaId = Convert.ToInt32(nvc["pa"]);
            if (nvc["ps"] != null)
                PropertyFilterObj.AllotmentStatusId = Convert.ToInt32(nvc["ps"]);
            if (nvc["yr"] != null)
                PropertyFilterObj.Year = Convert.ToInt32(nvc["yr"]);
            if (nvc["sc"] != null)
                PropertyFilterObj.Sector = nvc["sc"];
            if (nvc["bk"] != null)
                PropertyFilterObj.Block = nvc["bk"];
            if (nvc["pn"] != null)
                PropertyFilterObj.PropertyNo = nvc["pn"];
            if (nvc["an"] != null)
                PropertyFilterObj.AlloteName = nvc["an"];
            if (nvc["pln"] != null)
                PropertyFilterObj.PlotNumber = nvc["pln"];
            if (nvc["id"] != null)
            {
                int _RegistrationId = 0;
                if (int.TryParse(nvc["id"], out _RegistrationId))
                    PropertyFilterObj.RegistrationId = _RegistrationId;
            }
            return PropertyFilterObj;
        }
    }
}