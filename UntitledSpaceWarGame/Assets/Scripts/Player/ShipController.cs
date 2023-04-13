using System.Collections;
using UnityEngine;



public class ShipController : MonoBehaviour
{
    [Header ("Movement Variables")]
    public float _forwardSpeed = 35f;
    public float _strafeSpeed = 7.5f;
    public float _hoverSpeed = 5f;

    public float _LookRateSpeed = 90f;

    public float _rollSpeed = 90f;

    public float _activeForwardSpeed;
    private float _activeStrafeSpeed;
    private float _activeHoverSpeed;
        
    public GameObject _shootpoint;

    public AudioSource gunSound;

    //Weapon Prefabs
    [Header ("Weapon Prefabs")]
    public GameObject _bulletPrefab;
    public GameObject _laserPrefab;
    private bool _weaponChanged = false;

    [HideInInspector]
    public Vector3 velocity;

    //Weapon UI References
    [Header ("Weapon UI References")]
    public GameObject _missileIcon;
    public GameObject _missileCrossair;
    public GameObject _laserIcon;
    public GameObject _laserCrossair;
    private HealthBar _healthBar;

    //Ship Information Reference
    private ShipInformation _shipData;
    [HideInInspector] public bool _canMove = false;

    private float _forwardAcceleration = 2.5f, _strafeAcceleration = 2f, _hoverAcceleration = 2f;
    private float _rollAcceleration = 3.5f;

    private float _rollInput;

    private Vector2 _lookInput, _centerOfScreen, _mouseDistance;

    //Weapon Variables
    private LineRenderer laserLine;
    private bool _energyEmpty = false;
    [Header ("Weapon Variables")]
    [SerializeField] private float _laserStrength = 0.05f;
    [SerializeField] private GameObject _explosion;

    [Header ("Level Manager Reference")]
    [SerializeField] private LevelManager _levelManager;


    // Start is called before the first frame update
    void Start()
    {
        _centerOfScreen.x = Screen.width / 2;
        _centerOfScreen.y = Screen.height / 2;
        laserLine = gameObject.GetComponent<LineRenderer>();
        _healthBar = gameObject.GetComponent<HealthBar>();

        //Update Max Health Bar
        if (_shipData != null)
        {
            _healthBar.SetMaxEnergy(_shipData.GetEnergyLevel());
            _healthBar.SetMaxHP(_shipData.GetHP());
            _healthBar.SetMaxShieldHP(_shipData.GetShieldHP());
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward*100f, Color.yellow, 0.1f);

        _forwardSpeed = GetShipData()._movementSpeed;
        
        Debug.Log("My team is " + GetShipData().GetTeam());
        
        // Look rotation calculations
        _lookInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        _mouseDistance.x = (_lookInput.x - _centerOfScreen.x) / _centerOfScreen.y;
        _mouseDistance.y = (_lookInput.y - _centerOfScreen.y) / _centerOfScreen.y;

        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);

        // Roll rotation calculations
        _rollInput = Mathf.Lerp(_rollInput, Input.GetAxisRaw("Roll"), _rollAcceleration * Time.deltaTime);

        // Apply Transformation
        if(_canMove)
        {
            // Movement calculations
            _activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, Input.GetAxisRaw("Vertical") * _forwardSpeed, _forwardAcceleration * Time.deltaTime);
            _activeStrafeSpeed = Mathf.Lerp(_activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * _strafeSpeed, _strafeAcceleration * Time.deltaTime);
            _activeHoverSpeed = Mathf.Lerp(_activeHoverSpeed, Input.GetAxisRaw("Hover") * _hoverSpeed, _hoverAcceleration * Time.deltaTime);

            transform.position += transform.forward * _activeForwardSpeed * Time.deltaTime;
            transform.position += transform.right * _activeStrafeSpeed * Time.deltaTime;
            transform.position += transform.up * _activeHoverSpeed * Time.deltaTime;
        }
        else
        {
            if(_activeForwardSpeed > 0.0f)
            {
                _activeForwardSpeed = 0.0f;
            }
        }

        // Apply Rotation
        transform.Rotate(-_mouseDistance.y * _LookRateSpeed * Time.deltaTime, _mouseDistance.x * _LookRateSpeed * Time.deltaTime, _rollInput * _rollSpeed * Time.deltaTime, Space.Self);

        if (_canMove)
        {
            if (!_weaponChanged)
            {
                laserLine.enabled = false;
                if (Input.GetKeyDown(KeyCode.Mouse0) && !_energyEmpty)
                {
                    Shoot();
                    UseEnergy();
                }
                else
                {
                    ReplenishEnergy();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Mouse0) && !_energyEmpty)
                {
                    Shoot();
                    UseEnergy();
                }
                else
                {
                    laserLine.enabled = false;
                    ReplenishEnergy();
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeWeapon();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                TakeDamage(1f);
            }
        }
    }

    public void ChangeWeapon()
    {
        if (_missileIcon.activeSelf)
        {
            _missileIcon.SetActive(false);
            _missileCrossair.SetActive(false);
            _laserIcon.SetActive(true);
            _laserCrossair.SetActive(true);
            _weaponChanged = true;
        }
        else
        {
            _missileIcon.SetActive(true);
            _missileCrossair.SetActive(true);
            _laserIcon.SetActive(false);
            _laserCrossair.SetActive(false);
            _weaponChanged = false;
        }
    }

    public void Shoot()
    {
        var forwarddir = (_shootpoint.transform.position - transform.position).normalized;

        if (_weaponChanged)
        {
            laserLine.SetPosition(0, _shootpoint.transform.position);
            laserLine.enabled = true;
            RaycastHit hit;

            if (Physics.Raycast(_shootpoint.transform.position, forwarddir, out hit, 1000f))
            {
                //LASER COLLISION DETECTION
                laserLine.SetPosition(1, hit.point);
                if (hit.collider.gameObject.tag == "AI")
                {
                    hit.collider.gameObject.GetComponent<ShipAIBT>().GetShipData().TakeDamage(_shipData.GetAttackForce() * _laserStrength);
                    if (hit.collider.gameObject.GetComponent<ShipAIBT>().GetShipData().GetHP() <= 0f)
                    {
                        Instantiate(_explosion, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                        Destroy(hit.collider.gameObject);
                    }
                }
                else if (hit.collider.gameObject.tag == "Mothership")
                {
                    if (hit.collider.gameObject.GetComponent<Mothership>()._team != _shipData.GetTeam())
                    {
                        hit.collider.gameObject.GetComponent<Mothership>().TakeDamage(_shipData.GetAttackForce() * _laserStrength, gameObject);
                    }
                }
            }
            else
            {
                laserLine.SetPosition(1, _shootpoint.transform.position + (forwarddir * 1000f));
            }
        }
        else
        {
            //var aimDirection = Vector3.Normalize(transform.position + forwarddir);
            var b = Instantiate(_bulletPrefab, _shootpoint.transform.position, transform.rotation);
            b.GetComponent<Projectile>().direction = forwarddir;
            b.GetComponent<Projectile>().ownerShip = gameObject;
            b.GetComponent<Gun>()._attackStrength = _shipData.GetAttackForce();
            b.GetComponent<Gun>().ownerShip = this.gameObject;
            gunSound.Play();
        }
    }

    public void UseEnergy()
    {
        _shipData.UseEnergy();
        _healthBar.SetEnergy(_shipData.GetEnergyLevel());

        if (_shipData.GetEnergyLevel() == 0)
            _energyEmpty = true;
    }

    public void ReplenishEnergy()
    {
        _shipData.ReplenishEnergy();
        _healthBar.SetEnergy(_shipData.GetEnergyLevel());

        if (_shipData.GetEnergyLevel() > 50f)
            _energyEmpty = false;
    }

    public void TakeDamage(float AttackForce)
    {
        if (_shipData.TakeDamage(AttackForce))
        {
            UpdateHealth();
            Die();
        }
        else
            UpdateHealth();
    }

    public void UpdateHealth()
    {
        _healthBar.SetHealth(_shipData.GetHP());
        _healthBar.SetShieldHP(_shipData.GetShieldHP());
    }

    public ShipInformation GetShipData()
    {
        return _shipData;
    }

    public void SetShipData(int team, int shipType)
    {
        _shipData = new ShipInformation(team, shipType);
    }

    public void SetShipData(ShipInformation shipData)
    {
        _shipData = new ShipInformation(shipData);
    }

    public void Die()
    {
        Instantiate(_explosion, transform.position, transform.rotation);
        transform.GetChild(_shipData.GetTeam()).gameObject.SetActive(false);
        StartCoroutine(DeathOffset(3));
    }

    public void OnDestroy()
    {
        //Stop the Camera from following the player
        if(Camera.main != null)
            Camera.main.GetComponent<CameraMovementMenu>().enabled = false;
        _levelManager.EndGame(true);
    }

    IEnumerator DeathOffset(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Destroy(this.gameObject);
    }
}
