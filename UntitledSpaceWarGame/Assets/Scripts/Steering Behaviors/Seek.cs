using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    public Transform AI;
    public Transform target;

    // Start is called before the first frame update


    private Vector3 velocity;
    public float Mass = 15;
    public float MaxVelocity = 3;
    public float MaxForce = 15;
    void Start()
    {
        velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {

        if (AI == null)
        {
            return;
        }

        if (target == null)
        {
            return;
        }

        Vector3 desiredVelocity = target.position - AI.position;
        desiredVelocity = desiredVelocity.normalized * MaxVelocity;


        Vector3 steering = desiredVelocity - velocity;
        steering = Vector3.ClampMagnitude(steering, MaxForce);
        steering /= Mass;

        velocity = Vector3.ClampMagnitude(velocity + steering, MaxVelocity);


        AI.position += velocity * Time.deltaTime;

      
        AI.forward = velocity.normalized;

    }
}