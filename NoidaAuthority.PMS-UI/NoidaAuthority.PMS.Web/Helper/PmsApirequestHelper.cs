using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using NoidaAuthority.PMS.Common.Logger;

namespace NoidaAuthority.PMS.Web.Helper
{
    public class PmsApirequestHelper
    {

        public string PostRequestJson(string endpoint, string json)
        {
            // Create string to hold JSON response
            string jsonResponse = string.Empty;

            using (var client = new WebClient())
            {
                try
                {
                    client.UseDefaultCredentials = true;
                    client.Headers.Add("Content-Type:application/json");
                    client.Headers.Add("Accept:application/json");
                    client.Headers.Add("UserName","citizen");
                    client.Headers.Add("Password", "test#@!123");
                    var uri = new Uri(endpoint);
                    var response = client.UploadString(uri, "POST", json);
                    jsonResponse = response;
                }
                catch (WebException ex)
                {
                    var logger = LoggingManager.GetLogInstance();
                    logger.LogError("API Exception", ex);
                }
            }

            return jsonResponse;
        }

        public string GetRequest(string endpoint, int param)
        {
            string jsonResponse = string.Empty;

            using (var client = new WebClient())
            {
                try
                {
                    client.BaseAddress = endpoint;
                    // HTTP GET
                    client.UseDefaultCredentials = true;
                    jsonResponse = client.DownloadString(endpoint);
                }
                catch (WebException ex)
                {
                    var logger = LoggingManager.GetLogInstance();
                    logger.LogError("API Exception", ex);
                }
            }

            return jsonResponse;
        }
    }
}