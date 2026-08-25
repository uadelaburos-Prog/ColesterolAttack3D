using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    private GameObject bullet;

    public bool canShoot = true;

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            StartCoroutine(CoolDownWeapon());
        }
    }

    private IEnumerator CoolDownWeapon()
    {
        canShoot = false;
        bullet = BulletPool.Instance.GetBullet(shootPoint);
        yield return new WaitForSeconds(GameManager.Instance.Player.Stats.weaponCoolDown);
        Debug.Log("se puede volver a disparar");
        canShoot = true;
    }
}
