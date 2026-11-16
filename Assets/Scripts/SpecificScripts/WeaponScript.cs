using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour, IWeapon
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private LineRenderer line;
    [SerializeField] private LayerMask enemy;
    private float nextShotTime;
    public bool canShoot;
    private float range = 20f;
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
        SetLineColor(didHit ? Color.green : Color.red);
    }
    public void Attack()
    {
        if (!canShoot || Time.time < nextShotTime) return;
        if (!Physics.Raycast(shootPoint.position, shootPoint.forward, out RaycastHit hit, range, enemy)) return;

        Enemy e = hit.collider.GetComponentInParent<Enemy>();
        if (e == null) return;

        SetLineColor(Color.red);
        e.ReciveDmg(true);
        hit.collider.GetComponentInParent<Shielder>()?.ShieldDmg(true);

        Debug.Log($"GM: {GameManager.Instance != null} | Player: {GameManager.Instance?.Player != null} | Stats: {GameManager.Instance?.Player?.Stats != null}");
        nextShotTime = Time.time + GameManager.Instance.Player.Stats.weaponCoolDown;
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