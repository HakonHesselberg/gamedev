using Assets.Scripts;
using Assets.Scripts.NPCs;
using Assets.Scripts.NPCs.Chicken;
using UnityEngine;
using System.Collections;

public class ChickenBehaviour : MonoBehaviour, ICreature, IAreaOfEffect
{
    public GameObject EggPrefab;

    private float health;
    private float movementSpeed;
    
    private Color originalColor;

    private Vector3 originalPosition;
    
    private Vector3 targetPoint;
    private float maxRadius;
    private float minRadius;

    private float originalEggTime;
    private float eggTime;
    private float waitTime;

	void Start ()
	{
	    health = 100.0f;
	    movementSpeed = 2.0f;
	    originalColor = renderer.material.color;
	
        originalPosition = transform.position;
	    maxRadius = 5f;
	    minRadius = 2f;
        targetPoint = chooseNewTarget();

	    originalEggTime = 3.0f;
	    eggTime = originalEggTime;
	    waitTime = 1.5f;

	}
	
	void Update ()
	{
        eggTime -= Time.deltaTime;

        // If eggtime is greater than 0, move
        if (eggTime > 0)
        {
            movement();
        } 

        // If eggtime is below 0, do nothing until eggtime is below waitTime
        else if (eggTime <= -waitTime)
        {
            eggTime = originalEggTime;

            // Sometimes drop egg
            if (Random.Range(0f, 1f) < 0.5)
            {
                attack();
            }
        }
	}

    public void movement()
    {
        if ((targetPoint - transform.position).magnitude < 0.5f)
        {
            targetPoint = chooseNewTarget();               
        }
        
        transform.LookAt(targetPoint);

        transform.position += transform.forward * Time.deltaTime * movementSpeed;
    }

    public void attack()
    {
        var egg = Instantiate(EggPrefab, transform.position, transform.rotation) as GameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            takeDamage(other.GetComponent<BulletBehaviour>().getDamage());
            Destroy(other.gameObject);
        }
    }

    public void takeDamage(float damageTaken)
    {
        health -= damageTaken;
        StartCoroutine(flashOnHit());

        if (health <= 0)
        {
            death();
        }
    }

    public void death()
    {
        Destroy(gameObject);
        //TODO death animation
    }

    IEnumerator flashOnHit()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.material.color = originalColor;
    }

    private Vector3 chooseNewTarget()
    {
        Vector3 returnVector = Vector3.zero;
        
        returnVector.x = originalPosition.x + Random.Range(-maxRadius, maxRadius);
        returnVector.z = originalPosition.z + Random.Range(-maxRadius, maxRadius);

        if ((returnVector - transform.position).magnitude < minRadius)
        {
            returnVector = chooseNewTarget();
        }

        return returnVector;
    }

    public void TakeAoEDamage(float damageAmount)
    {
        health -= damageAmount;
    }
}
