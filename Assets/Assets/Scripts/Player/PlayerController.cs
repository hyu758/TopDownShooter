using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Weapons.Guns;

public class PlayerController : MonoBehaviour
{
    private PlayerInputAction playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isMoving;
    public bool IsMoving => isMoving;
    private WeaponController weaponController;
    private SpriteRenderer spriteRenderer;
    private CameraShake cameraShake;
    private static PlayerController instance;
    public static PlayerController Instance => instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        playerControls = new PlayerInputAction();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        GameObject activeWeapon = transform.Find("Active Weapon")?.gameObject;
        if (activeWeapon != null)
        {
            weaponController = activeWeapon.GetComponentInChildren<WeaponController>();
        }
        else
        {
            Debug.LogError("ActiveWeapon not found in Player.");
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        PlayerInput();
        HandlePlayerDirection();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>().normalized;
        animator.SetFloat("XDir", movement.x);
        animator.SetFloat("YDir", movement.y);
        isMoving = (movement.x != 0 || movement.y != 0);
        animator.SetBool("isMoving", isMoving);

        if (playerControls.Attacking.Attack.triggered)
        {
            weaponController.SetGunStatus(GunStatus.Shooting);
            
        }
        // if (playerControls.Attacking.Attack.IsPressed())
        // {
        //     Debug.Log("ALO???");
        //     weaponController.SetGunStatus(GunStatus.Shooting);
        // }

        if (playerControls.Attacking.Reload.triggered)
        {
            weaponController.SetGunStatus(GunStatus.Reloading);
        }
    }

    private void Move()
    {
        transform.position += new Vector3(movement.x, movement.y, 0) * (PlayerStatus.Instance.CurrentSpeed * Time.deltaTime);
    }

    private void HandlePlayerDirection()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }
    
}
