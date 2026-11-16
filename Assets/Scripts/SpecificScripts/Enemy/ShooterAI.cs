using UnityEngine;
using UnityEngine.AI;

public class ShooterAI : MonoBehaviour
{
    private readonly StateMachine machine = new StateMachine();
    private IEnemyState keepDistance;
    private IEnemyState charging;

    [SerializeField] private float minDistance = 6f;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float retreatSpeed = 3f;
    [SerializeField] private float agroSpeed = 10f;

    [SerializeField] private int maxAmmo = 8;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private Transform firePoint;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float eyeHeight = 1.6f;
    [SerializeField] private float targetHeight = 1.2f;
    [SerializeField] private float retreatCheckDistance = 1f;

    [SerializeField] private AStarMovement aStarMovement;

    private GameObject player;
    private Enemy self;
    private int ammo;
    private float nextFireTime;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player");

        keepDistance = new KeepDistanceState(this);
        charging = new ChargingState(this);
    }

    private void OnEnable()
    {
        ammo = maxAmmo;
        nextFireTime = 0f;

        if (self != null)
            self.canShootHim = true;

        if (aStarMovement != null) aStarMovement.ResetPath();

        machine.ChangeState(keepDistance);
    }

    private void Update()
    {
        machine.Tick();
    }

    private Vector3 DirectionToPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        return direction.normalized;
    }

    private bool HasLineOfSight()
    {
        Vector3 origin = transform.position + Vector3.up * eyeHeight;
        Vector3 target = player.transform.position + Vector3.up * targetHeight;
        Vector3 dir = target - origin;

        if (Physics.Raycast(origin, dir.normalized, out RaycastHit hit, dir.magnitude, obstacleMask, QueryTriggerInteraction.Ignore))
            return hit.transform == player.transform || hit.transform.IsChildOf(player.transform);

        return true;
    }

    private void FacePlayer()
    {
        Vector3 dir = DirectionToPlayer();
        if (dir.sqrMagnitude < 0.001f) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 8f * Time.deltaTime);
    }

    private void Retreat()
    {
        Vector3 away = -DirectionToPlayer();

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, away, retreatCheckDistance, obstacleMask, QueryTriggerInteraction.Ignore))
            return;

        transform.position += away * retreatSpeed * Time.deltaTime;
    }

    private void Shoot()
    {
        nextFireTime = Time.time + fireRate;
        ammo--;

        Vector3 target = player.transform.position + Vector3.up * targetHeight;
        firePoint.LookAt(target);
        BulletPool.Instance.GetBullet(firePoint);

        if (ammo <= 0)
            machine.ChangeState(charging);
    }

    private class KeepDistanceState : IEnemyState
    {
        private readonly ShooterAI owner;
        public KeepDistanceState(ShooterAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick()
        {
            float dist = Vector3.Distance(owner.transform.position, owner.player.transform.position);

            if (!owner.HasLineOfSight())
            {
                owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.chasingSpeed);
                return;
            }

            owner.FacePlayer();

            if (dist < owner.minDistance)
                owner.Retreat();
            else if (dist > owner.maxDistance)
                owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.chasingSpeed);

            if (dist <= owner.maxDistance && Time.time >= owner.nextFireTime)
                owner.Shoot();
        }
    }

    private class ChargingState : IEnemyState
    {
        private readonly ShooterAI owner;
        public ChargingState(ShooterAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.agroSpeed);
    }
}
