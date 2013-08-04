using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceLog
{
    using System.Net;

    class Program
    {
        static void Main(string[] args)
        {
            //get the latest log file;
            WebClient wc = new WebClient();
            string url =
                @"http://cockpit.autopilot.ch1d.osdinfra.net:81/files?cmd=get&path=%5c%5cCH1SCH060070627%5cdrived%5cData%5cLogs%5clocal";
            string s = wc.DownloadString(url);
            //wc.DownloadFile(url, @"D:\work\tmp\CH1SCH060070627_list.txt");
            string[] lines = (s.Split('\n')).Where(p => p.StartsWith("ADQPOperations_")).ToArray();

            int max = 0;
            string max_filename = string.Empty;
            foreach (var line in lines)
            {
                int p1 = 15;
                int p2 = line.IndexOf(".log");
                string s_number = line.Substring(p1, p2 - p1);
                int number = int.Parse(s_number);
                if(max < number)
                {
                    max = number;
                    max_filename = line.Substring(0, p2 + 4);
                }
            } 

            //get the log
            string log_url =
                string.Format(
                    "http://cockpit.autopilot.ch1d.osdinfra.net:81/files?cmd=get&path=%5c%5cCH1SCH060070627%5cdrived%5cData%5cLogs%5clocal%5c{0}",
                    max_filename);
            wc.DownloadFile(log_url, @"D:\work\tmp\"+max_filename);
            Console.WriteLine(max_filename);
            Console.ReadKey();
        }
    }
}
