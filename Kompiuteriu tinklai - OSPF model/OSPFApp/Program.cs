using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms;

namespace OSPFApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            Osbf osbf = new Osbf();
            osbf.BootRouters();
  
            Console.Read();

        }
    }
}
