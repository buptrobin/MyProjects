using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuerySetGenerator
{
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            string html = @"sb_adsW\";
            Console.WriteLine(html.Replace(@"sb_adsW\", @"sb_adsWv2\"));

            Console.WriteLine(RevertStr("abcde"));
            Console.WriteLine(RevertStr("abcd"));
            Console.WriteLine(RevertStr(""));
            Console.WriteLine(RevertStr(" "));
            Console.WriteLine(RevertStr("a"));
            Console.WriteLine(RevertStr("ab"));

            Console.ReadKey();
        }

        public static string RevertStr(string sIn)
        {
            if (string.IsNullOrEmpty(sIn)) return "";
            if (sIn.Length == 1) return sIn;

            char cbegin = sIn[0];
            char cend = sIn[sIn.Length - 1];
            return cend + RevertStr(sIn.Substring(1, sIn.Length - 2)) + cbegin;
        }


        static void queryMain(string[] args)
        {
            const string querylistfile = @"d:\tmp\querylist.txt";
            const string querysetfile = @"d:\tmp\queryset.dat";

            var queryhash = new List<UInt64>();
            string[] queries = File.ReadAllLines(querylistfile);
            foreach (var query in queries)
            {
                UInt64 h = ComputeQueryHashStatic(query);
                queryhash.Add(h);
            }

            using (var fs = new FileStream(querysetfile, FileMode.Create))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    foreach (var h in queryhash)
                    {
                        writer.Write((ulong)h);
                    }
                }
            }
            
        }

        private static UInt64 ComputeQueryHashStatic(string query)
        {
            const UInt64 HASH_MULTIPLIER = 37;

            UInt64 hash = 0;
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(query);

            foreach (var b in byteArray)
            {
                hash = HASH_MULTIPLIER * hash + b;
            }

            return hash;
        }
    }
}
