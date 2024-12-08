using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStatus : MonoBehaviour
{
    protected int currentHp;
    [SerializeField] protected int maxHp = 20;
    [SerializeField] protected float moveSpeed;
    protected StatusEffectController statusEffectController;
    protected Rigidbody2D rb;

    private void Awake()
    {
        currentHp = maxHp;
        statusEffectController = GetComponent<StatusEffectController>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void HandleHurt(int damage, Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        currentHp -= damage;
        if (statusEffectController != null)
        {
            statusEffectController.Flash(Color.white, 2, 0.05f);
        }
        else
        {
            Debug.LogError("StatusEffectController is null on " + gameObject.name);
        }
        StartCoroutine(Knockback(knockbackDirection, knockbackDistance, knockbackDuration));
        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    protected virtual IEnumerator Knockback(Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        float originalDrag = rb.drag;
        rb.drag = 0;
        Debug.Log("Knockback direction " + knockbackDirection + " " + "Knockback force: " + knockbackForce);
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        rb.drag = originalDrag;
    }
}
