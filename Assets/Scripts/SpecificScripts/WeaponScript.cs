using System.Collections;
using UnityEditor;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bulletPrefab;

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
        Instantiate(bulletPrefab,shootPoint.transform.position,shootPoint.rotation);
        yield return new WaitForSeconds(GameManager.Instance.Player.Stats.weaponCoolDown);
        Debug.Log("se puede volver a disparar");
        canShoot = true;
    }
}
