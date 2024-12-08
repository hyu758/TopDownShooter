using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class EnemyAI : EnemyStatus
{
    
    [SerializeField] float repeatTimeUpdatePath = 0.5f;
    [SerializeField] float nextWayPointDistance = 2f;
    private Transform target;
    Seeker seeker;
    Path path;
    Coroutine moveCoroutine;
    SpriteRenderer spriteRenderer;

    [SerializeField] private float fireRate = 1.5f;
    [SerializeField] private float fireSpeed = 5f;

    [SerializeField] private GameObject fireBall;
    private void Start()
    {
        seeker = GetComponent<Seeker>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = PlayerController.Instance.transform;

        InvokeRepeating("CalculatePath", 0f, repeatTimeUpdatePath);
        InvokeRepeating("Shoot", 0f, fireRate);
    }

 

    void CalculatePath()
    {
        float distanceToTarget = Vector2.Distance(rb.position, target.position) + Random.Range(0f, 2f);

        if (distanceToTarget <= nextWayPointDistance)
        {
            path = null;
            return;
        }

        Vector2 offset = Random.insideUnitCircle.normalized * nextWayPointDistance;
        Vector2 targetPosition = (Vector2)target.position + offset;

        if (seeker.IsDone())
            seeker.StartPath(rb.position, targetPosition, OnPathCompleted);
    }
    
    void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveToTargetCoroutine());
    }
    
    IEnumerator MoveToTargetCoroutine()
    {
        int currentWP = 0;

        while (currentWP < path.vectorPath.Count)
        {

            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - rb.position).normalized;
            Debug.Log("Direction: " + direction + ", " + moveSpeed);
            
            Vector2 force = direction * moveSpeed * Time.deltaTime;
            Debug.Log("Force: " + force);
            transform.position += (Vector3)force;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWP]);
            if (distance < nextWayPointDistance)
                currentWP++;

            if (force.x != 0)
                if (force.x < 0)
                    spriteRenderer.transform.localScale = new Vector3(-1, 1, 0);
                else
                    spriteRenderer.transform.localScale = new Vector3(1, 1, 0);

            yield return null;
        }
    }

    void Shoot()
    {
        var bullet  = Instantiate(fireBall, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector3 direction = (target.position - (Vector3)rb.position).normalized;
        if (direction.x < 0)
        {
            bullet.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            bullet.transform.localScale = new Vector3(1, 1, 1);
        }
        rb.AddForce(direction * fireSpeed, ForceMode2D.Impulse);
    }
    
}
