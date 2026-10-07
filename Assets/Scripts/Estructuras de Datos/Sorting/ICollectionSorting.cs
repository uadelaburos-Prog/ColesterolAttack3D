using ED262C;
using System;
using UnityEngine;
namespace Demonics
{
    public interface ICollectionSorting<T>
    {
        void SortArray(T[] array, Comparison<T> arrayCompare);
        void SortList(ISimpleList<T> list, Comparison<T> listCompare);
    }
}
