using UnityEngine;
using System.Collections;

public class TestingAIMovement : MonoBehaviour
{

    private float speed;
    private float behaviourTimer;

    private Vector3 targetPoint;
    private Vector3 originalPoint;
    // Use this for initialization
    void Start()
    {
        speed = 4.0f;
        behaviourTimer = 0;
        targetPoint = transform.position;
        originalPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviourTimer <= 0)
        {
            newTimer();
            updateTarget();
            transform.LookAt(targetPoint);
        }

        transform.position += transform.forward * Time.deltaTime * speed;

        behaviourTimer -= Time.deltaTime;
    }

    private void newTimer()
    {
        behaviourTimer = Random.Range(2, 3);
    }

    private void updateTarget()
    {
        targetPoint.x = originalPoint.x + Random.Range(-4, 4);
        targetPoint.z = originalPoint.z + Random.Range(-4, 4);
    }
}
