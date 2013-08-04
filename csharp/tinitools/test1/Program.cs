using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    using System.Web;

    class Program
    {
        static void Main(string[] args)
        {
            ParseExchangeRate();
            //TempTest();
            //Practice p = new Practice();
            //p.Exam_maxSubArray();
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

        public static void TempTest()
        {
            string q = "helly world";
            //q = q.Replace(" ", "%20");
            string s = HttpUtility.UrlEncode(Encoding.GetEncoding("UTF-8").GetBytes(q));
            s = s.Replace("+", "%20");
            Console.Write(s);
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

        public static void ParseExchangeRate()
        {
            const string filePath = @"D:\work\DEE2E\DumpExchangeRate.DAT";

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = 0;
                //omAlias::ExchangeRate exchangeRate = null;
                byte[] currencyId = new byte[2];
                byte[] rate = new byte[4];
                byte[] nameLengthBuffer = new byte[2];
                byte[] nameBuffer = new byte[50];
                UInt16 nameLength = 0;
                string currencyName = string.Empty;

                while (fs.Read(currencyId, 0, 2) > 0)
                {
                    
                    try
                    {
                        string sb = "";
                        // store currency ID
                        //exchangeRate.CurrencyId = (Currency)Enum.ToObject(typeof(Currency), BitConverter.ToInt16(currencyId, 0));

                        // get and store rate
                        fs.Read(rate, 0, 4);
                        ulong ExchangeRateToUSD = (ulong)BitConverter.ToInt32(rate, 0);

                        // get and store the currency name string
                        fs.Read(nameLengthBuffer, 0, 2);
                        nameLength = BitConverter.ToUInt16(nameLengthBuffer, 0);
                        bytesRead = fs.Read(nameBuffer, 0, nameLength);
                        string CurrencyName = UnicodeEncoding.Unicode.GetString(nameBuffer, 0, nameLength);

                        // here we make sure we are reading what we expect.  If the file is too short (incomplete rate entries)
                        // every file.Read() will return 0.  We only have to check the read once, so we check it after all the reads.
                        if (bytesRead < nameLength)
                        {
                            throw (new IOException("Unexpected end of file"));
                        }

                        sb = sb + "currentyID=" + BitConverter.ToInt16(currencyId, 0) + " ExchangeRateToUSD=" + ExchangeRateToUSD
                             + " CurrencyName=" + CurrencyName;
                        Console.WriteLine(sb);
                    }
                    catch (IOException e)
                    {
                        throw (e);
                    }
                }

                Console.Read();
            }
        }
    }

     
}
