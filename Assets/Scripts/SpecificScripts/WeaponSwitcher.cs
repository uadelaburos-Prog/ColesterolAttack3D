using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private IWeapon weapon;
    [SerializeField] private WeaponScript pistol;
    [SerializeField] private MeleeScript melee;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(pistol);
            pistol.gameObject.SetActive(true);
            melee.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(melee);
            pistol.gameObject.SetActive(false);
            melee.gameObject.SetActive(true);
        }
    }

    private void SetWeapon(IWeapon currentWeapon)
    {
        this.weapon = currentWeapon;
    }
}
