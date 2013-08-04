using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeetCode
{
    public class LongestPalindromicSubString
    {
        public static void Test()
        {
            var lps = new LongestPalindromicSubString();
            TestOne("AB", "A");
            TestOne("ABABC", "ABA");
            TestOne("ABCDDCCDDCDD", "CDDCCDDC");
            TestOne("ABBACCABBA", "ABBACCABBA");
            
            TestOne("ABCCBA", "ABCCBA");
        }

        public static void TestOne(string input, string expected)
        {
            Console.Write(input+"\t");
            string output = FindLongestPalindromicSubstring(input);
            if (expected.Equals(output))
            {
                Console.Write("Pass\t"+output);
            }
            Console.WriteLine();
        }

        public static string FindLongestPalindromicSubstring(string input)
        {
            if (input == null) return null;
            if (input.Length < 1) return "";
            if (input.Length == 1) return input;

            string reverseInput = ReverseString(input);

            return FindLongestCommonSubString(input, reverseInput);
        }

        static string ReverseString(string input)
        {
            if (input == null) return null;
            if (input.Length < 1) return "";

            char[] reversed = input.ToArray();
            for (int i = 0, j = input.Length - 1; i < j; i++, j--)
            {
                reversed[i] = input[j];
                reversed[j] = input[i];
            }

            return new string(reversed);
        }
        public static string FindLongestCommonSubString(string input1, string input2)
        {
            if (input1 == null || input2 == null) return null;
            if (input1.Length < 1 || input2.Length < 1) return "";
            int maxIndex = 0;
            int maxLen = 0;

            int len1 = input1.Length;
            int len2 = input2.Length;
            int[,] mark = new int[len1, len2];
            for (int i = 0; i < len1; i++)
            {
                for (int j = 0; j < len2; j++)
                {
                    if (input1[i] == input2[j])
                    {
                        if (i - 1 < 0 || j - 1 < 0)
                        {
                            mark[i, j] = 1;
                            if (maxLen < 1) maxLen = 1;
                            continue;
                        }
                        mark[i, j] = mark[i - 1, j - 1] + 1;
                        if (mark[i, j] > maxLen)
                        {
                            maxIndex = i;
                            maxLen++;
                        }
                    }
                }
            }
            int begin = maxIndex - maxLen + 1;
            return input1.Substring(begin, maxLen);
        }
    }
}
