using NoidaAuthority.PMS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NoidaAuthority.PMS.Entity;

namespace NoidaAuthority.PMS.Common.Helpers
{
    public class FtpHandler
    {
        //static string ftpPath = "ftp://52.172.186.197/UploadDocuments/";
        static string ftpPath = ConfigurationManager.AppSettings["FTPPath"];
        //static string ftpPath = ConfigurationManager.AppSettings["FTPSiteRootPath"];
        static string username = ConfigurationManager.AppSettings["FTPUsername"];
        static string password = ConfigurationManager.AppSettings["FTPPassword"];

        //For Property controller
        public string GetDirectoryPath(int? DepartmentId, string Id, string file, bool IsFtp)
        {
            string PathName = string.Empty;
            string RootPath = string.Empty;
            if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Institutional)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathIns"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Commercial)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathCom"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Residential)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathRes"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Industrial)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathInd"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Housing)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathHos"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.GroupHousing)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathGh"];
            }
            else if (DepartmentId == NoidaAuthority.PMS.Common.Contants.Department.Village)
            {
                RootPath = System.Configuration.ConfigurationManager.AppSettings["SiteRootPathVill"];
            }

            if (IsFtp)
            {
                if (RootPath == " ")
                { PathName = System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + Id; }
                else { PathName = System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + RootPath + "\\" + Id; }
            }
            else
            {
                if (RootPath == " ")
                { PathName = "http://" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + Id + "//" + file; }
                else { PathName = "http://" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + RootPath + "//" + Id + "//" + file; }
            }

            return PathName;
        }


        //For Customer Controller
        public string GetDocumentPath(int rid, int id, string file, bool IsFtp)
        {
            string Path = string.Empty;
            if (IsFtp) { Path = System.Configuration.ConfigurationManager.AppSettings["FTPRootPath"] + "//" + rid + "//" + id; }
            else { Path = "http://" + System.Configuration.ConfigurationManager.AppSettings["FTPSiteRootPath"] + "//" + rid + "//" + id + "//" + file; }
            return Path;
        }

        /// <summary>
        /// Used for searching Files in FTP Server.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<string> DirSearch(string path)
        {
            List<String> files = new List<String>();
            try
            {
                var request = CreateRequest(path, WebRequestMethods.Ftp.ListDirectory);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream, true))
                        {
                            while (!reader.EndOfStream)
                            {
                                files.Add(reader.ReadLine());
                            }
                        }
                    }
                }
            }
            catch (System.Exception excpt)
            {
            }

            return files;
        }

        public FtpWebRequest CreateRequest(string uri, string method)
        {
            var r = (FtpWebRequest)WebRequest.Create(uri);

            r.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FTPUsername"], System.Configuration.ConfigurationManager.AppSettings["FTPPassword"]);
            r.Method = method;
            return r;
        }

        public void RenameFiles(string currentFileName, string newFileName, string path)
        {
            try
            {
                Stream ftpStream = null;
                var r = (FtpWebRequest)WebRequest.Create(path + "/" + currentFileName);
                r.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["FTPUsername"], System.Configuration.ConfigurationManager.AppSettings["FTPPassword"]);
                r.Method = WebRequestMethods.Ftp.Rename;
                r.RenameTo = newFileName;
                FtpWebResponse response = (FtpWebResponse)r.GetResponse();
                ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
            }
            catch (WebException excpt)
            {
                String status = ((FtpWebResponse)excpt.Response).StatusDescription;
            }
        }

        public static bool UploadFiles(IEnumerable<HttpPostedFileBase> files, string registrationId)
        {
            var flag = false;
            var countIndex = 0;
            if (files != null && files.ToList().Count > 0)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }
                        var name = file.FileName;
                        string filename = StringExtensions.RemoveWhitespace(name);
                        UploadBinaryFiles(registrationId, filename, fileData);
                        countIndex++;
                    }
                }
            }
            return flag;
        }

        //public static string RemoveWhitespace(this string str)
        //{
        //    return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        //}

        public static bool UploadFiles(IEnumerable<HttpPostedFileBase> files, string registrationId, int requestId)
        {
            var flag = false;
            var countIndex = 0;
            if (files != null && files.ToList().Count > 0)
            {
                foreach (var file in files)
                {
                    if (file != null)
                    {
                        byte[] fileData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            fileData = binaryReader.ReadBytes(file.ContentLength);
                        }

                        var name = file.FileName;
                        string filename = StringExtensions.RemoveWhitespace(name);
                        UploadBinaryFiles(registrationId, requestId.ToString(), filename, fileData);

                        countIndex++;
                    }
                }
            }
            return flag;
        }

        private static void UploadBinaryFiles(string RegistrationId, string filename, byte[] fileData)
        {
            bool IsDirectory = false;
            if (IsDirectoryExists(ftpPath, RegistrationId))
            {
                IsDirectory = true;
            }
            else
            {
                IsDirectory = CreateDirectory(ftpPath, RegistrationId);
            }

            if (IsDirectory == true)
            {
                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + "/" + RegistrationId + "/" + filename));
                ftpClient.Credentials = new System.Net.NetworkCredential(username.Normalize(), password.Normalize());
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;

                ftpClient.ContentLength = fileData.Length;
                byte[] buffer = new byte[4097];
                int bytes = fileData.Length;
                int total_bytes = (int)fileData.Length;
                try
                {
                    System.IO.Stream rs = ftpClient.GetRequestStream();
                    while (total_bytes > 0)
                    {
                        rs.Write(fileData, 0, bytes);
                        total_bytes = total_bytes - bytes;
                    }
                    rs.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                var value = uploadResponse.StatusDescription;
                uploadResponse.Close();
            }
        }

        private static void UploadBinaryFiles(string RegistrationId, string RequestId, string filename, byte[] fileData)
        {
            bool IsDirectory = false;
            bool IsServiceDirectory = false;
            if (IsDirectoryExists(ftpPath, RegistrationId))
            {
                IsDirectory = true;
                if (IsDirectoryExists(ftpPath + "/" + RegistrationId, RequestId))
                {
                    IsServiceDirectory = true;
                }
                else
                {
                    IsServiceDirectory = CreateDirectory(ftpPath + "/" + RegistrationId, RequestId);
                }
            }
            else
            {
                IsDirectory = CreateDirectory(ftpPath, RegistrationId);
                IsServiceDirectory = CreateDirectory(ftpPath + "/" + RegistrationId, RequestId);
            }

            if (IsDirectory == true && IsServiceDirectory == true)
            {
                //FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + "/" + Convert.ToString(RegistrationId) + "/" + Convert.ToString(RequestId) + "/" + filename));
                FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath + "/" + RegistrationId + "/" + RequestId + "/" + filename));
                ftpClient.Credentials = new System.Net.NetworkCredential(username.Normalize(), password.Normalize());
                ftpClient.Method = System.Net.WebRequestMethods.Ftp.UploadFile;

                ftpClient.ContentLength = fileData.Length;
                byte[] buffer = new byte[4097];
                int bytes = fileData.Length;
                int total_bytes = (int)fileData.Length;
                try
                {
                    System.IO.Stream rs = ftpClient.GetRequestStream();
                    //System.IO.FileStream fs = fileData.op
                    while (total_bytes > 0)
                    {
                        rs.Write(fileData, 0, bytes);
                        total_bytes = total_bytes - bytes;
                    }
                    rs.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
                FtpWebResponse uploadResponse = (FtpWebResponse)ftpClient.GetResponse();
                var value = uploadResponse.StatusDescription;
                uploadResponse.Close();
            }
        }

        public static bool IsDirectoryExists(string path, string directory)
        {
            bool directoryExists;

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + "/" + directory));
            ftpClient.Credentials = new System.Net.NetworkCredential(username.Normalize(), password.Normalize());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.ListDirectory;
            try
            {
                using (ftpClient.GetResponse())
                {
                    directoryExists = true;
                }
            }
            catch (WebException)
            {
                directoryExists = false;
            }
            return directoryExists;
        }

        public static bool CreateDirectory(string path, string directory)
        {
            bool IsDirectoryCreated = false;

            FtpWebRequest ftpClient = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + "/" + directory));
            ftpClient.Credentials = new System.Net.NetworkCredential(username.Normalize(), password.Normalize());
            ftpClient.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                using (ftpClient.GetResponse())
                {
                    IsDirectoryCreated = true;
                }
            }
            catch (WebException)
            {
                IsDirectoryCreated = false;
            }
            return IsDirectoryCreated;
        }

        public string GetDocumentPath(int? rid, string file, bool IsFtp)
        {
            string Path = string.Empty;
            if (IsFtp)
            {
                Path = System.Configuration.ConfigurationManager.AppSettings["RootPath"] + "\\" + rid;
            }
            else { Path = "http://" + System.Configuration.ConfigurationManager.AppSettings["SiteRootPath"] + "//" + rid + "//" + file; }
            return Path;
        }

        public List<DocumentViewModel> GetServiceRequestUploadedDocumentsById(int registrationId, int RequestId)
        {
            string docpath = ConfigurationManager.AppSettings["FTPPath"];
            List<DocumentViewModel> documentList = new List<DocumentViewModel>();
            docpath = docpath + "/" + registrationId;
            if (IsDirectoryExists(docpath, Convert.ToString(RequestId)))
            {
                var filepath = docpath + "/" + RequestId + "/";
                List<string> fileList = DirSearch(filepath);
                //string filePath = GetDocumentPathForServiceRequest(RequestId, false);
                string filePath = "http://" + System.Configuration.ConfigurationManager.AppSettings["FTPSiteRootPath"] + "//" + registrationId + "//" + RequestId + "//";
                if (fileList != null)
                {
                    for (int x = 0; x < fileList.Count(); x++)
                    {
                        DocumentViewModel document = new DocumentViewModel();
                        document.Id = x + 1;
                        document.DocumentName = fileList.ElementAt(x).ToString();
                        document.DocumentPath = filePath + document.DocumentName;
                        documentList.Add(document);
                    }
                }
            }
            return documentList;
        }

        public string GetUploadedDocumentByServiceId(int registrationId, int RequestId)
        {
            string docpath = ConfigurationManager.AppSettings["FTPPath"];
            docpath = docpath + "/" + registrationId;
            string completeFilePath = string.Empty;
            if (IsDirectoryExists(docpath, Convert.ToString(RequestId)+Contants.ReqComplete))
            {
                var filepath = docpath + "/" + RequestId + Contants.ReqComplete + "/";
                List<string> fileList = DirSearch(filepath);
                string filePath = "http://" + System.Configuration.ConfigurationManager.AppSettings["FTPSiteRootPath"] + "//" + registrationId + "//" + RequestId +Contants.ReqComplete + "//";
                if (fileList.Count > 0)
                {
                    string documentName = fileList.ElementAt(0).ToString();
                    completeFilePath = filePath + documentName;
                } 
            }
            return completeFilePath;
        }

        public string GetObjectionDocumentByServiceId(int registrationId, int RequestId)
        {
            string docpath = ConfigurationManager.AppSettings["FTPPath"];
            docpath = docpath + "/" + registrationId;
            string completeFilePath = string.Empty;
            if (IsDirectoryExists(docpath, Convert.ToString(RequestId) + Contants.ReqObjection))
            {
                var filepath = docpath + "/" + RequestId + Contants.ReqObjection + "/";
                List<string> fileList = DirSearch(filepath);
                string filePath = "http://" + System.Configuration.ConfigurationManager.AppSettings["FTPSiteRootPath"] + "//" + registrationId + "//" + RequestId + Contants.ReqObjection + "//";
                if (fileList.Count > 0)
                {
                    string documentName = fileList.ElementAt(0).ToString();
                    completeFilePath = filePath + documentName;
                }
            }
            return completeFilePath;
        }
    }
}
