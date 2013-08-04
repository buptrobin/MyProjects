using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading;

    public class Program
    {
        private int count = 0;

        private static void Main(string[] args)
        {
            /*
            string URL = @"http://bj1.ah.xap.binginternal.com:84/answerstla.aspx?q=%E6%97%85%E6%B8%B8[form:%22MONITR%22|workflow:%22BingFirstPageResults%22|adult:demote|flag:debug]";
            
            var req = WebRequest.Create(URL);
            var rep = req.GetResponse();

            var webstream = rep.GetResponseStream();
            var sr = new StreamReader(webstream);

            string br = sr.ReadToEnd();

            Console.WriteLine(br);
            */

/*
            string pattern = "^(?<PREFIX>abc_)(?<ID>[0-9])+(?<POSTFIX>_def)$";
            string input = "abc_123_def";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            string replacement = "456";
            Console.WriteLine(regex.Replace(input, string.Format("${{PREFIX}}{0}${{POSTFIX}}", replacement)));
*/


            string input = "abc_123_def";
            var pattern = @"(_)(\d+)(_)";
            //var replaced = Regex.Replace(input, pattern, "$1AA$3"); 
            var reg = new Regex(pattern, RegexOptions.IgnoreCase);
            


/*
            string url =
                @"http://117.79.227.241:5128/s?cl=3&word=%90%9f%8c%90%95%fe%e4%b7%17q%01u&wd=%90%9f%8c%90%95%fe%e4%b7%17q%01u&pn=0&rn=10&tn=inspusads&ip=218.58.197.118&bcode=ms&uid=A0QzmKuNSr9ZbZZbNHOaHQ&ref=&agt=Mozilla%2f4.0+(compatible%3b+MSIE+8.0%3b+Windows+NT+6.1%3b+Trident%2f4.0%3b+SLCC2%3b+.NET+CLR+2.0.50727%3b+.NET+CLR+3.5.30729%3b+.NET+CLR+3.0.30729%3b+Media+Center+PC+6.0)&form=IE8SRC";
            var replaced = Regex.Replace(url, @"(word=)([^&]*)(&wd=)", "$1"+"----"+"$3"); 
*/
            //Console.WriteLine(replaced);
//            string src = "word:123&name:abc";
//            Regex reg = new Regex(@"word:([^&]*)&name:([^&]*)");
//            var match = reg.Match(src);
//            if(match.Success)
//            {
//                Console.WriteLine("match");
//                Console.WriteLine(match.Groups[0].ToString());
//                Console.WriteLine(match.Groups[1].ToString());
//                Console.WriteLine(match.Groups[2].ToString());
//                string dest =
//                reg.Replace(src, ).Replace(src, "$1", "xxx");
//            }
                 


//            ThreeSum ts = new ThreeSum();
//            ts.Find3Sum(new int[] { -1,0 ,1, 2, -1, -4 });

            Console.ReadKey();
        }

        public void Step(string s, int n)
        {
            if (n == 0)
            {
                Display(s);
                return;
            }

            Step(s + " 1", n - 1);
            if (n >= 2) Step(s + " 2", n - 2);
        }
        private void Display(string s)
        {
            //Console.WriteLine(s);
            count++;
        }


        public void MaxPath()
        {
            int[,] array = {
                               {1, 3, 8},
                               {4, 5, 6},
                               {2, 8, 9}
                           };
            int[,] maxvalue = new int[3,3];

            maxvalue[0, 0] = 1;

            for (int i = 0; i <= 2; i++)
                for (int j = 0; j <= 2; j++)
                {
                    maxvalue[i,j] = max(array, maxvalue, i, j);
                }

            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    Console.Write(maxvalue[i, j] + " ");
                }
                Console.WriteLine();
            }

            //Console.WriteLine("max value = " + maxvalue[2,2]);
        }

        private int max(int[,] array, int[,] maxarray, int x, int y)
        {
            if(x==0 && y==0) return array[0, 0];
            if(x==0 && y>0) return maxarray[x, y - 1] + array[x, y];
            if(x>0 && y==0) return maxarray[x - 1, y] + array[x, y];
            return array[x, y] + (maxarray[x - 1, y] > maxarray[x, y - 1] ? maxarray[x - 1, y] : maxarray[x, y - 1]);
        }

        public static void Reverse(int[] array, int begin, int end)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (begin < 0)
            {
                throw new ArgumentOutOfRangeException("begin", "begin < 0");
            }
            if (end > array.Length - 1)
            {
                throw new ArgumentOutOfRangeException("end", "end out of bound.");
            }
            if (begin > end)
            {
                throw new ArgumentException("begin should not small than end.");
            }

            while (begin < end)
            {
                int tmp = array[end];
                array[end] = array[begin];
                array[begin] = tmp;
                begin++;
                end--;
            }

        }
    }

    public class mylist<T> : List<T>
    {
        private T[] items;

        private int count;

        public void RemoveMultiple(IEnumerable<T> itemsToRemove)
        {
            if (itemsToRemove == null)
            {
                throw new ArgumentNullException("itemsToRemove");
            }

            return;
        }
    }

    public class MultiSet<T> : IEnumerable<T>
    {
        private Dictionary<T, int> set = new Dictionary<T, int>();

        private int count = 0;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var entry in this.set)
            {
                for (var i = 0; i < entry.Value; i++)
                {
                    yield return entry.Key;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(T o)
        {
            if (this.Contains(o))
            {
                this.set[o] = 1;
            }
            else
            {
                this.set[o]++;
            }
            this.count++;
        }

        public void Remove(T o)
        {
            if (this.Contains(o))
            {
                if (this.set[o] > 0)
                {
                    this.set[o]--;
                    this.count--;
                }
                else
                {
                    this.set.Remove(o);
                }
            }
        }

        public int Count()
        {
            return this.count;
        }

        public bool Contains(T o)
        {
            return this.set.ContainsKey(o);
        }
    }

    public class MySingleon
    {
        private static volatile MySingleon instance = null;
        private static object syncRoot = new object();

        private MySingleon(){ }

        public static MySingleon getInstance()
        {
            if (instance==null)
            {
                lock(syncRoot)
                {
                    if (instance==null)
                    {
                        instance = new MySingleon();
                    }
                }
            }
            return instance;
        }

    }

    public class exam
    {
        public void testit()
        {

        }

        public int bsearch(ArrayList al, int des)
        {
            int[] a = (int[])al.ToArray();
            if(al == null || al.)
        }
    }
}