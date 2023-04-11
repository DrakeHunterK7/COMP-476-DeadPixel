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

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }




}
