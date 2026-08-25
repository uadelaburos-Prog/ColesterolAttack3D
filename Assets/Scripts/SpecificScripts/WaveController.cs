using ED262C;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private int enemysAmount = 5;

    [SerializeField] private EnemyFactory factory;

    public int EnemysAmount => enemysAmount;

    private void Start()
    {

    }
}
