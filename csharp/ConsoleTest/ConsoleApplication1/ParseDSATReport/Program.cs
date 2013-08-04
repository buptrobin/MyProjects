using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ParseDSATReport
{
    class Program
    {

        static void Main(string[] args)
        {
            parseDSATStatistcs();
            Console.ReadKey();
        }

        private static string BINGGO_STATISTICS_PAGE = @"http://jiezhou-dev/DSATAnalysisStatistics/";
        public static void parseDSATStatistcs()
        {
            ArrayList al = new ArrayList();
            List<Dictionary<string,string>> DSATs = new List<Dictionary<string, string>>();

            using (WebClient wc = new WebClient())
            {
                //string value = wc.DownloadString(BINGGO_STATISTICS_PAGE);
                Stream stream = wc.OpenRead(BINGGO_STATISTICS_PAGE);
                StreamReader sr = new StreamReader(stream);
                string line = sr.ReadLine();
                bool start = false;
                while (line!=null)
                {
                    if (line.Contains("MainContent_GridView2")) {start = true;}
                    if (line.Contains("</table>")) start = false;

                    if (start)
                    {
                        //al.Add(line);
                        Dictionary<string, string> d = parseDSATLine(line);
                        if(d.Count>0) DSATs.Add(d);
                        //Console.WriteLine(line);
                    }
                    line = sr.ReadLine();
                }
            }

            using (StreamWriter sw = new StreamWriter("dsat.txt"))
            {

                string header = string.Format("DSATId\tQueryString\tCategoryName\tVerdictsArea\tSubArea\towner\tDSATStatus\tCreateTime");
                sw.WriteLine(header);
                foreach (var d in DSATs)
                {
                    string o = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}",
                                         d["DSATId"],
                                         d["QueryString"],
                                         d["CategoryName"],
                                         d["VerdictsArea"],
                                         d["SubArea"],
                                         d["owner"],
                                         d["DSATStatus"],
                                         d["CreateTime"]
                    );
                    sw.WriteLine(o);
                }
            }
            Console.WriteLine("Total="+DSATs.Count);
            statDSATs(DSATs);

        }

        public static void statDSATs    (List<Dictionary<string,string>> allDSAT )
        {
            Dictionary<string,int> verdicts = new Dictionary<string, int>();
            foreach (var dsat in allDSAT)
            {
                string verdict = dsat["VerdictsArea"];
                if(verdicts.ContainsKey(verdict))
                {
                    verdicts[verdict]++;
                }
                else
                {
                    verdicts[verdict] = 1;
                }
            }
            using (StreamWriter sw = new StreamWriter("dsatstat.txt"))
            {
                foreach (var v in verdicts.Keys)
                {
                    Console.WriteLine(v + "\t" + verdicts[v]);
                    sw.WriteLine(v + "\t" + verdicts[v]);
                }
                
            }

        }


        public static Dictionary<string, string> parseDSATLine(string s)
        {
            Regex r = new Regex(@".*<td><font color=""#333333"">(?<DSATId>\d+)</font></td><td><font color=""#333333"">(?<QueryString>.*)</font></td><td><font color=""#333333"">(?<CategoryName>.*)</font></td><td><font color=""#333333"">(?<VerdictsArea>.*)</font></td><td><font color=""#333333"">(?<SubArea>.*)</font></td><td><font color=""#333333"">(?<owner>.*)</font></td><td><font color=""#333333"">(?<DSATStatus>.*)</font></td><td><font color=""#333333"">(?<CreateTime>.*)</font></td>.*"); 
            Dictionary<string, string> dsat = new Dictionary<string, string>();
            if (r.IsMatch(s))
            {
                Match match = r.Match(s);
                dsat.Add("DSATId", match.Groups["DSATId"].Value);
                dsat.Add("QueryString", match.Groups["QueryString"].Value);
                dsat.Add("CategoryName", match.Groups["CategoryName"].Value);
                dsat.Add("VerdictsArea", match.Groups["VerdictsArea"].Value);
                dsat.Add("SubArea", match.Groups["SubArea"].Value);
                dsat.Add("owner", match.Groups["owner"].Value);
                dsat.Add("DSATStatus", match.Groups["DSATStatus"].Value);
                dsat.Add("CreateTime", match.Groups["CreateTime"].Value);
                string o = string.Format(@"{0}  {1} {2} {3} {4} {5} {6} {7}",
                                         match.Groups["DSATId"].Value,
                                         match.Groups["QueryString"].Value,
                                         match.Groups["CategoryName"].Value,
                                         match.Groups["VerdictsArea"].Value,
                                         match.Groups["SubArea"].Value,
                                         match.Groups["owner"].Value,
                                         match.Groups["DSATStatus"].Value,
                                         match.Groups["CreateTime"].Value
                    );
                Console.WriteLine(o);
            }

            return dsat;
        }


        public void XXX()
        {
            var Verdicts = new Dictionary<string, int>();
            var Verdict_Sub = new Dictionary<string, List<string>>();
            var SubVerditcs = new Dictionary<string, int>();

            foreach (var line in File.ReadAllLines(@"D:\tmp\dsat_report1.csv"))
            {
                //Console.WriteLine(line);
                string[] a = line.Split(',');
                string verdict = a[3];
                string subverdict = a[4];

                if (Verdicts.ContainsKey(verdict)) Verdicts[verdict]++;
                else Verdicts[verdict] = 1;

                if (SubVerditcs.ContainsKey(subverdict)) SubVerditcs[subverdict]++;
                else SubVerditcs[subverdict] = 1;

                if (Verdict_Sub.ContainsKey(verdict)) Verdict_Sub[verdict].Add(subverdict);
                else
                {
                    Verdict_Sub[verdict] = new List<string>();
                    Verdict_Sub[verdict].Add(subverdict);
                }
            }
            /*
            using (var sr = new StreamReader(@"D:\tmp\dsat_report1.csv"))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
                
            }*/
            foreach (var v in Verdicts.Keys)
            {
                Console.WriteLine(v + " " + Verdicts[v]);
                /*
                                if (Verdict_Sub.ContainsKey(v) && SubVerditcs.ContainsKey(Verdict_Sub[v].ToString()))
                                {
                                    Console.WriteLine(v + "/" + Verdict_Sub[v] + "/" + SubVerditcs[Verdict_Sub[v].ToString()]);
                                }

                 * */
            }

            Console.ReadKey();
        }
    }
}
