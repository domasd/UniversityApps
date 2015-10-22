using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoBackupTool_StrategyPattern.Entities;
using AutoBackupTool_StrategyPattern.Implementations;

namespace AutoBackupTool_StrategyPattern
{
    class Program
    {
        private static string pathToBackup = ConfigurationManager.AppSettings["ToBackupPath"];
        static void Main(string[] args)
        {
            Backuper backuper = new Backuper(pathToBackup,new DropboxService(), new LocalStorageService());
            Logger logger = new Logger("log.txt", new FtpService());

            try
            {
                logger.LogInFileStorage($"Preparing for backup to cloud. Time - {DateTime.Now}");
                backuper.BackupToCloud();
                logger.LogInFileStorage($"Backuped to cloud. Time - {DateTime.Now}");

                logger.LogInFileStorage($"Preparing for backup to file storage. Time - {DateTime.Now}");
                backuper.BackupToFileStorage();
                logger.LogInFileStorage($"Backuped to file storage. Time - {DateTime.Now}");
            }
            catch (Exception e)
            {
                logger.LogInFileStorage($"exception throwed: {e.Message}");
            }

        }
    }
}
