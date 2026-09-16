using System;
using System.Runtime.ExceptionServices;

namespace Demonics
{
    public class PriorityQueue<T> : IPriorityQueue<T>
    {
        private T[] items;
        private int[] priorities;
        private int count;
        public int Count => count;

        public bool IsEmpty => count == 0;

        public PriorityQueue(int capacity = 10)
        {
            items = new T[capacity];
            priorities = new int[capacity];
            count = 0;
        }

        public void Enqueue(T item, int priority)
        {
            if (count == items.Length) Resize();

            int insertIndex = count;

            for (int i = count; i > 0; i--)
            {
                if (priority < priorities[i - 1])
                {
                    items[i] = items[i - 1];
                    priorities[i - 1] = priorities[i - 1];
                    insertIndex = i - 1;
                }
                else break;
            }

            items[insertIndex] = item;
            priorities[insertIndex] = priority;
            count++;
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            T result = items[0];
            ShiftLeft();
            count--;
            return result;
        }

        private void ShiftLeft()
        {
            for (int i = 0; i < count - 1; i++)
            {
                items[i] = items[i + 1];
                priorities[i] = priorities[i + 1];
            }
        }

        public int GetHighestPriority()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            return priorities[0];
        }

        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            return items[0];
        }

        private void Resize()
        {
            Array.Resize(ref items, items.Length * 2);
            Array.Resize(ref priorities, priorities.Length * 2);
        }

        public void Clear()
        {
            items = null;
            priorities = null;
            count = 0;
        }
    }
}