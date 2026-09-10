using UnityEngine;

public interface iHealth
{   
    void DoDmg(int damage);
    int RegenHealth(int life);
    void Die();
    void ReciveDmg(bool dmg);
}