using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapeGet
{
    using System.IO;
    using System.Net;
    using System.Threading;

    class ScrapeGet
    {
        private static string serverport = @"http://bn1.ah.xap.playmsn.com:84/answerstla.aspx?q=";
        //private static string requestTemplate = @"[augments:%22%5bAdService%20AdUnitID=\%22933\%22%20PropertyID=\%223682\%22%20SupportedExtensionsMask=\%22512\%22%20SourceSchema=\%22AdService.Ads(1.9)\%22%20RAISEnabled=\%221\%22%20RAISTypes=\%2231\%22%20Referer=\%22cnrnrtuning3.com\%22%20AbTestNum=\%224845\%22%20ReqVersion=\%222\%22%20AdLanguage=\%22zh\%22%20WebSiteCountry=\%22CN\%22%20AdCenterEnabled=\%221\%22%5d%5bATLA%20Timeout=\%225000\%22%5d%22|variantconstraint:%22mkt:zh-CN%26truemkt:zh-CN%26brand:kiev%26workflow:BingFirstPageResults%26traffictype:Test_Regression%22|form:%22QBLH%22|workflow:%22BingFirstPageResults%22]";
        //private static string requestTemplate = @"[augments:%22%5bAdService%20AdUnitID=\%22933\%22%20PropertyID=\%223682\%22%20SupportedExtensionsMask=\%22512\%22%20SourceSchema=\%22AdService.Ads(1.9)\%22%20RAISEnabled=\%221\%22%20RAISTypes=\%2231\%22%20Referer=\%22cnrnrtuning4.com\%22%20AbTestNum=\%224845\%22%20ReqVersion=\%222\%22%20AdLanguage=\%22zh\%22%20WebSiteCountry=\%22CN\%22%20ABConfigOverrides=\%22<AbTestOverrideData%20AbTestId='0'%20AbTestVersion='0'><AbTestParam%20Key='326'%20Type='3'>1</AbTestParam><AbTestParam%20Key='328'%20Type='2'>100</AbTestParam><AbTestParam Key='233'%20Type='5'>1</AbTestParam></AbTestOverrideData>\%22%20AdCenterEnabled=\%221\%22%5d%5bATLA%20Timeout=\%225000\%22%5d%22|variantconstraint:%22mkt:zh-CN%26truemkt:zh-CN%26brand:kiev%26workflow:BingFirstPageResults%26traffictype:Test_Regression%22|form:%22QBLH%22|workflow:%22BingFirstPageResults%22]";
        private static string requestTemplate = @"[augments:%22%5bAdService%20AdUnitID=\%22933\%22%20PropertyID=\%223682\%22%20SupportedExtensionsMask=\%22512\%22%20SourceSchema=\%22AdService.Ads(1.9)\%22%20RAISEnabled=\%221\%22%20RAISTypes=\%2231\%22%20Referer=\%22cnrnrtuning7.com\%22%20AbTestNum=\%224845\%22%20ReqVersion=\%222\%22%20AdLanguage=\%22zh\%22%20WebSiteCountry=\%22CN\%22%20ABConfigOverrides=\%22<AbTestOverrideData%20AbTestId='0'%20AbTestVersion='0'><AbTestParam%20Key='267'%20Type='5'>0.0045</AbTestParam><AbTestParam%20Key='270'%20Type='5'>0.0045</AbTestParam><AbTestParam%20Key='326'%20Type='3'>1</AbTestParam><AbTestParam%20Key='328'%20Type='2'>100</AbTestParam><AbTestParam%20Key='233'%20Type='5'>1</AbTestParam><AbTestParam%20Key='217'%20Type='5'>0.22</AbTestParam><AbTestParam%20Key='218'%20Type='5'>0.2</AbTestParam><AbTestParam%20Key='264'%20Type='5'>2200</AbTestParam><AbTestParam%20Key='399'%20Type='5'>0.1</AbTestParam></AbTestOverrideData>\%22%20AdCenterEnabled=\%221\%22%5d%5bATLA%20Timeout=\%225000\%22%5d%22|variantconstraint:%22mkt:zh-CN%26truemkt:zh-CN%26brand:kiev%26workflow:BingFirstPageResults%26traffictype:Test_Regression%22|form:%22QBLH%22|workflow:%22BingFirstPageResults%22]";

        private static string DEFAULT_INPUT = @"D:\work\msm\scrapeget\bak\zh_cn_query_distinct000.txt";//".\scrape_inpput.txt";

        private static string DEFAULT_OUTPUT = @".\scrape_output.txt";

        private static WebClient wc = new WebClient();

        static void Main(string[] args)
        {
            string inputfile = DEFAULT_INPUT;
            string outputfile = DEFAULT_OUTPUT;
            if (args.Length > 0)
            {
                inputfile = @".\zh_cn_dump_query00" + args[0] + ".txt";
                outputfile = @"scrape_out_" + args[0] + ".txt";
            }
            var sg = new ScrapeGet();
            sg.DoScrape(inputfile, outputfile, requestTemplate);
            Console.Read();
        }

        public void DoScrape(string inputfile, string outputfile, string reqTemplate)
        {
            //var sr = new StreamReader(inputfile);
            var sw = new StreamWriter(outputfile);

            long count = 1;
            string[] keywords = File.ReadAllLines(inputfile);
            foreach (var keyword in keywords)
            {
                //Console.WriteLine("Query:" + keyword + "\tCount:" + count);
                string url = serverport + keyword + requestTemplate;
                //Console.WriteLine(url);
                string content = this.HitTarget(url);

                string rguid = "NoRGuid";
                int rguidbegin = content.IndexOf("RGuid");
                if (rguidbegin > 0)
                {
                    int rguidend = content.IndexOf("\"", rguidbegin);
                    if (rguidend > 0) rguid = content.Substring(rguidbegin - 10, rguidend - rguidbegin);
                }

                string server = "NoServer";
                int servertag = content.IndexOf("<c_AnswerResponseProcessingServer");
                if (servertag > 0)
                {
                    int serverbegin = content.IndexOf("BN1SCH", servertag);
                    if (serverbegin > 0)
                    {
                        int serverend = content.IndexOf("</c_AnswerResponseProcessingServer>", servertag);
                        if (serverend > 0)
                            server = content.Substring(serverbegin, serverend - serverbegin);
                    }
                }
                string s = string.Format("Query{2}\tCount{3}\tRGuid:{0}\tServer:{1}", rguid, server, keyword, count);
                Console.WriteLine(s);
                sw.WriteLine(s);

                count++;
            }
            sw.Close();
        }

        public string HitTarget(string url)
        {
            string result = "";
            try
            {
                HttpWebRequest hwr = null;
                HttpWebResponse response = null;

                int retry = 0;
                while (true)
                {
                    try
                    {
                        hwr = (HttpWebRequest)WebRequest.Create(url);
                        response = (HttpWebResponse)hwr.GetResponse();
                    }
                    catch (System.Net.WebException e)
                    {
                        Console.WriteLine(e.ToString());
                        if (e.ToString().IndexOf("503") > 0)
                        {
                            Console.WriteLine("retry " + retry);
                            Thread.Sleep(2000);
                            retry++;
                            if (retry < 5) continue;
                        }
                    }
                    break;
                }

                if (response == null) return "";

                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null) return "";
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return result;
        }
    }
}
