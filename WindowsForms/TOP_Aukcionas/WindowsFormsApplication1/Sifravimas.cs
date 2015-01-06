using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aukcionas
{
    static class Sifravimas
    {
        private static string key = "aaaaaaaaaaqwerty";
        //private static string IV = "123";

        public static string Encrypt(string input)
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length); 
            }
            catch (Exception e)
            {
                    Logeris.EntryLog(Resources.ResourceLogeris.klaida);
                    return Resources.Resource.Klaida;
            }
              
         }

         public static string Decrypt(string input)
         {
              byte[] inputArray = Convert.FromBase64String(input);
              TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
              tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
              tripleDES.Mode = CipherMode.ECB;
              tripleDES.Padding = PaddingMode.PKCS7;
              ICryptoTransform cTransform = tripleDES.CreateDecryptor();
              byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
              tripleDES.Clear();
              return UTF8Encoding.UTF8.GetString(resultArray);
         }
    }
}
