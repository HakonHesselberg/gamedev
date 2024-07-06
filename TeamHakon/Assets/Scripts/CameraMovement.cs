using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private bool _isCameraLocked = false;

        private PlayerMovement playerScript;

        private float cameraDisplacementZ, cameraDisplacementY;
        private float maxDistanceX, maxDistanceZ;

        private Vector3 previousPlayerPosition;

        private void Awake()
        {
            if (_player == null) 
                Debug.LogError($"{nameof(_player)} is not set in {nameof(CameraMovement)} script");
        }
        
        private void Start ()
        {
            cameraDisplacementZ = -3.0f;
            cameraDisplacementY = 20.0f;
            maxDistanceX = 3.5f;
            maxDistanceZ = 3.5f;

	        transform.position = new Vector3(_player.position.x, cameraDisplacementY, _player.position.z + cameraDisplacementZ);
            
            previousPlayerPosition = _player.position;

//            playerScript = Player.GetComponent<PlayerMovement>();
        }
	
        private void Update ()
        {
            float newX, newZ;  

            if (_player.GetComponent<Rigidbody>().velocity.magnitude < 0.03f)
            {
                newX = transform.position.x + (_player.position.x - transform.position.x) * Time.deltaTime;
                newZ = transform.position.z + (_player.position.z - transform.position.z + cameraDisplacementZ) * Time.deltaTime;
            }
            else
            {
                newX = transform.position.x + (_player.position.x - previousPlayerPosition.x) * 1.5f;
                newZ = transform.position.z + (_player.position.z - previousPlayerPosition.z) * 1.5f;

                if (newX > _player.position.x + maxDistanceX)
                {
                    newX = _player.position.x + maxDistanceX;
                }
                else if (newX < _player.position.x - maxDistanceX)
                {
                    newX = _player.position.x - maxDistanceX;
                }

                if (newZ > _player.position.z + cameraDisplacementZ + maxDistanceZ)
                {
                    newZ = _player.position.z + cameraDisplacementZ + maxDistanceZ;
                }
                else if (newZ < _player.position.z + cameraDisplacementZ - maxDistanceZ)
                {
                    newZ = _player.position.z + cameraDisplacementZ - maxDistanceZ;
                }
            }
            
            transform.position = new Vector3(newX, cameraDisplacementY, newZ);
            
            previousPlayerPosition = _player.position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isCameraLocked = !_isCameraLocked;
            }

            if (_isCameraLocked)
            {
                transform.position = new Vector3(_player.position.x, cameraDisplacementY, _player.position.z + cameraDisplacementZ);
            }
        }
    }
}
