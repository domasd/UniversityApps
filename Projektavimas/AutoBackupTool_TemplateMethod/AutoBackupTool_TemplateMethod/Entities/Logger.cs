using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBackupTool_StrategyPattern.Entities
{
    abstract class Logger
    {
        private string logFolderName = "Log";
        public string LogFileName { get; set; }

        protected abstract void CreateFileInSystemStorage(byte[] input, string name);

        protected abstract bool ExistsFileInSystemStorage(string name);

        protected abstract byte[] RemoveFileFromSystemStorage(string name);

        protected abstract void MoveToDirectory(string name);

        public Logger(string logFileName)
        {
            this.LogFileName = logFileName;
            MoveToDirectory(logFolderName);
            LogInFileStorage("----------------App started---------------------");

        }

        public void LogInFileStorage(string message)
        {
            if (ExistsFileInSystemStorage(LogFileName))
            {
                byte[] logFileToAppend = RemoveFileFromSystemStorage(LogFileName);
                System.IO.File.WriteAllBytes(LogFileName, logFileToAppend);
            }

            System.IO.File.AppendAllText(LogFileName, message + Environment.NewLine);
            var bytes = System.IO.File.ReadAllBytes(LogFileName);

            CreateFileInSystemStorage(bytes, LogFileName);

        }

    }
}
