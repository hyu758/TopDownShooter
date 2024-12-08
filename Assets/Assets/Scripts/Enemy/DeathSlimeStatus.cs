using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DeathSlimeStatus : EnemyStatus
{
    private AIPath aiPath;
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    protected override IEnumerator Knockback(Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        aiPath.canMove = false;
        float originalDrag = rb.drag;
        rb.drag = 0;
        Debug.Log("Knockback direction " + knockbackDirection + " " + "Knockback force: " + knockbackForce);
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        rb.drag = originalDrag;
        aiPath.canMove = true;
    }
}

