using UnityEngine;
using System;
using ED262C;
using UnityEngine.Rendering;
namespace Demonics
{
    public class CollectionSorting<T> : ICollectionSorting<T>
    {
        public void SortArray(T[] array, Comparison<T> arrayCompare)
        {
            int low = 0;
            int high = array.Length - 1;
            QuickSort(array, low, high, arrayCompare);
        }

        public void SortList(ISimpleList<T> list, Comparison<T> listCompare)
        {
            T[] sortedArray = list.ToArray();
            int low = 0;
            int high = sortedArray.Length - 1;
            QuickSort(sortedArray, low, high, listCompare);

            for (int i = 0; i < sortedArray.Length; i++)
                list[i] = sortedArray[i];
        }

        private void QuickSort(T[] array, int low, int high, Comparison<T> comparison)
        {
            if(low < high)
            {
                int pivotIndex = Partition(array, low, high, comparison);

                QuickSort(array, low, pivotIndex - 1,comparison);
                QuickSort(array, pivotIndex + 1, high, comparison);
            }
        }

        private int Partition(T[] array, int low, int high, Comparison<T> comparison)
        {
            T pivot = array[high];
            int i = low - 1;

            for(int j = low; j < high; j++)
            {
                if (comparison(array[j], pivot) <= 0)
                {
                    i++;
                    Swap(array, i, j);
                }
            }

            Swap(array, i + 1, high);
            return i + 1;
        }

        private void Swap(T[] array, int a, int b)
        {
            (array[a], array[b]) = (array[b], array[a]);
        }
    }
}

