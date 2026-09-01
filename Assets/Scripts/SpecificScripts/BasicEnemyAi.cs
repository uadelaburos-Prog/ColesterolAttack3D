using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.AdaptivePerformance;

enum EnemyStates
{
    Idle,
    LowHealth,
    Chasing,
};

public class BasicEnemyAi : MonoBehaviour
{
    private EnemyStates enemyState;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float chasingSpeed = 7.5f;

    [SerializeField] private float walkingRadius = 20;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 15f;

    private GameObject player;
    private Enemy self;
    private bool isMooving = false;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        Debug.Log($"Se obtuvo el componente {self}");
    }

    private void Start()
    {
        enemyState = EnemyStates.Idle;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position,player.transform.position);
        float distanceFactor = Mathf.InverseLerp(minDistance,maxDistance, distanceToPlayer);
        float actualVelocity = Mathf.Lerp(walkingSpeed, chasingSpeed,distanceFactor);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        if (self.currentLife == 1)
        {
            StartCoroutine(Regenlife(self.currentLife, 3.5f,1.5f, enemyState));
        }

        switch (enemyState)
        {
            case EnemyStates.Idle:
                enemyState = EnemyStates.Chasing;
                break;
            case EnemyStates.LowHealth:

                break;
            case EnemyStates.Chasing:
                if(!isMooving)
                    MoveTowardsPos(direction, actualVelocity);
                break;
        }   

    }

    private void MoveTowardsPos(Vector3 direction, float actualVelocity)
    {
        transform.position += direction * actualVelocity * Time.deltaTime;
    }

    private IEnumerator Regenlife(int life, float waitingTime, float secWaitingTime, EnemyStates eS)
    {
        eS = EnemyStates.LowHealth;
        isMooving = true;
        self.canShootHim = false;
        yield return new WaitForSeconds(waitingTime);
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life,self.currentLife);
        yield return new WaitForSeconds(waitingTime);
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life, self.currentLife);
        yield return new WaitForSeconds(secWaitingTime);
        isMooving = false;
        self.canShootHim = true;
        eS = EnemyStates.Chasing;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.crimson;
        Gizmos.DrawWireSphere(transform.position, walkingRadius);
    }
}