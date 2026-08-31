using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, iHealth
{
    [SerializeField] private int life = 3;
    private int currentLife;
    [SerializeField] private int damage = 1;
    [SerializeField] private HealthBar healthBar;

    private ShopManager shopM;
    public bool Dmg = false;
    public string ID;
    public event Action<Enemy> OnDeath;

    [System.Obsolete]
    private void OnEnable()
    {
        shopM = FindObjectOfType<ShopManager>();
        healthBar = GetComponent<HealthBar>();
        currentLife = life;
    }
    public void Die()
    {
        shopM.AddCoins(5);
        gameObject.SetActive(false);
        if (!isActiveAndEnabled)
        {
            Destroy(gameObject);
        }

        OnDeath?.Invoke(this);  
    }

    public void RegenHealth(int life)
    {
        throw new System.NotImplementedException();
    }

    public void DoDmg(int damage)
    {
        
    }

    public void ReciveDmg(bool dmg)
    {
        if (dmg)
        {
            currentLife--;
            healthBar.UpdateHealthBar(life, currentLife);

            if(currentLife <= 0)
            {
                Die();
            }
        }
    }
}
