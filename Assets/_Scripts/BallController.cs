using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public class BallController : MonoBehaviour
    {
        [SerializeField] private Material[] colors;
        [SerializeField] private AudioClip popSound, throwSound;
        [SerializeField] private Vector3 startSpeed;

        private Material _material;
        private Camera _cam;
        private bool _isSpawned;
        private bool _onGate;
        private bool _onZLimit;
        private SphereCollider _collider;
        private Rigidbody _rb;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
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
            _material.color = colors[Random.Range(0, colors.Length - 1)].color;
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

        private void StopParticle(GameObject obj)
        {
            Destroy(obj);
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

                var textParticle = Instantiate(Resources.Load<GameObject>("Text (TMP)"));
                textParticle.transform.position = transform.position;
                textParticle.transform.DOMoveY(3, 2).SetEase(Ease.InSine).OnComplete(() => StopParticle(textParticle));
                textParticle.transform.DOScale(Vector3.zero, 1).SetEase(Ease.InSine);
                BallManager.Instance.money++;
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
