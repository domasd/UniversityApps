using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace OSPFApp
{
    class Router : IEquatable<Router>
    {
        public RouterAddress address;

        public int LinkStatePacketDatabaseSequence;
        public AdjacencyGraph<Router, Edge<Router>> LinkStatePacketDatabase;
        public Dictionary<Edge<Router>, double> LinkStatePacketCostsDatabase;

        public Dictionary<Router, List<IEnumerable<Edge<Router>>>> CalculatedPaths;
        public Dictionary<Router, List<double>> CalculatedCosts;
        public RouterKinds Type;

        public Router(int As, int Area, int Router)
        {
            address = new RouterAddress()
            {
                As = As,
                Area = Area,
                Router = Router
            };

            LinkStatePacketDatabase = new AdjacencyGraph<Router, Edge<Router>>();
           LinkStatePacketCostsDatabase = new Dictionary<Edge<Router>, double>();
            CalculatedCosts = new Dictionary<Router, List<double>>();
            CalculatedPaths = new Dictionary<Router, List<IEnumerable<Edge<Router>>>>();

        }

        public override string ToString()
        {
            return this.address.Address;
        }


        public bool Equals(Router other)
        {
            if (other.address.Area == address.Area &&
                other.address.As == address.As
                && other.address.Router == address.Router) return true;

            return false;
        }
    }

    struct RouterAddress
    {
        public int As;
        public int Area;
        public int Router;

        public string Address
        {
            get { return string.Format("{0}.{1}.{2}", As, Area, Router); }
        }


       // public List<> 
    }

    enum RouterKinds
    {
        Undefined,
        AreaBorder,
        BackBone,
        Boundary
    }
}
