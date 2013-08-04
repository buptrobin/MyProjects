using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{

    public class Node
    {    
         public int value { get; set; }
         public List<Node> neighbors { get; set; }
    }

    public class GraphClone {

         public static void Test()
         {
             //TestOne("1,2 2,3 3,4");
             //TestOne(null);
             //TestOne("");
             //TestOne("1,2");
             TestOne("1,2 2,1");
             TestOne("1,2 2,3 3,4 3,1");
             TestOne("1,2 1,3 2,1 2,3");

         }
         
         public static void TestOne(string graph)
         {
             GraphClone gc = new GraphClone();
             Node node = gc.InitGraph(graph);
             Console.Write("ORIGIN: ");
             gc.Print(node);

             Node clone = gc.CloneDFS(node);
             Console.Write("CLONED: ");
             gc.Print(clone);
         }
         
         
         public Node Clone(Node startNode)
         {
              //check input
             if (startNode == null) return null;

              //initial data structure
              List<Node> travelList = new List<Node>();
              travelList.Add(startNode);
         
              HashSet<Node> nodeTravelled = new HashSet<Node>();
         
              Node cloned = new Node() {value = startNode.value};
              List<Node> cloneList = new List<Node>();
              Dictionary<int, Node> nodesCloned = new Dictionary<int, Node>();
              cloneList.Add(cloned);
              nodesCloned[cloned.value] = cloned;
         
              //do clone
              while(travelList.Count() > 0){
                   Node curr = travelList[0];
                   Node clonedCurr = cloneList[0];
                   if(curr.neighbors != null) {
                        clonedCurr.neighbors = new List<Node>();
                        foreach(var nb in curr.neighbors)
                        {

                             Node nbClone = GetNode(nodesCloned, nb.value);

                             cloneList.Add(nbClone);
                             clonedCurr.neighbors.Add(nbClone);
                         
						     if(!nodeTravelled.Contains(nb)) 
							    travelList.Add(nb);
                        }
				    }
                   travelList.RemoveAt(0);
                   cloneList.RemoveAt(0);
                   nodeTravelled.Add(curr);
              }
         
              return cloned;
         }
	 
	     public void Print(Node start)
	     {
		    if(start == null) return;
		    List<Node> printList = new List<Node>();
		    printList.Add(start);
		    HashSet<Node> travelled = new HashSet<Node>();


            var sb = new StringBuilder();
		
		    while(printList.Count>0)
		    {
			    Node curr = printList[0];
		        sb.Append(curr.value + ":");
			    if(curr.neighbors != null){
				    foreach(Node node in curr.neighbors)
				    {
					    sb.Append(String.Format("({0},{1}) ", curr.value, node.value));
					    if(!travelled.Contains(node))
						    printList.Add(node);
				    }
			    }
		        travelled.Add(curr);
			    printList.RemoveAt(0);
		    }
		
		    Console.WriteLine(sb.ToString());
	     }
	 
	     public Node InitGraph(string graph)
	     {
		    String[] pairs = graph.Split(' ');
		    if(pairs.Length<1) return null;
		
		    Dictionary<int, Node> nodes = new Dictionary<int, Node>();
		    foreach(string pair in pairs)
		    {
                string[] line = pair.Split(',');
			    if(line.Length<1) throw new Exception("input error");

                if (line.Length == 1) //only one node in graph
                {
                    int v;
                    if (!int.TryParse(line[0], out v))
                    {
                        throw new Exception("input error");
                    }
                    Node start = new Node() {value = v};
                    return start;
                }

		        int valueFrom;
		        int valueTo;
                if (!int.TryParse(line[0], out valueFrom) || !int.TryParse(line[1], out valueTo))
                {
                    throw new Exception("input error");
                }
                Node nodeFrom = GetNode(nodes, valueFrom);
                Node nodeTo = GetNode(nodes, valueTo);
			    if(nodeFrom.neighbors == null) 
				    nodeFrom.neighbors = new List<Node>();
			
			    nodeFrom.neighbors.Add(nodeTo);
		    }

	         return nodes[1];
	     }
	 
	     public Node GetNode(Dictionary<int, Node> nodes, int v)
	     {
		    if(nodes.ContainsKey(v)) 
			    return nodes[v];
		
		    Node node = new Node() {value = v};
		    nodes[v] = node;
		
		    return node;
	     }

         public Node CloneDFS(Node start)
         {
             if (start == null) return null;
             Node cloneStart = new Node() { value = start.value };
             HashSet<int> path = new HashSet<int>();
             Dictionary<int, Node> nodesCloned = new Dictionary<int, Node>();
             nodesCloned.Add(start.value, cloneStart);

             path.Add(start.value);

             DFSClone(start, cloneStart, path, nodesCloned);

             return cloneStart;
         }

         public void DFSClone(Node node, Node cloneNode, HashSet<int> pathSet, Dictionary<int, Node> nodesCloned)
         {
             if (node.neighbors == null) return;

             foreach (Node nb in node.neighbors)
             {
                 
                 Node clonenode = GetNode(nodesCloned, nb.value);
                 if (cloneNode.neighbors == null) cloneNode.neighbors = new List<Node>();
                 cloneNode.neighbors.Add(clonenode);

                 if (pathSet.Contains(nb.value)) continue;

                 pathSet.Add(nb.value);

                 DFSClone(nb, clonenode, pathSet, nodesCloned);

                 pathSet.Remove(nb.value);
             }

         }
    }
}
