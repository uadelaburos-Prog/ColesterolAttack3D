using System;
using UnityEngine;

public class SpawnEnemyEvent : IWaveEvent
{
    private readonly string type;
    private readonly Vector3 pos;
    private readonly Action<Enemy> onSpawned;

    public SpawnEnemyEvent(string type, Vector3 pos, Action<Enemy> onSpawned)
    {
        this.type = type;
        this.pos = pos;
        this.onSpawned = onSpawned;
    }

    public void Execute()
    {
        Enemy e = EnemyDPool.Instance.GetEnemy(type, pos);
        if (e != null) onSpawned?.Invoke(e);
    }
}
