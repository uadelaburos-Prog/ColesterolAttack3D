using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SimpleArraySet<T>: ISimpleSet<T>
{
    T[] internalArray;
    int count = 0;
    int defaultCapacity = 4;
    public int Count { get => count; }
    public T this[int index]
    {
        get
        {
            return internalArray[index];
        }
        set
        {
            internalArray[index] = value;
        }
    }

    public SimpleArraySet(int capacity)
    {
        internalArray = new T[capacity];
        count = 0;
    }
    public SimpleArraySet(ISimpleSet<T> original)
    {
        T[] originalArray = original.ToArray();
        internalArray = new T[originalArray.Length];

        for (int i = 0; i < originalArray.Length; i++)
        {
            internalArray[i] = originalArray[i];
        }

        count= original.Count;
    }

    public SimpleArraySet()
    {
        internalArray = new T[defaultCapacity];
        count = 0;
    }

    public T[] ToArray()
    {
        T[] result = new T[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = internalArray[i];
        }
        return result;
    }

    public bool Add(T item)
    { 
        if (Contains(item)) return false;

        checkSize(count);
        internalArray[count] = item;
        count++;
        return true;
    }

    public void Clear()
    {
        internalArray = new T[internalArray.Length];
        count = 0;
    }
    public bool Contains(T item) => IndexOf(item) >= 0;

    public bool Remove(T item)
    {
        int itemIndex = IndexOf(item);
        if (itemIndex < 0) return false;

        if (itemIndex != count - 1)
            internalArray[itemIndex] = internalArray[count - 1];

        internalArray[count - 1] = default;

        return true;
    }

    public ISimpleSet<T> UnionWith(ISimpleSet<T> otherSet)
    {
       ISimpleSet<T> result= new SimpleArraySet<T>(otherSet);
        
        for (int i = 0; i < count; i++)
        {
            result.Add(internalArray[i]);
        }
        return result;
    }

    public ISimpleSet<T> IntersectWith(ISimpleSet<T> otherSet)
    {
        ISimpleSet<T> result = new SimpleArraySet<T>();
  
        for (int i = 0; i < count; i++)
        {
            if (otherSet.Contains(internalArray[i]))
            {
                result.Add(internalArray[i]);
            }
        }
        return result;
    }

    public ISimpleSet<T> DifferenceWith(ISimpleSet<T> otherSet)
    {
        ISimpleSet<T> result = new SimpleArraySet<T>();
        for (int i = 0; i < count; i++)
        {
            if (!otherSet.Contains(internalArray[i]))
            {
                result.Add(internalArray[i]);
            }
        }
        return result;
    }


    private void checkSize(int nextIndex)
    {
        if (nextIndex >= internalArray.Length)
        {
            Resize(nextIndex);
        }
    }

    void Resize(int targetAmount)
    {   int currentLength = internalArray.Length;

        while (targetAmount > currentLength - 1)
        {
            currentLength *= 2;
        }

        T[] newArray = new T[currentLength];
    
        for (int i = 0; i < count; i++)
        {
            newArray[i] = internalArray[i];
        }
     
        internalArray = newArray;
    }

    private int IndexOf(T item) { 
    
        for (int i = 0; i < count; i++)
        {
            if (internalArray[i].Equals(item))
            {
                return i;
            }
        }
        
        return -1;
    }
}