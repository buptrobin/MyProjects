using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCache
{
    public abstract class CacheController
    {
        private int size;
        public abstract Object Get(Object key);
        public abstract void Put(Object key, Object val);

        public virtual void SetSize(int n)
        {
            size = n;
        }
    }
}
