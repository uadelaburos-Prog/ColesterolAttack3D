using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeScript : MonoBehaviour, IWeapon
{
    public string Name => "Melee";
    [SerializeField] private float coolDown = 5f;
    private bool canSwing = true;

    [SerializeField] private float radio = 10f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private LayerMask enemy;

    private void Update()
    { 
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radio, transform.right * -1, maxDistance, enemy);
        if (Input.GetKeyDown(KeyCode.Mouse0) && canSwing)
        {
            foreach (var hit in hits)
            {
                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                Shielder s = hit.collider.gameObject.GetComponent<Shielder>();
                Debug.Log($"{e}");
                Debug.Log($"{s}");
                if(e != null)
                    e.ReciveDmg(true);
                if(s != null)
                    s.ShieldDmg(true);
            }

            StartCoroutine(CoolDownMelee());
        }
    }

    private IEnumerator CoolDownMelee()
    {
        canSwing = false;
        yield return new WaitForSeconds(coolDown);
        canSwing = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radio);
    }
}
