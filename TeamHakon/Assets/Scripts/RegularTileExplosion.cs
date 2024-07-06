using System.Threading;
using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class RegularTileExplosion : MonoBehaviour
{

    public GameObject Explosion;
    public GameObject DeathCube;
    public GameObject Player;
    public float TimerDelay;
    public float RepeatTime;

    private bool isActive = true;

    // Use this for initialization
	void Start ()
	{
	    StartCoroutine(ShowExplosion());
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

    IEnumerator ShowExplosion()
    {
        while (isActive)
        {
            if (TimerDelay > 0)
            {
                yield return new WaitForSeconds(TimerDelay);
                TimerDelay = 0;
            }

            var deathCube = (GameObject)(Instantiate(DeathCube, transform.position, Quaternion.identity));
            if (deathCube.GetComponent<Collider>().bounds.Contains(Player.transform.position))
            {
                Player.GetComponent<PlayerMovement>().Dead();
            }
            Destroy(deathCube.gameObject, 0.001f);

            var deathParticle = (GameObject)(Instantiate(Explosion, transform.position, Quaternion.identity));
            Destroy(deathParticle.gameObject, 0.5f);

            yield return new WaitForSeconds(RepeatTime);
        }
    }
}
