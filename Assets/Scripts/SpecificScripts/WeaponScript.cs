using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask enemy;
    private float range = 20f;
    public bool canShoot = true;
    public string Name => "Pistol";

    private void Start()
    {
        line.enabled = true;
    }

    private void Update()
    {
        Vector3 origin = shootPoint.position;
        Vector3 dir = shootPoint.forward;

        bool didHit = Physics.Raycast(origin, dir, out RaycastHit hit, range, enemy);

        line.SetPosition(0, origin);
        line.SetPosition(1, didHit ? hit.point : origin + dir * range);

        if (!didHit) { SetLineColor(Color.grey); return; }

        Enemy e = hit.collider.GetComponentInParent<Enemy>();
        bool shooting = Input.GetKeyDown(KeyCode.Mouse0) && canShoot;

        if (e == null || !shooting) { SetLineColor(Color.white); return; }

        SetLineColor(Color.red);
        e.ReciveDmg(true);
        hit.collider.GetComponentInParent<Shielder>()?.ShieldDmg(true);
        StartCoroutine(CoolDownWeapon());
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