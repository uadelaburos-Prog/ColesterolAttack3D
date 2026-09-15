using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeScript : MonoBehaviour, IWeapon
{
    public string Name => "Melee";
    [SerializeField] private float coolDown = 5f;
    private bool canSwing = true;

    [SerializeField] private float radio = 0.5f;
    [SerializeField] private float maxDistance = 3;
    [SerializeField] private LayerMask enemy;

    private void Update()
    { 
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radio, transform.right * -1, maxDistance, enemy);
        if (Input.GetKeyDown(KeyCode.Mouse0) && canSwing)
        {
            foreach (var hit in hits)
            {
                RegeneratorAI regen = hit.collider.GetComponent<RegeneratorAI>();
                if(regen != null && regen.IsRegenerating)
                {
                    regen.MeleeExecute();
                    continue;
                }

                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                Shielder s = hit.collider.gameObject.GetComponent<Shielder>();
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
        Vector3 dir = transform.right * -1;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + dir * maxDistance, radio);
    }
}
