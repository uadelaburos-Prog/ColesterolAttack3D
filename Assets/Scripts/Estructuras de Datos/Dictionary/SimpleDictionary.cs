using System;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Demonics
{
    public class SimpleDictionary<Tkey, Tvalue> : ISimpleDictionary<Tkey, Tvalue>
    {
        KeyValuePair<Tkey, Tvalue>[] internalArray;

        int defaultCapacity = 4;

        int count = 0;

        public SimpleDictionary() => internalArray = new KeyValuePair<Tkey, Tvalue>[defaultCapacity];

        public Tvalue this[Tkey key]
        {
            get
            {
                if(key == null) throw new ArgumentNullException("key cannot be null");
                int index = IndexOf(key);

                if(index == -1) throw new KeyNotFoundException();
                return internalArray[index].Value;
            }

            set
            {
                if (key == null) throw new ArgumentNullException("key cannot be null");
                int index = IndexOf(key);
                if (index == -1) ExecuteAdd(key, value);
                internalArray[index] = new KeyValuePair<Tkey, Tvalue>(key, value);
            }
        }

        public int Count => count;

        public bool IsEmpty => count == 0;

        public void Add(Tkey key, Tvalue value)
        {
            if (ContainsKey(key)) throw new ArgumentException("The Key is already in the Dictionary");
            ExecuteAdd(key, value);

        }

        public bool Remove(Tkey key)
        {
            if(key == null) throw new ArgumentNullException("key cannot be null");

            int index = IndexOf(key);

            if(index == -1) return false;

            if(index != count -1) internalArray[index] = internalArray[count - 1];

            internalArray[count - 1] = default;
            count--;
            return true;
        }

        public void Clear()
        {
            internalArray = new KeyValuePair<Tkey, Tvalue>[count];
            count = 0;
        }

        public bool ContainsKey(Tkey key)
        {
            if(key == null) throw new ArgumentNullException("Key doesn´t exists");
            return IndexOf(key) >= 0;
        }

        public bool TryAdd(Tkey key, Tvalue value)
        {
            if (key == null) throw new ArgumentNullException("Key cannot be null");
            if(ContainsKey(key)) return false;
            ExecuteAdd(key, value);
            return true;
        }

        public bool TryGetValue(Tkey key, out Tvalue value)
        {
            if(key == null) throw new ArgumentNullException("Key cannot be null");
            int index = IndexOf(key);
            if(index == -1)
            {
                value = default;
                return false;
            }
            value = internalArray[index].Value;
            return true;
        }

        void ExecuteAdd(Tkey key, Tvalue value)
        {
            ValidateSize(count);
            internalArray[count] = new KeyValuePair<Tkey, Tvalue>(key, value);
            count++;
        }

        public Tkey[] Keys()
        {
            Tkey[] result = new Tkey[count];

            for(int i = 0; i < count; i++)
                result[i] = internalArray[i].Key;

            return result;
        }

        public Tvalue[] Values()
        {
            Tvalue[] result = new Tvalue[count];

            for (int i = 0; i < count; i++)
                result[i] = internalArray[i].Value;

            return result;
        }

        private void ValidateSize(int nextIndex)
        {
            if (nextIndex >= internalArray.Length)
            {
                Resize(nextIndex);
            }
        }

        void Resize(int targetAmount)
        {
            int currentLength = internalArray.Length;

            while (targetAmount > currentLength - 1)
            {
                currentLength *= 2;
            }

            KeyValuePair<Tkey, Tvalue>[] newArray = new KeyValuePair<Tkey, Tvalue>[currentLength];

            for (int i = 0; i < count; i++)
            {
                newArray[i] = internalArray[i];
            }

            internalArray = newArray;
        }

        private int IndexOf(Tkey key)
        {

            for (int i = 0; i < count; i++)
            {
                if (internalArray[i].Key.Equals(key))
                {
                    return i;
                }
            }

            return - 1;
        }
    }
}