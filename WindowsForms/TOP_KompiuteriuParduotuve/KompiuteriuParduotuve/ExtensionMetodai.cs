using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KompiuteriuParduotuve;

namespace Kompiuteriuparduotuve
{
   static class ExtensionMetodai
    {
       //Extension metodas
        public static IEnumerable<Preke> Filter(this IEnumerable<Preke> listas, Func<Preke, bool> filtras)
        {
            foreach (var elem in listas)
            {
                if (filtras(elem))
                {
                    yield return elem;
                }
            }
        }  
    }
}
