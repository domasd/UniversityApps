using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Aukcionas
{
    public static class  Logeris
    {
        private static string directory = @"F:\STUDY\2 kursas\1 semestras\C#";
        private static string fileName = "logas.txt";
        private static FileStream fileStream;



        public static void Open()
        {
            if (!File.Exists(directory + @"\" + fileName))
            {
                using (fileStream = File.Create(directory + @"\" + fileName)) ;
            }
        }

        public  static void EntryLog(string msg)
        {

            using (StreamWriter streamWriter =  File.AppendText(directory + @"\" + fileName))
            {
                streamWriter.WriteLine(msg);
            }


        }

    }
}
