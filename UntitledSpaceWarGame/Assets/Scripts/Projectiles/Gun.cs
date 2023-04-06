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
        Debug.Log(direction);

        if (Vector3.Distance(startpos, transform.position) > 1000f)
        {
            Destroy(this.gameObject);
        }
    }
}
