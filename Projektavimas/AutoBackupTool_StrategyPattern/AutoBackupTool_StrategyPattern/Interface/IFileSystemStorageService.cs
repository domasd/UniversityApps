using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool_StrategyPattern.Interface
{
    interface IFileSystemStorageService
    {
        void CreateFileInSystemStorage(byte[] input, string name);

        bool ExistsFileInSystemStorage(string name);

        byte[] RemoveFileFromSystemStorage(string name);

        void MoveToDirectory(string name);
    }
}
