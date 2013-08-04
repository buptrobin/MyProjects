using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDistinct
{
    using System.IO;

    class Program
    {
        private static string INPUTFILE = @"D:\work\tmp\MSM\zh_cn_dump_query.txt";
        private static string OUTPUT_NOFREQ = @"D:\work\tmp\MSM\zh_cn_query_distinct.txt";
        private static string OUTPUT_FREQ = @"D:\work\tmp\MSM\zh_cn_query_distinct_freq.txt";
        static void Main(string[] args)
        {
            Distinct();
        }

        public static void Distinct()
        {
            string[] queries = File.ReadAllLines(INPUTFILE);

            Dictionary<string, int> querycount = new Dictionary<string, int>();
            StreamWriter f_nofreq = new StreamWriter(OUTPUT_NOFREQ);
            StreamWriter f_freq = new StreamWriter(OUTPUT_FREQ);

            foreach (var query in queries)
            {
                if (!querycount.ContainsKey(query))
                {
                    querycount[query] = 0;
                }
                querycount[query] = querycount[query] + 1;
            }

            foreach (var kvp in querycount)
            {
                string q = kvp.Key;
                int count = kvp.Value;
                f_nofreq.WriteLine(q);
                f_freq.WriteLine(q+'\t'+count);
            }

            f_freq.Close();
            f_nofreq.Close();
        }
    }
}
