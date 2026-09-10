using UnityEngine;

public class PlayerStats : MonoBehaviour 
{
    public string damage = "Damage";
    public int playerDamage { get; set; } = 1;

    public int pricePlayerDamage { get; set; } = 5;

    public string health = "Health";
    public int playerHealth { get; set; } = 3;

    public int pricePlayerHealth { get; set; } = 10;

    public string speed = "Speed";
    public float playerSpeed { get; set; } = 10f;

    public float pricePlayerSpeed { get; set; } = 5f;

    public string weapon = "Weapon CoolDown";
    public float weaponCoolDown { get; set; } = 0.5f;

    public int priceWeaponCoolDown { get; set; } = 5;
}