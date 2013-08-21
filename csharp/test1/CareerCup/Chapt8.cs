using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    class Chapt8 : ExamBase
    {
        //8.1
        public long NFib(int n)
        {
	        if(n<0) throw new ArgumentException("Input n should >0");
	        if(n==0) return 0;
	        if(n==1) return 1;

	        long a = 0;
	        long b = 1;
            long c = 0;
	        for(int i=2;i<=n;i++)
	        {
		        c = a + b;
		        a = b;
		        b = c;
	        }
	        return c;
        }

        public long NFibRec(int n)
        {
	        if(n==0) return 0;
	        if(n==1) return 1;
            if (n > 1) return NFibRec(n - 1) + NFibRec(n - 2);
            else return -1;
        }

        //8.2
        public static int Grid(int N)
        {
	        if(N<0) throw new ArgumentException("N should >0");
	        if(N==0) return 0;
	        if(N==1) return 1;

	        return CalcGridPath(N, N);
        }

        public static int CalcGridPath(int x, int y)
        {
            if(x==1 && y==1) return 1;
	        if(x<1 || y<1) return 0;
	        return CalcGridPath(x-1, y) + CalcGridPath(x, y-1);
        }
        public static void Test8_2()
        {
            AssertEqual(2, Grid(8));
        }

        //8.5
        public static List<string> Perm(string s)
        {
	        if(s==null) return null;
	        if(s.Length<1) return new List<string>() {""};

	        return FindPerm(s, 0, s.Length-1);
        }

        public static List<string> FindPerm(string s, int index, int m)
        {
            if (index == m) return new List<string>() { s };

            List<string> subperms = FindPerm(s, index + 1, m);
            List<string> newperms = new List<string>();
            foreach (var a in subperms)
            {
                newperms.Add(a);
                for (int i = index+1; i <= m; i++)
                {
                    if (s[index] != s[i]) newperms.Add(swap(a, index, i));
                }
            }

            return newperms;
        }
        public static string swap(string s, int x, int y)
        {
            if (x == y) return s;
            if (x < 0 || y < 0) throw new ArgumentException("input error");

            char x1 = s[x];
            char y1 = s[y];
            string s1 = s.Substring(0, x) + y1 + s.Substring(x+1);
            string s2 = s1.Substring(0, y) + x1 + s1.Substring(y+1);
            return s2;
        }
        public static void Test8_5()
        {
            var list = Perm("121");
            for (int i = 0; i < list.Count;i++ )
                Console.WriteLine(list[i]);
        }
    }
}
