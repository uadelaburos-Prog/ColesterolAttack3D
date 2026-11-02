using UnityEngine;

public interface iHealth
{   
    void DoDmg();
    int RegenHealth(int life);
    void Die();
    void ReciveDmg(bool dmg);
}