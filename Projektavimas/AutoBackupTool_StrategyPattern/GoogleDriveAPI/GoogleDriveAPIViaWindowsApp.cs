using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveAPI
{
    public class GoogleDriveAPIViaWindowsApp
    {
        private string GoogleDrivePath;
        public GoogleDriveAPIViaWindowsApp(string googleDrivePath)
        {
            GoogleDrivePath = googleDrivePath;
        }

        public bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://drive.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public void MoveFileToDrive(byte[] content, string name)
        {
            File.WriteAllBytes(Path.Combine(GoogleDrivePath, name), content);
        }

        public byte[] RemovefileFromDrive(string name)
        {
            var path = Path.Combine(GoogleDrivePath, name);
            if (File.Exists(path))
            {
                var file = File.ReadAllBytes(path);
                File.Delete(Path.Combine(GoogleDrivePath, name));
                return file;
            }
            return null;
        }

        public bool Exists(string name)
        {
            return Directory.EnumerateFiles(GoogleDrivePath).Any(x => x.EndsWith(name));
        }

        public void DeleteContents()
        {
            foreach (var file in Directory.EnumerateFiles(GoogleDrivePath))
            {
                File.Delete(file);
            }  
        }


    }
}
