using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace AdsClient
{
    public partial class Form1 : Form
    {
        static string key = "bing_soso_ad";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //textBox1.Text =
            //    @"http://adxml.soso.com/s?cl=3&word=%cf%ca%bb%a8&wd=%cf%ca%bb%a8&pn=0&rn=10&tn=s500916_x1_a4&tt=browser&ip=60.247.79.132&bcode=-1&uid=t2NSpEIpgqxP4k7db4Eh3A&ref=http%3a%2f%2flocalhost%3a81%2fsearch%3fq%3d%25E8%25A5%25BF%25E6%259C%258D%26qs%3dn%26form%3dQBRE%26pq%3d%25E8%25A5%25BF%25E6%259C%258D%26sc%3d8-2%26sp%3d-1%26sk%3d&agt=Mozilla%2f5.0+(compatible%3b+MSIE+9.0%3b+Windows+NT+6.1%3b+WOW64%3b+Trident%2f5.0)";
            string url = textBox1.Text;
            
            //MessageBox.Show(url);

            //string rep = getAdsResponseByURL(url);
            //textBox2.Lines = new string[1];            ;
            
            string response = getAdsResponseByURL(url);
            
            textBox2.Text = response;
        }

        private string getAdsResponseByURL(string url)
        {
            try
            {
                var wc = new WebClient();
                Byte[] pageData = wc.DownloadData(url);                
                string content = AdsEncoding.codeIt(pageData, key);
                return content;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return e.ToString();
            }
            /*
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream mStream = res.GetResponseStream();
            byte[] mybyte = readFullStream(res.GetResponseStream());
            string str = Encoding.GetEncoding("GBK").GetString(mybyte);

            string key = "bing_soso_ad";
            string content = AdsEncoding.codeIt(mybyte, key);
            mStream.Close();
            res.Close();
            */
            //string rep = content;
            
            //string rep = wc.DownloadString(url);
            /*
            Stream resStream = wc.OpenRead(url);
            if (resStream == null) return "";

            var sr = new StreamReader(resStream);

            string rep = sr.ReadToEnd();

            sr.Close();
            resStream.Close();
            */

            //return rep;
        }

        public static byte[] readFullStream(Stream s)
        {
            byte[] buf = new byte[32768];
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int count = s.Read(buf, 0, buf.Length);
                    if (count <= 0)
                    {
                        break;
                    }
                    ms.Write(buf, 0, count);
                }
                result = ms.ToArray();
            }
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = @"C:\work\tmp\inspus.xml";

            XmlReader reader = null;

            var ret = new StringBuilder();
            try
            {
                var settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;

                reader = XmlReader.Create(path, settings);

                while (reader.Read())
                {
                    XmlNodeType nodeType = reader.NodeType;
                    if (nodeType != XmlNodeType.Element)
                    {
                        if (nodeType == XmlNodeType.EndElement)
                        {
                            if (reader.Name.ToLower() == "result")
                            {
                                ret.Append("-------\n");
                            }
                        }
                    }
                    else
                    {
                        if (reader.Name.ToLower() == "matchtype")
                        {
                            reader.Read();
                            
                            byte[] buf = Encoding.Default.GetBytes(reader.Value);
                            ret.Append("MATCHTYPE=" + AdsEncoding.codeIt(buf, key));
                        }

                        if (reader.Name.ToLower() == "bidprice")
                        {
                            reader.Read();
                            byte[] buf = Encoding.Default.GetBytes(reader.Value);
                            ret.Append("BIDPRICE=" + AdsEncoding.codeIt(buf, key));
                        }
                    }
                }
                

                textBox2.Text = ret.ToString();

            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

    }
}
