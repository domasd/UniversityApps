using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            Backuper backuper = new BackuperDropboxLocal(pathToBackup);
            Logger logger = new LoggerFtp("log.txt");
            try
            {
                logger.LogInFileStorage($"Preparing for backup to cloud from template method app. Time - {DateTime.Now}");
                backuper.BackupToCloud();
                logger.LogInFileStorage($"Backuped to cloud from template method app. Time - {DateTime.Now}");

                logger.LogInFileStorage($"Preparing for backup to file storage from template method app. Time - {DateTime.Now}");
                backuper.BackupToFileStorage();
                logger.LogInFileStorage($"Backuped to file storage from template method app. Time - {DateTime.Now}");
            }
            catch (Exception e)
            {
                logger.LogInFileStorage($"exception throwed: {e.Message}");
            }

        }
    }
}
