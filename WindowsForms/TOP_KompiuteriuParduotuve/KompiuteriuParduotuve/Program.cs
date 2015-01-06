using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using KompiuteriuParduotuve.Resources;
using Kompiuteriuparduotuve.UI;


namespace KompiuteriuParduotuve
{
    public delegate void Delegatas(string msg);

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Delegatas uzlogint = delegate(string msg)
            {
                Logeris.EntryLog(msg);
            }; // anonymous method
            Logeris.Open();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new IkeltiPreke(uzlogint));
        }


    }
}
