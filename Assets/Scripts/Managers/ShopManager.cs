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

    [Header("Variables")]
    [SerializeField] private int coins;
    [SerializeField] private TMP_Text coinIU;

    [SerializeField] private Button[] purchBtm;
    [SerializeField] private TMP_Text[] title;
    [SerializeField] private TMP_Text[] price;

    private struct Upgrade
    {
        public string name;
        public Func<float> currentPrice;
        public Action applyUpgrade;
    }

    private Upgrade[] upgrades;

    private void Awake() => upgrades = new Upgrade[]
        {
            new Upgrade
            {
                name = playerStats.damage,
                currentPrice = () => playerStats.pricePlayerDamage,
                applyUpgrade = () =>
                {
                    playerStats.playerDamage++;
                    playerStats.pricePlayerDamage += 5;
                }
            },

            new Upgrade
            {
                name = playerStats.health,
                currentPrice = () => playerStats.pricePlayerHealth,
                applyUpgrade = () =>
                {
                    playerStats.playerHealth++;
                    playerStats.pricePlayerHealth += 5;
                }
            },

            new Upgrade
            {
                name = playerStats.speed,
                currentPrice = () => playerStats.pricePlayerSpeed,
                applyUpgrade = () =>
                {
                    playerStats.playerSpeed++;
                    playerStats.pricePlayerSpeed += 5;
                }
            },

            new Upgrade
            {
                name = playerStats.weapon,
                currentPrice = () => playerStats.weaponCoolDown,
                applyUpgrade = () =>
                {
                    playerStats.weaponCoolDown -= 0.05f;
                    playerStats.priceWeaponCoolDown += 5;
                }
            },
        };


    private void Start()
    {
        coinIU.text = "Coins: " + coins.ToString();
        SetUI();
    }

    private void SetUI()
    {
        for(int i = 0; i < upgrades.Length; i++)
        {
            int index = i;

            title[i].text = upgrades[i].name;
            price[i].text = "Price: " + upgrades[i].currentPrice().ToString();

            purchBtm[i].onClick.RemoveAllListeners();
            purchBtm[i].onClick.AddListener(() => TryToPurch(index));
        }
    }

    private void TryToPurch(int index)
    {
        float cost = upgrades[index].currentPrice();

        if(coins < cost)
        {
            Debug.Log("pobre");
            return;
        }

        coins -= (int)cost;
        coinIU.text = $"Coins: {coins.ToString()}";

        upgrades[index].applyUpgrade();
        price[index].text = upgrades[index].currentPrice().ToString();  
    }

    public void AddCoins()
    {
        coins++;
        coinIU.text = "Coins: " + coins.ToString();
    }

}
