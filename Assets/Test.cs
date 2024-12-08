using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("ALO??????");
            Vector2 knockbackDirection = Vector2.up + Vector2.right; // Hướng knockback
            float knockbackForce = 20f; // Lực knockback
            Debug.Log($"Velocity Before: {rb.velocity}");
            rb.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode2D.Impulse);
            Debug.Log($"Velocity After: {rb.velocity}");
        }
    }
}
