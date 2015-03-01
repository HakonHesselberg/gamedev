using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float destroyTimer;
    private float speed;
    private float damage;
    // Use this for initialization
    private void Start()
    {
        damage = 10.0f;
        gameObject.renderer.material.color = new Color(Random.Range(254, 255)/255f, Random.Range(10, 255)/255f,
            Random.Range(0, 1)/255f);
    }

    // Update is called once per frame
    private void Update()
    {
        destroyTimer -= Time.deltaTime;
        if (destroyTimer < 0)
        {
            selfDestruct();
        }

        transform.position += speed*transform.forward*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            selfDestruct();
        }
    }

    private void selfDestruct()
    {
        Destroy(gameObject);
    }

    public void setDestroyTimer(float time)
    {
        destroyTimer = time;
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public float getDamage()
    {
        return damage;
    }
}