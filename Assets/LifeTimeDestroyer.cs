using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimeDestroyer : MonoBehaviour
{
    [SerializeField] private float LifeTime;
    void Start()
    {
        Destroy(gameObject, LifeTime);
    }

}
