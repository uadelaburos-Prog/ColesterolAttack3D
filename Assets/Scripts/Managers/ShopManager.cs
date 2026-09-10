using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;

    [SerializeField] private List<TrinketsSO> itemList = new List<TrinketsSO>();
    private Dictionary<string, TrinketsSO> itemDictionary = new Dictionary<string, TrinketsSO>();
    private HashSet<string> purchasedItems = new HashSet<string>();

    [Header("Variables")]
    [SerializeField] private int coins;
    [SerializeField] private TextMeshProUGUI coinsUi;

    private void Awake()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemDictionary.Add(itemList[i].ItemID, itemList[i]);
        }

        UpdateCoinsUI();
    }

    public bool TryToPurchItem(string id)
    {
        if (!itemDictionary.TryGetValue(id, out TrinketsSO item))
        {
            return false;
        }
        if (purchasedItems.Contains(id))
        {
            return false;
        }
        if (coins < item.price)
        {
            return false;
        }

        RemoveCoins(item.price);
        purchasedItems.Add(id);

        item.ApplyEffect(playerStats);

        return true;
    }
    private void UpdateCoinsUI()
    {
        coinsUi.text = coins.ToString();
    }

    public bool WasPurchased(string id) => purchasedItems.Contains(id);

    public void ClearPurchasedItems()
    {
        purchasedItems.Clear();
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