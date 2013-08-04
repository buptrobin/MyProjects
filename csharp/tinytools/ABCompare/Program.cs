using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCompare
{
    using System.IO;

    class Program
    {
        public static string DEFAULT_AB_1 = @"D:\work\tmp\MSM\1\ab_10.txt";
        public static string DEFAULT_AB_2 = @"D:\work\tmp\MSM\1\ab_4845.txt";
        public static string DEFAULT_AB_3 = @"D:\work\tmp\MSM\1\ab_7016.txt";
        public static string DEFAULT_OUTPUT = @"D:\work\tmp\MSM\1\ab_compare.txt";
        static void Main(string[] args)
        {
            string[] ab1 = File.ReadAllLines(DEFAULT_AB_1);
            string[] ab2 = File.ReadAllLines(DEFAULT_AB_2);
            string[] ab3 = File.ReadAllLines(DEFAULT_AB_3);

            List<string> output = new List<string>();
            Dictionary<string, string> setting4845 = new Dictionary<string, string>();
            Dictionary<string, string> setting7016 = new Dictionary<string, string>();
            string name = "";
            foreach (var line in ab2)
            {
                string value = GetValue(line, out name);
                setting4845[name] = value;
            }
            foreach (var line in ab3)
            {
                string value = GetValue(line, out name);
                setting7016[name] = value;
            }

            foreach (var line in ab1)
            {
                string value = GetValue(line, out name);
                if (value == null) continue;

                string value2 = setting4845.ContainsKey(name) ? setting4845[name] : "";
                string value3 = setting7016.ContainsKey(name) ? setting7016[name] : "";

                if (value.Equals(value3) && (!value.Equals(value2)))
                {
                    string s = string.Format("{0}\t{1}\t{2}\t{3}", name, value, value2, value3);
                    output.Add(s);
                }
            }

            File.WriteAllLines(DEFAULT_OUTPUT, output);
        }

        public static string GetValue(string line, out string name)
        {
            name = "";
            int p1 = line.IndexOf('<');
            int p2 = line.IndexOf('>', p1);
            int p3 = line.IndexOf("</", p2);
            
            if (p3 < 0) return null;
            name = line.Substring(p1 + 1, p2 - p1 - 1);
            string value = line.Substring(p2 + 1, p3 - p2 - 1);
            return value;
        }

    }
}
