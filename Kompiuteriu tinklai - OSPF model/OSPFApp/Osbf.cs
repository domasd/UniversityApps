using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuickGraph;
using QuickGraph.Algorithms;
using System.Timers;


//********************************************
// Please, do not use this as real example...
//********************************************



namespace OSPFApp
{
    delegate void NoRouteHandler(object sender, NoRouteFoundArgs e);

    class Osbf
    {
        private AdjacencyGraph<Router, Edge<Router>> graph1; //The real physical links
        private Dictionary<Edge<Router>, double> costs1; //The real current costs of links

        private AdjacencyGraph<Router, Edge<Router>> graph0; //The real physical links
        private Dictionary<Edge<Router>, double> costs0; //The real current costs of links

        private System.Timers.Timer timer;
        private Random random;

        private event NoRouteHandler NoRouteFound;

        //Boot
        public void BootRouters()
        {
            BeforeBooting();

            graph1 = new AdjacencyGraph<Router, Edge<Router>>();
            costs1 = new Dictionary<Edge<Router>, double>();
            graph0 = new AdjacencyGraph<Router, Edge<Router>>();
            costs0 = new Dictionary<Edge<Router>, double>();

            var r1011 = new Router(10, 1, 1);
            var r1012 = new Router(10, 1, 2);
            var r1013 = new Router(10, 1, 3);
            var r1014 = new Router(10, 1, 4);
            var r1015 = new Router(10, 1, 5) { Type = RouterKinds.AreaBorder };
            var r1016 = new Router(10, 1, 6);

            AddEdgeWithCosts(r1011, r1012, 1.0);
            AddEdgeWithCosts(r1011, r1013, 2.0);
            AddEdgeWithCosts(r1013, r1014, 3.0);
            AddEdgeWithCosts(r1012, r1013, 1.0);
            AddEdgeWithCosts(r1014, r1015, 1.0);
            AddEdgeWithCosts(r1016, r1011, 1.0);
            AddEdgeWithCosts(r1011, r1016, 1.0);

            AddEdgeWithCosts(r1015, r1014, 1.0);
            AddEdgeWithCosts(r1014, r1013, 1.0);
            AddEdgeWithCosts(r1013, r1011, 1.0);

            //BackBoneArea
            var r1002 = new Router(10, 0, 2) { Type = RouterKinds.BackBone };
            var r1001 = new Router(10, 0, 1) { Type = RouterKinds.Boundary }; ;
            var r1003 = new Router(10, 0, 3);

            AddEdgeWithCosts0(r1015, r1002, 2);
            AddEdgeWithCosts0(r1002, r1015, 4);
            AddEdgeWithCosts0(r1002, r1001, 1);
            AddEdgeWithCosts0(r1001, r1002, 1);
            AddEdgeWithCosts0(r1002, r1003, 6);
            AddEdgeWithCosts0(r1003, r1001, 6);





            //Learn routes from other routers Area 1
            foreach (var vertice in graph1.Vertices)
            {
                vertice.LinkStatePacketDatabase = graph1.Clone();
                vertice.LinkStatePacketCostsDatabase = new Dictionary<Edge<Router>, double>(costs1);
                vertice.LinkStatePacketDatabaseSequence = 0;

                //Compute the shortest path to every other router
                foreach (var vertice2 in graph1.Vertices)
                {
                    if (vertice.Equals(vertice2)) continue; //To itself
                    vertice.CalculatedPaths.Add(vertice2, computeShortesPathsFromRouter(vertice, vertice2, graph1, costs1));
                }
            }

            //Learn routes from other routers Area 0
            foreach (var vertice in graph0.Vertices)
            {
                vertice.LinkStatePacketDatabase = graph0.Clone();
                vertice.LinkStatePacketCostsDatabase = new Dictionary<Edge<Router>, double>(costs0);
                vertice.LinkStatePacketDatabaseSequence = 0;

                //Compute the shortest path to every other router
                foreach (var vertice2 in graph0.Vertices)
                {
                    if (vertice.Equals(vertice2)) continue; //To itself
                    vertice.CalculatedPaths.Add(vertice2, computeShortesPathsFromRouter(vertice, vertice2, graph0, costs0));
                }
            }


            SendPackage(r1011, r1015);

            RemoveRouter(r1012);

            Thread.Sleep(1000);

            SendPackage(r1016, r1015);
            SendPackage(r1011, r1001); //To boundary


        }



        private void BeforeBooting()
        {
            //For Periodical stuff
            int wait = 4 * 1000; // 3secs
            timer = new System.Timers.Timer(wait);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;

            //when the route is not found
            NoRouteFound += NoRoute;

            //Initialize random
            random = new Random();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int index = random.Next(0, graph1.Vertices.Count());
            int index2 = random.Next(0, graph0.Vertices.Count());
            Router router;

            switch (random.Next(0, 1))
            {
                case 0:
                    router = graph1.Vertices.ToArray()[index];
                    Console.WriteLine("LINK STATE UPDATE: Router {0} floods link state update to adjacent routers", router);
                    foreach (var edge in router.LinkStatePacketDatabase.OutEdges(router))
                    {
                        LinkStateUpdate(router, edge.Target);
                    }
                    break;
                case 1:
                    router = graph0.Vertices.ToArray()[index];
                    Console.WriteLine("LINK STATE UPDATE: Router {0} floods link state update to adjacent routers", router);
                    foreach (var edge in router.LinkStatePacketDatabase.OutEdges(router))
                    {
                        LinkStateUpdate(router, edge.Target);
                    }
                    break;

            }




        }

        public void SendPackage(Router @from, Router to)
        {
            if (@from.Equals(to)) return;
            Console.WriteLine("Trying to send from {0} to {1}", @from, to);

            if (@from.address.Area != to.address.Area && from.Type != RouterKinds.AreaBorder)
            {
                //Get to the border!
                Router border =
                @from.LinkStatePacketDatabase.Vertices.FirstOrDefault(x => x.Type == RouterKinds.AreaBorder);
                SendPackage(from, border);
                //No we are in the border
                //Send to destination
                Console.WriteLine("Reacherd area border");
                SendPackage(border, to);
                return;

            }

            var paths = @from.CalculatedPaths[to];
            if (paths == null || paths.Count == 0)
            {
                //There is no routes to this router
                //Invoke event
                NoRouteFound(this, new NoRouteFoundArgs() { @from = from, @to = to });
                return;

            }

            List<IEnumerable<Edge<Router>>> pathsToSend = new List<IEnumerable<Edge<Router>>>();

            foreach (var path in @from.CalculatedPaths[to])
            {
                if (PathExists(graph1, path) || PathExists(graph0, path))
                {
                    pathsToSend.Add(path);
                }
                else
                {
                    Console.WriteLine("Router tried accesing path which is not real");
                    //Link state request
                    foreach (var edge in @from.LinkStatePacketDatabase.OutEdges(@from))
                    {
                        LinkStateRequest(@from, edge.Target);
                        SendPackage(from, to);

                    }

                    return;
                }


            }

            if (pathsToSend.Count == 0)
            {
                NoRouteFound(this, new NoRouteFoundArgs() { @from = from, @to = to });
                return;
            }

            if (paths.Count > 1) Console.WriteLine("The traffic is split accross multiple routes (ECMP)");
            foreach (var path in pathsToSend)
            {
                PrintPath(path);
            }
            Console.WriteLine("Delivered");




            Console.WriteLine();
        }


        private void AddEdgeWithCosts(Router source, Router target, double cost)
        {
            var edge = new Edge<Router>(source, target);
            graph1.AddVerticesAndEdge(edge);
            costs1.Add(edge, cost);
        }

        private void AddEdgeWithCosts0(Router source, Router target, double cost)
        {
            var edge = new Edge<Router>(source, target);
            graph0.AddVerticesAndEdge(edge);
            costs0.Add(edge, cost);
        }

        private void RemoveRouter(Router toRemove)
        {
            graph1.RemoveVertex(toRemove);
            //Adjacent routers sees that they lost connection with this router and updates their db
            foreach (var edge in toRemove.LinkStatePacketDatabase.Edges.Where(x => x.Target == toRemove))
            {
                UpdateState(edge.Source);
            }
            foreach (var edge in toRemove.LinkStatePacketDatabase.OutEdges(toRemove))
            {
                UpdateState(edge.Target);
            }
        }

        private bool PathExists(AdjacencyGraph<Router, Edge<Router>> graph, IEnumerable<Edge<Router>> Path)
        {
            foreach (var edge in Path)
            {
                if (!graph.Edges.Any(x => x.Equals(edge)))
                {
                    return false;
                }
            }
            return true;
        }


        public List<IEnumerable<Edge<Router>>> computeShortesPathsFromRouter(Router @from, Router to, AdjacencyGraph<Router, Edge<Router>> graph, Dictionary<Edge<Router>, double> costs)
        {
            List<IEnumerable<Edge<Router>>> paths = new List<IEnumerable<Edge<Router>>>();

            if (@from.Equals(to)) return null; //Route to itself

            var edgeCost = AlgorithmExtensions.GetIndexer(costs);
            var tryGetPath = graph.ShortestPathsDijkstra(edgeCost, @from);

            IEnumerable<Edge<Router>> mainPath;

            //main path
            if (tryGetPath(to, out mainPath))
            {

                int area = to.address.Area;
                double cost = CalculateCost(mainPath, area);

                paths.Add(mainPath);


                //Let's search for another
                foreach (var e in mainPath)
                {
                    var graphTemp = graph.Clone();
                    IEnumerable<Edge<Router>> pathTemp;
                    graphTemp.RemoveEdge(e);
                    var tryGetPathTemp = graphTemp.ShortestPathsDijkstra(edgeCost, @from);

                    if (tryGetPathTemp(to, out pathTemp))
                    {
                        //Found alternative. Check if it is equal 
                        double costTemp = CalculateCost(pathTemp, area);

                        if (costTemp.Equals(cost))
                        {
                            paths.Add(pathTemp);

                        }

                    }

                }
            }

            return paths;

        }


        public double CalculateCost(IEnumerable<Edge<Router>> path, int area)
        {
            double cost;
            return cost = path.Sum(x =>
            {
                double costEdge;
                if (area == 1)
                    costs1.TryGetValue(x, out costEdge);
                else costs0.TryGetValue(x, out costEdge);
                return costEdge;
            });
        }

        private void PrintPath(IEnumerable<Edge<Router>> path)
        {
            // Console.Write("Path found from {0} to {1}: {0}", @from, to);
            double cost = 0.0;

            Console.Write(path.First().Source);

            foreach (var e in path)
            {
                double costEdge;
                Console.Write(" > {0}", e.Target);
                costs1.TryGetValue(e, out costEdge);
                cost += costEdge;
            }
            Console.WriteLine();
            //Console.WriteLine("Time: " + cost);
        }

        public void PrintGraph(AdjacencyGraph<string, Edge<string>> graph)
        {
            foreach (var v in graph.Vertices)
            {
                foreach (var e in graph.OutEdges(v))
                    Console.WriteLine(e);
            }
        }

        public void NoRoute(object sender, NoRouteFoundArgs e)
        {
            Console.WriteLine("No such route from {0} to {1} found", e.from, e.to);
            //Recalculate immeadeatly?
            UpdateState(e.from);
        }

        public void UpdateState(Router router)
        {
            //Get newst version
            var newGraph = graph1.Clone();
            var newCosts = new Dictionary<Edge<Router>, double>(costs1);

            bool g = !router.LinkStatePacketDatabase.DeepCompare(newGraph);
            bool c = !router.LinkStatePacketCostsDatabase.DeepCompare(newCosts);
            if (!router.LinkStatePacketDatabase.DeepCompare(newGraph) || !router.LinkStatePacketCostsDatabase.DeepCompare(newCosts))
            {
                Console.WriteLine("Changes in area {1} occured and Router {0} updates his database", router, router.address.Area);

                //Updates to newest version
                router.LinkStatePacketDatabase = newGraph;
                router.LinkStatePacketCostsDatabase = newCosts;
                router.CalculatedPaths.Clear();

                //Increment sequence, so others will know it's newer
                router.LinkStatePacketDatabaseSequence++;

                //Calculate paths to other routes
                foreach (var routerTo in graph1.Vertices)
                {
                    if (router.Equals(routerTo)) continue; //To itself
                    router.CalculatedPaths.Add(routerTo, computeShortesPathsFromRouter(router, routerTo, graph1, costs1));
                }

            }

        }

        public void LinkStateUpdate(Router router, Router sender)
        {
            //Has the same database
            if (sender.LinkStatePacketDatabaseSequence <= router.LinkStatePacketDatabaseSequence)
            {
                //  Console.WriteLine("Routers have same databases");
                return;
            }


            //Get newer version
            var newGraph = sender.LinkStatePacketDatabase.Clone();
            var newCosts = new Dictionary<Edge<Router>, double>(sender.LinkStatePacketCostsDatabase);
            if (!router.LinkStatePacketDatabase.Equals(newGraph) ||
                !router.LinkStatePacketCostsDatabase.Equals(newCosts))
            {
                Console.WriteLine("LINK STATE UPDATE from {1}: Router {0} updates his database", router, sender);
                //Updates to newest version
                router.LinkStatePacketDatabase = newGraph;
                router.LinkStatePacketCostsDatabase = newCosts;
                router.CalculatedPaths.Clear();

                //Equal sequence, so others will know it's newer
                router.LinkStatePacketDatabaseSequence = sender.LinkStatePacketDatabaseSequence;

                //Recalculates routes to other routers
                foreach (var routerTo in newGraph.Vertices)
                {
                    if (router.Equals(routerTo)) continue; //To itself
                    router.CalculatedPaths.Add(routerTo,
                        computeShortesPathsFromRouter(router, routerTo, newGraph, newCosts));
                }


            }
            else
            {
                // Console.WriteLine("Routers have same databases");
            }
        }

        public void LinkStateRequest(Router from, Router to)
        {
            Console.WriteLine("LINK STATE REQUEST from {0} to {1}", from, to);

            if (to.LinkStatePacketDatabase.OutEdges(to).Any(x => x.Target.Equals(from)))
            {
                //Router sends link state update only if it has a link to it
                LinkStateUpdate(from, to);
            }

        }

    }
}
