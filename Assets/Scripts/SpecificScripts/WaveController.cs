using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ItemGenerator itemGenerator;
    [SerializeField] private string[] names;
    [SerializeField] private int enemysAmount = 5;
    [SerializeField] private int waveCount = 1;
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private EnemyFactory factory;

    public event Action OnWaveCompleted;

    private List<Enemy> aliveEnemies = new List<Enemy>();
    private bool areEnemies;

    private void Start()
    {
        SpawnWave();
    }

    public void Update()
    {
        if(!areEnemies)
        {
            SpawnWave();
            HandelWave();
        }
    }

    public void SpawnWave()
    {
        aliveEnemies.Clear();

        for (int i = 0; i < enemysAmount; i++)
        {
            Vector3 ramdomPos = GetRandomPosInArea();
            string type = names[Random.Range(0, names.Length)];
            Enemy e = factory.CreateEnemy(type, ramdomPos, Quaternion.identity);

            if(e != null)
            {
                aliveEnemies.Add(e);
                e.OnDeath += HandleEnemyDeath;
            }
        }
        areEnemies = true;
    }

    private void HandleEnemyDeath(Enemy e)
    {
        e.OnDeath -= HandleEnemyDeath;
        aliveEnemies.Remove(e);

        if(aliveEnemies.Count <= 0)
        {
            enemysAmount += 2;
            OnWaveCompleted?.Invoke();
            waveCount++;
            areEnemies = false;
        }
    }

    private void HandelWave()
    {
        shopManager.ClearPurchasedItems();
        itemGenerator.GenerateItems();
    }

    private Vector3 GetRandomPosInArea()
    {
        float x = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float z = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);
        return spawnCenter.position + new Vector3(x, 0f, z);
    }
}