
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private int itemsToGenerate = 5;
    [SerializeField] private List<TrinketsSO> itemsList = new List<TrinketsSO>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] itemsPos;

    private void Awake()
    {
        int count = Mathf.Min(itemsToGenerate, itemsList.Count);
        Debug.Log($"count = {count}, itemsList.Count = {itemsList.Count}, spawnPoints.Length = {itemsPos.Length}");


        for (int i = 0; i < count; i++)
        {
            TrinketsSO item = itemsList[i];

            Vector3 itemP = itemsPos[i % itemsPos.Length].position;
            GameObject go = Instantiate(prefab, itemP, Quaternion.identity);

            ItemSourceData instance = go.GetComponent<ItemSourceData>();
            if (instance == null)
            {
                instance = go.AddComponent<ItemSourceData>();
            }

            instance.SetData(item);
        }

        Debug.Log("Se generaron todos los items");
    }
}
