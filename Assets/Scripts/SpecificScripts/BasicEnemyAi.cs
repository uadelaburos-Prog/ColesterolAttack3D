using UnityEngine;
using System;

public class BasicEnemyAi : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }
}
