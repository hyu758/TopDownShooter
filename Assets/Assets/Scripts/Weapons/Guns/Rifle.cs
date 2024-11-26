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
        HandleWeaponAnimation();
    }

    private void Update()
    {
        RotateWeaponTowardsMouse();  // Xoay súng theo chuột
        shootTimer -= Time.deltaTime;

        if (isReloading)
            return;
        if (gunStatus == GunStatus.Shooting && shootTimer <= 0 && !isReloading)
        {
            Debug.Log("Ban?");
            Fire();
            shootTimer = shootInterval;
        }

        shootTimer -= Time.deltaTime;
    }

    protected override void Fire()
    {
        Debug.Log("So dan con lai: " + currentAmmo);
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        // Lấy đạn từ pool
        GameObject bullet = GetBulletFromPool();
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

            currentAmmo--;

            if (muzzle != null)
            {
                StartCoroutine(ShowMuzzleFlash());
            }
            SetGunStatus(GunStatus.Idle);
        }
    }

    protected override IEnumerator ShowMuzzleFlash()
    {
        muzzle.SetActive(true);
        yield return new WaitForSeconds(muzzleFlashDuration);
        muzzle.SetActive(false);
    }
    
    private void HandleWeaponAnimation()
    {
        animator.SetBool("isMoving", PlayerController.Instance.IsMoving);
    }
}