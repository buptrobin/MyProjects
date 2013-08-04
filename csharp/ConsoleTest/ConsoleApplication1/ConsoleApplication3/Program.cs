using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication3
{
    using System.Diagnostics;
    using System.IO;
    using System.Text.RegularExpressions;

    class Program
    {
        static void Main(string[] args)
        {
            /*
            string path = @"D:\tmp\1.txt";
            string output = @"D:\tmp\2.txt";
            string pattern = @"[^\u0009\u000a\u000d\u0020-\ud7ff\ue000-\ufffd\u10000-\u10ffff]";
            //@"^(\x9|\xa|\xd|[\x20-\xd7ff]|[\xe000-\xfffd]|[\x10000-\x10ffff])";\x9|\xa|\xd|[\xu0020-\ud7ff]|[\ue000-\ufffd]|
            string s = File.ReadAllText(path);
            Stopwatch sw = new Stopwatch();
            var regex = new Regex(pattern, RegexOptions.Compiled);
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                string sout = regex.Replace(s, String.Empty);
//                if (regex.IsMatch(s))
//                {
//                    string sout = regex.Replace(s, String.Empty);
//                    //File.WriteAllText(output, sout);
//                    //Console.WriteLine("match!");
//                    //File.WriteAllText(sout, tmpContents, Encoding.UTF8);
//                }
                //string sout = stripNonValidXMLCharacters(s, "1.0");
            }
            
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds/100);
            */

            computeMD5("flowers");
            

            Console.ReadKey();
        }

        private static string computeMD5(string input)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string output = s.ToString();
            Console.WriteLine("output:"+output);
            return output;
        }

        private static string stripNonValidXMLCharacters(string content, string XMLVersion)
        {
            var ret = new StringBuilder();

            if (string.IsNullOrEmpty(content))
            {
                return content;
            }

            if (XMLVersion.Equals("1.0"))
            {
                //#x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]

                foreach (var c in content)
                {
                    var current = (int)c;
                    if ((current == 0x9) || (current == 0xA) || (current == 0xD)
                        || ((current >= 0x20) && (current <= 0xD7FF)) || ((current >= 0xE000) && (current <= 0xFFFD))
                        || ((current >= 0x10000) && (current <= 0x10FFFF)))
                    {
                        ret.Append((char)current);
                    }
                }
            }
            else if (XMLVersion.Equals("1.1"))
            {
                foreach (var c in content)
                {
                    var current = (int)c;
                    if (((current >= 0x1) && (current <= 0xD7FF)) || ((current >= 0xE000) && (current <= 0xFFFD))
                        || ((current >= 0x10000) && (current <= 0x10FFFF)))
                    {
                        ret.Append((char)current);
                    }
                }
            }
            else
            {
                throw new Exception("Error: Invalid XML Version!");
            }

            return ret.ToString();
        }
    }
}
