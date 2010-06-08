using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CSharpInterviewPractice
{
    class HashTable<K, V>
    {
        public class KVPair
        {
            public KVPair(K key, V value)
            {
                this.key = key;
                this.value = value;
            }

            private K key;
            private V value;
            public K Key { get { return key; } }
            public V Value { get { return value; } }
        }

        public class KVPairCollisionArray
        {
            private KVPair[] array;
            private int filledItemCount = 0;

            public KVPairCollisionArray(int size)
            {
                this.array = new KVPair[size];
            }

            public void AddOrReplace(KVPair kvp)
            {
                Nullable<int> index = this.IndexOfKey(kvp.Key);
                if (index.HasValue)
                {
                    this.array[index.Value] = kvp;
                }
                else
                {
                    if (filledItemCount >= this.array.Length)
                    {
                        ResizeArray();
                    }
                    this.array[filledItemCount] = kvp;
                    filledItemCount++;
                }
            }

            public V Get(K key)
            {
                Nullable<int> index = this.IndexOfKey(key);
                if (index.HasValue)
                {
                    return this.array[index.Value].Value;
                }
                return default(V);
            }

            private Nullable<int> IndexOfKey(K key)
            {
                for (int i = 0; i < filledItemCount; i++)
                {
                    if (array[i].Key.Equals(key))
                    {
                        return i;
                    }
                }

                return null;
            }

            private void ResizeArray()
            {
                KVPair[] origArray = this.array;
                this.array = new KVPair[origArray.Length * 2];
                origArray.CopyTo(this.array, 0);
            }
        }

        private KVPairCollisionArray[] backingArray;

        public HashTable(int size)
        {
            this.backingArray = new KVPairCollisionArray[size];
        }

        public V Get(K key)
        {
            return SubArray(key).Get(key);
        }

        public void Set(K key, V value)
        {
            SubArray(key).AddOrReplace(new KVPair(key, value));
        }

        private KVPairCollisionArray SubArray(K key)
        {
            int index = BackingArrayIndex(key);
            KVPairCollisionArray subArray = this.backingArray[index];
            if (subArray == null)
            {
                subArray = new KVPairCollisionArray(10);
                this.backingArray[index] = subArray;
            }
            return subArray;
        }

        private int BackingArrayIndex(K key)
        {
            return Math.Abs(key.GetHashCode()) % this.backingArray.Length;
        }
    }

    [TestFixture]
    class HashTableTest
    {
        [TestFixture]
        class KVPairCollisionArrayTest
        {
            [Test]
            public void returnsDefaultForMissingKeys()
            {
                HashTable<string, int>.KVPairCollisionArray instance = new HashTable<string, int>.KVPairCollisionArray(3); 
                Assert.AreEqual(default(int), instance.Get("Foo"));
            }

            [Test]
            public void basicAddWorks()
            {
                HashTable<string, int>.KVPairCollisionArray instance = new HashTable<string, int>.KVPairCollisionArray(3); 
                
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Bob", 7));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Joe", 14));

                Assert.AreEqual(7, instance.Get("Bob"));
                Assert.AreEqual(14, instance.Get("Joe"));
            }

            [Test]
            public void arrayGrowsAsNecessary()
            {
                HashTable<string, int>.KVPairCollisionArray instance = new HashTable<string, int>.KVPairCollisionArray(3); 
                
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Bob", 7));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Joe", 14));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Sally", 3));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Leah", 4));

                Assert.AreEqual(4, instance.Get("Leah"));
                Assert.AreEqual(3, instance.Get("Sally"));
                Assert.AreEqual(7, instance.Get("Bob"));
                Assert.AreEqual(14, instance.Get("Joe"));
            }

            [Test]
            public void replaceWorks()
            {
                HashTable<string, int>.KVPairCollisionArray instance = new HashTable<string, int>.KVPairCollisionArray(3); 
                
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Bob", 7));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Joe", 14));
                instance.AddOrReplace(new HashTable<string, int>.KVPair("Bob", 2));

                Assert.AreEqual(2, instance.Get("Bob"));
                Assert.AreEqual(14, instance.Get("Joe"));
            }
        }
 
        static private void UseHashTable(int size)
        {
            HashTable<String, String> ht = new HashTable<String, String>(size);

            ht.Set("Myron", "4/27/82");
            ht.Set("Lori", "2/2/82");
            ht.Set("Matt", "11/11/79");
            ht.Set("Drew", "7/7/77");
            ht.Set("Mike", "9/20/77");

            Assert.AreEqual("7/7/77", ht.Get("Drew"));
            Assert.AreEqual("2/2/82", ht.Get("Lori"));
            Assert.AreEqual("4/27/82", ht.Get("Myron"));
            Assert.AreEqual("9/20/77", ht.Get("Mike"));
            Assert.AreEqual("11/11/79", ht.Get("Matt"));
            Assert.IsNull(ht.Get("Someone Else"));
        }

        [Test]
        static public void BasicExample()
        {
            UseHashTable(1000);
        }

        [Test]
        static public void ExampleWithCollisions()
        {
            UseHashTable(2);
        }
    }
}
