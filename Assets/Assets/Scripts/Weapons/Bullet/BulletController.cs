using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackDistance = 4f;
    [SerializeField] private float knockbackDuration = 0.2f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            // gameObject.transform.position = new Vector3(-9999, -9999, gameObject.transform.position.z);
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            collision.gameObject.GetComponent<DeathSlimeStatus>().HandleHurt(damage, knockbackDirection, knockbackDistance, knockbackDuration);
            
            gameObject.SetActive(false);
            
        }
    }
}
