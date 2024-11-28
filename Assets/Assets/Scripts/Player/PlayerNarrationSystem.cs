using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNarrationSystem : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerSubject;
    

    private Dictionary<PlayerAction, System.Action<float>> _playerActionHandlers;

    private void Awake()
    {
        _playerSubject = PlayerStatus.Instance;
        _playerActionHandlers = new Dictionary<PlayerAction, System.Action<float>>()
        {
            { PlayerAction.Heal, HandleHeal },
            { PlayerAction.Hurt, HandleHurt },
            { PlayerAction.Die, HandleDie },
        };
    }

    public void OnNotify(PlayerAction action, float n)
    {
        if (_playerActionHandlers.ContainsKey(action))
        {
            _playerActionHandlers[action](n);
        }
    }

    void HandleHeal(float amount)
    {
        Debug.Log("Player healed by: " + amount);
    }

    void HandleHurt(float damage)
    {
        Debug.Log("Player hurt by: " + damage);
    }

    void HandleDie(float dummy)
    {
        Debug.Log("Player died.");
    }
    
    private void OnEnable()
    {
        _playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        _playerSubject.RemoveObserver(this);
    }
}

