using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;

namespace AutoBackupTool_StrategyPattern.Entities
{
    class Backuper
    {
        public ICloudService CloudService { get; set; }
        public IFileSystemStorageService FileSystemService { get; set; }

        private List<File> files = new List<File>();


        public Backuper(string pathToBackup, ICloudService cloudService, IFileSystemStorageService fileSystemStorageService)
        {
            CloudService = cloudService;
            FileSystemService = fileSystemStorageService;

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

            FileSystemService.MoveToDirectory(DateTime.Now.ToString("yyyy-MM-dd"));

        }

        public void BackupToCloud()
        {
            if (!CloudService.CheckCloudConnection()) throw new Exception("Cant access cloud");

            CloudService.CleanCloud();

            foreach (var file in files)
            {

                CloudService.UploadFileToCloud(file.Bytes, file.Name);
            }
        }

        public void BackupToFileStorage()
        {

            foreach (var file in files)
            {
                if (FileSystemService.ExistsFileInSystemStorage(file.Name))
                {
                    FileSystemService.RemoveFileFromSystemStorage(file.Name);
                }
                FileSystemService.CreateFileInSystemStorage(file.Bytes, file.Name);
            }

        }
    }
}
