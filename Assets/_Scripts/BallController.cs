using System;
using UnityEngine;

namespace _Scripts
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Vector3 startSpeed;

        private bool _onGate;
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

        private void OnTriggerEnter(Collider other)
        {
            if(_onGate) return;
            
            if (other.gameObject.TryGetComponent(out IGate gate)) 
            {
                gate.Execute();
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
