using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    class SmallOnes
    {
        //Generate Parentheses
        public void GenerateParentheses(int n)
        {
            if (n <= 0) return;
            GP("", n, 0, 0);
        }
        public void GP(string s, int n, int leftcount, int rightcount)
        {
            if (n == leftcount && n == rightcount)
            {
                Console.WriteLine(s);
                return;
            }
            if (leftcount < rightcount || leftcount > n) return;

            GP(s + "(", n, leftcount + 1, rightcount);
            GP(s + ")", n, leftcount, rightcount + 1);
        }
        public static void TestGenerateParentheses()
        {
            Console.WriteLine("n=5");
            SmallOnes so = new SmallOnes();
            so.GenerateParentheses(5);
        }

    }
}
