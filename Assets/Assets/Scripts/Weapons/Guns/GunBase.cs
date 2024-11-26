using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Weapons.Guns;
using UnityEngine;

public abstract class GunBase : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    [Header("Gun properties")]
    [SerializeField] protected float shootInterval = 0.1f;
    [SerializeField] protected int maxAmmo = 10;
    [SerializeField] protected float reloadTime = 1.5f;
    [SerializeField] protected float ammoSpeed  = 10f;
    [SerializeField] protected float offSet = 1f;
    
    protected float shootTimer;
    protected int currentAmmo;
    protected bool isReloading = false;
    protected bool isIdling = false;
    protected GunStatus gunStatus = GunStatus.Idle;
    public GunStatus GunStatus => gunStatus;
    protected Transform playerTransform;
    protected List<GameObject> activeBullets = new List<GameObject>();
    protected bool isOnTheLeft = false;
    protected Animator animator;

    [SerializeField] protected GameObject muzzle;
    [SerializeField] protected float muzzleFlashDuration = 0.1f;
    

    public void RotateWeaponTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        if (mousePosition.x < playerTransform.position.x)
        {
            if (!isOnTheLeft)
            {
                Debug.Log(("Trai"));
                transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                Debug.Log(transform.position);
            }
            isOnTheLeft = true;

            
        }
        else
        {
            if (isOnTheLeft)
            {
                Debug.Log(("Phai"));
                transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                Debug.Log(transform.position);
            }
            isOnTheLeft = false;
            
        }
    }

    protected abstract void Fire();
    protected abstract IEnumerator ShowMuzzleFlash();

    public virtual void SetGunStatus(GunStatus status)
    {
        gunStatus = status;
    }

    
    protected GameObject GetBulletFromPool()
    {
        foreach (var bullet in activeBullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        
        GameObject newBullet = Instantiate(bulletPrefab);
        activeBullets.Add(newBullet);
        return newBullet;
    }
    
    protected virtual IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime); 
        currentAmmo = maxAmmo;
        foreach (var bullet in activeBullets)
        {
            bullet.SetActive(false);
        }

        isReloading = false;
        gunStatus = GunStatus.Idle;
    }
    
    protected void InitializeBullets()
    {
        for (int i = 0; i < maxAmmo; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            activeBullets.Add(bullet);
        }
    }
}