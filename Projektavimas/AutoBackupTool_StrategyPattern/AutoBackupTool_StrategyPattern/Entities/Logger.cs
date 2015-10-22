using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Interface;

namespace AutoBackupTool_StrategyPattern.Entities
{
    class Logger
    {

        private string logFolderName = "Log";
        public IFileSystemStorageService FileSystemService { get; set; }

        public string LogFileName { get; set; }

        public Logger(string logFileName, IFileSystemStorageService fileSystemStorageService)
        {
            this.FileSystemService = fileSystemStorageService;
            this.LogFileName = logFileName;
            FileSystemService.MoveToDirectory(logFolderName);
            LogInFileStorage("----------------App started---------------------");

        }

        public void LogInFileStorage(string message)
        {
            if (FileSystemService.ExistsFileInSystemStorage(LogFileName))
            {
                byte[] logFileToAppend = FileSystemService.RemoveFileFromSystemStorage(LogFileName);
                System.IO.File.WriteAllBytes(LogFileName, logFileToAppend);
            }

            System.IO.File.AppendAllText(LogFileName, message + Environment.NewLine);
            var bytes = System.IO.File.ReadAllBytes(LogFileName);

            FileSystemService.CreateFileInSystemStorage(bytes, LogFileName);

        }

    }
}
