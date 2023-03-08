using System;
using UnityEngine;

namespace _Scripts
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Vector3 startSpeed;

        private bool _onGate;
        private bool _onZLimit;
        private SphereCollider _collider;
        private Rigidbody _rb;

        private void Awake()
        {
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
