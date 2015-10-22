using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;
using DropboxAPI;


namespace AutoBackupTool_StrategyPattern.Implementations
{
    class DropboxService : ICloudService
    {

        private string DropBoxPath = ConfigurationManager.AppSettings["DropBoxPath"];
        private DropBoxFileService api;

        public DropboxService()
        {
            api = new DropBoxFileService(DropBoxPath);
            api.DeleteContents();
        }
        public bool CheckCloudConnection()
        {
            return api.CheckConnection();
        }

        public void UploadFileToCloud(byte[] input, string name)
        {
            if (api.ExistsCompressedArchive())
            {
                api.AppendCompressedArchive(input, name);
            }
            else
            {
                api.AddCompressedArchivewithFile(input,name);
            }
        }

        public void CleanCloud()
        {
            api.DeleteContents();
        }

    }
}
