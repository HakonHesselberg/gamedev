using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    private float bulletSpeed = 10.0f;
    private float destroyTimer = 6f;
    private float shootCD;
    private float shootCDMax = 0.1f;
    private float spawnDistanceFromPlayer;


    // Use this for initialization
    private void Start()
    {
        spawnDistanceFromPlayer = (GetComponent<CapsuleCollider>().radius*transform.localScale.z) +
                                  (bulletPrefab.GetComponent<SphereCollider>().radius*
                                   bulletPrefab.transform.localScale.z);
        shootCD = shootCDMax;
    }

    // Update is called once per frame
    private void Update()
    {
        shootCD -= Time.deltaTime;

        if (Input.GetButton("Fire1"))
        {
            if (shootCD < 0)
            {
                Vector3 bulletPosition = transform.position + transform.forward*spawnDistanceFromPlayer;
                var bullet = Instantiate(bulletPrefab, bulletPosition, transform.rotation) as Rigidbody;
                bullet.GetComponent<BulletBehaviour>().setSpeed(bulletSpeed);
                bullet.GetComponent<BulletBehaviour>().setDestroyTimer(destroyTimer);
                shootCD = shootCDMax;
            }
        }
    }
}