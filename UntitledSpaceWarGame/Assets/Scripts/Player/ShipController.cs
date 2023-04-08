using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ShipController : MonoBehaviour
{
    public float _forwardSpeed = 35f;
    public float _strafeSpeed = 7.5f;
    public float _hoverSpeed = 5f;

    public float _LookRateSpeed = 90f;

    public float _rollSpeed = 90f;

    private float _activeForwardSpeed;
    private float _activeStrafeSpeed;
    private float _activeHoverSpeed;



    public GameObject bulletPrefab;

    public GameObject shootpoint;



    private float _forwardAcceleration = 2.5f, _strafeAcceleration = 2f, _hoverAcceleration = 2f;
    private float _rollAcceleration = 3.5f;

    private float _rollInput;

    private Vector2 _lookInput, _centerOfScreen, _mouseDistance;

    // Start is called before the first frame update
    void Start()
    {
        _centerOfScreen.x = Screen.width / 2;
        _centerOfScreen.y = Screen.height / 2;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

    }




    public void Shoot()
    {
        var forwarddir = (shootpoint.transform.position - transform.position).normalized;
            
                //var aimDirection = Vector3.Normalize(transform.position + forwarddir);
                var b = Instantiate(bulletPrefab, shootpoint.transform.position, Quaternion.identity);
                b.GetComponent<Projectile>().direction = forwarddir;

            
        

    }


}
