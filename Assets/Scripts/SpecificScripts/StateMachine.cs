using UnityEngine;

public interface IEnemyState
{
    void Enter();
    void Tick();
    void Exit();
}

public class StateMachine
{
    public IEnemyState Current { get; private set; }

    public void ChangeState(IEnemyState next)
    {
        Current?.Exit();
        Current = next;
        Current?.Enter();
    }

    public void Tick() => Current?.Tick();
}
