using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using NUnit.Framework;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private List<TrinketsSO> itemList = new List<TrinketsSO>();
    private Dictionary<string, TrinketsSO> itemDictionary = new Dictionary<string, TrinketsSO>();
    private HashSet<string> purchasedItems = new HashSet<string>();


    [Header("Variables")]
    [SerializeField] private int coins;

    private void Start()
    {

        for(int i = 0; i < itemList.Count; i++)
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

        coins -= item.price;
        purchasedItems.Add(id);

        item.ApplyEffect(playerStats);

        return true;
    }

    public bool WasPurchased(string id) => purchasedItems.Contains(id);

    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log($"Coins: {coins}");
    }

}
