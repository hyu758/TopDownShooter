using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSlimeStatus : MonoBehaviour
{
    [SerializeField] private int currentHp = 20;
    private StatusEffectController statusEffectController;
    private Rigidbody2D rb;
    void Awake()
    {
        statusEffectController = GetComponent<StatusEffectController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleHurt(int damage, Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        currentHp -= damage;
        statusEffectController.Flash(Color.white, 2, 0.05f);
        StartCoroutine(Knockback(knockbackDirection, knockbackDistance, knockbackDuration));
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Knockback(Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        float timeElapsed = 0;
        
        while (timeElapsed < knockbackDuration)
        {
            rb.velocity = knockbackDirection * knockbackDistance;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
        rb.velocity = Vector2.zero;
    }
}

