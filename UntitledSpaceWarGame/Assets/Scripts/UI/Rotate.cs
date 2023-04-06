using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private Vector3 _rotationAxis;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_velocity * _rotationAxis * Time.deltaTime);
    }
}
