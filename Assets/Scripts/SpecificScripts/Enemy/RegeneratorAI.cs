using UnityEngine;
using System.Collections;
using ED262C;
using System.Collections.Generic;

public class RegeneratorAI : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float chasingSpeed = 7.5f;

    [SerializeField] private float walkingRadius = 20;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 15f;

    [SerializeField] private AStarMovement aStarMovement;
    private GameObject player;
    private Enemy self;

    [SerializeField] private List<GameObject> spheresList = new List<GameObject>();
    private SimpleArrayStack<GameObject> spheresStack = new SimpleArrayStack<GameObject>();

    private readonly StateMachine machine = new StateMachine();
    private IEnemyState idle;
    private IEnemyState regenerating;
    private IEnemyState chasing;

    public bool IsRegenerating => machine.Current == regenerating;

    public void MeleeExecute()
    {
        StopAllCoroutines();
        self.Die();
    }

    private void Awake()
    {
        self = GetComponent<Enemy>();
        player = GameObject.Find("Player");

        idle = new IdleState(this);
        regenerating = new RegeneratingState(this);
        chasing = new ChasingState(this);
    }

    private void OnEnable()
    {
        StopAllCoroutines();

        if (self != null)
            self.canShootHim = true;

        // Reponer las esferas de regeneración al estado inicial
        spheresStack.Clear();
        SpheresListToStack();
        RegenDeactiveSpheres();

        //Astar Movement
        if (aStarMovement != null) aStarMovement.ResetPath();

        machine.ChangeState(idle);
    }

    private void Update()
    {
        if (machine.Current != regenerating)
        {
            if (self.currentLife == 1 && !spheresStack.IsEmpty)
            {
                StartCoroutine(Regenlife(self.currentLife, 3.5f, 1.5f));
            }
            else if (spheresStack.IsEmpty)
            {
                StartCoroutine(RegenSpheres());
            }
        }

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
        return Mathf.Lerp(walkingSpeed, chasingSpeed, distanceFactor);
    }

    private IEnumerator Regenlife(int life, float waitingTime, float secWaitingTime)
    {
        machine.ChangeState(regenerating);

        yield return new WaitForSeconds(waitingTime);

        ConsumeSpheres();
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life, self.currentLife);

        yield return new WaitForSeconds(waitingTime);

        ConsumeSpheres();
        self.currentLife = life += 1;
        self.pHealtBar.UpdateHealthBar(self.Life, self.currentLife);

        yield return new WaitForSeconds(secWaitingTime);

        machine.ChangeState(chasing);
    }

    private IEnumerator RegenSpheres()
    {
        machine.ChangeState(regenerating);

        yield return new WaitForSeconds(5);

        SpheresListToStack();
        RegenDeactiveSpheres();

        yield return new WaitForSeconds(2);

        machine.ChangeState(chasing);
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

    private class IdleState : IEnemyState
    {
        private readonly RegeneratorAI owner;
        public IdleState(RegeneratorAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() => owner.machine.ChangeState(owner.chasing);
    }

    private class RegeneratingState : IEnemyState
    {
        private readonly RegeneratorAI owner;
        public RegeneratingState(RegeneratorAI owner) { this.owner = owner; }

        public void Enter() => owner.self.canShootHim = false;
        public void Exit() => owner.self.canShootHim = true;
        public void Tick() { }
    }

    private class ChasingState : IEnemyState
    {
        private readonly RegeneratorAI owner;
        public ChasingState(RegeneratorAI owner) { this.owner = owner; }

        public void Enter() { }
        public void Exit() { }
        public void Tick() =>
            owner.aStarMovement.FollowPath(owner.DirectionToPlayer(), owner.ChasingVelocity());
    }
}