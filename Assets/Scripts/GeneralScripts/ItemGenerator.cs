
using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private int itemsToGenerate = 5;
    [SerializeField] private List<TrinketsSO> itemsList = new List<TrinketsSO>();
    private List<GameObject> spawnItems = new List<GameObject>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] itemsPos;

    private void Awake()
    {
        GenerateItems();
    }

    public void GenerateItems()
    {
        ClearPrevItems();

        int count = Mathf.Min(itemsToGenerate, itemsList.Count);

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
            spawnItems.Add(go);
        }

        Debug.Log("Se generaron todos los items");
    }

    private void ClearPrevItems()
    {
        foreach(var go in spawnItems)
        {
            if (go != null) Destroy(go);
        }
        spawnItems.Clear();
    }
}
