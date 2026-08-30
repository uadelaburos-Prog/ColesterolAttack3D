using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, iHealth
{
    [SerializeField] private int life = 3;
    [SerializeField] private int damage = 1;

    private ShopManager shopM;
    public bool Dmg = false;
    public string ID;
    public event Action<Enemy> OnDeath;

    [System.Obsolete]
    private void OnEnable()
    {
        shopM = FindObjectOfType<ShopManager>();
    }

    private void Update()
    {
        Debug.Log($"Vida: {life}");
        if(life <= 0)
        {
            Die();
        }
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
            life--;
        }
    }
}
