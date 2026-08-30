using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemylist = new List<Enemy>();
    private Dictionary<string, Enemy> enemyDiccionary = new Dictionary<string, Enemy>();

    public List<Enemy> EnemyList => enemylist;

    void Awake()
    {
        for(int i = 0; i < enemylist.Count; i++)
        {
            enemyDiccionary.TryAdd(enemylist[i].ID, enemylist[i]);
        }
    }
    
    public Enemy CreateEnemy(string enemyType, Vector3 position, Quaternion rotation)
    {
        if (enemyDiccionary.ContainsKey(enemyType))
        {
            return Instantiate(enemyDiccionary[enemyType], position, rotation);
        }
        else
            return null;
    }
}
