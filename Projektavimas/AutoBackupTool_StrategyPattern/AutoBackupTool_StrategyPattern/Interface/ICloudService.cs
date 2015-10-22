using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool_StrategyPattern.Interface
{
    interface ICloudService
    {
        bool CheckCloudConnection();

        void UploadFileToCloud(byte[] input, string name);

        void CleanCloud();
    }
}
