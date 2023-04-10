using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    [Header ("Movement Variables")]
    public float _forwardSpeed = 35f;
    public float _strafeSpeed = 7.5f;
    public float _hoverSpeed = 5f;

    public float _LookRateSpeed = 90f;

    public float _rollSpeed = 90f;

    private float _activeForwardSpeed;
    private float _activeStrafeSpeed;
    private float _activeHoverSpeed;
        
    public GameObject _shootpoint;

    //Weapon Prefabs
    [Header ("Weapon Prefabs")]
    public GameObject _bulletPrefab;
    public GameObject _laserPrefab;
    private bool _weaponChanged = false;


    //Weapon UI References
    [Header ("Weapon UI References")]
    public GameObject _missileIcon;
    public GameObject _missileCrossair;
    public GameObject _laserIcon;
    public GameObject _laserCrossair;

    //Ship Information
    private ShipInformation _shipData;

    //Health and Energy Bar References
    [Header("Health Bar Reference")]
    [SerializeField] private HealthBar _healthBar;

    private float _forwardAcceleration = 2.5f, _strafeAcceleration = 2f, _hoverAcceleration = 2f;
    private float _rollAcceleration = 3.5f;

    private float _rollInput;

    private Vector2 _lookInput, _centerOfScreen, _mouseDistance;

    void Awake()
    {
        _centerOfScreen.x = Screen.width / 2;
        _centerOfScreen.y = Screen.height / 2;
    }

    private void Start()
    {
        //Set HP Values
    }

    // Update is called once per frame
    void Update()
    {
        // Look rotation calculations
        _lookInput = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        _mouseDistance.x = (_lookInput.x - _centerOfScreen.x) / _centerOfScreen.y;
        _mouseDistance.y = (_lookInput.y - _centerOfScreen.y) / _centerOfScreen.y;

        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);

        // Roll rotation calculations
        _rollInput = Mathf.Lerp(_rollInput, Input.GetAxisRaw("Roll"), _rollAcceleration * Time.deltaTime);

      
        // Movement calculations
        _activeForwardSpeed = Mathf.Lerp(_activeForwardSpeed, Input.GetAxisRaw("Vertical") * _forwardSpeed, _forwardAcceleration * Time.deltaTime);
        _activeStrafeSpeed = Mathf.Lerp(_activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * _strafeSpeed, _strafeAcceleration * Time.deltaTime);
        _activeHoverSpeed = Mathf.Lerp(_activeHoverSpeed, Input.GetAxisRaw("Hover") * _hoverSpeed, _hoverAcceleration * Time.deltaTime);


        // Apply Transformation
        transform.position += transform.forward * _activeForwardSpeed * Time.deltaTime;
        transform.position += transform.right * _activeStrafeSpeed * Time.deltaTime;
        transform.position += transform.up * _activeHoverSpeed * Time.deltaTime;

        // Apply Rotation
        transform.Rotate(-_mouseDistance.y * _LookRateSpeed * Time.deltaTime, _mouseDistance.x * _LookRateSpeed * Time.deltaTime, _rollInput * _rollSpeed * Time.deltaTime, Space.Self);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeWeapon();
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
            //TODO: Add a laser prefab
            var b = Instantiate(_laserPrefab, _shootpoint.transform.position, Quaternion.identity);
            //var b = Instantiate(_bulletPrefab, _shootpoint.transform.position, Quaternion.identity);
            b.GetComponent<Projectile>().direction = forwarddir;
        }
        else
        {
            //var aimDirection = Vector3.Normalize(transform.position + forwarddir);
            var b = Instantiate(_bulletPrefab, _shootpoint.transform.position, Quaternion.identity);
            b.GetComponent<Projectile>().direction = forwarddir;
        }
        
    }

    public void TakeDamage(float AttackForce)
    {
        _shipData.TakeDamage(AttackForce);
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
}
