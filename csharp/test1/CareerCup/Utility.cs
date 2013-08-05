using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    public class LinkedListNode
    {
        public int value;
        public LinkedListNode next;

        public LinkedListNode(int v)
        {
            value = v;
        }

        public LinkedListNode()
        {
            value = 0;
        }

        public LinkedListNode Add(LinkedListNode node)
        {
            LinkedListNode tail = this;
            while (tail.next != null) tail = tail.next;
            tail.next = node;
            return node;
        }

        public LinkedListNode Add(int v)
        {
            LinkedListNode node = new LinkedListNode();
            node.value = v;
            return Add(node);
        }
    }
}
