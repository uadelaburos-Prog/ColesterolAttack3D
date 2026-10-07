using System.Collections.Generic;
using UnityEngine;
using Demonics;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemylist = new List<Enemy>();
    // private Dictionary<string, Enemy> enemyDiccionary = new Dictionary<string, Enemy>();
    private SimpleArrayDictionary<string, Enemy> enemyDictionary = new(); 

    public List<Enemy> EnemyList => enemylist;

    void Awake()
    {
        for(int i = 0; i < enemylist.Count; i++)
        {
            enemyDictionary.TryAdd(enemylist[i].ID, enemylist[i]);
        }
    }
    
    public Enemy CreateEnemy(string enemyType, Vector3 position, Quaternion rotation) // Factory: crea x cosa al ser llamado y dandole los datos necesarios
    {
        if (enemyDictionary.ContainsKey(enemyType))
        {
            return Instantiate(enemyDictionary[enemyType], position, rotation);
        }
        else
            return null;
    }
}