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
    
    private StatusEffectController statusEffectController;

    private Rigidbody2D rb;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        statusEffectController = GetComponent<StatusEffectController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleHurt(float damage, Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        currentHp -= damage;
        NotifyObserver(PlayerAction.Hurt, damage);
        HandleKnockBack( knockbackDirection, knockbackDistance, knockbackDuration);
        statusEffectController.Flash(Color.white, 2, 0.05f);
        if (currentHp <= 0)
        {
            NotifyObserver(PlayerAction.Die, 0);
            Destroy(gameObject);
        }
    }

    public void HandleHeal(float hp)
    {
        currentHp = Mathf.Max(currentHp + hp, maxHp);
        NotifyObserver(PlayerAction.Heal, hp);
    }

    public void HandleKnockBack(Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        StartCoroutine(KnockbackRoutine(knockbackDirection, knockbackDistance, knockbackDuration));
    }

    private IEnumerator KnockbackRoutine(Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        Vector3 start = transform.position;
        Vector3 target = start + (Vector3)(knockbackDirection.normalized * knockbackDistance);

        float elapsedTime = 0f;
        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(start, target, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
    }
}