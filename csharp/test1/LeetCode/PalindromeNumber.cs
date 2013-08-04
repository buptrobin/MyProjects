using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    class PalindromeNumber
    {

        public static void Test()
        {
            TestOne(-2147483647, false);
            TestOne(-2147447412, false);
            TestOne(-101, false);
            TestOne(-10, false);
            TestOne(-1, false);
            TestOne(0, true);
            TestOne(1, true);
            TestOne(3, true);
            TestOne(5, true);
            TestOne(10, false);
            TestOne(11, true);
            TestOne(44, true);
            TestOne(121, true);
            TestOne(123, false);
            TestOne(213, false);
            TestOne(313, true);
            TestOne(1001, true);
            TestOne(1881, true);
            TestOne(101, true);
            TestOne(500, false);
            TestOne(9999, true);
            TestOne(88888, true);
            TestOne(789656987, true);
            TestOne(2222222, true);
            TestOne(2147447412, true);
            TestOne(2147483647, false);

        }
        public static void TestOne(int number, bool expected)
        {
            PalindromeNumber pn = new PalindromeNumber();
            bool ret = pn.IsPalindrome4(number);

            Console.Write(number);
            Console.WriteLine("\t" + (ret == expected ? "Pass" : "Fail"));
        }
        
        public bool IsPalindrome(int number)
        {
            if (number < 0) return false;
            if (number < 10) return true;

            string s = number.ToString();

            int half = s.Length / 2;
            for (int i = 0; i < half; i++)
            {
                if (s[i] != s[s.Length - 1 - i]) return false;
            }
            return true;
        }

        public bool IsPalindrome2(int number)
        {
	        if (number < 0) return false;
	        if (number < 10) return true;
		
	        string s = number.ToString();
	        int length = GetNumberLength(number);
	        int half = length/2;
	        for(int i=0;i<half;i++)
	        {
		        if(GetDigial(number, i) != GetDigial(number, length-1-i)) return false;
	        }
	        return true;
        }
	
        private int GetNumberLength(int number)
        {
	        return (int)Math.Log(number, 10) + 1;
        }

        private int GetDigial(int number, int pos)
        {
            if (pos < 0) throw new Exception("pos should >=0!");

            while (pos >= 0)
            {
                int digi = number%10;
                if (pos == 0) return digi;
                number /= 10;
                pos--;
            }

            return 0;
        }

        public bool IsPalindrome3(int number)
        {
            if (number < 0) return false;
            int div = 1;

            int y = number;
            while (y/div >= 10)
            {
                div *= 10;
            }

            while (y > 0)
            {

                int l = y/div;
                int r = y%10;
                if (l != r) return false;

                y = (y%div)/10;
                div /= 100;
            }
            return true;
        }

        public bool IsPalindrome4(int number)
        {
            if (number < 0) return false;
            return GoPaldin(number, ref number);
        }

        public bool GoPaldin(int x, ref int y)
        {
            if (x < 1) return true;
            if (GoPaldin(x/10, ref y) && (x%10 == y%10))
            {
                y /= 10;
                return true;
            };
            return false;
        }
    }
}
