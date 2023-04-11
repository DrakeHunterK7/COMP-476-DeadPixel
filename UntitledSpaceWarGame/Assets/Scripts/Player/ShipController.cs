using System.Collections;
using System.Collections.Generic;
using AI;
using TreeEditor;
using UnityEngine;



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

    [HideInInspector]
    public Vector3 velocity;

    //Weapon UI References
    [Header ("Weapon UI References")]
    public GameObject _missileIcon;
    public GameObject _missileCrossair;
    public GameObject _laserIcon;
    public GameObject _laserCrossair;

    //Ship Information Reference
    private ShipInformation _shipData;

    private float _forwardAcceleration = 2.5f, _strafeAcceleration = 2f, _hoverAcceleration = 2f;
    private float _rollAcceleration = 3.5f;

    private float _rollInput;

    private Vector2 _lookInput, _centerOfScreen, _mouseDistance;

    private LineRenderer laserLine;

    // Start is called before the first frame update
    void Start()
    {
        _centerOfScreen.x = Screen.width / 2;
        _centerOfScreen.y = Screen.height / 2;
        laserLine = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward*100f, Color.yellow, 0.1f);

        _forwardSpeed = GetShipData()._movementSpeed;
        
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


        if (!_weaponChanged)
        {
            laserLine.enabled = false;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Shoot();
            }
            else
            {
                laserLine.enabled = false;
            }
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
            //var b = Instantiate(_laserPrefab, _shootpoint.transform.position, transform.rotation);
            ////var b = Instantiate(_bulletPrefab, _shootpoint.transform.position, Quaternion.identity);
            //b.GetComponent<Projectile>().direction = forwarddir;
            
            laserLine.SetPosition(0, _shootpoint.transform.position);
            laserLine.enabled = true;
            RaycastHit hit;

            if (Physics.Raycast(_shootpoint.transform.position, forwarddir, out hit, 1000f))
            {
                laserLine.SetPosition(1, hit.point);
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


        }
        
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
