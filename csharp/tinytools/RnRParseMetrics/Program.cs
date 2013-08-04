using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RnRParseMetrics
{
    using System.IO;

    class Program
    {
        private static string INPUTFILE = @"D:\work\tmp\MSM\result\IE03_Scarpe_Result_0625.txt";

        private static string OUTPUTFILE = @"D:\work\tmp\MSM\result\IE03_Metrics.txt";

        static void Main(string[] args)
        {
            Program p = new Program();
            p.RnRMetrics();
            //p.IE03Metrics();
            Console.Read();
        }

        public void IE03Metrics()
        {
            string[] allLines = File.ReadAllLines(INPUTFILE);
            var adsresult = new List<AdData>();

            foreach (var line in allLines)
            {
                if (line.StartsWith("m:RGUID")) continue;

                this.ParseLine(line, adsresult);        
            }
            
            this.CalculateMetrics(adsresult);
        }

        public void ParseLine(string line, List<AdData> adsresult)
        {
            string[] adfields = line.Split('\t');
            var ad = new AdData() { query = adfields[1], matchtype = adfields[4], position = adfields[12] };
            Console.Write(ad.query + "\t" + ad.matchtype + "\t" + ad.position);
            adsresult.Add(ad);
        }

        public void CalculateMetrics(List<AdData> adsresult)
        {
            var queries = new HashSet<string>();
            var mlqueries = new HashSet<string>();
            int coverage = 0;
            int mlcoverage = 0;
            int iy = 0;
            int mliy = 0;
            int phrasecount = 0;
            int broadcount = 0;
            int exactcount = 0;

            foreach (var adData in adsresult)
            {
                iy++;
                if (!queries.Contains(adData.query))
                {
                    coverage++;
                    queries.Add(adData.query);
                }

                if (adData.matchtype == "phrase") phrasecount++;
                if (adData.matchtype == "broad") broadcount++;
                if (adData.matchtype == "exact") exactcount++;

                if (adData.position == "mainline")
                {
                    mliy++;
                    if (!mlqueries.Contains(adData.query))
                    {
                        mlcoverage++;
                        mlqueries.Add(adData.query);
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("Coverage={0:0.00%}" , ((double)coverage) / 3289);
            Console.WriteLine("IY={0:0.00%}" , ((double)iy / 3289));
            Console.WriteLine("MLCoverage={0:0.00%}" , ((double)mlcoverage) / 3289);
            Console.WriteLine("MLIY={0:0.00%}" , ((double)mliy / 3289));
            Console.WriteLine("MLDensity={0:0.00}" , ((double)mliy / mlcoverage));
            Console.WriteLine("PhraseCount={0:0.00%}" , ((double)phrasecount / iy));
            Console.WriteLine("BroadCount={0:0.00%}" , ((double)broadcount / iy));
            Console.WriteLine("ExactCount={0:0.00%}" , ((double)exactcount / iy));
            /*
            using (StreamWriter sw = new StreamWriter(OUTPUTFILE))
            {
                sw.WriteLine("IY=" + adsresult.Count);
                int mliy = 0;
                int mlcount = 0;

                foreach (var kwp in adsresult)
                {

                }

            }
             * */
        }

        public class AdData
        {
            public string query { get; set; }

            public string matchtype { get; set; }

            public string position { get; set; }
        }

        //Calculate the Monetization log metrics
        public void RnRMetrics()
        {
            string freq_file = @"D:\work\tmp\MSM\scrape\zh_cn_query_distinct_freq.txt";
            string pv_file = @"D:\work\tmp\MSM\scrape\debug_rnr3_pageview.txt";
            string impress_file = @"D:\work\tmp\MSM\scrape\debug_rnr3_Impression.txt";

            string[] allLines = File.ReadAllLines(freq_file);
            var queryfreq = new Dictionary<string, int>();

            int freqTotal = 0;
            foreach (var line in allLines)
            {
                string[] s = line.Split('\t');
                int freq;
                if (!int.TryParse(s[1], out freq)) continue;
                queryfreq[CleanQuery2(s[0])] = freq;
                freqTotal += freq;
            }
            Console.WriteLine("distinct_count={0}  totalrequest={1}", queryfreq.Count, freqTotal);

            int srpv = CalculateSRPV(pv_file, queryfreq);
            int mlcoverage = 0;
            int mlcount = 0;
            int adscount = 0;
            int coverage = CalculateCoverage(impress_file, queryfreq, out mlcoverage, out mlcount, out adscount);

            Console.WriteLine("SRPV={0}\tCoverage={1}\tMLCoverage={2}\tMLCount={3}\tAdsCount={4}\n", srpv, coverage, mlcoverage, mlcount, adscount);
            Console.WriteLine("Coverage={0:0.00%}", ((double)coverage) / srpv);
            Console.WriteLine("IY={0:0.00}", ((double)adscount / srpv));
            Console.WriteLine("MLCoverage={0:0.00%}", ((double)mlcoverage) / srpv);
            Console.WriteLine("MLIY={0:0.00}", ((double)mlcount / srpv));
            Console.WriteLine("MLDensity={0:0.00}", ((double)mlcount / mlcoverage));
            /*
            Console.WriteLine("PhraseCount={0:0.00%}", ((double)phrasecount / iy));
            Console.WriteLine("BroadCount={0:0.00%}", ((double)broadcount / iy));
            Console.WriteLine("ExactCount={0:0.00%}", ((double)exactcount / iy));
            */
        }

        private int CalculateSRPV(string pvfile, Dictionary<string, int> frequence)
        {
            int ret = 0;
            int missedInFrequency = 0;
            string[] allLines = File.ReadAllLines(pvfile);
            foreach (var line in allLines)
            {
                string[] s = line.Split('\t');
                if(s.Length<2) continue;

                string q = this.CleanQuery(s[1]);
                if (frequence.ContainsKey(q)) 
                    ret += frequence[q];
                else
                {
                    //Console.WriteLine("Query {0} not found in frequecy file.", q);
                    missedInFrequency++;
                }
            }

            return ret;
        }

        private int CalculateCoverage(string imprefile, Dictionary<string, int> frequence, out int mlcover, out int mlcount, out int adscount)
        {
            int coverage = 0;
            mlcover = 0;
            mlcount = 0;
            adscount = 0;
            int missedInFrequency = 0;
            string[] allLines = File.ReadAllLines(imprefile);
            var rguid = new HashSet<string>();
            int[] matchtype = new int[6];

            foreach (var line in allLines)
            {
                string[] s = line.Split('\t');
                if (s.Length < 11) continue;

                string q = this.CleanQuery(s[1]);

                if (frequence.ContainsKey(q))
                {
                    if (!rguid.Contains(s[0]))
                    {
                        coverage += frequence[q];
                        if (s[4].StartsWith("ML")) mlcover += frequence[q];
                        rguid.Add(s[0]);
                    }
                    if (s[4].StartsWith("ML")) mlcount += frequence[q];
                    adscount += frequence[q];

                    int matchtypeid;
                    if (int.TryParse(s[10], out matchtypeid))
                    {
                        if (matchtypeid > 4) Console.WriteLine("{0} matchtypeid={1}", q, matchtypeid);
                        else matchtype[matchtypeid] += frequence[q];
                    }
                    else
                    {
                        //Console.WriteLine("Query {0} not found in frequecy file.", q);
                        missedInFrequency++;
                    }
                }
            }

            int count = 0;
            for (int i = 1; i <= 4; i++) count += matchtype[i];

            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine("MatchType {0} count={1} {2:0.00%}",i,matchtype[i], (double)matchtype[i]/count);
            }
            return coverage;
        }

        private string CleanQuery(string q)
        {
            if (q.Length > 128) return q;
            string cleaned = q.Trim();
            cleaned = cleaned.ToLower();
            cleaned = cleaned.Replace("c++", "c__");

            cleaned = cleaned.Replace(' ', '+');
            cleaned = cleaned.Replace("++", "+").Replace("+++", "+").Replace("++++", "+").Replace("+++++", "+");
            cleaned = cleaned.Replace(' ', '+');
            cleaned = cleaned.Replace("c__", "c++");

            return cleaned;
        }

        private string CleanQuery2(string q)
        {
            if (q.Length > 128) return q;
            string cleaned = q.Trim();
            cleaned = cleaned.ToLower();
            cleaned = cleaned.Replace("c++", "c__");

            cleaned = cleaned.Replace('+', ' ').Trim();

            cleaned = cleaned.Replace(' ', '+');

            cleaned = cleaned.Replace("++", "+").Replace("+++", "+").Replace("++++", "+").Replace("+++++", "+");

            cleaned = cleaned.Replace("c__", "c++");

            if (cleaned.Length>1 && cleaned[cleaned.Length - 1] == ',') cleaned = cleaned.Substring(0, cleaned.Length - 1);

            return cleaned;
        }
    }
}
