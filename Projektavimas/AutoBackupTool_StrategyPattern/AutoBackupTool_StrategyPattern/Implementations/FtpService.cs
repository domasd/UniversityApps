﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;

namespace AutoBackupTool_StrategyPattern.Implementations
{
    class FtpService : IFileSystemStorageService
    {

        private string _ftpUrl = ConfigurationManager.AppSettings["FtpUrl"];
        private string _ftpPsw = ConfigurationManager.AppSettings["FtpPsw"];
        private string _ftpLogin = ConfigurationManager.AppSettings["FtpLogin"];
        private NetworkCredential _credential;


        public FtpService()
        {
            GetCredentials();
        }

        public void CreateFileInSystemStorage(byte[] input, string name)
        {
            var request = (FtpWebRequest)WebRequest.Create(Path.Combine(_ftpUrl, name));
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = _credential;



            request.ContentLength = input.Length;
            var requestStream = request.GetRequestStream();
            try
            {
                requestStream.Write(input, 0, input.Length);
            }
            finally
            {
                requestStream.Close();
            }

        }

        public bool ExistsFileInSystemStorage(string name)
        {
            var request = (FtpWebRequest) WebRequest.Create(Path.Combine(_ftpUrl,name));
            request.Credentials = _credential;
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }
            return true;
        }

        public byte[] RemoveFileFromSystemStorage(string name)
        {
            byte[] bytes;
            //Download
            FtpWebRequest requestDownload = (FtpWebRequest)WebRequest.Create(Path.Combine(_ftpUrl,name));
            requestDownload.Method = WebRequestMethods.Ftp.DownloadFile;

            requestDownload.Credentials = _credential;

            using(FtpWebResponse responseDownload = (FtpWebResponse)requestDownload.GetResponse())
            using (Stream responseStream = responseDownload.GetResponseStream())
            using(BinaryReader reader = new BinaryReader(responseStream))
            {
                int n;
                var bytesArrayList = new ArrayList();
                do
                {
                    try
                    {
                        var bytesTemp = reader.ReadBytes(100);
                        n = bytesTemp.Length;
                        bytesArrayList.AddRange(bytesTemp);
                    }
                    catch
                    {
                        break;
                    }
                } while (n > 0);

                bytes = (byte[])bytesArrayList.ToArray(typeof(byte));

            }


            //Delete
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Path.Combine(_ftpUrl, name));

            request.Credentials = _credential;

            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();

            return bytes;
        }

        public void MoveToDirectory(string name)
        {
            if (_credential == null)
            {
                GetCredentials();
            }

            var dir = Path.Combine(_ftpUrl, name);
            WebRequest request = WebRequest.Create(dir);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = _credential;
            try
            {
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    // response not used
                }

            }
            catch (WebException e)
            {
                String status = ((FtpWebResponse)e.Response).StatusDescription;
                if (status != "550 Can't create directory: File exists\r\n")
                {
                    throw new InvalidOperationException("No compressed file");
                }
            }

            _ftpUrl = dir;

        }

        private void GetCredentials()
        {
            _credential = new NetworkCredential(_ftpLogin, _ftpPsw);
        }
    }
}
