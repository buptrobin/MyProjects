using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    using System.Threading;

    class HttpSender
    {
        private int maxThreads = 512;

        //public int MaxThreads {get;}

        public int MaxThreads { get; set; }

        public HttpSender(int maxThread)
        {
            MaxThreads = maxThread;
        }

        public HttpSender()
            : this(512)
        { }

        public void Start()
        {
            Thread t =new Thread(run);
            t.Start();
        
        }

        public void run()
        {

        }
    }
}
