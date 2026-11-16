using ED262C;
using UnityEngine;

public class WaveEventQueue
{
    private readonly SimpleArrayQueue<IWaveEvent> queue = new SimpleArrayQueue<IWaveEvent>();

    public bool IsEmpty => queue.IsEmpty;

    public void Enqueue(IWaveEvent e) => queue.Enqueue(e);
    public void Clear() => queue.Clear();

    public void ExecuteNext()
    {
        if(!queue.IsEmpty) queue.Dequeue().Execute();
    }
}
