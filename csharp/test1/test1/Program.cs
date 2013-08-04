using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class Program
    {
        static void Main(string[] args)
        {

            var lc = new Leetcode();
            lc.test();
            //Practice p = new Practice();
            //p.tri_sum();
            /*
            string a = "abc";
            string b = a;
            a = a.ToUpper();
            Console.WriteLine(a);
            Console.WriteLine(b);
            Practice p = new Practice();
            //p.practice3();
            //p.CreateHeap2(new int[]{3,3,5,4,2}, 5);
            //p.practice4();
             */
            Console.Read();
        }

        public static void ParseMatchLog()
        {            
            StreamReader sr = File.OpenText(@"c:\work\matchlog.txt");
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                int begin = line.IndexOf("rawQuery");
                if (begin < 0) continue;
                int end = line.IndexOf("\"", begin + 10);
                if (end < 0) continue;

                string rawQuery = line.Substring(begin + 10, end);

                begin = line.IndexOf("|augments");
                if (begin < 0) continue;
                end = line.IndexOf("|", begin + 1);


            }

        }

        public static void GetIPs()
        {
            StreamReader sr = File.OpenText(@"C:\work\snatips.txt");
            string line;
            while( (line=sr.ReadLine()) !=null)
            {
                if(line.IndexOf("SNAT")<0) continue;
                Console.WriteLine(line);

                //get the ip section
                int startslash = line.IndexOf("/");
                //Console.WriteLine(startslash);
                int startdot = line.LastIndexOf(".", startslash, startslash);//
                //Console.WriteLine("startdot=" + startdot);

                //Console.WriteLine(line.Substring(startdot, startslash - startdot + 1));

                string firstthree = line.Substring(0, startdot);
                string str_a = line.Substring(startdot + 1, startslash - startdot - 1);
                int a;
                if (!int.TryParse(str_a, out a)) continue;

                int b = line.IndexOf(",", startslash);
                string strMask = line.Substring(startslash + 1, b - startslash - 1);
                int mask;
                if (!int.TryParse(strMask, out mask)) continue;

                for (int i = a; i <= 255; i++)
                {
                    if ((i | mask) == (a | mask))
                    {
                        Console.WriteLine(firstthree + "." + i);
                    }
                }

                //Console.WriteLine(a);
                //get the all the IP
            }
            Console.Read();
        }


        static void DownloadIt(){
            string url = "http://117.79.227.241:5128/s?wd=%bb%fa%c6%b1&pn=0&rn=10&ip=207.46.13.184&bcode=-1";
            string localfile = "c:/work/1.txt";
            DownloadFile(url, localfile);

            Console.Read();
            return;
        }
        /**
         * Magic squares:
         * A magic square of order n is an arrangement of the numbers from 1 to n^2 in an n by n matrix with each number occuring
         * exactly once so that each row,each column and each main diagonal has the same sum.
         * Any other better approach than exhaustive search approach??? Even this is not working in many instances of n.
         */
        public static void DownloadFile(string url, string filename)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(url, filename);
        }
    }

     
}
