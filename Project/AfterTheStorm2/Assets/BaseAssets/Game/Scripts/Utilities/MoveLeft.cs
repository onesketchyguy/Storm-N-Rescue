using UnityEngine;

namespace LowEngine.Navigation
{
    public class MoveLeft : MonoBehaviour
    {
        [Range(0.01f, 20)] public float speed = 1;

        private void Start()
        {
            speed = Random.Range(speed + -(speed / 2), speed + (speed / 2));
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.left, speed * Time.deltaTime);
        }
    }
}