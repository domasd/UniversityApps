using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool_StrategyPattern.Entities
{
    abstract class Backuper
    {


        private List<File> files = new List<File>();

        protected abstract bool CheckCloudConnection();

        protected abstract void UploadFileToCloud(byte[] input, string name);

        protected abstract void CleanCloud();


        protected abstract void CreateFileInSystemStorage(byte[] input, string name);

        protected abstract bool ExistsFileInSystemStorage(string name);

        protected abstract byte[] RemoveFileFromSystemStorage(string name);

        protected abstract void MoveToDirectory(string name);



        public Backuper(string pathToBackup)
        {
            DirectoryInfo dir = new DirectoryInfo(pathToBackup);

            if (dir.Exists)
            {
                foreach (var file in dir.EnumerateFiles())
                {
                    files.Add(new File()
                    {
                        Bytes = System.IO.File.ReadAllBytes(file.FullName),
                        Name = file.Name
                    });
                }
            }
            else
            {
                throw new ArgumentException("Directory to backup does not exist");
            }

            MoveToDirectory(DateTime.Now.ToString("yyyy-MM-dd"));

        }

        public void BackupToCloud()
        {
            if (!CheckCloudConnection()) throw new Exception("Cant access cloud");

            CleanCloud();

            foreach (var file in files)
            {

                UploadFileToCloud(file.Bytes, file.Name);
            }
        }

        public void BackupToFileStorage()
        {

            foreach (var file in files)
            {
                if (ExistsFileInSystemStorage(file.Name))
                {
                    RemoveFileFromSystemStorage(file.Name);
                }
                CreateFileInSystemStorage(file.Bytes, file.Name);
            }

        }
    }
}
