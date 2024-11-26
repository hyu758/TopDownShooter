using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private int currentHp = 20;


    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleHurt(int damage, Vector2 direction)
    {
        currentHp -= damage;
        Debug.Log("Slime hp: " + currentHp);
        if (currentHp <= 0)
        {
            Debug.Log("Slime is dead");
            Destroy(gameObject);
        }
    }
}
