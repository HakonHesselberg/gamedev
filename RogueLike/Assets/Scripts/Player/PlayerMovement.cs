using System.Security.Policy;
using Assets.Scripts;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 6.0f;
    private Vector3 moveDirection = Vector3.zero;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        rotatePlayer();
        movePlayer();
    }

    private void movePlayer()
    {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection *= movementSpeed;

        controller.Move(moveDirection * Time.deltaTime);
    }

    private void rotatePlayer()
    {
        Vector3 targetPoint = Vector3.zero;

        float hitDistance;

        var playerPlane = new Plane(Vector3.up, transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (playerPlane.Raycast(ray, out hitDistance))
        {
            targetPoint = ray.GetPoint((hitDistance));
        }

        transform.LookAt(targetPoint);
    }


}
