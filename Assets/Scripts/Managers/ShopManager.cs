using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Demonics;
public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private List<TrinketsSO> itemList = new List<TrinketsSO>();
    //private Dictionary<string, TrinketsSO> itemDictionary = new Dictionary<string, TrinketsSO>();
    private SimpleArrayDictionary<string, TrinketsSO> itemDictionary2 = new SimpleArrayDictionary<string, TrinketsSO>();
    //private HashSet<string> purchasedItems = new HashSet<string>();
    private SimpleArraySet<string> purchasedItems2 = new SimpleArraySet<string>(); 

    [Header("Variables")]
    [SerializeField] private int coins;
    [SerializeField] private TextMeshProUGUI coinsUi;

    private void Awake()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemDictionary2.Add(itemList[i].ItemID, itemList[i]);
        }

        UpdateCoinsUI();
    }

    public bool TryToPurchItem(string id)
    {
        if (!itemDictionary2.TryGetValue(id, out TrinketsSO item))
        {
            return false;
        }
        if (purchasedItems2.Contains(id))
        {
            return false;
        }
        if (coins < item.price)
        {
            return false;
        }

        RemoveCoins(item.price);
        //purchasedItems.Add(id);
        purchasedItems2.Add(id);

        item.ApplyEffect(playerStats);

        return true;
    }
    private void UpdateCoinsUI()
    {
        coinsUi.text = coins.ToString();
    }

    //public bool WasPurchased(string id) => purchasedItems.Contains(id);
    public bool WasPurchased2(string id) => purchasedItems2.Contains(id);

    public void ClearPurchasedItems()
    {
        //purchasedItems.Clear();
        purchasedItems2.Clear();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinsUI();
    }

    public void RemoveCoins(int amount)
    {
        coins = Mathf.Max(0, coins - amount);
        UpdateCoinsUI();
    }
}