using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using File = Google.Apis.Drive.v2.Data.File;

namespace GoogleDriveAPI
{
    public class GoogleDriveFileServiceGoogleSDK
    {
        public static string serviceAccountEmail = "113193872121-uv4rdohrlqcnk1rqj30fjck0r9giho6g@developer.gserviceaccount.com";

        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = System.AppDomain.CurrentDomain.FriendlyName;

        private static string keyFilePath = @"credentials.p12";

        private static X509Certificate2 _certificate;
        private static ServiceAccountCredential _credential;


        private static DriveService service;


        private static string ApplicationFolderId;
        private static string ApplicationfodlerName = "AutoBackupToolFolder";


        public static void InitializeStaticFields()
        {
            _certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            _credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = Scopes
            }.FromCertificate(_certificate));

            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = _credential,
                ApplicationName = ApplicationName,
            });

            CheckConnection();


        }

        public static bool CheckConnection()
        {
            try
            {
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.MaxResults = 1000;
                FileList files = listRequest.Execute();

                ApplicationFolderId = files.Items[0].Id;
                if (string.IsNullOrEmpty(ApplicationFolderId))
                {
                    return false;
                }
                return true;
            }
            catch 
            {
                return false;
            }
        
        }

        public static byte[] RemoveFile(string name)
        {


            return null;
        }

        public static Permission InsertPermission(DriveService service, String fileId, String value,
      String type, String role)
        {
            Permission newPermission = new Permission();
            newPermission.Value = value;
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return service.Permissions.Insert(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }

        public static void UploadFile(string filename, byte[] contents)
        {
                Google.Apis.Drive.v2.Data.File body = new Google.Apis.Drive.v2.Data.File();
                body.Title = filename;
                body.Description = "File uploaded by AutoBackupTool";
                body.MimeType = GetMimeType(filename);
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = ApplicationFolderId } };

              
                System.IO.MemoryStream stream = new System.IO.MemoryStream(contents);
                try
                {
                    FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, GetMimeType(filename));

                    request.Upload();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                }
        }
        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }



    }
}
