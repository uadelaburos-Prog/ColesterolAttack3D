using System.Collections;
using UnityEngine;

public class Shielder : MonoBehaviour
{
    private readonly StateMachine machine = new StateMachine();
    private IEnemyState chasing;
    private IEnemyState shieldDown;
    private IEnemyState agro;

    [SerializeField] private LayerMask playerMask;

    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float agroSpeed = 10f;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldLife = 6;
    private DamageFlash flash;
    private float initialShieldLife;
    private bool shieldReciveDmg;

    [SerializeField] private AStarMovement aStarMovement;

    private GameObject player;
    private Enemy self;

    private void Awake()
    {
        self = GetComponent<Enemy>();
        flash = GetComponent<DamageFlash>();
        initialShieldLife = shieldLife;
        player = GameObject.Find("Player");

        chasing = new ChasingState(this);
        shieldDown = new ShieldDownState(this);
        agro = new AgroState(this);
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        shieldLife = initialShieldLife;

        if (shield != null)
            shield.SetActive(true);

        if (self != null)
            self.canShootHim = false;

        if (aStarMovement != null) aStarMovement.ResetPath();

        machine.ChangeState(chasing);
    }

    private void Update()
    {
        if (shieldLife <= 0 && machine.Current != shieldDown && machine.Current != agro)
            machine.ChangeState(shieldDown);

        machine.Tick();
    }

    private Vector3 DirectionToPlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        return direction.normalized;
    }

    private float ChasingVelocity()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer);
        return Mathf.Lerp(chasingSpeed, agroSpeed, distanceFactor);
    }
    private IEnumerator ShieldDownRutine(float agroPassTime)
    {
        yield return new WaitForSeconds(agroPassTime);

        self.canShootHim = true;
        machine.ChangeState(agro);
    }

    public void ShieldDmg(bool reciveDmg)
    {
        shieldReciveDmg = reciveDmg;
        if (shieldReciveDmg)
        {
            shieldLife -= 1;
            flash.StartFlash();
        }
        else
            reciveDmg = false;
    }

    private class ChasingState : IEnemyState
    {
        private readonly Shielder owner;
        public ChasingState(Shielder owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.ChasingVelocity());
    }

    private class ShieldDownState : IEnemyState
    {
        private readonly Shielder owner;
        public ShieldDownState(Shielder owner) { this.owner = owner; }

        public void Enter()
        {
            owner.shield.SetActive(false);
            owner.StartCoroutine(owner.ShieldDownRutine(2f));
        }
        public void Exit() { }
        public void Tick() { }
    }

    private class AgroState : IEnemyState
    {
        private readonly Shielder owner;
        public AgroState(Shielder owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.agroSpeed);
    }
}