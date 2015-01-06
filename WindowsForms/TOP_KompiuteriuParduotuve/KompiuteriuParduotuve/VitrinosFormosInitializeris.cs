using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kompiuteriuparduotuve.UI;

namespace KompiuteriuParduotuve
{
   public static class VitrinosFormosInitializeris
   {
       private static readonly Shop instance = new Shop(); // singleton

       public static void Show()
       {
           instance.Show();
       }

       public static Shop Get()
       {
           return instance;
       }

       public static void Hide()
       {
           instance.Hide();
       }

       public static void AddRow(Preke elem)
       {
           instance.addRowToList(elem);
           instance.addRowToGrid(elem);
       }

   }
}
