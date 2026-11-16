using NUnit.Framework.Interfaces;
using UnityEngine;

public class CheckpointSave
{
    private RunMemento memento;

    public void Save(RunState originator) => memento = originator.SaveState();

    public void Restore(RunState originator)
    {
        if (memento != null) originator.RestoreState(memento);
    }

    public void Clear() => memento = null; 
}
