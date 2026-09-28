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
    public bool Remove(T item);

}
