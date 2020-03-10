using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NoidaAuthority.PMS.Common;
using NoidaAuthority.PMS.Entity;
using NoidaAuthority.PMS.Service;
using NoidaAuthority.PMS.Web.Models;
using NoidaAuthority.PMS.Repository.Entities;
using System.Data.SqlClient;


namespace NoidaAuthority.PMS.Web.WebAPI
{
    public class PaymentController : ApiController
    {
        // GET: api/Payment
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Payment/5
        public HttpResponseMessage Get(int id)
        {
            Boolean flag = false;
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["connectionStringName"].ConnectionString;
            using (SqlConnection myConnection = new SqlConnection(connection))
            {
                string sqlCommand = string.Format("UPDATE dbo.PRE_PAYMENT_SCHEDULE SET PaymentType = 2, paid_status = 'Y' WHERE Id = {0}", id);
                myConnection.Open();
                SqlCommand command = new SqlCommand(sqlCommand, myConnection);
                command.ExecuteNonQuery();
                myConnection.Close();
                flag = true;               
            }            
            return Request.CreateResponse<Boolean>(HttpStatusCode.OK, flag); 
        }

        // POST: api/Payment
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Payment/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Payment/5
        public void Delete(int id)
        {
        }
    }
}
