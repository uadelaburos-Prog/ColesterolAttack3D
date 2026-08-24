using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "shopMenu", menuName = "TrinketsSO/New Trinket", order = 0)]
public class TrinketsSO : ScriptableObject
{
    public enum StatType
    {
        Damage,
        Health,
        Speed,
        WeaponCooldown
    }

    [SerializeField] private string itemID;
    public string ItemID => itemID;

    [Header("Stat")]
    public StatType statType;
    [SerializeField] private string itemName;
    public string ItemName => itemName;

    public int price;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemID))
            itemID = Guid.NewGuid().ToString();

        itemName = GenerateName(statType);
    }

    public void ApplyEffect(PlayerStats stats)
    {
        switch (statType)
        {
            case StatType.Damage:
                stats.playerDamage++;
                stats.pricePlayerDamage += 5;
                break;
            case StatType.Health:
                stats.playerHealth++;
                stats.pricePlayerHealth += 5;
                break;
            case StatType.Speed:
                stats.playerSpeed++;
                stats.pricePlayerSpeed += 5;
                break;
            case StatType.WeaponCooldown:
                stats.weaponCoolDown -= 0.05f;
                stats.priceWeaponCoolDown += 5;
                break;
        }
    }

    private string GenerateName(StatType type)
    {
        return type switch
        {
            StatType.Damage => "Trinket de " + "Damage",
            StatType.Health => "Trinket de " + "Health",
            StatType.Speed => "Trinket de " + "Speed",
            StatType.WeaponCooldown => "Trinket de " + "Weapon CoolDown",
            _ => "Trinket Desconocido"
        };
    }
}
