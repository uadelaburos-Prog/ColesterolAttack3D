using System.Collections;
using UnityEngine;

enum ShielderStates
{
    Chasing,
    ShieldDown,
    Agro,
}
public class Shielder : MonoBehaviour
{
    private ShielderStates shielderStates;
    [SerializeField] private LayerMask playerMask;

    [SerializeField] private float chasingSpeed = 3f;
    [SerializeField] private float agroSpeed = 10f;

    //[SerializeField] private float chasingRadius = 10;

    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 10f;

    [SerializeField] private GameObject shield;
    [SerializeField] private float shieldLife = 6;
    private bool shieldReciveDmg;

    private GameObject player;
    private Enemy self;
    [SerializeField] private bool isMooving = false;

    private void Awake()
    {
        self = GetComponent<Enemy>();
    }

    private void Start()
    {
        self.canShootHim = false;
        shielderStates = ShielderStates.Chasing;
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        float distanceFactor = Mathf.InverseLerp(minDistance, maxDistance, distanceToPlayer);
        float actualVelocity = Mathf.Lerp(chasingSpeed, agroSpeed, distanceFactor);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        direction.Normalize();

        if(shieldLife == 0)
        {
            StartCoroutine(ShieldDownRutine(2f));
        }

        switch (shielderStates)
        {
            case ShielderStates.Chasing:
                if (!isMooving)
                    MoveTowardsPlayer(direction, actualVelocity);
                break;
            case ShielderStates.Agro:
                MoveTowardsPlayer(direction, agroSpeed);
                break;
            case ShielderStates.ShieldDown:

                break;

        }
    }

    private IEnumerator ShieldDownRutine(float agroPassTime) 
    {
        shielderStates = ShielderStates.ShieldDown;
        shield.SetActive(false);
        isMooving = true;

        yield return new WaitForSeconds(agroPassTime);

        isMooving = false;
        self.canShootHim = true;
        shielderStates = ShielderStates.Agro;
    }

    private void MoveTowardsPlayer(Vector3 direction, float actualVelocity)
    {
        transform.position += direction * actualVelocity * Time.deltaTime;
    }

    public void ShieldDmg(bool reciveDmg) 
    {
        shieldReciveDmg = reciveDmg;
        if (shieldReciveDmg)
        {
            shieldLife -= 1;
        }
        else 
            reciveDmg = false;
    }
}
