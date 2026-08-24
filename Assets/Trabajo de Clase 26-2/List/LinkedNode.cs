namespace ED262C
{
    public class LinkedNode<T>
    {
        // referencia a los nodos
        public LinkedNode<T> prev;
        public LinkedNode<T> next;

        // el dato que realmente guarda el nodo
        public T value;

        public LinkedNode(T value)
        {
            this.value = value;
        }


    }
}

