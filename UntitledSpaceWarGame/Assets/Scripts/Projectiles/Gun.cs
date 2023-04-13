using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Projectile
{
    private float bulletspeed = 2000f;

    public float _attackStrength;
    public GameObject destroyPrefab;

    private List<Collider> damageableentities = new List<Collider>();

    private Vector3 startpos;

    private Collider collider;

    public AudioClip explosionSound;

    [SerializeField] private GameObject _explosion;

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

        if (Vector3.Distance(startpos, transform.position) > 1500f)
        {
            Destroy(this.gameObject);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitData;

        if (Physics.Raycast(ray, out hitData, 500f, LayerMask.GetMask("Ships")))
        {
            var hitObject = hitData.collider.gameObject;

            if (hitObject.GetComponent<ShipAIBT>() != null)
            {
                var script = hitObject.GetComponent<ShipAIBT>();
                script.SetRootData("Target", ownerShip);
            }
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
                    if (entity.gameObject == ownerShip) continue;
                    
                    if (collider.bounds.Intersects(entity.bounds))
                    {
                        if (entity.gameObject.tag == "Player")
                        {
                            entity.gameObject.GetComponent<ShipController>().TakeDamage(_attackStrength);
                        }
                        else
                        {
                            if (entity.gameObject.GetComponent<ShipAIBT>().GetShipData().TakeDamage(_attackStrength))
                            {
                                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
                                Instantiate(_explosion, entity.gameObject.transform.position, entity.gameObject.transform.rotation);
                                entity.gameObject.GetComponent<ShipAIBT>().mothership._teamShips
                                    .Remove(entity.gameObject);
                                entity.gameObject.GetComponent<ShipAIBT>().mothership.ships
                                    .Remove(entity.gameObject.GetComponent<ShipAIBT>());
                                Destroy(entity.gameObject);
                            }
                        }

                        Instantiate(destroyPrefab, entity.gameObject.transform.position, entity.gameObject.transform.rotation);
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
