using System.Collections.Generic;
using System;

public interface ISimpleSet<T>
{
    public T this[int index] { get; set; }

    public int Count { get; }
    public bool Add(T item);
    public void Clear();
    public bool Contains(T item);
    public T[] ToArray();
    public void Insert(int index, T item);
    public bool Remove(T item);
    public void RemoveAt(int index);
    public void RemoveRange(int index, int count);
    public void AddRange(T[] items);

}
