using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {


        public GameObject RightClickParticles;
        public GameObject OnDeathParticles;

        // Variables for wintimer
        public GUISkin GUITimerSkin;
        private float timer;
        private Boolean hasWon;
        private Boolean usedShortcut;
        private float timeDeathPenalty;
        
        private float originalTurnForce;
        private float originalMoveForce;

        private float turnForce;
        private float moveForce;
        private Vector3 facingDirection;
        private Vector3 targetPoint;
        private Vector3 vectorToTarget;
        private float hitDist;
        private Vector3 desiredDirection;
        private RaycastHit hit;

        private Vector3 respawnPosition;
        private float timePlayerIsFrozenWhenRespawning;

        private float friction;
        private float baseDrag;
        private float baseAngularDrag;

        private float grassFriction;
        private float softIceFriction;
        private float hardIceFriction;



        private void Start ()
        {
            // Rigidbody variables
            GetComponent<Rigidbody>().mass = 10;
            baseAngularDrag = 20;
            baseDrag = 10;

            originalMoveForce = 500;
            moveForce = originalMoveForce;

            originalTurnForce = 500;
            turnForce = originalTurnForce;
            
            
            targetPoint = transform.position;
            respawnPosition = transform.position;
            hitDist = 0.0f;
            desiredDirection = (targetPoint - transform.position).normalized;
            friction = 1.0f;
            GetComponent<Rigidbody>().angularDrag = baseAngularDrag;
            GetComponent<Rigidbody>().drag = baseDrag;

            grassFriction = 1.0f;
            softIceFriction = 0.05f;
            hardIceFriction = 0.0f;
            timePlayerIsFrozenWhenRespawning = 2.0f;

            timer = 0.0f;
            hasWon = false;
            usedShortcut = false;
            timeDeathPenalty = 30.0f;
        }

        void OnGUI()
        {
            GUI.skin = GUITimerSkin;
            if (!usedShortcut && !hasWon)
            {
                GUI.Label(new Rect(20, 20, 300, 300), "Score: " + Math.Round(timer, 2).ToString("F2"));
            }
            else if (usedShortcut && !hasWon)
            {
                GUI.Label(new Rect(20, 20, 300, 300), "You used a shortcut, score disabled.");
            }

            else if (usedShortcut && hasWon)
            {
                GUI.Label(new Rect(20, 20, 300, 300), "Try without shortcuts next time :)");
            } else if (!usedShortcut && hasWon)
            {
                GUI.Label(new Rect(20, 20, 300, 300), "You won, congratulations! Your score was: " + Math.Round(timer, 2).ToString("F2"));
            }
        }

        // Update is called once per frame
        void FixedUpdate ()
        {
            #region As long as player has not won, increase timer
            if (!hasWon)
            {
                timer += Time.deltaTime;
            }
            #endregion

            #region Switch for what we hit
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
            {
                switch (hit.transform.tag)
                {
                    case "Grass":
                        friction = grassFriction;

                        break;

                    case "Soft Ice":
                        friction = softIceFriction;

                        break;

                    case "Hard Ice":
                        friction = hardIceFriction;
                        break;

                    case "Lava":
                        Dead();
                        break;
                    
                    case "Shortcut":
                        usedShortcut = true;
                        break;
                }

//                rigidbody.angularDrag = baseAngularDrag * friction;
                GetComponent<Rigidbody>().drag = baseDrag*friction;
            }
            #endregion

            //TODO This is probably bad
            else Dead();

            // Set targetpoint when rightclick
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                var playerPlane = new Plane(Vector3.up, transform.position);
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (playerPlane.Raycast(ray, out hitDist))
                {
                    targetPoint = ray.GetPoint(hitDist);
                }

                // Spawns rightClickParticle
                var rightClickParticle = (GameObject)(Instantiate(RightClickParticles, targetPoint, Quaternion.identity));
                Destroy(rightClickParticle.gameObject, 3);
               
            }

            facingDirection = new Vector3(Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad), 0, -Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad));
            vectorToTarget = targetPoint - transform.position;
          

            if (vectorToTarget.magnitude > 0.2f)
            {
                UpdateDirection();
                ApplyMovementForce();
            }
            else
            {
                UpdateAngle(desiredDirection);
            }
        }

        public void Dead()
        {
            // Add penalty to timer
            timer += timeDeathPenalty;

            var deathParticle = (GameObject)(Instantiate(OnDeathParticles, transform.position, Quaternion.identity));
            Destroy(deathParticle.gameObject, 2.0f);

            StartCoroutine(DisablePlayerMovement(timePlayerIsFrozenWhenRespawning));
        }

        IEnumerator DisablePlayerMovement(float waitTime)
        {
//          print("Disable player movement now");
            transform.position = respawnPosition;
            targetPoint = respawnPosition;
            turnForce = 0.0f;
            moveForce = 0.0f;

            // This also makes camera slowpan to player from death
            GetComponent<Rigidbody>().velocity = Vector3.zero; 
            
            yield return new WaitForSeconds(waitTime);
//          print("Enable player movement now, 2 seconds later");
            targetPoint = respawnPosition; // Makes sure player won't set target before they're able to move
            turnForce = originalTurnForce;
            moveForce = originalMoveForce;
        }
        
        void UpdateDirection()
        {
            UpdateAngle(vectorToTarget);
            desiredDirection = vectorToTarget.normalized;
        }

        void ApplyMovementForce()
        {
            GetComponent<Rigidbody>().AddForce(vectorToTarget.normalized * moveForce * friction);
        }

        void UpdateAngle(Vector3 desired)
        {
            var angleToPoint = Mathf.Rad2Deg *
                              (Mathf.Atan2(facingDirection.z, facingDirection.x) -
                               Mathf.Atan2(desired.z, desired.x));

            if (angleToPoint > 180)
            {
                angleToPoint -= 360;
            }
            else if (angleToPoint < -180)
            {
                angleToPoint += 360;
            }

            if (Math.Abs(angleToPoint) > 5)
            {
                GetComponent<Rigidbody>().AddTorque(new Vector3(0, Math.Sign(angleToPoint), 0) * turnForce * (Mathf.Abs(angleToPoint) + 180) / 360);
            }
        }

        public Vector3 GetFacingDirection()
        {
            return facingDirection;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Checkpoint")
            {
                // Makes every other checkpoint blue
                foreach (var checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
                {
                    checkpoint.GetComponent<Renderer>().material.color = Color.blue;
                }

                // Make this checkpoint green and set respawnposition to this position
                respawnPosition = other.transform.position;
                respawnPosition.y = 1;
                other.GetComponent<Renderer>().material.color = Color.green;
            }
            if (other.tag == "Goal")
            {
                hasWon = true;
            }

            if (other.tag == "Explosion")
            {
                Dead();
            }
        }
    }
}
