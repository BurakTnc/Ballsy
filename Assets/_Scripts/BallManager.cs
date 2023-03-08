using UnityEngine;

namespace _Scripts
{
    public class BallManager : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        public int money;

        public void SpawnBall()
        {
            var ball = Instantiate(Resources.Load<GameObject>("Ball"));
            ball.transform.position = spawnPosition.position;
        }
    }
}
