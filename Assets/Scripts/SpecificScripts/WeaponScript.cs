using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    private float range = 20f;
    public bool canShoot = true;

    private void FixedUpdate()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                if (e == null) return;
                e.ReciveDmg(true);
                StartCoroutine(CoolDownWeapon());
            }
        }
    }

    private IEnumerator CoolDownWeapon()
    {
        canShoot = false;
        yield return new WaitForSeconds(GameManager.Instance.Player.Stats.weaponCoolDown);
        Debug.Log("se puede volver a disparar");
        canShoot = true;
    }

    private void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            bool isEnemy = hit.collider.GetComponent<Enemy>() != null;
            Gizmos.color = isEnemy ? Color.red : Color.yellow;

            Gizmos.DrawLine(shootPoint.position, hit.point);
            Gizmos.DrawSphere(hit.point, 0.15f);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + shootPoint.forward * range);
        }
    }
}
