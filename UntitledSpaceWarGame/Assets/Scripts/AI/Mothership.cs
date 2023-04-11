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

    //Colliders Variables
    private List<Collider> _projectiles = new List<Collider>();
    private Collider _mothershipCollider;

    private void Awake()
    {
        SetMaxHP(_maxHealth);
        ships = new List<ShipAIBT>();
        _teamShips = new List<GameObject>();
        _mothershipCollider = gameObject.GetComponent<Collider>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            TakeDamage(10f);
        }

        CheckForCollisions();
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

    public void CheckForCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 250f);

        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("Projectile"))
                {
                    if (hitCollider.gameObject.GetComponent<Gun>().ownerShip.tag == "AI")
                    {
                        ShipInformation ai = hitCollider.gameObject.GetComponent<Gun>().ownerShip.GetComponent<ShipAIBT>().GetShipData();
                        int team = ai.GetTeam();

                        if (team != _team)
                        {
                            if (!_projectiles.Contains(hitCollider))
                                _projectiles.Add(hitCollider);
                        }
                        else
                            Destroy(hitCollider.gameObject);
                    }
                    else
                    {
                        ShipInformation player = hitCollider.gameObject.GetComponent<Gun>().ownerShip.GetComponent<ShipController>().GetShipData();
                        int team = player.GetTeam();

                        if (team != _team)
                        {
                            if (!_projectiles.Contains(hitCollider))
                                _projectiles.Add(hitCollider);
                        }
                        else
                            Destroy(hitCollider.gameObject);
                    }
                }
            }
        }

        if (_projectiles.Count > 0)
        {
            foreach (Collider projectile in _projectiles)
            {
                if (projectile != null)
                {
                    if (_mothershipCollider.bounds.Intersects(projectile.bounds))
                    {
                        float AttackStrength;
                        if (projectile.gameObject.GetComponent<Gun>().ownerShip.gameObject.tag == "Player")
                        {
                            AttackStrength = projectile.gameObject.GetComponent<Gun>().ownerShip.gameObject.GetComponent<ShipController>().GetShipData().GetAttackForce();
                        }
                        else
                        {
                            AttackStrength = projectile.gameObject.GetComponent<Gun>().ownerShip.gameObject.GetComponent<ShipAIBT>().GetShipData().GetAttackForce();
                        }

                        TakeDamage(AttackStrength);
                        Destroy(projectile.gameObject);
                    }
                }
                else
                {
                    _projectiles.Remove(projectile);
                    break;
                }
            }
        }
    }
}
