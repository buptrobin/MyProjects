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

    }
}
