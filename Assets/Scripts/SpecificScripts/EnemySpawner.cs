using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WaveController waveController;
    [SerializeField] private EnemyFactory factory;
    [SerializeField] private GameObject[] target;
    [SerializeField] private string[] names;

    private void Start()
    {
        for (int i = 0; i < waveController.EnemysAmount; i++)
        {
            string tipo = names[Random.Range(0, names.Length)];
            factory.CreateEnemy(tipo, target[i].transform);
        }
    }
}
