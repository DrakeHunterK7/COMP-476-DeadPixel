using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePhysics : MonoBehaviour
{
    public float _mass;
    public float _radius;
    public Vector3 _velocity;
    public Vector3 _rotation;
    public float _accelerationRate;

    public float _maxSpeed;
    private float _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState((int) System.DateTime.Now.Millisecond);

        float x_rotation = Random.Range(0.0f, 1.0f);
        float y_rotation = Random.Range(0.0f, 1.0f);
        float z_rotation = Random.Range(0.0f, 1.0f);

        _rotation = new Vector3(x_rotation, y_rotation, z_rotation).normalized / _mass;
    }

    // Update is called once per frame
    void Update()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _velocity.normalized.magnitude * _maxSpeed, _accelerationRate * Time.deltaTime);

        transform.position += _velocity.normalized * _currentSpeed * Time.deltaTime;
        transform.Rotate(_rotation, Space.World);
    }
}
