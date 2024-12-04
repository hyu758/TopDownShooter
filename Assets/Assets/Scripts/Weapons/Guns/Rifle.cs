using System;
using System.Collections;
using Assets.Scripts.Weapons.Guns;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Rifle : GunBase
{
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
        playerTransform = transform.parent.parent;
        InitializeBullets();
    }

    private void FixedUpdate()
    {
        
    }

    private void Update()
    {
        RotateWeaponTowardsMouse();  // Xoay súng theo chuột
        shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shootTimer < 0)
        {
            Fire();
        }
    }

    protected override void Fire()
    {
        shootTimer = shootInterval;
        if (isReloading) return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        GameObject bullet = GetBulletFromPool(currentAmmo - 1);
        if (bullet != null)
        {
            Vector3 offset = transform.right * offSet;
            bullet.transform.position = transform.position + offset;
            bullet.transform.rotation = transform.rotation;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.right * ammoSpeed;
            }
            StartCoroutine(BulletLifeTime(bullet, bulletLifeTime));
            currentAmmo--;

            if (muzzle != null)
            {
                StartCoroutine(ShowMuzzleFlash());
            }
        }
    }

    protected override IEnumerator ShowMuzzleFlash()
    {
        muzzle.SetActive(true);
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzle.SetActive(false);
    }
    
}