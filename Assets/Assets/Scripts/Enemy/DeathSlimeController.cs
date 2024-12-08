using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSlimeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float damage = 10f;
    [SerializeField] private float knockbackDistance = 3f;
    [SerializeField] private float knockbackDuration = 0.2f;
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 knockbackDirection = collision.transform.position - transform.position;
            PlayerStatus.Instance.HandleHurt(damage, knockbackDirection, knockbackDistance, knockbackDuration);

        }
    }
    
}