using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
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
    }

}
