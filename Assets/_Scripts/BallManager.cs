using System;
using TMPro;
using UnityEngine;

namespace _Scripts
{
    public class BallManager : MonoBehaviour
    {
        public static BallManager Instance;
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI buttonPrice;
        public int money;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            moneyText.text = "$" + money;
        }

        public void SpawnBall()
        {
            var ball = Instantiate(Resources.Load<GameObject>("MainBall"));

            ball.name = "MainBall";
            ball.transform.position = spawnPosition.position;
            var rb = ball.GetComponent<Rigidbody>();
            rb.AddForce(spawnPosition.transform.forward * 1000, ForceMode.Acceleration);
            money -= 100;
            buttonPrice.text = "$200";
        }
    }
}
