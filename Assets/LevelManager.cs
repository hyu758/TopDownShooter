using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deathSlime;
    [SerializeField] private WalkerGenerator walkerGenerator;
    [SerializeField] private int numberOfEnemies = 5;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if (walkerGenerator == null)
        {
            Debug.LogWarning("WalkerGenerator is missing");
        }
        if (deathSlime == null)
        {
            Debug.LogError("DeathSlime chưa được gán trong Inspector!");
            return;
        }
        walkerGenerator.SpawnGameObject(deathSlime, numberOfEnemies);
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
        walkerGenerator.SpawnGameObject(player, numberOfEnemies);
    }
}
