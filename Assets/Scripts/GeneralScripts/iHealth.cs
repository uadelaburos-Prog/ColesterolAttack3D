using UnityEngine;

public interface iHealth
{   
    void DoDmg(int damage);
    void RegenHealth(int life);
    void Die();
    void ReciveDmg(bool dmg);
}

