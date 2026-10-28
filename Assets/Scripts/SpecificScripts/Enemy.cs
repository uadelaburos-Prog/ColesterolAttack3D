using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, iHealth
{
    [SerializeField] private int life = 3;
    public int currentLife;
    [SerializeField] private int damage = 1;
    [SerializeField] private HealthBar healthBar;

    private MeshRenderer mr;
    private Color orgColor;

    private ShopManager shopM;
    private DamageFlash flash;
    public bool Dmg = false;
    public string ID;
    public event Action<Enemy> OnDeath;
    public HealthBar pHealtBar => healthBar;
    public int Life => life;
    public bool canShootHim = true;

    [System.Obsolete]
    private void OnEnable()
    {
        shopM = FindObjectOfType<ShopManager>();
        healthBar = GetComponent<HealthBar>();
        flash = GetComponent<DamageFlash>();
        mr = GetComponent<MeshRenderer>();
        
        orgColor = mr.material.color;

        mr.material.color = orgColor;

        currentLife = life;
    }

    private void Update()
    {
        healthBar.UpdateHealthBar(life, currentLife);
    }

    public void Die()
    {
        shopM.AddCoins(5);
        OnDeath?.Invoke(this);  
        EnemyDPool.Instance.ReturnEnemy(ID, this);
    }

    public int RegenHealth(int life)
    {
        life += 1;
        return life;
    }

    public void DoDmg(int damage)
    {
        
    }

    public void ReciveDmg(bool dmg)
    {
        if (canShootHim)
        {
            if (dmg)
            {
                currentLife--;
                healthBar.UpdateHealthBar(life, currentLife);
                flash.StartFlash();

                if (currentLife <= 0)
                {
                    Die();
                }
            }
        } 
    }
}