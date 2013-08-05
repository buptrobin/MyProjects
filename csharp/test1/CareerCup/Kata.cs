using System;

public class Kata
{
    //Kata 2, Karate Chop
    public static int chop(int num, int[] ar)
    {
        if (ar == null) return -1;
        if (ar.Length < 1) return -1;
        //0 1 2 3 4 5 
        //0 1 2   
        return chop_recursive(num, ar, 0, ar.Length-1);            
    }
    private static int chop_recursive(int num, int[] ar, int begin, int end)
    {
        if (begin>end) return -1;
        
        if (begin<0 || end<0) return -1;
        
        int mid = (begin + end)/2; //TODO: here maybe overflow
        //Console.WriteLine(mid);
        if(num==ar[mid]) return mid;
        if(num>ar[mid]) return chop_recursive(num, ar, mid+1, end);
        else return chop_recursive(num, ar, begin, mid-1);
    }
    
    public static void Test_Kata2()
    {
        AssertEqual(-1, chop(0, new int[] {}));
        AssertEqual(1, chop(3, new int[] { 1, 3, 5 }));
        AssertEqual(1, chop(3, new int[] { 1, 3, 5 }));
        AssertEqual(0, chop(1, new int[] { 1, 3, 5 }));
        AssertEqual(2, chop(5, new int[] { 1, 3, 5 }));
        AssertEqual(-1, chop(0, new int[] { 1, 3, 5 }));
        AssertEqual(-1, chop(2, new int[] { 1, 3, 5 }));
        AssertEqual(-1, chop(4, new int[] { 1, 3, 5 }));
        AssertEqual(-1, chop(6, new int[] { 1, 3, 5 }));

        AssertEqual(0, chop(1, new int[] { 1, 3, 5, 7 }));
        AssertEqual(1, chop(3, new int[] { 1, 3, 5, 7 }));
        AssertEqual(2, chop(5, new int[] { 1, 3, 5, 7 }));
        AssertEqual(3, chop(7, new int[] { 1, 3, 5, 7 }));
        AssertEqual(-1, chop(0, new int[] { 1, 3, 5, 7 }));
        AssertEqual(-1, chop(2, new int[] { 1, 3, 5, 7 }));
        AssertEqual(-1, chop(4, new int[] { 1, 3, 5, 7 }));
        AssertEqual(-1, chop(6, new int[] { 1, 3, 5, 7 }));
        AssertEqual(-1, chop(8, new int[] { 1, 3, 5, 7 }));

    }


    #region assert funcitons
    public static void AssertEqual(int s1, int s2)
    {
        if( s1 == s2 ) Console.Write("Pass\t"); else Console.Write("Fail\t");
        Console.WriteLine("s1={0}\ts2={1}", s1, s2);
    }


    #endregion
	
}
