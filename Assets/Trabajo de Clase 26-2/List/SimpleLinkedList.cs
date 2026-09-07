using System;
using System.Diagnostics;

namespace ED262C
{
    public class SimpleLinkedList<T> : ISimpleList<T>
    {
        // Referencias al primer y ultimo nodo
        LinkedNode<T> first = null;
        LinkedNode<T> last = null;
        
        // Cantidad de elementos guardados en la lista
        int count = 0;

        // myList[3] y va al "indice" 3
        public T this[int index] 
        {
            get => GetNodeByIndex(index).value;
            
            // Cuando hacemos myList[3] = 10, value es 10
            // Guardamos 10 en el nodo (tambien tiene una variable value)
            set => GetNodeByIndex(index).value = value; 
        }

        public int Count => count;

        public void Add(T item)
        {
            // Creamos un nodo que guarde el dato a insertar
            LinkedNode<T> newNode = new LinkedNode<T>(item);

            // Si la lista estaba vacia, el nuevo nodo es el primero Y ultimo
            if (count == 0) first = newNode;

            // Si no, conectamos al nuevo nodo con el que era ultimo
            else
            {
                newNode.prev = last;
                last.next = newNode;
            }

            // El ultimo ahora es el nuevo y subimos count
            last = newNode;
            count++;
        }

        // Toma un array y agrega todo al final la lista
        public void AddRange(T[] items)
        {
            for (int i = 0; i < items.Length; i++)
                Add(items[i]);
        }

        public void Clear()
        {
            // Si borramos el primero y ultimo, todo lo demas queda aislado
            // No hay ningun objeto referenciando a esos nodos
            // En ese caso, el Garbage Collector borra todo automaticamente
            first = null;
            last = null;
            count = 0;
        }

        public bool Contains(T item)
        {
            return GetNodeByValue(item) != null;
        }

        // Agrega el elemento en un indice especifico
        public void Insert(int index, T item)
        {
            // Si se intenta agregar al final, es Add
            if (index == count)
            {
                Add(item);
                return;
            }

            // Si el indice no existe, rompemos el programa
            if (index < 0 || index >= count)
                throw new ArgumentOutOfRangeException("Index is outside of bounds");

            // Creamos el nodo a insertar
            LinkedNode<T> newNode = new LinkedNode<T>(item);

            // Buscamos el nodo que actualmente esta en ese indice
            LinkedNode<T> current = GetNodeByIndex(index);

            // Si insertamos al principio, no hay prev y actualizamos first
            if(index == 0)
            {
                newNode.next = current;
                current.prev = newNode;
                first = newNode;
            }

            // Si llegamos hasta aca, no es el primero ni el ultimo
            else
            {
                // Seteamos las conexiones del nuevo nodo (no pisa nada)
                newNode.prev = current.prev;
                newNode.next = current;

                // Seteamos las conexiones de los nodos para que apunten al nuevo
                current.prev.next = newNode;
                current.prev = newNode;
            }

            count++;
        }

        public bool Remove(T item)
        {
            LinkedNode<T> toRemove = GetNodeByValue(item);
            if (toRemove == null) return false;

            // Si solo hay un elemento, vaciamos todo
            // No necesitamos chequear que sea el elemento buscado porque
            // si no es ya devolvimos false antes
            if (count == 1)
            {
                Clear(); // Clear ya borra first, last y deja count en 0
                return true;
            }

            // Si quiero borrar el primero pero no es el unico
            if (toRemove == first)
            {
                // El segundo pasa a ser el nuevo primero
                // Y no tiene mas prev
                first.next.prev = null;
                first = first.next;
            }

            // Si quiero borrar el ultimo y no es el unico
            else if (toRemove == last)
            {
                // El anteultimo pasa a ser el nuevo ultimo
                // Y no tiene mas next
                last.prev.next = null;
                last = last.prev;
            }

            // Si esta en el medio, reconectamos
            else RemoveAndReconnect(toRemove);

            count--;
            return true;
        }

        public void RemoveAt(int index)
        {
            // Validamos que el indice exista
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            // Si solo hay un elemento, vaciamos todo
            // No necesitamos chequear que el indice sea 0, porque ya lo hicimos arriba
            if (count == 1)
            {
                Clear(); // Clear ya borra first, last y deja count en 0
                return;
            }

            // Si quiero borrar el primero pero no es el unico
            if (index == 0)
            {
                // El segundo pasa a ser el nuevo primero
                // Y no tiene mas prev
                first.next.prev = null;
                first = first.next;
            }

            // Si quiero borrar el ultimo y no es el unico
            else if(index == count -1)
            {
                // El anteultimo pasa a ser el nuevo ultimo
                // Y no tiene mas next
                last.prev.next = null;
                last = last.prev;
            }

            // Buscamos el nodo con GetNodeByIndex
            // Reconectamos a sus vecinos entre si con RemoveAndReconnect
            else RemoveAndReconnect(GetNodeByIndex(index));

            count--;
        }

        public void RemoveRange(int index, int count)
        {
            // Si el indice no existe, rompemos el programa
            if (index < 0 || index >= this.count)
                throw new ArgumentOutOfRangeException("Index is outside of bounds");

            // Validamos que se puedan remover la cantidad de elementos indicados desde el indice indicado
            if (index + count >= this.count)
                throw new ArgumentException("Offset and remove count exceed the last element on the list.");

            // Si estamos borrando del primero al ultimo, es un Clear
            if(index == 0 && count == this.count)
            {
                Clear();
                return;
            }

            // Si solo quiero remover un elemento, es RemoveAt
            if (count == 1)
            {
                RemoveAt(index);
                return;
            }

            // Buscamos al primer nodo a remover
            LinkedNode<T> firstToRemove = GetNodeByIndex(index);

            // Y de ahi buscamos al ultimo
            LinkedNode<T> lastToRemove = firstToRemove;
            
            // Desde ese primer nodo, pasamos al siguiente count veces
            // OJO: este es el count del parametro, no de la lista
            for (int i = 0; i < count -1; i++)
            {
                lastToRemove = lastToRemove.next;
            }

            // Si removemos desde el primero, pero no hasta el ultimo
            if (firstToRemove == first)
            {
                // Queda como primero el que venga despues del ultimo a remover
                first = lastToRemove.next;

                // Y ese nuevo primer nodo, no tiene prev
                first.prev = null;
            }
            // Si removemos hasta el ultimo, pero no desde el primero
            else if(lastToRemove == last)
            {
                // Queda como ultimo el que venga antes del primero a remover
                last = firstToRemove.prev;

                // Y ese nuevo ultimo nodo, no tiene next
                last.next = null;
            }
            else
            {
                UnityEngine.Debug.Log("Removimos en el medio");
                // Conectamos el anterior del primero a remover
                // con el siguiente del ultimo a remover
                firstToRemove.prev.next = lastToRemove.next;
                lastToRemove.next.prev = firstToRemove.prev;
                UnityEngine.Debug.Log(firstToRemove.prev.next.value.ToString());
                UnityEngine.Debug.Log(lastToRemove.next.prev.value.ToString());
            }

            this.count -= count;
        }

        public T[] ToArray()
        {
            // Este es el array que devolvemos
            T[] result = new T[count];

            // Empezamos desde el primer nodo, pasamos uno por uno
            LinkedNode<T> current = first;
            for(int i = 0; i < count; i++)
            {
                // Guardamos lo que esta en este nodo, en el indice actual
                result[i] = current.value;

                // Pasamos al siguiente
                current = current.next;
            }

            return result;
        }

        // Usamos esta funcion para el indexer, Remove, Insert, etc.
        LinkedNode<T> GetNodeByIndex(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is outside of List bounds");

            //Decidimos si empezar por el primer nodo o el ultimo
            // Para saber si esta mas cerca del principio o final,
            // Hacemos count / 2
            if(index <= count / 2)
            {
                // Empezamos por el primero
                LinkedNode<T> current = first;

                // Vamos pasando por el siguiente del siguiente, etc.
                // Hasta llegar al indice
                for(int i = 0; i < index; i++)
                    current = current.next;

                // Una vez que repetimos el proceso, devolvemos el que
                // estaba en la posicion index
                return current;
            }
            else
            {
                // Empezamos por el utimo
                LinkedNode<T> current = last;

                // Vamos pasando por el anterior del anterior, etc.
                // Hasta llegar al indice
                for (int i = count -1; i > index; i--)
                    current = current.prev;

                // Una vez que repetimos el proceso, devolvemos el que
                // estaba en la posicion index
                return current;
            }
        }

        LinkedNode<T> GetNodeByValue(T value)
        {
            // Como no sabemos donde esta el valor, siempre arrancamos en first
            LinkedNode<T> current = first;
            
            // Solo podemos chequear un nodo si existe
            while(current != null)
            {
                // Si el nodo tiene lo que buscamos, lo devolvemos
                if (current.value.Equals(value)) return current;
                
                // Si llegamos hasta aca, no era el valor
                // Pasamos al siguiente
                current = current.next;
            }

            // Si llegamos hasta aca, vimos toda la lista y no estaba
            return null;
        }

        // Vamos a llamar a esta funcion en Remove, RemoveAt
        void RemoveAndReconnect(LinkedNode<T> toRemove)
        {
            // Antes de remover
            // toRemove.prev <---> toRemove <---> toRemove.next

            // Despues de remover
            // toRemove.prev <------------------> toRemove.next

            // Asignamos las referencias de los nodos vecinos de toRemove
            toRemove.next.prev = toRemove.prev;
            toRemove.prev.next = toRemove.next;

            // Ahora que estan conectados, borramos a toRemove
            toRemove = null;
        }
    }
}

