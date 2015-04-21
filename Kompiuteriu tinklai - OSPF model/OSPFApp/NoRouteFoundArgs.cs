using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPFApp
{
    class NoRouteFoundArgs : EventArgs
    {
        public Router @from;
        public Router to;
    }
}
