using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private int itemsToGenerate = 5;
    [SerializeField] private List<TrinketsSO> itemsList = new List<TrinketsSO>();
    private List<GameObject> spawnItems = new List<GameObject>();
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform[] itemsPos;

    [SerializeField] private AVL<TrinketsSO> AVLItems = new AVL<TrinketsSO>();

    private void Awake()
    {
        foreach(TrinketsSO t in itemsList)
        {
            if (t != null)
                AVLItems.Insert(t);
        }

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
            if (i >= count) break;
            Debug.Log($"lista: {itemsList.Count} | AVL.Count: {AVLItems.Count} | toGenerate: {itemsToGenerate} | pos: {sortedPos.Length} | count: {count}");
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