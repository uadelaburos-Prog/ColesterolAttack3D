using System.Collections.Generic;
using UnityEngine;
using Demonics;
using ED262C;
using System;

public class StatOrdener : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    private SimpleArrayList<float> statsList = new();

    private CollectionSorting<float> sorting = new();

    private Comparison<float> comparison;

    private void Start()
    {
        comparison = (a, b) => b.CompareTo(a);
    }

    private void StatsCharge()
    {
        statsList.Clear();

        statsList.Add(stats.pricePlayerDamage);
        statsList.Add(stats.pricePlayerHealth);
        statsList.Add(stats.pricePlayerSpeed);
        statsList.Add(stats.priceWeaponCoolDown);
    }

    public void ShowStats()
    {
        StatsCharge();
        sorting.SortList(statsList, comparison);

        for (int i = 0; i < statsList.Count; i++)
            Debug.Log(statsList[i]);
    }
}
