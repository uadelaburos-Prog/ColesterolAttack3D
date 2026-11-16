using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private IWeapon weapon;
    [SerializeField] private WeaponScript pistol;
    [SerializeField] private MeleeScript melee;

    private void Start() => EquipPistol();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))EquipPistol();

        if (Input.GetKeyDown(KeyCode.Alpha2))EquipMelee();
                                                
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log($"click | arma: {(weapon == null ? "NULL" : weapon.Name)}");
            weapon?.Attack();
        }
    }

    private void EquipPistol()
    {
        SetWeapon(pistol);
        pistol.gameObject.SetActive(true);
        melee.gameObject.SetActive(false);
    }

    private void EquipMelee()
    {
        SetWeapon(melee);
        pistol.gameObject.SetActive(false);
        melee.gameObject.SetActive(true);
    }

    private void SetWeapon(IWeapon currentWeapon)
    {
        weapon = currentWeapon;
    }
}
