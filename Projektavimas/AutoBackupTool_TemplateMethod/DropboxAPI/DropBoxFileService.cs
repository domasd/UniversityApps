using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using System.IO.Compression;

namespace DropboxAPI
{
    public class DropBoxFileService
    {
        private string DropBoxPath;
        private string compressedArchiveName = "bck.zip";
        public DropBoxFileService(string path)
        {
            DropBoxPath = path;
        }

        public bool CheckConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://dropbox.com"))
                    {
                        if (Process.GetProcesses().Any(x => x.ProcessName == "Dropbox"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public void MoveFileToDropbox(byte[] content, string name)
        {
            File.WriteAllBytes(Path.Combine(DropBoxPath, name), content);
        }

        public byte[] RemovefileFromDropbox(string name)
        {
            var path = Path.Combine(DropBoxPath, name);
            if (File.Exists(path))
            {
                var file = File.ReadAllBytes(path);
                File.Delete(Path.Combine(DropBoxPath, name));
                return file;
            }
            return null;
        }

        public bool Exists(string name)
        {
            return Directory.EnumerateFiles(DropBoxPath).Any(x => x.EndsWith(name));
        }

        public void DeleteContents()
        {
            foreach (var file in Directory.EnumerateFiles(DropBoxPath))
            {
                File.Delete(file);
            }

        }

        public bool ExistsCompressedArchive()
        {
            return File.Exists(Path.Combine(DropBoxPath, compressedArchiveName));
        }

        public void AppendCompressedArchive(byte[] contents, string name)
        {
            if (!ExistsCompressedArchive()) throw new InvalidOperationException("No compressed file");

            using (FileStream zipToOpen = new FileStream(Path.Combine(DropBoxPath, compressedArchiveName), FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry(name);
                    using (BinaryWriter writer = new BinaryWriter(readmeEntry.Open()))
                    {
                        writer.Write(contents);
                    }
                }
            }
        }

        public void AddCompressedArchivewithFile(byte[] contents, string name)
        {
            Directory.CreateDirectory(Path.Combine(DropBoxPath, "temp"));
            File.WriteAllBytes(Path.Combine(DropBoxPath, "temp", name), contents);

            ZipFile.CreateFromDirectory(Path.Combine(DropBoxPath, "temp"), Path.Combine(DropBoxPath, compressedArchiveName));

            foreach (var file in Directory.EnumerateFiles(Path.Combine(DropBoxPath, "temp")))
            {
                File.Delete(file);
            }

            try
            {
                Directory.Delete(Path.Combine(DropBoxPath, "temp"));
            }
            catch 
            {
                // Could not delete - skip and resume process, will be deleted later
            }
        }


    }
}
