using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Projectile
{
    private float bulletspeed = 1000f;

    private Vector3 startpos;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * bulletspeed * Time.deltaTime;

        if (Vector3.Distance(startpos, transform.position) > 1000f)
        {
            Destroy(this.gameObject);
        }

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 50f, ~LayerMask.GetMask("Projectiles")))
        {
            Debug.Log("RUNNING");
            if (hit.collider.gameObject.CompareTag("AI"))
            {
                Debug.Log("GONNA HIT");
                var shipScript = hit.collider.gameObject.GetComponentInParent<ShipAIBT>();
                shipScript.SetRootData("Target", ownerShip);
            }
        };
    }
}
