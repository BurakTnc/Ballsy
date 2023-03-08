using System;
using UnityEngine;

namespace _Scripts
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        [SerializeField] private Transform spawnPosition;
        public int money;

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnBall()
        {
            var ball = Instantiate(Resources.Load<GameObject>("Ball"));
            ball.transform.position = spawnPosition.position;
        }
    }
}
