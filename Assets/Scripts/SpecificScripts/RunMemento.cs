using UnityEngine;

public class RunMemento
{
    //Enemigos, Monedas, Cantidad de Oleadas
    public int WaveCount { get; }
    public int EnemysAmount { get; }
    public int Coins { get; }

    //Stats del Player
    public int PlayerDamage { get; } 
    public int PlayerHealth { get; }
    public float PlayerSpeed { get; }
    public float WeaponCoolDown { get; }

    // Precio de mejoras de Stats
    public int PriceplayerDamage { get; }
    public int PricePlayerHealth { get; }
    public float PricePlayerSpeed { get; }
    public int PriceWeaponCoolDown { get; }

    public RunMemento(int waveCount, int enemysAmount, int coins, int playerDamage, int playerHealth, float playerSpeed, float weaponCoolDown, int priceplayerDamage, int pricePlayerHealth, float pricePlayerSpeed, int priceWeaponCoolDown)
    {
        WaveCount = waveCount;
        EnemysAmount = enemysAmount;
        Coins = coins;
        PlayerDamage = playerDamage;
        PlayerHealth = playerHealth;
        PlayerSpeed = playerSpeed;
        WeaponCoolDown = weaponCoolDown;
        PriceplayerDamage = priceplayerDamage;
        PricePlayerHealth = pricePlayerHealth;
        PricePlayerSpeed = pricePlayerSpeed;
        PriceWeaponCoolDown = priceWeaponCoolDown;
    }
}