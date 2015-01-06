using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aukcionas
{
   public struct Kaina
    {
        private decimal pradKaina;
        public decimal dabartKaina { get; set; }

        public decimal PradKaina
        {
            get { return pradKaina; }
        }

        public Kaina(decimal prad) : this()
        {
            pradKaina = prad;
            dabartKaina = pradKaina;
        }

    }
}
