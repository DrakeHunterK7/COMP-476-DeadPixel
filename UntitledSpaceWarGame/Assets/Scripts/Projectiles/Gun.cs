using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Projectile
{
    private float bulletspeed = 1000f;

    private List<Collider> damageableentities = new List<Collider>();

    private Vector3 startpos;

   private Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        collider = gameObject.GetComponent<Collider>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * bulletspeed * Time.deltaTime;

        if (Vector3.Distance(startpos, transform.position) > 1000f)
        {
            Destroy(this.gameObject);
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100f);

        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.CompareTag("AI") || hitCollider.gameObject.CompareTag("Player"))
                {
                    if (!damageableentities.Contains(hitCollider))
                    damageableentities.Add(hitCollider);
                }
            }
        }

        if (damageableentities.Count > 0)
        {
            foreach (Collider entity in damageableentities)
            {
                if (entity != null)
                {
                    if (collider.bounds.Intersects(entity.bounds))
                    {
                        Debug.Log("Has been hit");
                        Destroy(this.gameObject);
                    }
                }
                else
                {
                    damageableentities.Remove(entity);
                    break;
                }
            }
        }




    }
}
