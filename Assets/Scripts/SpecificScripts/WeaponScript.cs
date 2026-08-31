using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer line;
    private float range = 20f;
    public bool canShoot = true;

    private void Start()
    {
        line.enabled = true;
    }

    private void Update()
    {
        Ray ray = new Ray(shootPoint.position, shootPoint.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            line.SetPosition(0, shootPoint.position);
            line.SetPosition(1, hit.point);

            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                if (e == null) return;
                SetLineColor(Color.red);
                e.ReciveDmg(true);
                StartCoroutine(CoolDownWeapon());
            }
            else 
                SetLineColor(Color.white);
        }
        else
        {
            line.SetPosition(0, shootPoint.position);
            line.SetPosition(1, shootPoint.position + shootPoint.forward * range);
            SetLineColor(Color.grey);
        }
    }

    private IEnumerator CoolDownWeapon()
    {
        canShoot = false;
        yield return new WaitForSeconds(GameManager.Instance.Player.Stats.weaponCoolDown);
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

    private void SetLineColor(Color color)
    {
        line.startColor = color;
        line.endColor = color;
    }
}
