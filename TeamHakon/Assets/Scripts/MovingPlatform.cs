using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class MovingPlatform : MonoBehaviour
    {

        private Vector3 originalPosition;
        private Vector3 markerPosition;

        public float PlatformSpeed;

        private Boolean movingTowardsMarker;

        private float dx;
        private float dz;

        // Use this for initialization
        void Start ()
        {
            originalPosition = transform.position;
            var children = GetComponentInChildren<Transform>();
            foreach (var child in children.Cast<Transform>().Where(child => child.tag == "Marker"))
            {
                markerPosition = child.position;
            }

            movingTowardsMarker = true;

            dx = markerPosition.x - originalPosition.x;
            dz = markerPosition.z - originalPosition.z;

        }
	

        // Update is called once per frame
        void Update () {

            if (movingTowardsMarker)
            {
                transform.position = new Vector3(transform.position.x + (dx * Time.deltaTime * PlatformSpeed), 0, transform.position.z + (dz * Time.deltaTime * PlatformSpeed));
            }
            else
            {
                transform.position = new Vector3(transform.position.x - (dx*Time.deltaTime*PlatformSpeed), 0,
                    transform.position.z - (dz*Time.deltaTime*PlatformSpeed));
            }

            if (transform.position == markerPosition)
            {
                movingTowardsMarker = false;
            }

            if (transform.position == originalPosition)
            {
                movingTowardsMarker = true;
            }
           
        }
    }
}
