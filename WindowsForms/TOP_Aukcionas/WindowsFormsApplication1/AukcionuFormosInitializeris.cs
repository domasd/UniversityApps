using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aukcionas
{
   public static class AukcionuFormosInitializeris
   {
       private static readonly Aukcionai instance = new Aukcionai();

       public static void Show()
       {
           instance.Show();
       }

       public static Aukcionai Get()
       {
           return instance;
       }

       public static void Hide()
       {
           instance.Hide();
       }

       public static void AddRow(AukcionoElementas elem)
       {
           instance.addRowToList(elem);
           instance.addRowToGrid(elem);
       }

   }
}
