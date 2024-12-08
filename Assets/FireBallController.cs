using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockbackDistance = 4f;
    [SerializeField] private float knockbackDuration = 0.2f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
            PlayerStatus.Instance.HandleHurt(damage, knockbackDirection, knockbackDistance, knockbackDuration);
            Destroy(gameObject);
        }
    }
}
