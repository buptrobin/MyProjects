using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    class Chapt5 : ExamBase
    {
        //5.2
        //123.321 1.1 0.3 33 0.322 .222 111. 
        //range
        public static string BinaryDouble(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            int pointIndex = s.IndexOf('.');
            bool noint = true;
            bool nodouble = true;
            string sInt = "";
            string sDecimal = "";
            if (pointIndex > 0)
            {
                sInt = s.Substring(0, pointIndex);
                sDecimal = s.Substring(pointIndex, s.Length - pointIndex);
                noint = false;
            }
            if (pointIndex > 0 && pointIndex < s.Length - 1) nodouble = false;

            if (pointIndex < 0)
            {
                sInt = s;
                sDecimal = "0";
                noint = false;
            }

            int intPart = 0;
            double doublePart = 0;
            string sOut = "";
            if (!noint)
            {
                if (!int.TryParse(sInt, out intPart)) intPart = 0;
                if (intPart == 0) sOut = "0";
                while (intPart > 0)
                {
                    sOut = intPart % 2 + sOut;
                    intPart /= 2;
                }
            }
            else sOut = "0";

            if (!nodouble)
            {
                sOut = sOut + ".";
                if (!Double.TryParse(sDecimal, out doublePart)) doublePart = 0;
                else
                {
                    while (doublePart > 0.000000000000001)
                    {
                        int k = doublePart * 2 >= 1 ? 1 : 0;
                        sOut = sOut + k;
                        doublePart *= 2;
                        if (doublePart >= 1) doublePart -= 1;
                    }
                }
            }
            return sOut;
        }

        public static void Test5_2()
        {
            AssertEqual("1111011", BinaryDouble("123"));
            AssertEqual("0.1", BinaryDouble("0.5"));
        }

        //5.7
        public static int FindMissingNumber(int n, int x)
        {
            int a = 0;
            for (int i = 0; i <= n; i++) a ^= i;

            int b = 0;
            for (int i = 0; i <= n; i++) 
                if(i!=x) b ^= i;

            int ret = a ^ b;
            Console.WriteLine("a={0} b={1} ret={2}\n", a,b,ret);
            return ret;
        }
        public static void Test5_7()
        {
            AssertEqual(3, FindMissingNumber(10,3));
            AssertEqual(0, FindMissingNumber(10, 0));
            AssertEqual(1, FindMissingNumber(11, 1));
        }

    }
}
