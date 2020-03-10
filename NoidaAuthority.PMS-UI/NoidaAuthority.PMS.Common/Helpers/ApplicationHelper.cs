using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Common
{
    public static class ApplicationHelper
    {
        private static string SMS_BASEURL = ConfigurationManager.AppSettings["SMSsend"].ToString();
        private static string SMS_USERNAME = ConfigurationManager.AppSettings["SMSUsername"].ToString();
        private static string SMS_PASSWORD = ConfigurationManager.AppSettings["SMSPassword"].ToString();
        private static string SMS_SENDERID = ConfigurationManager.AppSettings["SMSSenderID"].ToString();
        private static string SMS_URL = SMS_BASEURL + "username=" + SMS_USERNAME + "&password=" + SMS_PASSWORD + "&sendername=" + SMS_SENDERID;

        public static int GenerateOTP()
        {
            Random random = new Random();
            int maxValue = 999999;
            int otp = random.Next(maxValue);
            return otp;
        }

        public static string CreatePassword()
        {
            const string PasswordCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            int PasswordLength = 8;
            StringBuilder password = new StringBuilder();
            Random random = new Random();
            while (0 < PasswordLength--)
            {
                password.Append(PasswordCharacters[random.Next(PasswordCharacters.Length)]);
            }
            return password.ToString();
        }

        public static int SendSMS(string mobileNo, string msg)
        {
            WebClient client = new WebClient();
            //string baseurl = ConfigurationManager.AppSettings["SMSsend"].ToString() + ConfigurationManager.AppSettings["SMSUsername"].ToString() + "&password=" + ConfigurationManager.AppSettings["SMSPassword"].ToString() + "&sendername=" + ConfigurationManager.AppSettings["SMSSenderID"].ToString() + "&mobileno=" + mobileNo + "&message=" + msg;
            string baseurl = SMS_URL + "&mobileno=" + mobileNo + "&message=" + msg;
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            return ReturnType.Success;
        }

        public static void SendEmail(string toAddress, string subject, string body)
        {
            Task.Factory.StartNew(() => Send(toAddress, subject, body, false), TaskCreationOptions.LongRunning);
        }

        private static void Send(string toAddress, string subject, string body, bool priority)
        {
            MailAddress fromAddress = new MailAddress(ConfigurationManager.AppSettings["SmtpFromAddress"]);
            MailAddress toAddressNew = new MailAddress(toAddress);
            string serverName = ConfigurationManager.AppSettings["SmtpServerName"];
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            string userName = ConfigurationManager.AppSettings["SmtpUserName"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];

            var message = new MailMessage(fromAddress, toAddressNew);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.HeadersEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.BodyEncoding = Encoding.UTF8;
            if (priority) message.Priority = MailPriority.High;

            //SmtpClient client = new SmtpClient(serverName, port);
            SmtpClient client = new SmtpClient(serverName);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpSsl"]);
            client.UseDefaultCredentials = false;

            NetworkCredential smtpUserInfo = new NetworkCredential(userName, password);
            client.Credentials = smtpUserInfo;
            ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
            client.Send(message);

            client.Dispose();
            message.Dispose();
        }


        public static string GenerateTransactionId()
        {
            Random rnd = new Random();
            string strHash = GenerateSHA512HashCode(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);
            return txnid1;
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

    }
}
