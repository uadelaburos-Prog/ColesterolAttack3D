using ED262C;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyDPool : MonoBehaviour
{
    [SerializeField] private EnemyFactory _factory;
    [SerializeField] private WaveController _waveController;

    private readonly Dictionary<string, SimpleArrayQueue<Enemy>> _pool = new();

    private static EnemyDPool instance;
    public static EnemyDPool Instance => instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }   

    private void Start()
    {
        AddEnemysToThePool(_waveController.Amount);
    }

    private void AddEnemysToThePool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            string type = _waveController.Names[Random.Range(0, _waveController.Names.Length)]; // cambiar al wavecontroller

            Enemy e = _factory.CreateEnemy(type, _waveController.GetRandomPosInArea(), quaternion.identity);
            e.gameObject.SetActive(false);
            
            if(!_pool.TryGetValue(type, out var queue))
            {
                queue = new SimpleArrayQueue<Enemy>();
                _pool.Add(type, queue);
            }
            queue.Enqueue(e);
        }
    }
    
    public Enemy GetEnemy(string type, Vector3 pos)
    {
        Enemy e;

        if(_pool.TryGetValue(type, out var queue) && queue.Count > 0)
        {
            e = queue.Dequeue();
            e.transform.position = pos;
        }
        else
        {
            e = _factory.CreateEnemy(type, pos, quaternion.identity);
        }

        e.gameObject.SetActive(true);
        return e;
    } 

    public void ReturnEnemy(string type, Enemy enemy)
    {
        enemy.gameObject.SetActive(false);

        if(!_pool.TryGetValue(type, out var queue))
        {
            queue = new SimpleArrayQueue<Enemy>();
            _pool[type] = queue;
        }
        queue.Enqueue(enemy);
    }  
}
