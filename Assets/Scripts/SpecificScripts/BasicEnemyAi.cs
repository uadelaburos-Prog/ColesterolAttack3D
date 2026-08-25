using UnityEngine;
using System;

public class BasicEnemyAi : MonoBehaviour
{
    [SerializeField] private GameObject playerTransform;
    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        playerTransform = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.transform.position, speed * Time.deltaTime);
    }   
}
