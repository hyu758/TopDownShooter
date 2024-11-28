using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSlimeController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float damage = 10f;
    [SerializeField] float speed = 2f;
    [SerializeField] private float knockbackDistance = 3f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private GameObject playerObj;
    private Transform target;
    
    void Start()
    {
        playerObj = LevelManager.Instance.getPlayer();
        if (playerObj != null)
        {
            target = playerObj.transform;
            Debug.Log(target.position);
            Debug.Log("Target assigned successfully: " + target.name);
        }
        else
        {
            Debug.LogError("Player not found by LevelManager!");
        }
    }
        
    void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 knockbackDirection = collision.transform.position - transform.position;


            PlayerStatus.Instance.HandleHurt(damage, knockbackDirection, knockbackDistance, knockbackDuration);

        }
    }
    
}