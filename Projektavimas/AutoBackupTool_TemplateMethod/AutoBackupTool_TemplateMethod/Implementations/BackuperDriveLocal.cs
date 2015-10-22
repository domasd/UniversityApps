﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Entities;
using GoogleDriveAPI;

namespace AutoBackupTool_StrategyPattern.Implementations
{
    class BackuperDriveLocal : Backuper
    {
        private GoogleDriveAPIViaWindowsApp api;
        private string path = ConfigurationManager.AppSettings["GoogleDrivePath"];

        private string backupPath = ConfigurationManager.AppSettings["FileStoragePath"];

        public BackuperDriveLocal(string pathToBackup) : base(pathToBackup)
        {
            api = new GoogleDriveAPIViaWindowsApp(path);
        }

        protected override bool CheckCloudConnection()
        {
            return api.CheckConnection();
        }

        protected override void UploadFileToCloud(byte[] input, string name)
        {
            api.MoveFileToDrive(input, name);
        }

        protected override void CleanCloud()
        {
            api.DeleteContents();
        }

        protected override void CreateFileInSystemStorage(byte[] input, string name)
        {
            if (Directory.Exists(backupPath))
            {
                System.IO.File.WriteAllBytes(Path.Combine(backupPath, name), input);
            }
        }

        protected override bool ExistsFileInSystemStorage(string name)
        {
            return System.IO.File.Exists(Path.Combine(backupPath, name));
        }

        protected override byte[] RemoveFileFromSystemStorage(string name)
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

        protected override void MoveToDirectory(string name)
        {

            if (!Directory.Exists(Path.Combine(backupPath, name)))
            {
                Directory.CreateDirectory(Path.Combine(backupPath, name));
            }
            backupPath = Path.Combine(backupPath, name);
        }
    }
}
