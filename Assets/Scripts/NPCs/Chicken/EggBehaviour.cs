using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class EggBehaviour : MonoBehaviour, IAreaOfEffect
{

    private float blastRadius;
    private float damage;
    private float detonationTime;

    private float health;

    // Use this for initialization
    void Start()
    {
        blastRadius = 3.0f;
        damage = 10.0f;
        detonationTime = 3.0f;

        health = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            detonate();
        }

        detonationTime -= Time.deltaTime;
        if (detonationTime < 0)
        {
            detonate();
        }
    }

    private void detonate()
    {
        // TODO make explosion animation
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider objectHit in hitColliders)
        {
            IAreaOfEffect IAoE = (IAreaOfEffect)objectHit.GetComponent(typeof(IAreaOfEffect));

            if (IAoE != null)
            {
                IAoE.TakeAoEDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    public void TakeAoEDamage(float damageAmount)
    {
        health -= damageAmount;
    }
}
