using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform _target;

    public float _smoothSpeed = 0.3f;
    public Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        Vector3 desiredPosition = -_target.forward + _offset;
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothSpeed);

        transform.position = smoothPosition;

        transform.LookAt(_target);

    }
}
