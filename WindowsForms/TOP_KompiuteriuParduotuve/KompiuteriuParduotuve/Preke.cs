using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kompiuteriuparduotuve.Properties;
using Kompiuteriuparduotuve.Resources;

namespace KompiuteriuParduotuve
{
   public class Preke : IEquatable<Preke>, IComparable
    {
        bool IEquatable<Preke>.Equals(Preke other)
        {
            if (other.pavadinimas != other.pavadinimas)
            {
                return false;
            }
            else return true;
        }

        int IComparable.CompareTo(object obj)
        {
            try
            {
                Preke other = (Preke) obj;
                if (this.kaina.dabartKaina < other.kaina.dabartKaina)
                {
                    return 1;
                }
                else if (this.kaina.dabartKaina > other.kaina.dabartKaina)
                {
                    return -1;
                }
                return 0;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                Logeris.EntryLog(ResourceLogeris.klaida + " bandant palyginti dvi prekes" );
                return -2;
            }
        }


        public string pavadinimas { get; set; }
        public string aprasymas { get; set; }
        public Kaina kaina = new Kaina();
        public Image nuotrauka { get; set; }
       private int vienetai;// { get; set; }

        public int Vienetai
        {
            get { return vienetai; }
        }

        public Preke(string pav, string apras, Kaina kain, int laik, Image nuotr)
        {
            pavadinimas = pav;
            aprasymas = apras;
            kaina = kain;
            nuotrauka = nuotr;
            vienetai = laik;
        }


       public void atimtiVieneta()
       {
           vienetai--;
       }

       public static void Swap<T>(ref T lhs, ref T rhs)
       {
           T temp;
           temp = lhs;
           lhs = rhs;
           rhs = temp;
       }

       

    }
}
