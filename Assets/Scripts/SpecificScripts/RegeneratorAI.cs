using UnityEngine;
using System.Collections;
using ED262C;
using System.Collections.Generic;

enum EnemyStates
{
    Idle,
    Regenerating,
    Chasing,
};

public class RegeneratorAI : MonoBehaviour
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
    [SerializeField] private bool isMooving = false;

    [SerializeField] private List<GameObject> spheresList = new List<GameObject>();
    private SimpleArrayStack<GameObject> spheresStack = new SimpleArrayStack<GameObject>();

    private void Awake()
    {
        SpheresListToStack();

        self = GetComponent<Enemy>();
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
        direction.Normalize();

        if (self.currentLife == 1 && !isMooving && !spheresStack.IsEmpty)
        {
            StartCoroutine(Regenlife(self.currentLife, 3.5f,1.5f));
        }

        if (spheresStack.IsEmpty)
        {
            StartCoroutine(RegenSpheres());
        }

        switch (enemyState)
        {
            case EnemyStates.Idle:
                enemyState = EnemyStates.Chasing;
                break;
            case EnemyStates.Regenerating:

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

    private IEnumerator Regenlife(int life, float waitingTime, float secWaitingTime)
    {
        enemyState = EnemyStates.Regenerating;
        isMooving = true;
        self.canShootHim = false;

        yield return new WaitForSeconds(waitingTime);

        ConsumeSpheres();
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life,self.currentLife);

        yield return new WaitForSeconds(waitingTime);

        ConsumeSpheres();
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life, self.currentLife);

        yield return new WaitForSeconds(secWaitingTime);

        isMooving = false;
        self.canShootHim = true;
        enemyState = EnemyStates.Chasing;
    }

    private IEnumerator RegenSpheres()
    {
        self.canShootHim = false;
        isMooving = true;
        enemyState = EnemyStates.Regenerating;

        yield return new WaitForSeconds(5);

        SpheresListToStack();
        RegenDeactiveSpheres();

        yield return new WaitForSeconds(2);

        enemyState = EnemyStates.Chasing;
        isMooving = false;
        self.canShootHim = true;
    }

    private void ConsumeSpheres()
    {
        if (spheresStack.IsEmpty)
        {
            return;
        }

        GameObject sphere = spheresStack.Pop();
        sphere.SetActive(false);
    }

    private void RegenDeactiveSpheres()
    {
        for (int i = 0; i < spheresStack.Count; i++)
        {
            GameObject s = spheresStack.Pop();
            s.SetActive(true);
        }
    }

    private void SpheresListToStack()
    {
        for (int i = 0; i < spheresList.Count; i++)
        {
            spheresStack.Push(spheresList[i]);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, walkingRadius);
    }
}