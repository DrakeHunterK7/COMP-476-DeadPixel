using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolvePlayerCollision : MonoBehaviour
{
    List<Collider> _collidersInRange = new List<Collider>();
    Collider _playerCollider;
    ShipController _controller;
    bool _collided = false;
    float _maxDisableTime = 3.0f;
    float _disableTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _playerCollider = GetComponent<Collider>();
        _controller = GetComponent<ShipController>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckForColliders());

        foreach(Collider collider in _collidersInRange)
        {
            if(_playerCollider.bounds.Intersects(collider.bounds))
            {
                _collided = true;
                if(collider.tag == "Obstacle")
                {
                    collider.gameObject.GetComponent<CollisionManager>().ResolveCollisionWithPlayer(_playerCollider);
                }
            }
        }

        if(_collided)
        {
            Debug.Log("Player Controls Disabled");
            _controller._activeForwardSpeed = Mathf.Lerp(_controller._activeForwardSpeed, -75.0f, 5.0f * Time.deltaTime);
            _controller._canMove = false;
            transform.position += transform.forward * _controller._activeForwardSpeed * Time.deltaTime;

            
            if (_disableTime > _maxDisableTime)
            {
                _disableTime = 0.0f;
                _controller._canMove = true;
                _collided = false;
            }
            else
            {
                _disableTime += Time.deltaTime;
            }
        }
    }

    IEnumerator CheckForColliders()
    {
        List<Collider> collidersInRange = new List<Collider>(Physics.OverlapSphere(transform.position, 20.0f, 1 << 7));

        foreach (Collider collider in collidersInRange)
        {
            if(collider != _playerCollider)
            {
                _collidersInRange.Add(collider);
            }
        }

        yield return null;
    }
}
