using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCache
{
    public class LRUCacheController: CacheController
    {
        private Dictionary<Object, Object> dict = new Dictionary<Object, Object>(); 
        private List<Object> link = new List<Object>();
        private int waysize;

        public override void SetSize(int n)
        {
            link = new List<Object>();
            waysize = n;
        }

        public override Object Get(Object key)
        {
            if (key == null) return null;

            lock (this)
            {
                if (!dict.ContainsKey(key)) return null;

                //move the item the the tail
                link.Remove(key);
                link.Add(key);

                return dict[key];
            }
        }

        public override void Put(Object key, Object val)
        {
            if (key == null) return;
            //remove the oldest one
            lock (this)
            {
                if (link.Count == waysize)
                {
                    dict.Remove(link[0]);
                    link.RemoveAt(0);
                }

                dict.Add(key, val);
                link.Add(key);
            }
        }
    }
}
