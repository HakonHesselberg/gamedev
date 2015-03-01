using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class TestingAIHealth : MonoBehaviour, IAreaOfEffect
{

    private float health;
    private Color originalColor;

	// Use this for initialization
	void Start ()
	{
	    health = 100;
	    originalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	    if (health < 0)
	    {
	        Destroy(gameObject);
	    }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            health -= 10;

            StartCoroutine(flashOnHit());

        }
    }

    IEnumerator flashOnHit()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        renderer.material.color = originalColor;
    }

    public void TakeAoEDamage(float damageAmount)
    {
        health -= damageAmount;
    }
}
