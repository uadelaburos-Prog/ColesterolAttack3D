using UnityEngine;
using System;
using System.Transactions;

namespace Demonics
{
    public class LinkedPriorityNode<T>
    {
        public T Item;
        public int Priority;
        public LinkedPriorityNode<T> Prev;
        public LinkedPriorityNode<T> Next;
        public LinkedPriorityNode(T item, int priority)
        {
            Item = item;
            Priority = priority;
        }
    }

    public class LinkedPriorityQueue<T> : IPriorityQueue<T>
    {
        private LinkedPriorityNode<T> first;
        private LinkedPriorityNode<T> last;
        private int count;

        public int Count => count;

        public bool IsEmpty => count == 0;

        public void Clear()
        {
            first = null;
            last = null;
            count = 0;
        }

        public T Dequeue()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            T result = first.Item;
            first = first.Next;

            if (first != null) first.Prev = null;
            else last = null;

            count--;
            return result;
        }

        public void Enqueue(T item, int priority)
        {
            var newNode = new LinkedPriorityNode<T>(item, priority);

            if (IsEmpty)
            {
                first = newNode;
                last = newNode;
            }
            else if (priority < first.Priority)
            {
                newNode.Next = first;
                first.Prev = newNode;
                first = newNode;
            }
            else
            {
                var current = last;

                while (current.Prev != null && priority < current.Priority)
                {
                    current = current.Prev;
                }

                newNode.Next = current.Next;
                newNode.Prev = current;

                if (current.Next != null)
                    current.Next.Prev = newNode;
                else
                    last = newNode;

                current.Next = newNode;
            }

            count++;
        }

        public int GetHighestPriority()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            return first.Priority;
        }

        public T Peek()
        {
            if (IsEmpty) throw new InvalidOperationException("Queue vacia");
            return first.Item;
        }
    }
}