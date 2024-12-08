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
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Bullet collided with: " + col.gameObject.name);

        if (col.gameObject.CompareTag("Wall"))
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit enemy!");

            Vector2 knockbackDirection = (col.transform.position - transform.position).normalized;
            var enemy = col.gameObject.GetComponent<EnemyStatus>();
            enemy.HandleHurt(damage, knockbackDirection, knockbackDistance, knockbackDuration);
            gameObject.SetActive(false);
        }
    }
}
