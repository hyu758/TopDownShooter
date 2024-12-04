
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapons.Guns;

public class WeaponController : MonoBehaviour
{
    private WeaponController instance;
    public WeaponController Instance => instance;
    
    private List<GunBase> weapons = new List<GunBase>();
    public GunBase currentWeapon;

    private void Start()
    {
        weapons.AddRange(GetComponentsInChildren<GunBase>());
        
        if (weapons.Count > 0)
        {
            currentWeapon = weapons[0];
            SetWeaponActive(currentWeapon);
        }
    }
    public void SwitchWeapon(GunBase newWeapon)
    {
        if (newWeapon != null)
        {
            foreach (var weapon in weapons)
            {
                SetWeaponInactive(weapon);
            }
            
            currentWeapon = newWeapon;
            SetWeaponActive(currentWeapon);
        }
    }
    
    private void SetWeaponActive(GunBase weapon)
    {
        weapon.gameObject.SetActive(true);
    }
    
    private void SetWeaponInactive(GunBase weapon)
    {
        weapon.gameObject.SetActive(false);
    }
    
}


