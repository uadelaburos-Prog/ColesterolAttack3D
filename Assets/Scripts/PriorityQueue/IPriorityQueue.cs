namespace Demonics
{
    public interface IPriorityQueue<T>
    {
        void Enqueue(T item, int priority);
        void Clear();
        T Dequeue();
        T Peek();
        int GetHighestPriority();
        int Count {  get; }
        bool IsEmpty {  get; }
    }
}
