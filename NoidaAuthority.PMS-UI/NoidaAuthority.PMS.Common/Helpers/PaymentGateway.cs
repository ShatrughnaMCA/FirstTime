using com.fss.plugin.bob;
using NoidaAuthority.PMS.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace NoidaAuthority.PMS.Common
{
    public class PaymentGateway
    {
        static string PAY_BASE_URL = ConfigurationManager.AppSettings["PAYU_BASE_URL"];
        static string PAYU_URL = ConfigurationManager.AppSettings["PAYU_PAYMENT_URL"];
        static string KEY = ConfigurationManager.AppSettings["PAYU_KEY"];
        static string SALT = ConfigurationManager.AppSettings["PAYU_SALT"];
        static string surl = ConfigurationManager.AppSettings["PAYU_RETURN_URL"];
        static string furl = ConfigurationManager.AppSettings["PAYU_RETURN_URL"];

        static string return_url = ConfigurationManager.AppSettings["PAYU_RETURN_URL"];
        
        static string success_url = ConfigurationManager.AppSettings["PAYU_RETURN_URL"];
        static string failed_url = ConfigurationManager.AppSettings["PAYU_RETURN_URL"];
        static string HashSequence = ConfigurationManager.AppSettings["hashSequence"];
        static string HDFC_URL = ConfigurationManager.AppSettings["HDFC_URL"];
        static string HDFC_RETURN_URL = ConfigurationManager.AppSettings["HDFC_RETURN_URL"];
        static string HDFC_SALT = ConfigurationManager.AppSettings["HDFC_SALT"];
        static string HDFC_MERCHANT_KEY = ConfigurationManager.AppSettings["HDFC_MERCHANT_KEY"];

        //<add key="hashSequence" value="key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10"/>

        private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();

        public string Url = "";
        public string Method = "post";
        public string FormName = "form1";

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");
            System.Web.HttpContext.Current.Response.End();
        }

        // use for multiple banks
        public void Post(string formName, string method, string url)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", formName));
            System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", formName, method, url));
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
            }
            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");
            System.Web.HttpContext.Current.Response.End();
        }

        private string PreparePOSTForm()
        {
            string responseForm = string.Empty;
            responseForm = "<html><head>";
            responseForm = responseForm + string.Format("</head><body onload=\"document.{0}.submit()\">", FormName);
            responseForm = responseForm + string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url);
            for (int i = 0; i < Inputs.Keys.Count; i++)
            {
                responseForm = responseForm + string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]);
            }
            responseForm = responseForm + "</form></body></html>";
            return responseForm;
        }

        public void PayOnline(OnlinePaymentViewModel model)
        {
            Url = PAYU_URL;
            string txnid = GenerateTransactionId();

            Add("key", KEY);
            Add("txnid", txnid);
            Add("surl", success_url);
            Add("furl", failed_url);
            Add("udf1", model.TransactionId);
            Add("amount", model.Amount.ToString());
            Add("productinfo", model.ProductInfo);
            Add("firstname", model.FirstName);
            Add("phone", model.MobileNo);
            Add("email", model.Email);
            string hashString = KEY + "|" + txnid + "|" + model.Amount + "|" + model.ProductInfo + "|" + model.FirstName + "|" + model.Email + "|" + model.TransactionId + "||||||||||" + SALT;
            string hash = GenerateSHA512HashCode(hashString);
            Add("hash", hash);
            Post();
        }


        public void PayOnlineHDFC(OnlinePaymentViewModel model)
        {
            Url = HDFC_URL;
            //string txnid = GenerateTransactionId();
            //Add("txnid", txnid);
            Add("txnid", model.TransactionKey);
            Add("key", HDFC_MERCHANT_KEY);
            Add("surl", HDFC_RETURN_URL);
            Add("furl", HDFC_RETURN_URL);
            Add("udf1", model.TransactionId);
            Add("udf2", model.RegistrationId.ToString());
            Add("udf3", model.OnlineRequestId.ToString());
            Add("udf4", model.ChallanId.ToString());
            Add("amount", model.Amount.ToString());
            Add("productinfo", model.ProductInfo);
            Add("firstname", model.Applicant);
            Add("phone", model.MobileNo);
            Add("email", model.Email);
            Add("address1", model.AddressI);
            Add("address2", model.AddressII);
            Add("city", model.City);
            Add("state", model.State);
            Add("country", model.Country);
            Add("zipcode", model.Zipcode);
            string hashString = HDFC_MERCHANT_KEY + "|" + model.TransactionKey + "|" + model.Amount + "|" + model.ProductInfo + "|" + model.Applicant + "|" + model.Email + "|" + model.TransactionId + "|" + model.RegistrationId.ToString() + "|" + model.OnlineRequestId.ToString() + "|" + model.ChallanId.ToString() + "|||||||" + HDFC_SALT;
            string hash = GenerateSHA512HashCode(hashString);
            Add("hash", hash);
            string responseForm = PreparePOSTForm();
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(responseForm);
            System.Web.HttpContext.Current.Response.End();
            //Post();
        }

        public void PayOnlineHDFCII(OnlinePaymentViewModel model)
        {
            Url = HDFC_URL;
            System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in hash table for data post
            //string txnid = GenerateTransactionId();
            //data.Add("txnid", txnid);
            data.Add("txnid", model.TransactionKey);
            data.Add("key", HDFC_MERCHANT_KEY);
            data.Add("amount", model.Amount.ToString());
            data.Add("firstname", model.Applicant);
            data.Add("email", model.Email);
            data.Add("phone", model.MobileNo);
            data.Add("productinfo", model.ProductInfo);
            data.Add("surl", success_url);
            data.Add("furl", failed_url);
            
            data.Add("address1", model.AddressI);
            data.Add("address2", model.AddressII);
            data.Add("city", model.City);
            data.Add("state", model.State);
            data.Add("country", model.Country);
            data.Add("zipcode", model.Zipcode);
            
            data.Add("udf1", model.TransactionId);
            data.Add("udf2", model.RegistrationId.ToString());
            data.Add("udf3", model.OnlineRequestId.ToString());
            data.Add("udf4", model.Udf4);
            data.Add("udf5", model.Udf5);
            
            //string hashString = HDFC_MERCHANT_KEY + "|" + txnid + "|" + model.Amount + "|" + model.ProductInfo + "|" + model.FirstName + "|" + model.Email + "|" + model.TransactionId + "||||||||||" + HDFC_SALT;
            string hashString = HDFC_MERCHANT_KEY + "|" + model.TransactionKey + "|" + model.Amount + "|" + model.ProductInfo + "|" + model.Applicant + "|" + model.Email + "|" + model.TransactionId + "|" + model.RegistrationId.ToString() + "|" + model.OnlineRequestId.ToString() + "|" + model.ChallanId.ToString() + "|||||||" + HDFC_SALT;
            string hash = GenerateSHA512HashCode(hashString);
            data.Add("hash", hash);
            string strForm = PreparePOSTForm(Url, data);
            //Page.Controls.Add(new LiteralControl(strForm));
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(strForm);
            System.Web.HttpContext.Current.Response.End();
            //Post();
        }

        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + key.Key + "\" value=\"" + key.Value + "\">");
            }

            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        public void PayOnline(string transactionId, string firstName, string amount, string productInfo, string email, string mobile)
        {
            Url = PAYU_URL;
            string txnid = GenerateTransactionId();
            Add("key", KEY);
            Add("txnid", txnid);
            Add("amount", amount);
            Add("productinfo", productInfo);
            Add("firstname", firstName);
            Add("phone", mobile);
            Add("email", email);
            Add("surl", success_url);
            Add("furl", failed_url);
            Add("udf1", transactionId);
            string hashString = KEY + "|" + txnid + "|" + amount + "|" + productInfo + "|" + firstName + "|" + email + "|" + transactionId + "||||||||||" + SALT;
            string hash = GenerateSHA512HashCode(hashString);
            Add("hash", hash);
            Post();
        }

        public static string GenerateSHA512HashCode(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public static string GenerateTransactionId()
        {
            Random rnd = new Random();
            string strHash = GenerateSHA512HashCode(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);
            return txnid1;
        }

        public void PayBankOfBaroda(OnlinePaymentViewModel model)
        {
            iPayPipe pipe = new iPayPipe();
            //Initialization
            string resourcePath = ConfigurationManager.AppSettings["BOB_RESOURCE_PATH"];
            string recieptURL = ConfigurationManager.AppSettings["BOB_RETURN_URL"];
            string errorURL = ConfigurationManager.AppSettings["BOB_RETURN_URL"];
            string aliasName = ConfigurationManager.AppSettings["BOB_ALIAS_NAME"];
            string paymentURL = ConfigurationManager.AppSettings["CHLN_PAYMENT_URL"];
            
            String action = "1";
            String currency = "356"; //(ex: “356”)
            String language = "USA"; //(ex: “USA”)
            String transId = model.TransactionId;
            String trackId = model.TransactionKey;
            
            pipe.setResourcePath(resourcePath);
            pipe.setKeystorePath(resourcePath);
            pipe.setAlias(aliasName);
            pipe.setAction( action );
            pipe.setCurrency(currency);
            pipe.setLanguage(language);
            pipe.setResponseURL(recieptURL);
            pipe.setErrorURL(errorURL);
            pipe.setTransId(transId);
            pipe.setTrackId(trackId);            
            //pipe.setAmt("1");
            pipe.setAmt(model.Amount.ToString());
            //pipe.setUdf1() - pipe.setUdf5() reserved for BOB gateway
            pipe.setUdf6("Noida Authority");
            pipe.setUdf7(model.Applicant);
            pipe.setUdf8(model.Email);
            pipe.setUdf9(model.MobileNo);
            pipe.setUdf10(model.AddressI);
            pipe.setUdf11(model.Amount.ToString());
            pipe.setUdf12(model.TransactionId);
            pipe.setUdf13(model.City);
            pipe.setUdf14(model.State);
            pipe.setUdf15(model.Country);
            pipe.setUdf16(model.Zipcode);
            pipe.setUdf17(model.BankId.ToString());
            pipe.setUdf18(model.BankName);
            pipe.setUdf19(model.ChallanId.ToString());
            pipe.setUdf20(model.RegistrationId.ToString());
            pipe.setUdf21(model.TransactionId);
            pipe.setUdf22(model.OnlineRequestId.ToString());
            
            pipe.performPaymentInitializationHTTP();
            var address = pipe.getWebAddress();
            if (address != null)
            {
                HttpContext.Current.Response.Redirect(pipe.getWebAddress());
            }
            else
            {
                //HttpContext.Current.Response.Redirect("http://localhost:53885/Customer/Payment/Index?rid=" + model.RegistrationId + "&id=" + model.OnlineRequestId + "&actionflag=ServiceCharge");
                HttpContext.Current.Response.Redirect(paymentURL + "?rid=" + model.RegistrationId + "&id=" + model.OnlineRequestId + "&actionflag=ServiceCharge");
            }
        }

    }
}
