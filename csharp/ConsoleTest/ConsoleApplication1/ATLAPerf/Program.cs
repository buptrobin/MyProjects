using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ATLAPerf
{
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Web;

    public class Program
    {
        private static string[] _queryList;
        private static Random _rand; 

        public static void Main(string[] args)
        {
            string s = "A's tickets";

            Console.WriteLine(HttpUtility.UrlEncode(s));
            Console.WriteLine(HttpUtility.JavaScriptStringEncode(s));
                /*
            int n = 1;
            if (args.Length > 0)
            {
                n = int.Parse(args[0]);
            }

            Console.WriteLine(n);

            for (var i = 0; i < n; i++)
            {
                runOnce();
            }
                */
            Console.ReadKey();
        }

        private static void runOnce()
        {
            _rand = new Random((int)(System.DateTime.Now.Ticks));
            LoadQuery();
            
            WebRequest wr = null;
            string urlTemplate =
                @"http://ch1.cache.binginternal.com:84/answerstla.aspx?q={0}[augments:""%5bAdService AdUnitID=\%22933\%22 PropertyID=\%223682\%22 RAISEnabled=\%221\%22 ReqVersion=\%222\%22 AdLanguage=\%22en\%22 WebSiteCountry=\%22US\%22 DbgString=\%22<de><mode>logs</mode></de>\%22%5d%5bATLA Timeout=\%225000\%22%5d%5bWebAnswer ResponseFormat=\%22kif\%22%5d""|variantconstraint:""mkt:en-US%26brand:kiev%26workflow:BingFirstPageResultsForAdsPlusVMoney%26flt6:6303%26debuginfo:1""|userip:%2271.212.115.102%22|form:%22MONITR%22|useragent:%22Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)%22|workflow:%22BingFirstPageResultsForAdsPlusVMoney%22|adult:demote|flag:debug]";

            TraceSwitch ts = new TraceSwitch("System.Net", "description");
            string query = NextQuery();
            try
            {
                Stopwatch sw = new Stopwatch();

                string url = string.Format(urlTemplate, query);
//                Console.WriteLine(url);
                sw.Start();
                
                wr = (HttpWebRequest)WebRequest.Create(url);
                //wr.Proxy = WebRequest.DefaultWebProxy;
                var webResponse = wr.GetResponse();
                sw.Stop();
                string machineName = string.Empty;
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var responseContent = reader.ReadToEnd();
                    int p1 = responseContent.IndexOf("AnswersTLA");
                    int p2 = responseContent.IndexOf("machine", p1);
                    int p3 = responseContent.IndexOf("machineFunction", p2);
                    if (p3 > p2 && p2 > p1 && p1 > 0)
                    {
                        machineName = responseContent.Substring(p2 + 9, p3 - 3 - p2 - 9);
                    }
                }

                webResponse.Close();                
                
                Console.WriteLine("{0} {1} Time={2}", query, machineName, sw.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Trace.WriteIf(ts.TraceVerbose, "System.Net");
        }

        private static void LoadQuery()
        {
            var querylist = new List<string>();
            foreach (var q in File.ReadAllLines(@"querys.txt"))
            {
                string qstr = q.Trim();
                if (!string.IsNullOrEmpty(qstr))
                {
                    querylist.Add(qstr);
                }
            }

            _queryList = querylist.ToArray();
        }

        private static string NextQuery()
        {
            return _queryList[_rand.Next(_queryList.Length - 1)].Replace(" ", "+");
        }

        public static void LogActionPerf(string tag, string actionName, Action action)
        {
            if (action != null)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    action();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    sw.Stop();
                }

                Console.WriteLine("{0}:{1} Time={2}", tag, actionName, sw.ElapsedMilliseconds);
            }
        }
    }

    public static class WebRequestFactory
    {
        private static Regex CockpitHostNameRegex = new Regex("cockpit.autopilot.[^\\.]+.search.msn.com.*", RegexOptions.Compiled);

        public static HttpWebRequest CreateHttpWebRequest(string url)
        {
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // only add extra authentication info for cockpit request, don't update none-cockpit requests.
            if (CockpitHostNameRegex.IsMatch(uri.Host))
            {
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                request.CookieContainer = new CookieContainer();
            }

            return request;
        }

        public static HttpWebRequest CreateHttpWebRequest2(string url)
        {
            var uri = new Uri(url);
            WebRequest.DefaultWebProxy = null;
            var request = (HttpWebRequest)WebRequest.Create(uri);
            
            

            // only add extra authentication info for cockpit request, don't update none-cockpit requests.
            if (CockpitHostNameRegex.IsMatch(uri.Host))
            {
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
                request.CookieContainer = new CookieContainer();
            }

            return request;
        }

    }
}
