using System;
using System.Collections.Generic;

namespace MCache
{
    public class MRUCacheController: ICacheController
    {
        private Dictionary<Object, Object> dict = new Dictionary<Object, Object>();
        private List<Object> link = new List<Object>();
        private int waysize;

        public void SetSize(int n)
        {
            if (n <= 0) throw new ArgumentException("Way size should larger than 0.");

            link = new List<Object>();
            waysize = n;
        }

        public Object Get(Object key)
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

        public void Put(Object key, Object val)
        {
            if (key == null || waysize <= 0) return;
            lock (this)
            {
                if (link.Count == waysize)
                {
                    dict.Remove(link[waysize - 1]);
                    link.RemoveAt(waysize - 1);
                }

                dict.Add(key, val);
                link.Add(key);
            }
        }
    }
}
