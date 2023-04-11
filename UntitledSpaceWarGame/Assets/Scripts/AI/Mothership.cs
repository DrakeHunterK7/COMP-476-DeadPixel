using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mothership : MonoBehaviour
{
    // Start is called before the first frame update

    public int _team;
    public float _currentHealth;

    [HideInInspector]
    public static float _maxHealth = 100.0f;

    [HideInInspector] public List<ShipAIBT> ships;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        ships = new List<ShipAIBT>();
    }

    private void Start()
    {
        var shipList = GameObject.FindGameObjectsWithTag("AI");
    }
}
