using Demonics;
using System.Runtime.CompilerServices;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    [SerializeField] private float nextScanTime;

    private GameObject player;
    private Enemy self;
    [SerializeField] private bool isMooving = false;

    private PriorityQueue<StalkerStates> desisions = new PriorityQueue<StalkerStates>();

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

        switch (states)
        {
            case StalkerStates.Idle:
                states = StalkerStates.Following; break;
            case StalkerStates.Following: 
                states = StalkerStates.Following;
                MoveTowardsShielder();
                break;
            case StalkerStates.AttackPlayer:
                states = StalkerStates.AttackPlayer;
                MoveTowards(direction,actualVelocity);
                break;
        }
    }

    private void MoveTowardsShielder()
    {
        Debug.Log("Se esta moviendo hacia el shielder");
        RaycastHit hit;
        Vector3 direc = new Vector3();

        if (Physics.SphereCast(transform.position, followingShielderRadius, transform.up, out hit, maxDistance, shielderMask))
        {
            Shielder s = hit.collider.GetComponent<Shielder>();
            if (s == null)
                Debug.Log($"No se encontro el Shielder");
            
        }

        MoveTowards(direc, followingSpeed);
    }

    private void MoveTowardsPlayer()
    {

    }

    private void MoveTowards(Vector3 direc, float velocity)
    {
        transform.position += direc * velocity * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followingShielderRadius);
    }
}
