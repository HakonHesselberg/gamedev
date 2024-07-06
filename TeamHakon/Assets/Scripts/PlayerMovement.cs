using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [FormerlySerializedAs("RightClickParticles")]
        public GameObject _rightClickParticles;

        [FormerlySerializedAs("OnDeathParticles")]
        public GameObject _onDeathParticles;

        // Variables for win timer
        [FormerlySerializedAs("GUITimerSkin")] public GUISkin _guiTimerSkin;
        private float _timer;
        private bool _hasWon;
        private bool _usedShortcut;
        private float _timeDeathPenalty;

        private float _originalTurnForce;
        private float _originalMoveForce;

        private float _turnForce;
        private float _moveForce;
        private Vector3 _facingDirection;
        private Vector3 _targetPoint;
        private Vector3 _vectorToTarget;
        private float _hitDist;
        private Vector3 _desiredDirection;
        private RaycastHit _hit;

        private Vector3 _respawnPosition;
        private float _timePlayerIsFrozenWhenRespawning;

        private float _friction;
        private float _baseDrag;
        private float _baseAngularDrag;

        private float _grassFriction;
        private float _softIceFriction;
        private float _hardIceFriction;

        private Rigidbody _rigidbody;
        private Camera _mainCamera;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            // Rigidbody variables
            _rigidbody.mass = 10;
            _baseAngularDrag = 20;
            _baseDrag = 10;

            _originalMoveForce = 500;
            _moveForce = _originalMoveForce;

            _originalTurnForce = 500;
            _turnForce = _originalTurnForce;


            _targetPoint = transform.position;
            _respawnPosition = transform.position;
            _hitDist = 0.0f;
            _desiredDirection = (_targetPoint - transform.position).normalized;
            _friction = 1.0f;
            _rigidbody.angularDrag = _baseAngularDrag;
            _rigidbody.drag = _baseDrag;

            _grassFriction = 1.0f;
            _softIceFriction = 0.05f;
            _hardIceFriction = 0.0f;
            _timePlayerIsFrozenWhenRespawning = 2.0f;

            _timer = 0.0f;
            _hasWon = false;
            _usedShortcut = false;
            _timeDeathPenalty = 30.0f;
        }

        private void OnGUI()
        {
            GUI.skin = _guiTimerSkin;
            var labelRect = new Rect(20, 20, 300, 300);
            if (!_usedShortcut && !_hasWon)
            {
                GUI.Label(labelRect, "Score: " + Math.Round(_timer, 2).ToString("F2"));
            }
            else if (_usedShortcut && !_hasWon)
            {
                GUI.Label(labelRect, "You used a shortcut, score disabled.");
            }

            else if (_usedShortcut && _hasWon)
            {
                GUI.Label(labelRect, "Try without shortcuts next time :)");
            }
            else if (!_usedShortcut && _hasWon)
            {
                GUI.Label(labelRect,
                    "You won, congratulations! Your score was: " + Math.Round(_timer, 2).ToString("F2"));
            }
        }

        private void FixedUpdate()
        {
            if (!_hasWon) _timer += Time.deltaTime;

            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 10))
            {
                switch (_hit.transform.tag)
                {
                    case "Grass":
                        _friction = _grassFriction;
                        break;

                    case "Soft Ice":
                        _friction = _softIceFriction;
                        break;

                    case "Hard Ice":
                        _friction = _hardIceFriction;
                        break;

                    case "Lava":
                        Dead();
                        break;

                    case "Shortcut":
                        _usedShortcut = true;
                        break;
                }

                _rigidbody.drag = _baseDrag * _friction;
            }

            else
                Dead();
            // Set target point when right click
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonDown(1) ||
                Input.GetMouseButton(1))
            {
                var playerPlane = new Plane(Vector3.up, transform.position);
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

                if (playerPlane.Raycast(ray, out _hitDist))
                {
                    _targetPoint = ray.GetPoint(_hitDist);
                }

                // Spawns rightClickParticle
                var rightClickParticle = Instantiate(_rightClickParticles, _targetPoint, Quaternion.identity);
                Destroy(rightClickParticle.gameObject, 3);
            }

            _facingDirection = new Vector3(Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad), 0,
                -Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad));
            _vectorToTarget = _targetPoint - transform.position;

            if (_vectorToTarget.magnitude > 0.2f)
            {
                UpdateDirection();
                ApplyMovementForce();
            }
            else
            {
                UpdateAngle(_desiredDirection);
            }
        }

        public void Dead()
        {
            // Add penalty to timer
            _timer += _timeDeathPenalty;

            var deathParticle = Instantiate(_onDeathParticles, transform.position, Quaternion.identity);
            Destroy(deathParticle.gameObject, 2.0f);

            StartCoroutine(DisablePlayerMovement(_timePlayerIsFrozenWhenRespawning));
        }

        private IEnumerator DisablePlayerMovement(float waitTime)
        {
            transform.position = _respawnPosition;
            _targetPoint = _respawnPosition;
            _turnForce = 0.0f;
            _moveForce = 0.0f;

            // This also makes camera slow pan to player from death
            _rigidbody.velocity = Vector3.zero;

            yield return new WaitForSeconds(waitTime);
//          print("Enable player movement now, 2 seconds later");
            _targetPoint = _respawnPosition; // Makes sure player won't set target before they're able to move
            _turnForce = _originalTurnForce;
            _moveForce = _originalMoveForce;
        }

        private void UpdateDirection()
        {
            UpdateAngle(_vectorToTarget);
            _desiredDirection = _vectorToTarget.normalized;
        }

        private void ApplyMovementForce()
        {
            _rigidbody.AddForce(_vectorToTarget.normalized * (_moveForce * _friction));
        }

        private void UpdateAngle(Vector3 desired)
        {
            var angleToPoint = Mathf.Rad2Deg *
                               (Mathf.Atan2(_facingDirection.z, _facingDirection.x) -
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
                _rigidbody.AddTorque(new Vector3(0, Math.Sign(angleToPoint), 0) *
                    (_turnForce * (Mathf.Abs(angleToPoint) + 180)) / 360);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Checkpoint")
            {
                // Makes every other checkpoint blue
                foreach (var checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
                {
                    checkpoint.GetComponent<Renderer>().material.color = Color.blue;
                }

                // Make this checkpoint green and set respawn position to this position
                _respawnPosition = other.transform.position;
                _respawnPosition.y = 1;
                other.GetComponent<Renderer>().material.color = Color.green;
            }

            if (other.tag == "Goal")
            {
                _hasWon = true;
            }

            if (other.tag == "Explosion")
            {
                Dead();
            }
        }
    }
}