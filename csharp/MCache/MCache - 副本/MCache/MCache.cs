using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCache
{
    public class MCache<TKey, TVal, TController> where TController:CacheController, new()
    {
        private List<TController> sets;
        private int nSet;
        private int nWay;

        public MCache(int nSet, int nWay)
        {
            if (nSet <= 0) throw new ArgumentException("Set number should > 0.");
            if (nWay <= 0) throw new ArgumentException("Way number should > 0.");

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

        private int GetSlotByKey(TKey key)
        {
            int hash = key.GetHashCode();
            if (hash < 0) hash = -hash;
            return  hash % nSet;   
        }

    }
}
