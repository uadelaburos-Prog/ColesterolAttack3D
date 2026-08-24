using System;
using UnityEngine.UI;
namespace ED262C {
    public class SimpleLinkedList<T> : ISimpleList<T>
    {
        // refencia al primer y ultimo nodo
        LinkedNode<T> first;
        LinkedNode<T> last;

        // cantidad de elementos en la lista
        int count;
        LinkedNode<T> current;

        public T this[int index]
        { 
            get => GetNodeByIndex(index).value; 
            set => GetNodeByIndex(index).value = value; 
        }

        public int Count => count;

        public void Add(T item)
        {
            LinkedNode<T> newNode = new LinkedNode<T>(item);

            if(count == 0) first = newNode;
            else
            {
                newNode.prev = last;
                last.next = newNode;
            }

            last = newNode;
            count++;       
        }

        public void AddRange(T[] items)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            first = null;
            last = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            return GetNodeByValue(item) != null;
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            LinkedNode<T> toRemove = GetNodeByValue(item);
            if(toRemove == null) return false;

            if (count == 1)
            {
                Clear();
                return true;
            }

            if (toRemove == first)
            {
                first.next.prev = null;
                first = first.next;    
            }

            else if(toRemove == last)
            {
                last.prev.next = null;
                last = last.prev;
            }

            return true;
           // else 
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            if (count == 1)
            {
                Clear();
                return;
            } 

            if(index == 0)
            {
                first.next.prev = null;
                first = first.next;
            }

            else if (index == count - 1) 
            {
                last.prev.next = null;
                last = last.prev;
            }

            else RemoveAndReconnect(GetNodeByIndex(index));

            count--;
        }

        public void RemoveRange(int index, int count)
        {
            throw new System.NotImplementedException();
        }

        public T[] ToArray()
        {
            throw new System.NotImplementedException();
        }

        // Usamos esta funcion para el indexer, remove, Insert, etc.
        LinkedNode<T> GetNodeByIndex(int index)
        {
            if(index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            // decidimos si se va al principio o al final 
            // para saber si esta mas cerca del principio o el final 
            // hecemos count / 2
            if(index <= count / 2)
            {
                // empezamos por el primero
                current = first;

                // pasamos por cada uno hasta llegar al indice
                for(int i = 0; i <= index; i++)
                    current = current.next;

        
            }
            else
            {
                // empezamos por el ultimo
                current = last;

                // pasamos por cada uno hasta llegar al indice
                for (int i = count - 1; i >= index; i--)
                    current = current.prev;

            } 
            // una vez llegado se devuelve la pos del index
            return current;
        }

        LinkedNode<T> GetNodeByValue(T value)
        {
            LinkedNode<T> current = first;

            while (current != null)
            {
                if(current.value.Equals(value)) return current;
                current = current.next;
            }

            return null;
        }

        void RemoveAndReconnect(LinkedNode<T> toRemove)
        {
            toRemove.next.prev = toRemove.prev;
            toRemove.prev.next = toRemove.next;

            toRemove.next = null;
        }
    }
}

