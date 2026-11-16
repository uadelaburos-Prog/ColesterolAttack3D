using Demonics;
using UnityEngine;

public class StalkerAI : MonoBehaviour
{
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

    [SerializeField] private AStarMovement aStarMovement;

    private GameObject player;
    private Enemy self;

    private readonly StateMachine machine = new StateMachine();
    private IEnemyState idle;
    private IEnemyState following;
    private IEnemyState attackPlayer;

    private PriorityQueue<IEnemyState> desisions = new PriorityQueue<IEnemyState>(10);

    private Shielder currentShielder;

    // Datos que se recalculan en cada Update y leen los estados
    private Vector3 playerDir;
    private Vector3 currentShielderDir;
    private float currentSpeed;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player");

        idle = new IdleState();
        following = new FollowingState(this);
        attackPlayer = new AttackPlayerState(this);
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        if (self != null)
            self.canShootHim = true;

        //Astar Movement
        if (aStarMovement != null) aStarMovement.ResetPath();

        machine.ChangeState(idle);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer);
        currentSpeed = Mathf.Lerp(followingSpeed, attackSpeed, distanceFactor);

        playerDir = (player.transform.position - transform.position).normalized;
        playerDir.y = 0;
        playerDir.Normalize();

        currentShielderDir = MoveTowardsShielder();

        IEnemyState next = EvaluateState(distanceToPlayer, currentShielderDir);
        if (next != machine.Current)
            machine.ChangeState(next);

        machine.Tick();
    }

    private IEnemyState EvaluateState(float distanceToPlayer, Vector3 shielderDir)
    {
        desisions.Clear();

        desisions.Enqueue(idle, 3);

        if (shielderDir != Vector3.zero)
            desisions.Enqueue(following, 2);

        if (distanceToPlayer <= followingPlayerRadius)
            desisions.Enqueue(attackPlayer, 1);

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
                return Vector3.zero;
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, followingShielderRadius);
    }

    private class IdleState : IEnemyState
    {
        public void Enter() { }
        public void Exit() { }
        public void Tick() { }
    }

    private class FollowingState : IEnemyState
    {
        private readonly StalkerAI owner;
        public FollowingState(StalkerAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.currentShielderDir, owner.currentSpeed);
    }

    private class AttackPlayerState : IEnemyState
    {
        private readonly StalkerAI owner;
        public AttackPlayerState(StalkerAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.playerDir, owner.currentSpeed);
    }
}