using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform Player;

        private PlayerMovement playerScript;

        private float cameraDisplacementZ, cameraDisplacementY;
        private float maxDistanceX, maxDistanceZ;

        private Vector3 previousPlayerPosition;

        public Boolean isCameraLocked = false;

        void Start ()
        {
            cameraDisplacementZ = -3.0f;
            cameraDisplacementY = 20.0f;
            maxDistanceX = 3.5f;
            maxDistanceZ = 3.5f;

	        transform.position = new Vector3(Player.position.x, cameraDisplacementY, Player.position.z + cameraDisplacementZ);
            
            previousPlayerPosition = Player.position;

//            playerScript = Player.GetComponent<PlayerMovement>();
        }
	
        // Update is called once per frame
        void Update ()
        {
            float newX, newZ;  

            if (Player.rigidbody.velocity.magnitude < 0.03f)
            {
                newX = transform.position.x + (Player.position.x - transform.position.x) * Time.deltaTime;
                newZ = transform.position.z + (Player.position.z - transform.position.z + cameraDisplacementZ) * Time.deltaTime;
            }
            else
            {
                newX = transform.position.x + (Player.position.x - previousPlayerPosition.x) * 1.5f;
                newZ = transform.position.z + (Player.position.z - previousPlayerPosition.z) * 1.5f;

                if (newX > Player.position.x + maxDistanceX)
                {
                    newX = Player.position.x + maxDistanceX;
                }
                else if (newX < Player.position.x - maxDistanceX)
                {
                    newX = Player.position.x - maxDistanceX;
                }

                if (newZ > Player.position.z + cameraDisplacementZ + maxDistanceZ)
                {
                    newZ = Player.position.z + cameraDisplacementZ + maxDistanceZ;
                }
                else if (newZ < Player.position.z + cameraDisplacementZ - maxDistanceZ)
                {
                    newZ = Player.position.z + cameraDisplacementZ - maxDistanceZ;
                }
            }
            
            transform.position = new Vector3(newX, cameraDisplacementY, newZ);
            
            previousPlayerPosition = Player.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isCameraLocked = !isCameraLocked;
            }

            if (isCameraLocked)
            {
                transform.position = new Vector3(Player.position.x, cameraDisplacementY, Player.position.z + cameraDisplacementZ);
            }
        }
    }
}
