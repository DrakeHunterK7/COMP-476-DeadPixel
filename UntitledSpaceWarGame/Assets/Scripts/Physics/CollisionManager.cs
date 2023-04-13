using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{

    private Collider _collider;
    private ObstaclePhysics _obstaclePhysics;

    private float _torqueMultiplier = 5.0f;

    List<Collider> _resolvedCollisions = new List<Collider>();


    // Start is called before the first frame update
    void Start()
    {
        _collider = gameObject.GetComponent<Collider>();
        _obstaclePhysics = gameObject.GetComponent<ObstaclePhysics>();
    }

    // Update is called once per frame
    void Update()
    {
        List<Collider> collidersWithinRange = new List<Collider>(GetNearbyColliders());

        foreach(Collider obstacleCollider in collidersWithinRange)
        {
            if(_collider != obstacleCollider && !_resolvedCollisions.Contains(obstacleCollider) && _collider.bounds.Intersects(obstacleCollider.bounds))
            {
                // Handle the collision resolution
                Debug.Log("Collision has occured");
                ResolveCollision(obstacleCollider);

                _resolvedCollisions.Add(obstacleCollider);
            }
        }

        foreach(Collider collider in _resolvedCollisions)
        {
            if(!collidersWithinRange.Contains(collider))
            {
                _resolvedCollisions.Remove(collider);
                break;
            }
        }
    }

    private Collider[] GetNearbyColliders()
    {
        return Physics.OverlapSphere(transform.position, _obstaclePhysics._radius);
    }

    private void ResolveCollision(Collider obstacleCollider)
    {
        if(obstacleCollider.tag == "Player")
        {
            ResolveCollisionWithPlayer(obstacleCollider);
            return;
        }

        Collider collider1 = _collider;
        Collider collider2 = obstacleCollider;

        ObstaclePhysics physicsObject1 = collider1.GetComponent<ObstaclePhysics>();
        ObstaclePhysics physicsObject2 = collider2.GetComponent<ObstaclePhysics>();

        // Get the points of intersection
        // This is wrong
        Vector3 intersectionPoint1 = collider1.ClosestPoint(collider2.transform.position);
        Vector3 intersectionPoint2 = collider2.ClosestPoint(collider1.transform.position);

        // Calculate the penetration depth
        float depth = (intersectionPoint1 - intersectionPoint2).magnitude;

        // Calculate the collision normal and tangent
        Vector3 normal = (intersectionPoint2 - intersectionPoint1).normalized;
        Vector3 tangent = Vector3.Cross(Vector3.up, normal);

        // Apply the collision response
        //transform.position -= depth * normal;

        // Calculate the relative velocity of the colliding objects
        Vector3 relativeVelocity = collider2.transform.position - transform.position;

        // Calculate the impulse along the collision normal
        float impulse = Vector3.Dot(relativeVelocity, normal) 
            //* GetComponent<MeshFilter>().sharedMesh.bounds.size.magnitude
            * (1/physicsObject1._mass + 1/physicsObject2._mass);

        // Apply the impulse to the colliding objects
        physicsObject1._velocity = -impulse/physicsObject1._mass * normal;
        physicsObject2._velocity = impulse/physicsObject2._mass * normal;

        //
        //collider1.transform.position -= impulse * normal.normalized;
        //collider2.transform.position += impulse * normal.normalized;

        // Calculate the torque along the collision tangent
        float torque = Vector3.Dot(relativeVelocity, tangent) 
            * (1/physicsObject1._mass + 1/physicsObject2._mass)
            * _torqueMultiplier;// * GetComponent<MeshFilter>().sharedMesh.bounds.size.magnitude;

        // Apply the torque to the colliding objects
        physicsObject1._rotation += torque / physicsObject1._mass * tangent.normalized * Time.deltaTime;
        physicsObject2._rotation += -torque / physicsObject2._mass * tangent.normalized * Time.deltaTime;
    }

    public void ResolveCollisionWithPlayer(Collider playerCollider)
    {
        Collider collider1 = _collider;
        ObstaclePhysics physicsObject1 = collider1.GetComponent<ObstaclePhysics>();

        // Get the points of intersection
        // This is wrong
        Vector3 intersectionPoint1 = collider1.ClosestPoint(playerCollider.transform.position);
        Vector3 intersectionPoint2 = playerCollider.ClosestPoint(collider1.transform.position);

        // Calculate the penetration depth
        float depth = (intersectionPoint1 - intersectionPoint2).magnitude;

        // Calculate the collision normal and tangent
        Vector3 normal = (intersectionPoint2 - intersectionPoint1).normalized;
        Vector3 tangent = Vector3.Cross(Vector3.up, normal);

        // Apply the collision response
        //transform.position -= depth * normal;

        // Calculate the relative velocity of the colliding objects
        Vector3 relativeVelocity = playerCollider.transform.position - transform.position;

        // Calculate the impulse along the collision normal
        float impulse = Vector3.Dot(relativeVelocity, normal)
            //* GetComponent<MeshFilter>().sharedMesh.bounds.size.magnitude
            * (1 / physicsObject1._mass + 1 / 10.0f);

        // Apply the impulse to the colliding objects
        physicsObject1._velocity = -impulse / physicsObject1._mass * normal;
        Debug.Log("ASTEROID VELOCITY: " + physicsObject1._velocity);

        //
        //collider1.transform.position -= impulse * normal.normalized;
        //collider2.transform.position += impulse * normal.normalized;

        // Calculate the torque along the collision tangent
        float torque = Vector3.Dot(relativeVelocity, tangent)
            * (1 / physicsObject1._mass + 1 / 30.0f)
            * _torqueMultiplier;// * GetComponent<MeshFilter>().sharedMesh.bounds.size.magnitude;

        // Apply the torque to the colliding objects
        physicsObject1._rotation += torque / physicsObject1._mass * tangent.normalized * Time.deltaTime;
    }



}
