using System;
using UnityEngine;

namespace _Scripts
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private AudioClip popSound, throwSound;
        [SerializeField] private Vector3 startSpeed;

        private Camera _cam;
        private bool _isSpawned;
        private bool _onGate;
        private bool _onZLimit;
        private SphereCollider _collider;
        private Rigidbody _rb;

        private void Awake()
        {
            _cam = Camera.main;
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<SphereCollider>();
        }

        private void Start()
        {
            _rb.velocity = startSpeed;
        }

        public void BlockMultiply()
        {
            _onGate = true;
        }

        public void SetAsSpawned()
        {
            _isSpawned = true;
        }

        private void AllowMultiply()
        {
            _onGate = false;
        }

        private void Update()
        {
            if(_onZLimit) return;
            
            var position = transform.position;
            var clampedPos = new Vector3(position.x, position.y,
                Math.Clamp(position.z, 0, 0));
            position = clampedPos;
            transform.position = position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(_onGate) return;
            
            if (other.gameObject.TryGetComponent(out IGate gate)) 
            {
                gate.Execute();
            }

            if (other.gameObject.CompareTag("Destroy"))
            {
                if(!_isSpawned) return;

                AudioSource.PlayClipAtPoint(popSound, _cam.transform.position);
                var particle = Instantiate(Resources.Load<GameObject>("Confetti"));
                particle.transform.position = transform.position;
                Destroy(gameObject);
            }

            if (other.gameObject.CompareTag("Respawn"))
            {
                _rb.velocity = Vector3.zero;
                _rb.AddForce(other.transform.forward*1500,ForceMode.Acceleration);
            }

            if (other.gameObject.CompareTag("Finish"))
            {
                _onZLimit = !_onZLimit;
                _rb.velocity = Vector3.zero;
                _rb.AddForce(other.transform.forward*1500,ForceMode.Acceleration);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                AudioSource.PlayClipAtPoint(throwSound, _cam.transform.position);
                _onZLimit = !_onZLimit;
                _rb.AddForce(other.transform.forward*750,ForceMode.Acceleration);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if(!_onGate) return;
            
            if (other.gameObject.TryGetComponent(out IGate gate)) 
            {
                AllowMultiply();
            }
        }
    }
}
