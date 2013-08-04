using System;
using MCache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MCacheTest
{
    [TestClass]
    public class MCacheTest
    {
        [TestMethod]
        public void TestMethodLRU1()
        {
            var c = new MCache<String, String, LRUCacheController>(10, 3);
            c.Put("1","a");
            c.Put("2","b");

            Assert.AreEqual(c.Get("1"), "a");
            Assert.AreEqual(c.Get("2"), "b");
        }

        [TestMethod]
        public void TestMethodLRU2()
        {
            var c = new MCache<Int32, String, LRUCacheController>(10, 3);
            c.Put( 1, "a");
            c.Put(11, "b");
            c.Put(21, "c");
            c.Put(31, "d");
           
            Assert.AreEqual(c.Get(1), null);

        }

        [TestMethod]
        public void TestMethodLRU3()
        {
            var c = new MCache<Int32, String, LRUCacheController>(10, 3);
            c.Put(1, "a");
            c.Put(11, "b");
            c.Put(21, "c");
            c.Put(31, "d");

            Assert.AreEqual(c.Get(1), null);
        }

        [TestMethod]
        public void TestMethodLRUNeg1()
        {
            bool flag = true;
            try
            {
                var c = new MCache<String, String, LRUCacheController>(0, -1);
            }
            catch (ArgumentException e)
            {
                flag = false;
            } 

            Assert.IsFalse(flag);
        }

        [TestMethod]
        public void TestMethodLRUNeg2()
        {
            bool flag = true;
            try
            {
                var c = new MCache<String, String, LRUCacheController>(-1, 0);
            }
            catch (ArgumentException e)
            {
                flag = false;
            }

            Assert.IsFalse(flag);
        }


        [TestMethod]
        public void TestMethodMRU1()
        {
            var c = new MCache<Int32, String, MRUCacheController>(10, 3);
            c.Put(1, "a");
            c.Put(11, "b");
            c.Put(21, "c");
            c.Put(31, "d");

            Assert.AreEqual(c.Get(21), null);

        }

        [TestMethod]
        public void TestMethodMRU2()
        {
            var c = new MCache<int, string, MRUCacheController>(3, 1);
            c.Put(1, "a");
            c.Put(2, "b");
            c.Put(3, "c");
            c.Put(4, "d");

            Assert.AreEqual(c.Get(1), null);
            Assert.AreEqual(c.Get(2), "b");
            Assert.AreEqual(c.Get(3), "c");
            Assert.AreEqual(c.Get(4), "d");

        }

        [TestMethod]
        public void TestMethodClean1()
        {
            var c = new MCache<int, string, MRUCacheController>(3, 1);
            c.Put(1, "a");
            c.Put(2, "b");
            c.Put(3, "c");

            Assert.AreEqual(c.Get(1), "a");
            Assert.AreEqual(c.Get(2), "b");
            Assert.AreEqual(c.Get(3), "c");
            c.Clean();
            Assert.AreEqual(c.Get(2), null);
            Assert.AreEqual(c.Get(3), null);
            Assert.AreEqual(c.Get(4), null);
        }


    }
}
