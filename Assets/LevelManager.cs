using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathSlime;
    [SerializeField] private int numberOfEnemies = 5;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public GameObject getPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player");
    }

    public GameObject getSlime()
    {
        return deathSlime;
    }
    
    void Update()
    {
        
    }

    public void HandleEndWay()
    {
        numberOfEnemies = (int)((float)numberOfEnemies * 1.2f);
        SpawnEnemiesForNextWay();
    }
    void SpawnEnemiesForNextWay()
    {
        
    }
}
