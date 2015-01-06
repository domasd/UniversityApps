using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aukcionas
{
   public class AukcionoElementas : IEquatable<AukcionoElementas>, IComparable
    {
        bool IEquatable<AukcionoElementas>.Equals(AukcionoElementas other)
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
                AukcionoElementas other = (AukcionoElementas) obj;
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
                Logeris.EntryLog(Resources.ResourceLogeris.klaida);
                return -2;
            }
        }


        public string pavadinimas { get; set; }
        public string aprasimas { get; set; }
        public Kaina kaina = new Kaina();
        public Image nuotrauka { get; set; }
       private int likeslaikas;// { get; set; }

        public int Likeslaikas
        {
            get { return likeslaikas; }
        }

        public AukcionoElementas(string pav, string apras, Kaina kain, int laik, Image nuotr)
        {
            pavadinimas = pav;
            aprasimas = apras;
            kaina = kain;
            nuotrauka = nuotr;
            likeslaikas = laik;
        }

       public void atimtiSekunde()
       {
           likeslaikas--;
       }

       

    }
}
