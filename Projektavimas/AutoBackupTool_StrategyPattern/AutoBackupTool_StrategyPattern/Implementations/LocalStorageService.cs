using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;

namespace AutoBackupTool_StrategyPattern.Implementations
{
    class LocalStorageService : IFileSystemStorageService
    {

        private string backupPath = ConfigurationManager.AppSettings["FileStoragePath"];

        public void CreateFileInSystemStorage(byte[] input, string name)
        {
            if (Directory.Exists(backupPath))
            {
                System.IO.File.WriteAllBytes(Path.Combine(backupPath, name), input);
            }
        }

        public bool ExistsFileInSystemStorage(string name)
        {
            return System.IO.File.Exists(Path.Combine(backupPath, name));
        }

        public void MoveToDirectory(string name)
        {
            if (!Directory.Exists(Path.Combine(backupPath, name)))
            {
                Directory.CreateDirectory(Path.Combine(backupPath, name));
            }
            backupPath = Path.Combine(backupPath, name);

        }

        public byte[] RemoveFileFromSystemStorage(string name)
        {
            var dir = Path.Combine(backupPath, name);
            if (System.IO.File.Exists(dir))
            {
                var file = System.IO.File.ReadAllBytes(dir);
                System.IO.File.Delete(dir);
                return file;
            }

            return null;
        }
    }
}
