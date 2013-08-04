using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JZ
{

    //1. good: simple
    //2. bad: will be intialized when use this class, even not need this instance, e.g. calling other static utlity method
    public class Singleton1
    {
	    private static Singleton1 instance = new Singleton1();
	
	    Singleton1() {}
	
	    public static Singleton1 Instance { 
		    get { return instance; }
	    }
    }

    public class Singleton2
    {
	    class RealObject{
		    static RealObject() {}
            internal static readonly Singleton2 instance = new Singleton2();	
	    }

        Singleton2() { }

        public static Singleton2 GetInstance()
        {
		   
            return RealObject.instance;
	    }

    }
}
