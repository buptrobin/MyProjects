using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Globalization;
using ConsoleApplication1.UserManager;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ConsoleApplication1
{
    public enum Color
    {
        red=0,
        blue=1
    };
    public enum AdPosition_1 : byte
    {
        PositionMainline = 1,
        PositionSidebar = 2,
        PositionBottom = 3,
    }
    class Program

    {
        static void Main(string[] args)
        {
            //AdPosition_1 a = AdPosition_1.PositionMainline;


            long v = 123456789;
            Console.WriteLine(v.ToString("N0"));

//            List<string> aa = new List<string>();
//            Console.WriteLine(aa.Count);
            Console.ReadKey();
            //Directory.CreateDirectory(@"\\apc-server-109\d$\1\2\3");
            /*
            if (Directory.Exists(@"\\apc-server-109\d$\1"))
            {
                Directory.Delete(@"\\apc-server-109\d$\1", true);
            }
            */
            //File.Copy(@"D:\1.txt", @"\\apc-server-109\d$\1.txt",true);
            //enumtry();
            //GenerateAdsResponse();
        }

        public static void GenerateAdsResponse()
        {
            var res = new XElement("results",
                                           new XElement("itm",
                                                        new XElement("KifSchema", "AdService.Ads[1.0]"),
                                                        new XElement("AdResults",
                                                                     new XAttribute("Kif.Type", "typedList"),
                                                                     new XAttribute("Kif.ElementSchema", "AdService.Ad[1.1]"),
                                                                     new XElement("Kif.Value",
                                                                         new XElement("item",
                                                                                      //new XElement("Kif.Schema", "AdService.Ad[1.6]"),
                                                                                      new XElement("Title","Edmunds.com For Cars"),
                                                                                      new XElement("Description","Free Price Quotes from Edmunds.com. Find a dealer in Atlanta"),
                                                                                      new XElement("DisplayUrl","www.Edmunds.com"),
                                                                                      new XElement("TargetUrl","http://0.r.msn.com/?ld=4vnZBrxKZu_KnBDZPg0B0SO-JuAw_W6CU10wb5dj_T10Ous6wumZuPYhHunmlEkAouYNbgeDu5etWWNIaAv6-pq1qnTAEN7gAGWlmltcU5qhBFmm4vasQoGiPMAkh19MMaTCMF0af_5fI0y2aHLGeQr7k3Fsl4fpCtIYouFrXD7sKQApgHu--qLPoEUY8flsqkTVVYsrqiPZvUzN4r028szJGnpn0yLNy19q7mUXZQL4lHGQVrXTRbIc_raBU6EaKDg8RCTnekZhEkHXTt2H9o5nV-NTsQJmNzPX6Dx7310uusHYYGX82Do8zPOSjgEvjOZkxCv72pidNj"),
                                                                                      new XElement("ImpressionId","427651482"),
                                                                                      new XElement("ListingId", "4832753381"),
                                                                                      new XElement("ClickProbability", 309),
                                                                                      new XElement("ImpressionRevenue", 14),
                                                                                      new XElement("Cpc", 466),
                                                                                      new XElement("RankScore", "201295"),
                                                                                      new XElement("IsRebate", "false"),
                                                                                      new XElement("IsAdult", "false"),
                                                                                      new XElement("IsImageAdMatched","false"),
                                                                                      new XElement("RichAdType", "1"),
                                                                                      new XElement("Position", 1)
                                                                             )
                                                                        )
                                                                     ),
                                                                     new XElement("HighVisibilityIndex", "0"),
                                                                     new XElement("LowVisibilityIndex", "3"),
                                                                     new XElement("Category1", "2"),
                                                                     new XElement("Category2", "7"),
                                                                     new XElement("MainlineReserve", "23100"),
                                                                     new XElement("RankScoreReserve", "3000"),
                                                                     new XElement("Latency", "49"),
                                                                     new XElement("IsFreshSpellerAnswer", false),
                                                                     new XElement("IsTruncatedTQS", false),
                                                                     new XElement("IsPassBestAltQuery", false),
                                                                     new XElement("EntryPoint", ""),
                                                                     new XElement("CurRequestTime", ""),
                                                                     new XElement("IsAlreadyHighlighted", false),
                                                                     new XElement("IsDaqFeatureTesting", false),
                                                                     new XElement("ProviderId", "Overture"),
                                                                     new XElement("IsExpectedPosition", false),
                                                                     new XElement("IsFoundPosition", false),
                                                                     new XElement("LogUrl", ""),
                                                                     new XElement("ExperimentId", 0),
                                                                     new XElement("TraceId","175a6f3885c2444798224091451375cf")
                                               )

                );
            Console.WriteLine(res);
            Console.ReadKey();
            
        }

        public static void GetAds()
        {
            string content =
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<AdResults HResult=""0"" WebCacheLatency=""0"" QRDTCacheStatus=""0"" considered=""1"" abId=""3126"" MainlineReserve=""22783.11"" RankScoreReserve=""2200""><LogURL>http://c.msn-int.com/c.gif?rg=4023c58866a54f4a86aaf9b9d304ac3e&amp;ls=SM</LogURL><AdsMetaData>0.0:22783.11</AdsMetaData><Result rank=""1"" id=""343531140"" listing=""10054159862"" type=""PFP"" Bid=""15"" pClick=""1573"" eCPI=""2"" CPC=""15"" CPCSB=""5"" CPCML=""15"" RankScore=""23595"" position=""mainline"" MatchType=""exact"" ORV=""0.750836""><Title>adf adfa df</Title><Description>adf adfa adf adf adf</Description><DisplayURL>www.test.com</DisplayURL><DestinationURL>url=www.test.com</DestinationURL><AdLinkURL>http://0.redir.si.adcenter.msn.com/?ld=4vzvidqaMB28E71BZ8W7Y_qHL9Qq9WzghHERJ06kY4xj9T5gJoCtN4E3kpbvrKcs7znrLrJMNkV707kua7nfXuwNhvAZxYUemk6-Ur5Pg5_ooebTw_lc7WUuP5vaFYs4zRxHStiwRkKQuVOCh3ccFEiIU-5JP-h7Zv_DiCbnLDzUCphl6RA746OTDrXRM4G4Wfg8RCTjyAOetAXaq6OOTdygDuDQoQJmNzPeDhU4APx5rPxb4VmyxI4_wnHZKCyHB3mpRrRbf4T_Md</AdLinkURL><ClearLinkURL>http://0.redir.si.adcenter.msn.com/?ld=4v&amp;url=www.test.com&amp;rg=4023c58866a54f4a86aaf9b9d304ac3e&amp;imp=1324370719&amp;adid=343531140&amp;dcid=1&amp;listid=10054159862&amp;convdomid=0&amp;cpc=15&amp;cid=54574374&amp;advid=765193&amp;src=M&amp;adunitid=933</ClearLinkURL><Icon></Icon><Image></Image><BiddedKeywords>九陽豆漿機</BiddedKeywords></Result></AdResults>";
            Dictionary<string,string> ad = new Dictionary<string, string>();
            XElement doc = XElement.Parse(content);

            foreach (var p in doc.Descendants("Result"))
            {
                
/*
                ad["rank"] = p.Attribute("rank").ToString();
                ad["id"] = p.Attribute("id").ToString();
                ad["listing"] = p.Attribute("listing").ToString();
                ad["type"] = p.Attribute("type").ToString();
                ad["Bid"] = p.Attribute("Bid").ToString();
                ad["pClick"] = p.Attribute("pClick").ToString();
                ad["eCPI"] = p.Attribute("eCPI").ToString();
                ad["CPC"] = p.Attribute("CPC").ToString();
                ad["CPCSB"] = p.Attribute("CPCSB").ToString();
                ad["CPCML"] = p.Attribute("CPCML").ToString();
                ad["RankScore"] = p.Attribute("RankScore").ToString();
                ad["position"] = p.Attribute("position").ToString();
                ad["exact"] = p.Attribute("exact").ToString();
                ad["ORV"] = p.Attribute("ORV").ToString();
                ad["Title"] = p.Attribute("Title").ToString();
*/
                foreach (var att in p.Attributes())
                {
                    ad[att.Name.ToString()] = att.Value.ToString();

                }
                foreach (var e in p.Elements())
                {
                    ad[e.Name.ToString()] = e.Value.ToString();
                }                             
            }
            var results = from p in doc.Descendants("Result")
                          select new
                                     {
                                         ORV = p.Attribute("ORV")
                                     };
        }


        public static void WCFTest()
        {
            WSHttpBinding binding = new WSHttpBinding();
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReliableSession.Enabled = false;


            binding.ReaderQuotas = new XmlDictionaryReaderQuotas();

            binding.ReaderQuotas.MaxStringContentLength = 2147483647;

            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            //binding.Security.Mode = SecurityMode.None;


            //EndpointAddress address = new EndpointAddress("http://apc-rowa-v1/WinService/UserManager");
            string serverUrl = "http://apc-rowa-v1/WinService/UserManager";
            SpnEndpointIdentity id = new SpnEndpointIdentity("");
            EndpointAddress address = new EndpointAddress(new Uri(serverUrl), id, new AddressHeaderCollection());
            UserClient uc = new UserManager.UserClient(binding, address);

            User[] users = uc.GetList();
            foreach (User u in users)
            {
                Console.WriteLine(u.username);
            }
            Console.ReadKey();
        }

        public static void GetMemoryExperiment()
        {
            string url = @"http://apc-server-109:9340/ACTIVEEXPERIMENTS";            

            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream());

            XElement doc = XElement.Load(sr);
            var experiments = from p in doc.Descendants("Experiment")
                              select new
                              {
                                  id = p.Attribute("Id").Value,
                                  name = p.Attribute("Name").Value,
                                  source = p.Attribute("Source").Value,
                                  owner = p.Attribute("Owner").Value,
                                  state = p.Attribute("State").Value
                              };

            foreach (var experiment in experiments)
            {
                Console.WriteLine("id={0}, name={1}, source={2}, owner={3}, state={4}", experiment.id, experiment.name, experiment.source, experiment.owner, experiment.state);
            }
            return;

        }

        public static void enumtry(){
            Console.WriteLine((int)Color.blue);

            Console.ReadKey();
        }
    }
}
