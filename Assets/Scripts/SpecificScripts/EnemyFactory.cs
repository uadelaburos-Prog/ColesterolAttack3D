using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemylist = new List<Enemy>();
    private Dictionary<string, Enemy> enemyDiccionary = new Dictionary<string, Enemy>();

    public List<Enemy> EnemyList => enemylist;

    void Start()
    {
        for(int i = 0; i < enemylist.Count; i++)
        {
            enemyDiccionary.Add(enemylist[i].ID, enemylist[i]);
        }
    }
    
    public Enemy CreateEnemy(string enemyType, Transform position)
    {
        if (enemyDiccionary.ContainsKey(enemyType))
        {
            return Instantiate(enemyDiccionary[enemyType], position.position, position.rotation);
        }
        else
            return null;
    }
}
