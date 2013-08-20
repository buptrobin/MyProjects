using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    class Chapt5
    {
        //5.2
        //123.321 1.1 0.3 33 0.322 .222 111. 
        //range
        public string BinaryDouble(string s)
        {
            if (string.IsNullOrEmpty(s)) return;
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
                sDecimal = "";
            }

            int intPart = 0;
            double doublePart = 0;
            string sOut = "";
            if (!noint)
            {
                if (!int.TryParse(sInt, out intPart)) intPart = 0;
                while (intPart > 0)
                {
                    sOut = intPart % 2 + sOut;
                    intPart /= 2;
                }
            }

            if (!nodouble)
            {
                sOut = sOut + ".";
                if (!Double.TryParse(sDecimal, out doublePart)) doublePart = 0;
                else
                {
                    while (doublePart < 1)
                    {
                        int k = doublePart * 2 > 0 ? 1 : 0;
                        sOut = sOut + k;
                        doublePart *= 2;
                        if (doublePart >= 1) doublePart -= 1;
                    }
                }
            }
            return sOut;
        }
    }
}
