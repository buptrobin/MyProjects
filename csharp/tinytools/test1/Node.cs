using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace test1
{
    class Node
    {
        public int data;
        public List<Node> neighbors = new List<Node>();
    }

    class GraphCloner
    {
        HashSet<Node> set = new HashSet<Node>();

        public Node clonegraph(Node graph)
        {
            Node newgraph = new Node();

            newgraph.data = graph.data;
            foreach (Node node in graph.neighbors)
            {
                if(set.Contains(node))
                {

                }
            }

            return newgraph;
        }
    }
}
