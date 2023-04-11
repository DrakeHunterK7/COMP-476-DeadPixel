using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mothership : MonoBehaviour
{
    public int _team;
    public float _currentHealth;

    [HideInInspector] public List<ShipAIBT> ships;
    public static float _maxHealth = 1000.0f;

    //HP References
    [Header("Health Settings")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private Gradient _hpGradient;
    [SerializeField] private Image _hpFill;

    //Explosion Prefab
    [SerializeField] private GameObject _shipExplosion;
    [SerializeField] private GameObject _mothershipExplosion;

    //Team ship
    [HideInInspector] public List<GameObject> _teamShips;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamage(15f);
        }
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        ships = new List<ShipAIBT>();
        _teamShips = new List<GameObject>();
    }
    
    public void SetMaxHP(float maxHP)
    {
        _hpSlider.maxValue = maxHP;
        _hpSlider.value = maxHP;
        _hpFill.color = _hpGradient.Evaluate(1f);
        _currentHealth = _maxHealth;
    }

    public void SetHealth(float value)
    {
        _hpSlider.value = value;
        _hpFill.color = _hpGradient.Evaluate(_hpSlider.normalizedValue);
    }

    public void TakeDamage(float AttackForce)
    {
        if (_currentHealth > AttackForce)
        {
            _currentHealth -= AttackForce;
        }
        else
        {
            //MOTHERSHIP DESTROYED
            _currentHealth = 0;
            DestroyTeam();
        }

        if (_currentHealth > 0)
        {
            SetHealth(_currentHealth);
        }
    }

    public void DestroyTeam()
    {
        foreach (GameObject teamShip in _teamShips)
        {
            Instantiate(_shipExplosion, teamShip.transform.position, teamShip.transform.rotation);
            Destroy(teamShip);
        }

        GameObject mothershipExplosion = Instantiate(_mothershipExplosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }


}
