using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Subject
{
    // Start is called before the first frame update
    [Header("Player Properties")]
    [SerializeField] private float maxHp = 100;
    public float MaxHp => maxHp;
    [SerializeField] private float currentHp = 100;
    public float CurrentHp => currentHp;
    [SerializeField] private float currentSpeed = 7f;
    public float CurrentSpeed => currentSpeed;
    
    private static PlayerStatus instance;
    public static PlayerStatus Instance => instance;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleHurt(float damage)
    {
        currentHp -= damage;
        NotifyObserver(PlayerAction.Hurt, damage);
        if (currentHp <= 0)
        {
            NotifyObserver(PlayerAction.Die, 0);
        }
    }

    public void HandleHeal(float hp)
    {
        currentHp = Mathf.Max(currentHp + hp, maxHp);
        NotifyObserver(PlayerAction.Heal, hp);
    }
}
