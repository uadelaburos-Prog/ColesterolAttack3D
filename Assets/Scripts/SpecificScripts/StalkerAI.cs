using Demonics;
using UnityEngine;

enum StalkerStates
{
    Idle,
    Following,
    AttackPlayer,
}

public class StalkerAI : MonoBehaviour
{
    private StalkerStates states;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask shielderMask;

    [SerializeField] private float followingSpeed = 2f;
    [SerializeField] private float attackSpeed = 6f;

    [SerializeField] private float followingPlayerRadius = 15f;
    [SerializeField] private float followingShielderRadius = 20f;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private float scanInterval = 0.2f;
    [SerializeField] private float shielderStopDistance = 2f;

    [SerializeField] private float nextScanTime;

    private GameObject player;
    private Enemy self;

    private PriorityQueue<StalkerStates> desisions = new PriorityQueue<StalkerStates>(10);

    private Shielder currentShielder;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer);
        float actualVelocity = Mathf.Lerp(followingSpeed, attackSpeed, distanceFactor);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        direction.Normalize();

        Vector3 shielderDir = MoveTowardsShielder();
        states = EvaluateState(distanceToPlayer, shielderDir);

        switch (states)
        {
            case StalkerStates.Idle:
                break;
            case StalkerStates.Following: 
                MoveTowards(shielderDir, actualVelocity);
                break;
            case StalkerStates.AttackPlayer:
                MoveTowards(direction,actualVelocity);
                break;
        }
    }

    private StalkerStates EvaluateState(float distanceToPlayer, Vector3 shielderDir)
    {
        desisions.Clear();

        desisions.Enqueue(StalkerStates.Idle,3);

        if (shielderDir != Vector3.zero)
            desisions.Enqueue(StalkerStates.Following, 2);

        if (distanceToPlayer <= followingPlayerRadius)
            desisions.Enqueue(StalkerStates.AttackPlayer, 1);

        return desisions.Dequeue();
    }

    private Vector3 MoveTowardsShielder()
    {
        if (currentShielder != null)
        {
            var enemyComp = currentShielder.GetComponent<Enemy>();
            if (enemyComp == null || enemyComp.currentLife <= 0)
                currentShielder = null;
        }

        if (currentShielder == null)
        {
            if (Time.time < nextScanTime) return Vector3.zero;
            nextScanTime = Time.time + scanInterval;

            currentShielder = FindClosestShielder();

            if (currentShielder == null)
            {
                Debug.Log("no se encontro al shielder");
                return Vector3.zero;
            }
        }

        Vector3 toShielder = currentShielder.transform.position - transform.position;
        toShielder.y = 0f;

        if (toShielder.sqrMagnitude <= shielderStopDistance * shielderStopDistance)
            return Vector3.zero;

        return toShielder.normalized;
    }

    private Shielder FindClosestShielder()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, followingShielderRadius, shielderMask);

        Shielder best = null;
        float bestSqr = float.MaxValue;

        foreach (var col in hits)
        {
            var s = col.GetComponent<Shielder>();
            if (s == null) continue;

            var enemyComp = s.GetComponent<Enemy>();
            if (enemyComp == null || enemyComp.currentLife <= 0) continue;

            float sqr = (s.transform.position - transform.position).sqrMagnitude;
            if (sqr < bestSqr) { bestSqr = sqr; best = s; }
        }
        return best;
    }

    private void MoveTowards(Vector3 direc, float velocity)
    {
        transform.position += direc * velocity * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, followingShielderRadius);
    }
}
