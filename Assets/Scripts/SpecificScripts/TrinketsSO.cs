using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "shopMenu", menuName = "TrinketsSO/New Trinket", order = 0)]
public class TrinketsSO : ScriptableObject, IComparable // Scriptable Object: objecto que se puede crear multiples instancias de uno con distintos datos entre si
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

    [FormerlySerializedAs("price")]
    [SerializeField] private int basePrice;

    public int BasePrice => basePrice;

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

    public int GetPrice(PlayerStats stats) => statType switch
    {
        StatType.Damage => stats.pricePlayerDamage,
        StatType.Health => stats.pricePlayerHealth,
        StatType.WeaponCooldown => (int)stats.priceWeaponCoolDown,
        StatType.Speed => (int)stats.pricePlayerSpeed,
        _ => basePrice
    };

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

    public int CompareTo(object obj)
    {
        if (obj is not TrinketsSO other) return 1;
        int c = other.BasePrice.CompareTo(BasePrice);
        return c != 0 ? c : GetInstanceID().CompareTo(other.GetInstanceID());
    }
}