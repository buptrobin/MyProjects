using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerCup
{
    class WebSite
    {

        /***
         * Develop an algorithm and write code to break a sentence without spaces into a sentence of valid words separated by spaces. 
         * For ex: thissentenceisseparated needs to be broken into: this c is separated 
         * Assume that you have a dictionary to check for valid words. Your algorithm should return false if the sentence cannot be separated into valid words.
         */
        public static void TestSentenceSeparate()
        {
            WebSite ws = new WebSite();
            Console.WriteLine(ws.BreakSentence("thissentenceisseparated"));
        }
        public string BreakSentence(string sentence)
        {
            //check input
            if (string.IsNullOrEmpty(sentence)) return null;

            //initial
            string seperatedString = GoSeperate(sentence, 0, "");

            return seperatedString;
        }

        public string GoSeperate(string s, int lastpos, string sout)
        {
	        for(int curpos = lastpos+1; curpos<s.Length; curpos++)
	        {
		        if(IsWord(s.Substring(lastpos, curpos-lastpos)))
		        {
                    string snew = sout + " " + s.Substring(lastpos, curpos - lastpos);
                    if (IsWord(s.Substring(curpos, s.Length-curpos)))
                        return snew + " " + s.Substring(curpos, s.Length - curpos);
			        else return GoSeperate(s, curpos, snew);
		        }
	        }
	        return null;
        }
        public bool IsWord(string s)
        {
            if (s == "this" || s == "sentence" || s == "is" || s == "separated") return true;

            return false;
        }
    }
    
    
}
