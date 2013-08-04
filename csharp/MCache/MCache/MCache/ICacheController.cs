using System;

namespace MCache
{
    /// <summary>
    /// The interface of the cache controller
    /// </summary>
    public interface ICacheController
    {
        Object Get(Object key);
        void Put(Object key, Object val);

        void SetSize(int n);

    }
}
