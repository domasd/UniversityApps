using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;
using GoogleDriveAPI;

namespace AutoBackupTool_StrategyPattern.Implementations
{
    class GoogleDriveService : ICloudService
    {

        private GoogleDriveAPIViaWindowsApp api;
        private string path = ConfigurationManager.AppSettings["GoogleDrivePath"];

        public GoogleDriveService()
        {
            api = new GoogleDriveAPIViaWindowsApp(path);
        }

        public bool CheckCloudConnection()
        {
            return api.CheckConnection();
        }

        public void UploadFileToCloud(byte[] input, string name)
        {
            api.MoveFileToDrive(input,name);
        }

        public void CleanCloud()
        {
            api.DeleteContents();
        }
    }
}
