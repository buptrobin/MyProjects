using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class Practice
    {
        public void practice4()
        {
            string s = "ABC";
            Console.WriteLine(HomoBlock(s));

        }
        public void practice3()
        {
            string IPs = "131.253.38.208/31";
            string[] s1 = IPs.Split('/');
            string baseip = s1[0];
            int netmask = int.Parse(s1[1]);
            ComputeNetMask(baseip, netmask);
            Console.ReadKey();
        }
        public void practice2()
        {
            myatoi("123");
            Console.ReadKey();
        }

        public int myatoi(string s)
        {

            return 0;
        }
        public void practic1()
        {
            MyNode h = Array2List(new int[] { 3, 2, 1 });
            if (h == null) return;
            do
            {
                Console.Write(h.value);
                h = h.next;
            }
            while (h != null);
            
        }

        public MyNode Array2List(int[] arr)
        {
            Console.WriteLine(arr.Length);
            MyNode head = new MyNode();
            head.next = null;
            MyNode p = head;
            MyNode tail = new MyNode();
            for (int i = 0; i < arr.Length; i++)
            {
                MyNode newnode = new MyNode();
                newnode.value = arr[i];
                newnode.next = null;

                p.next = newnode;
                p = newnode;
            }

            return head.next;
        }

        public void atoi(string s)
        {
            int n = 0;
            for (int i = 0; i<s.Length; i++)
            {
                char c = s[i];
                if (c > '9' || c < '0') throw new Exception("Input not number.");

                n = n * 10 + (c - '0');

            }
            Console.WriteLine(n);
        }

        public void fib(int n)
        {
            int x = 0;
            int y = 1;
            for (int i = 0; i < n; i++, y = x + y, x = y - x)
            {
                Console.Write(y + " ");
            }

        }

        public void CreateHeap(int[] a, int m)
        {
            int[] h = new int[m];
            int end = 0;
            h[0] = a[0];
            for (int i = 1; i < m; i++)
            {
                int k = a[i];
                int curr = 0;
                while (curr<end)
                {
                    //IF k<h[curr] THEN swap(k, h.curr)
                    if (k < h[curr])
                    {
                        int tmp = k;
                        k = h[curr];
                        h[curr] = tmp;
                    }
                    int left = curr * 2;
                    int right = curr * 2 + 1;
                    if (left <= end && right > end) curr = right;
                    else curr = left;
                }
                h[curr] = k;
                end = curr;
            }
        }

        public void CreateHeap2(int[] a, int m)
        {
            for (int i = m/2-1; i >= 0; i--)
            {
                int k = i;
                int j = 2 * k + 1;
                while (j < m)
                {
                    if (j < m - 1 && a[j] > a[j + 1]) j++;
                    if (a[k] < a[j]) break;
                    swap(a, k, j);
                    k = j;
                    j = j * 2 + 1;
                }
            }
            for (int i = 0; i < m; i++) Console.Write(a[i] + " ");
        }

        private void swap(int[] a, int x, int y)
        {
            if (x < a.Length && y < a.Length & x>=0 && y>=0 && x!=y)
            {
                int tmp = a[x];
                a[x] = a[y];
                a[y] = tmp;
            }
        }

        private int max(int[] a, int x, int y)
        {
            if (x < a.Length && y < a.Length & x >= 0 && y >= 0)
            {
                if (a[x] > a[y]) return x;
                else return y;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        private void ComputeNetMask(string baseip, int netmask)
        {
            string[] arr = baseip.Split('.');
            int last = int.Parse(arr[3]);

            int k = 1;
            k= 1<<(32-netmask);            

            for (int i = 0; i < k; i++)
            {
                Console.WriteLine("{0}.{1}.{2}.{3}", arr[0], arr[1], arr[2], last+i);
            }

        }

        private int HomoBlock(string str)
        {
            if (string.IsNullOrEmpty(str)) return -1;

            int maxLen = 1;
            int currentLen = 1;
            int maxStart = 0;
            int currStart = 0;
            char currChar = str[0];

            for (int i = 1; i < str.Length; i++)
            {
                //if next char != current char
                if (str[i] != currChar)
                {
                    currStart = i;
                    currentLen = 1;
                    currChar = str[i];
                    continue;
                }

                //if next char == current char
                currentLen++;
                if (currentLen > maxLen)
                {
                    maxLen = currentLen;
                    maxStart = currStart;
                }
            }

            return maxStart;
        }

        //1 3 5 7 9 11
        public int BinarySearch(int[] array, int v, int begin, int end)
        {
            if (array == null || array.Length < 1) return -1;
            if (begin == end || v <　array[begin] || v > array[end]) return -1;
            int m = (begin + end) / 2 + 1;
            if (array[m] == v) return m;
            if (array[m] > v) return BinarySearch(array, v, begin, m);
            else return BinarySearch(array, v, m + 1, end);

        }

        public void practiceBinarySearch()
        {
            for(;;)
            {
                string s = Console.ReadLine();
                if(s.Equals("end",StringComparison.InvariantCultureIgnoreCase)) return;

                string[] sa = s.Split();
                int[] a = new int[sa.Length];
                for(int i=0;i<sa.Length;i++)
                {
                    if(!int.TryParse(sa[i], out a[i]))
                        a[i]=0;
                }
                Console.WriteLine("===="+BinarySearch(a, 10, 0, a.Length-1));
            }
        }

        public void Exam1_8()
        {
            bool stop = false;
            for (; ; )
            {
                string s1 = Console.ReadLine();
                if (s1.Equals("exit", StringComparison.OrdinalIgnoreCase)) return;
                string s2 = Console.ReadLine();
                

                Console.WriteLine(isSubString(s1, s2));
            }
            
        }
        private bool isSubString(string s1, string s2)
        {            
            if ( s1 == s2 ) return true;

            if (s1.Length == s2.Length) return false;

            string s = String.Concat(s1, s2);

            if (s.IndexOf(s2, System.StringComparison.Ordinal) >= 0) return true;

            return false;

        }

        public void Exam1_9()
        {
            for (;;)
            {
                string s = Console.ReadLine();
                if (s == null) continue;
                
                if ("exit" == s.ToLower()) return;

                string[] sarray = s.Split(' ');
                MyLinkedList list = new MyLinkedList();
                foreach (var s1 in sarray)
                {
                    int v;
                    if(int.TryParse(s1, out v))
                    {
                        list.AppendToTail(v);
                    }
                }
                list.PrintList();

            }
            
        }

        public void Exam_maxSubArray()
        {
            int[] eee = new int[255];
            string ss = "123";
            Console.WriteLine(eee[ss[1]]);
            for (;;)
            {
                string s = Console.ReadLine();
                if (s == null) continue;
                if ("exit" == s.ToLower()) return;

                int[] array = GetArray(s);
                Console.WriteLine(GetMaxSubArraySum(array));
            }

        }

        private int GetMaxSubArraySum(int[] array)
        {
            if (array == null || array.Length < 1) return 0;
            int max = int.MinValue;
            int begin = 0;
            int sum = 0;
            for (int tail = 0; tail < array.Length; tail++)
            {
                sum += array[tail];
                if (sum > max) max = sum;
                if (sum < 0) sum = 0;
            }

            return max;
        }

        private int[] GetArray(string s)
        {
            string[] sarray = s.Split(' ');
            var al = new ArrayList();
            foreach (var s1 in sarray)
            {
                int v;
                if (int.TryParse(s1, out v))
                {
                    al.Add(v);
                }
            }
            return (int[]) al.ToArray(typeof(int));
        }
    }

    public class MyLinkedList
    {
        public MyNode Header = null;
        public MyNode Tail = null;

        public void AppendToTail(int d)
        {
            MyNode node = new MyNode(d);
            AppendToTail(node);
        }

        public void AppendToTail(MyNode node)
        {
            if (node == null) return;
            if (Header == null)
            {
                Header = node;
            }
            if (Tail != null)
            {
                Tail.next = node;
            }

            MyNode t = node;
            while (t.next != null) t = t.next;
            Tail = t;
            return;
        }

        public void PrintList()
        {
            if (Header == null) return;
            MyNode t = Header;
            while (t != null)
            {
                Console.Write(" "+t.value);
                t = t.next;
            }
            Console.WriteLine();
        }

}
    public class MyNode
    {
        public int value;
        public MyNode next = null;

        public MyNode(int d)
        {
            value = d;
        }

        public MyNode() : this(0)
        {
        }

    
    }
}
