using UnityEngine;

namespace _Scripts
{
    public class GateScript : MonoBehaviour,IGate
    {
        [SerializeField] private GateMode gateMode;
        [SerializeField] private int value;
   

        private void SpawnBalls()
        {
            if (gateMode == GateMode.Addition) 
            {
                for (var i = 0; i < value; i++)
                {
                    var newBall = Instantiate(Resources.Load<GameObject>("Ball"));
                    var ballScript = newBall.GetComponent<BallController>();
                    ballScript.BlockMultiply();
                    newBall.transform.position = transform.position;
                }
            }
            else
            {
                for (var i = 0; i < value; i++)
                {
                    var newBall = Instantiate(Resources.Load<GameObject>("Ball"));
                    var ballScript = newBall.GetComponent<BallController>();
                    ballScript.BlockMultiply();
                    newBall.transform.position = transform.position;
                }
            }
        }

        public void Execute()
        {
            SpawnBalls();
        }
    }
}
