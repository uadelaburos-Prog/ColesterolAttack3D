using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private List<TrinketsSO> itemList = new List<TrinketsSO>();
    private Dictionary<string, TrinketsSO> itemDictionary = new Dictionary<string, TrinketsSO>();
    private HashSet<string> purchasedItems = new HashSet<string>();

    [Header("Variables")]
    [SerializeField] private int coins;
    [SerializeField] private TextMeshProUGUI coinsUi;

    private void Start()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemDictionary.Add(itemList[i].name, itemList[i]);
        }
    }

    public bool TryToPurchItem(string id, int uiIndex = -1)
    {
        if (!itemDictionary.TryGetValue(id, out TrinketsSO item))
        {
            return false;
        }

        if(purchasedItems.Contains(id))
        {
            return false;
        }

        if(coins < item.price)
        {
            return false;
        }

        RemoveCoins(item.price);
        purchasedItems.Add(id);

        item.ApplyEffect(playerStats);

        return true;
    }

    public bool WasPurchased(string id) => purchasedItems.Contains(id);

    public void AddCoins(int amount)
    {
        coins += amount;
        coinsUi.text = coins.ToString();
    }

    public void RemoveCoins(int amount)
    {
        coins -= amount;
        coinsUi.text = coins.ToString();
    }

}
