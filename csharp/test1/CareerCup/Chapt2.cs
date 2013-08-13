using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    using System.Collections;

    class Chapt2 : ExamBase
    {
        //2.1
        static void DeleteDups(LinkedListNode header)
        {
	        if(header==null) return;
	        HashSet<int> exist = new HashSet<int>();
	        exist.Add(header.value);
	
	        LinkedListNode pa = header.next;
	        LinkedListNode pb = header;
	
	        while(pa!=null)
	        {
		        if(exist.Contains(pa.value)) {
			        pb.next = pa.next;
		        }
		        else
		        {
			        exist.Add(pa.value);
			        pb = pb.next;
		        }
                pa = pa.next;
	        }
        }
        public static void Test2_1()
        {
            LinkedListNode header = new LinkedListNode(1);

            header.Add(2).Add(2).Add(1);
            PrintLinkedList(header);
            Console.WriteLine("--------------------");
            DeleteDups(header);
            PrintLinkedList(header);
        }

<<<<<<< HEAD
        //2.2
        public static LinkedListNode NthToLast(LinkedListNode header, int N)
        {
            if (header == null) return null;
	        if(N<=0) throw new Exception("Incorrect Argument!");
	
	        LinkedListNode first = header;
	        LinkedListNode second = header;
	
	        for(int i=1;i<N;i++)
	        {
		        if(first!=null) first = first.next;
		        else
			        return null;
	        }
	
	        while(first.next != null)
	        {
		        first = first.next;
		        second = second.next;
	        }
	
	        return second;
        }
        public static void Test2_2()
        {
            LinkedListNode header = new LinkedListNode(1);
            LinkedListNode ret = NthToLast(header, 1);
            AssertEqual(1, ret.value);

            header.Add(2).Add(3).Add(4);

            ret = NthToLast(header, 1);
            AssertEqual(4, ret.value);
            ret = NthToLast(header, 4);
            AssertEqual(1, ret.value);
        }
=======
        //2.4
        public static LinkedListNode AddLink(LinkedListNode L1, LinkedListNode L2)
        {
            if (L1 == null || L2 == null) return null;
            LinkedListNode ret = new LinkedListNode(0);
            LinkedListNode head = ret;
            int carry = 0;

            while (L1 != null || L2 != null)
            {
                int value = carry;
                if (L1 != null)
                {
                    value += L1.value;
                    L1 = L1.next;
                }
                if (L2 != null)
                {
                    value += L2.value;
                    L2 = L2.next;
                }

                carry = value > 10 ? 1 : 0;

                ret = AddNext(ret, value % 10);
            }
            if (carry > 0) AddNext(ret, carry);

            return head.next;
        }

        public static LinkedListNode AddNext(LinkedListNode node, int value)
        {	
	        LinkedListNode n = new LinkedListNode(value);
	        if(node==null) return n;
	
	        node.next = n;
	        return n;
        }
        public static void Test2_4()
        {
            LinkedListNode h1 = new LinkedListNode(3);
            h1.Add(1).Add(5);
            PrintLinkedList(h1);

            var h2 = new LinkedListNode(5);
            h2.Add(9).Add(2);
            PrintLinkedList(h2);

            LinkedListNode h = AddLink(h1, h2);
            PrintLinkedList(h);
        }

        //2.5
        public static LinkedListNode FindCircleStart(LinkedListNode head)
        {
	        if(head==null) return null;
	
	        //two pointers,  p1, p2. everstep p1+1, p2+2, until meet
	        LinkedListNode p1 = head;
	        LinkedListNode p2 = head;
	        p1 = p1.next; 
	        if(p1 == null) return null;
	        p2 = p2.next.next;
	        if(p2 == null) return null;
	        while(p1!=p2)
	        {
		        p1 = p1.next;
		        if(p1==null) return null;
		        p2 = p2.next.next;
		        if(p2==null) return null;
	        }
	        Console.WriteLine(p1.value);

	        //caculate the circle length
	        int len = 1;
	        p1 = p1.next;
	        p2 = p2.next.next;
	        while(p1!=p2){
		        p1 = p1.next;
		        p2 = p2.next.next;
		        len+=1;
	        }
	        Console.WriteLine("len="+len);
	        //from header, evertime, p1+1, P2+L, until meet, then get the first node of the circle
	        p1 = head;
	        p2 = GoSteps(head, len);
	        while(p1!=p2){
		        p1 = p1.next;
		        p2 = p2.next;		
	        }
	        return p1;
        }

        private static LinkedListNode GoSteps(LinkedListNode node, int steps)
        {
	        if(node==null) return null;
	        if(steps<0) return null;
	        for(int i=0;i<steps;i++){
		        node = node.next;
		        if(node==null) return null;
	        }
	
	        return node;
        }

        public static void Test2_5()
        {
            LinkedListNode n = new LinkedListNode(1);
            LinkedListNode head = n;
            n = n.Add(2).Add(3).Add(4).Add(5);
            PrintLinkedList(head);
            n.next = head.next.next;

            LinkedListNode ret = FindCircleStart(head);
            Console.WriteLine(ret.value);

        }

>>>>>>> daa5ab961e002709d1a599c3d6d06305a8039082
    }

}
