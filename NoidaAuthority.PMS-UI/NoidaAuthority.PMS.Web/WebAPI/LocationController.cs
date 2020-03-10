using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace NoidaAuthority.PMS.Web.WebAPI
{
    public class LocationController : ApiController
    {
        IPropertyService _propertyService;
        // GET api/location
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
        // GET api/location/5
        public string Get(int id)
        {
            return "value";
        }

       // GET /api/location/SectorLocation/11
         [ActionName("SectorLocation")]
        public DtoSectorLocation GetSectorLocation()
        {
            _propertyService = new PropertyService();
            DtoSectorLocation sectorLocation = new DtoSectorLocation();
            DtoPropertyFilter objPropertyFilter = new DtoPropertyFilter();
            objPropertyFilter.Sector = "11";
            sectorLocation = _propertyService.GetSectorLocation(objPropertyFilter);
          
             return sectorLocation;
        }

         // GET api/location/PropertyLocation/11

          [ActionName("PropertyLocation")]
         public IEnumerable<DtoPropertyLocation> GetPropertyLocation()
         {
             NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
             string id = nvc["id"];   
             _propertyService = new PropertyService();
             DtoPropertyFilter objPropertyFilter = new DtoPropertyFilter();
             if (!string.IsNullOrEmpty(id))
             { objPropertyFilter.PropertyId = Convert.ToInt32(id); }
             
             
             objPropertyFilter.Sector = "11";
             IEnumerable<DtoPropertyLocation> propertyLocation = _propertyService.GetPropertyLocations(objPropertyFilter);          

             return propertyLocation;
         }
        // POST api/location
        public void Post([FromBody]string value)
        {
        }

        // PUT api/location/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/location/5
        public void Delete(int id)
        {
        }
    }
}
