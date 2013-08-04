using System;
using System.Collections.Generic;

namespace MCache
{
    /// <summary>
    /// The N-Way set-associative cache
    /// </summary>
    /// <typeparam name="TKey">The key of the cache item</typeparam>
    /// <typeparam name="TVal">The value of the cache item</typeparam>
    /// <typeparam name="TController">The cache algo controller, like LRU, MRU</typeparam>
   
    public class MCache<TKey, TVal, TController> where TController:ICacheController, new()
    {
        private List<TController> sets;
        private int nSet;
        private int nWay;

        /// <summary>
        /// Constructor of the N-Way set-associative cache
        /// </summary>
        /// <param name="nSet">how many sets the cache has</param>
        /// <param name="nWay">how many way for one set</param>
        public MCache(int nSet, int nWay)
        {
            if (nSet <= 0) throw new ArgumentException("Set number should larger than 0.");
            if (nWay <= 0) throw new ArgumentException("Way number should larger than 0.");

            this.nSet = nSet;
            this.nWay = nWay;

            sets = new List<TController>(nSet);
            for (var i = 0; i < nSet; i++)
            {
                var controller = new TController();
                controller.SetSize(nWay);

                sets.Add(controller);
            }
        }
        
        public TVal Get(TKey key)
        {
            int slot = GetSlotByKey(key);
            return (TVal)sets[slot].Get(key);
        }

        public void Put(TKey key, TVal val)
        {
            var slot = GetSlotByKey(key);
            sets[slot].Put(key, val);
        }

        public void Clean()
        {
            for (var i = 0; i < nSet; i++)
            {
                var controller = new TController();
                controller.SetSize(nWay);

                sets[i] = controller;
            }
        }

        private int GetSlotByKey(TKey key)
        {
            int hash = key.GetHashCode();
            if (hash < 0) hash = -hash;
            return  hash % nSet;   
        }

    }
}
