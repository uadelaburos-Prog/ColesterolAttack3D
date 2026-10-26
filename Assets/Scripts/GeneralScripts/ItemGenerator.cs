using System;
using System.Collections.Generic;
using UnityEngine;
using ED262C;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private int itemsToGenerate = 5;
    [SerializeField] private List<TrinketsSO> itemsList = new List<TrinketsSO>();
    private List<GameObject> spawnItems = new List<GameObject>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] itemsPos;

    private AVL<TrinketsSO> AVLItems = new AVL<TrinketsSO>();

    private void Awake()
    {
        GenerateItems();
    }

    public void GenerateItems()
    {
        ClearPrevItems();

        Transform[] sortedPos = itemsPos.OrderBy(t => t.transform.position.x).ToArray();

        int count = Mathf.Min(itemsToGenerate, AVLItems.Count);
        int i = 0;

        foreach(TrinketsSO item in AVLItems.InOrder())
        {
            if (i > count) break;

            Vector3 itemP = sortedPos[i % sortedPos.Length].position;
            GameObject go = Instantiate(prefab, itemP, Quaternion.identity);
            ItemSourceData instance = go.GetComponent<ItemSourceData>();

            if(instance == null)
            {
                instance = go.AddComponent<ItemSourceData>();
            }

            instance.SetData(item);
            spawnItems.Add(go);
            i++;
        }
    }

    private void ClearPrevItems()
    {
        foreach (var go in spawnItems)
        {
            if (go != null) Destroy(go);
        }
        spawnItems.Clear();
    }
}