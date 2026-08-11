using UnityEngine;

public interface ITakeDmg
{
    void TakeDmg();
}

public interface IAttack
{
    int Dmg { get; }
    void Attack();
}

public interface IHealth
{
    int Health { get; }
}

public interface IEffect
{
    void Effect();
}
