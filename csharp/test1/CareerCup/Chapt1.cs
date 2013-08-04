using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    class Chapt1
    {
        //1.1
        public bool IsAllCharUnique(string s)
        {
            if (string.IsNullOrEmpty(s)) return true;
            HashSet<char> hash = new HashSet<char>();
            foreach (char c in s)
            {
                if (hash.Contains(c)) return false;
                else hash.Add(c);
            }

            return true;
        }

        //1.2
        public void Reverse(ref string s)
        {
            if (string.IsNullOrEmpty(s)) return;

            int len = s.Length;
            int begin = 0;
            int end = len - 1;
            StringBuilder sb = new StringBuilder(s);
            
            while (begin < end)
            {
                char tmp = sb[begin];
                sb[begin] = sb[end];
                sb[end] = tmp;
                begin++;
                end--;
            }

            s = sb.ToString();
        }

        //1.3
        public string RemoveDuplicate(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            HashSet<char> set = new HashSet<char>();
            int head = 0;
            int tail = 0;
            StringBuilder sb = new StringBuilder(s);
            while (tail <= sb.Length - 1)
            {
                if (set.Contains(sb[tail]))
                {
                    tail++;
                }
                else
                {
                    set.Add(sb[tail]);
                    sb[head] = sb[tail];
                    head++;
                    tail++;
                }
            }
            sb.Length = head;
            return sb.ToString();
        }
        public static void Test1_3()
        {
            Chapt1 cp = new Chapt1();
            AssertEqual(cp.RemoveDuplicate("abcabc"), "abc");
            AssertEqual(cp.RemoveDuplicate("abc"), "abc");
            AssertEqual(cp.RemoveDuplicate("a"), "a");
            AssertEqual(cp.RemoveDuplicate("aaa"), "a");
            AssertEqual(cp.RemoveDuplicate("aabc"), "abc");
            AssertEqual(cp.RemoveDuplicate("abcc"), "abc");
            AssertEqual(cp.RemoveDuplicate(""), "");
            AssertEqual(cp.RemoveDuplicate("aaaabb"), "ab");
        }

        //1.4
        public bool IsAnagram(string s1, string s2)
        {
            if (s1 == null || s2 == null) return false;
            if (s1.Length != s2.Length) return false;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] != s2[s2.Length - 1 - i]) return false;
            }
            return true;
        }
        public static void Test1_4()
        {
            var cp = new Chapt1();
            AssertEqual(cp.IsAnagram("abc", "cba"), true);
            AssertEqual(cp.IsAnagram("a", "a"), true);
            AssertEqual(cp.IsAnagram("abc", "abc"), false);
            AssertEqual(cp.IsAnagram("abc", "CBA"), false);
            AssertEqual(cp.IsAnagram("", ""), true);
            AssertEqual(cp.IsAnagram("abc", "cba "), false);
            AssertEqual(cp.IsAnagram("a", "c"), false);
            AssertEqual(cp.IsAnagram("abc", null), false);
        }

        //1.5

        //1.6               
        static void RotateMatrix(int[,] M, int N)
        {
	        if(M==null || N<=0) return;
	        if (N==1) return;
	
	        int half = (N-1)/2;
	        for(int i=0;i<=half;i++){
		        for(int j=i;j<=N-i-2;j++)
		        {
		            RotateOne(M, N, i, j);
		            //PrintMatrix(M, N);
		        }
	        }
        }
        static void RotateOne(int[,] M, int N, int i, int j)
        {
            int p = M[i, j];
            int ix = i;
            int jx = j;
            for (int r = 0; r < 3; r++)
            {
                //Console.WriteLine("{0},{1}->{2},{3}", N - 1 - jx, ix, ix, jx);
                M[ix, jx] = M[N - 1 - jx, ix];

                int tmp = N - 1 - jx;
                jx = ix;
                ix = tmp;
            }
            M[j, N - 1 - i] = p;
        }
        static void PrintMatrix(int[,] M, int N)
        {
            for (int i1 = 0; i1 < N; i1++)
            {
                for (int j1 = 0; j1 < N; j1++)
                    Console.Write(M[i1, j1] + "\t");
                Console.WriteLine();
            }
        }
        public static void Test1_6()
        {
            int[,] M = new int[,] {{1, 2, 3}, {4, 5, 6}, {7, 8, 9}};
            //{{1, 2, 3,4}, {5,6,7,8}, {9,10,11,12},{13,14,15,16}};
            int N = 3;
            
            PrintMatrix(M,N);
            Console.WriteLine("===================");
            RotateMatrix(M,N);
            PrintMatrix(M, N);
        }


        //1.7
        

        #region Assert Methods
        public static void AssertEqual(string s1, string s2)
        {
            if(s1.Equals(s2)) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}",s1,s2);
        }

        public static void AssertEqual(bool s1, bool s2)
        {
            if (s1.Equals(s2)) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}", s1, s2);
        }

        public static void AssertEqual(int s1, int s2)
        {
            if( s1 == s2 ) Console.Write("Pass\t"); else Console.Write("Fail\t");
            Console.WriteLine("s1={0}\ts2={1}", s1, s2);
        }
        #endregion
    }
}
