using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace OSPFApp
{
    static class ExtensionMethods
    {

        public static bool DeepCompare(this AdjacencyGraph<Router,Edge<Router>> graph1, AdjacencyGraph<Router,Edge<Router>>  graph2)
        {
            if (graph1.Vertices.Except(graph2.Vertices).Any()
                || graph2.Vertices.Except(graph1.Vertices).Any()) return false;

            if (graph1.Edges.Except(graph2.Edges).Any()
             || graph2.Edges.Except(graph1.Edges).Any()) return false;

            return true;


        }

        public static bool DeepCompare(this Dictionary<Edge<Router>, double> dict1, Dictionary<Edge<Router>, double> dict2)
        {
            foreach (var key in dict1.Keys)
            {
                if (dict2.ContainsKey(key))
                {
                    if (!dict2[key].Equals(dict1[key])) return false;
                }
                else return false;
            }

            //foreach (var vertice1 in graph1.Vertices)
            //{
            //    if(graph2.Vertices.Except(vertice1.address)
            //}
            return true;


        }
     

    }

}
