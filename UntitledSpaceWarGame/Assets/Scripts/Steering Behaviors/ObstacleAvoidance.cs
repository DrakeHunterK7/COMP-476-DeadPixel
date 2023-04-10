using AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : AIMovement
{
    public LayerMask obstacleMask;
    public float lookAhead;
    public float avoidDistance;
    public bool _debugRays;

    public bool _avoidingCollision = false;

    [HideInInspector]
    private Vector3[] rayDirections = new Vector3[numberOfRays];

    private const int numberOfRays = 300;


    public override SteeringOutput GetMovement(AIAgent agent)
    {
        SteeringOutput output =  base.GetMovement(agent);
        float furthestDis = 0.0f;
        Vector3 bestDir = Vector3.zero;

        GetRayDirs(agent);

        // Visualizations
        if (_debugRays)
        {
            foreach (Vector3 dir in rayDirections)
            {
                Debug.DrawRay(agent.transform.position, dir * avoidDistance, Color.magenta);
            }
        }

        if (checkForCollision(agent))
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                Vector3 dir = agent.transform.TransformDirection(rayDirections[i]);
                Ray ray = new Ray(agent.transform.position, dir);
                RaycastHit hit;
                if (Physics.Raycast(agent.transform.position, dir, out hit, avoidDistance, obstacleMask))
                {
                    if(hit.distance > furthestDis)
                    {
                        bestDir = dir;
                        furthestDis = hit.distance;
                    }
                    output._rotation = Quaternion.LookRotation(bestDir, agent.transform.up);
                }
                else
                {
                    output._rotation = Quaternion.LookRotation(dir, agent.transform.up);
                    break;
                }
            }
            _avoidingCollision = true;
        }
        else
        {
            output._rotation = Quaternion.identity;
            _avoidingCollision = false;
        }

        //output._velocity = agent.transform.forward;

        return output;
    }

    // Runs each frame to check if the ai is close to colliding with an obstacle
    private bool checkForCollision(AIAgent agent)
    {
        RaycastHit[] hit;

        //Debug.Log();

        Debug.DrawRay(agent.transform.position, agent.transform.forward * lookAhead, Color.green);

        hit = Physics.SphereCastAll(agent.transform.position, 40.0f, agent.transform.forward, lookAhead, obstacleMask);

        if (hit.Length > 0)
        {
            Debug.Log("A collision is about to happen");
            return true;
        }

        return false;
    }

    // Method fills an array of rays for collision detection
    private void GetRayDirs(AIAgent agent)
    {
        rayDirections = new Vector3[numberOfRays];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * goldenRatio;

        for (int i = 0; i < numberOfRays; i++)
        {
            float t = (float)i / numberOfRays;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            rayDirections[i] = new Vector3(x, y, z).normalized;
        }
    }
}
