using ED262C;
using System;
using System.Collections;
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
    [SerializeField] private float delaySpawnTime = 0.3f;

    public event Action OnWaveCompleted;

    private List<Enemy> aliveEnemies = new List<Enemy>();
    private SimpleArrayQueue<SpawnData> spawnQueue = new SimpleArrayQueue<SpawnData>();
    private bool areEnemies;
    private bool spawning;

    private struct SpawnData
    {
        public string type;
        public Vector3 pos;
    }

    private void Start()
    {
        SpawnWave();
    }

    public void Update()
    {
        if(!areEnemies && !spawning)
        {
            SpawnWave();
            HandelWave();
        }
    }

    public void SpawnWave()
    {
        aliveEnemies.Clear();
        spawnQueue.Clear();

        for (int i = 0; i < enemysAmount; i++)
        {
            spawnQueue.Enqueue(new SpawnData
            {
                type = names[Random.Range(0, names.Length)],
                pos = GetRandomPosInArea()
            });
        }

        StartCoroutine(SpawnQueueRoutine());
    }

    private IEnumerator SpawnQueueRoutine()
    {
        spawning = true;

        while (!spawnQueue.IsEmpty)
        {
            SpawnData data = spawnQueue.Dequeue();
            Enemy e = factory.CreateEnemy(data.type, data.pos, Quaternion.identity);

            if(e != null)
            {
                aliveEnemies.Add(e);
                e.OnDeath += HandleEnemyDeath;
            }
            yield return new WaitForSeconds(delaySpawnTime);
        }

        areEnemies = true;
        spawning = false;
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