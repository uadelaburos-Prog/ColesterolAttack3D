using System;
using UnityEngine;

public class ItemSourceData : MonoBehaviour
{
    public TrinketsSO sourceData;
    public string itemID;
    public int cost;

    public void SetData(TrinketsSO data)
    {
        sourceData = data;
        itemID = data.ItemID;
        cost = data.price;
    }
}
