using UnityEngine;

namespace LowEngine.CameraBehaviour
{
    public class CameraFollow : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 0, -10);

        public float moveSpeed = 10;

        public Transform Following;

        private void UpdateFollower()
        {
            var go = GameObject.FindGameObjectWithTag("Player");

            if (go != null)
            {
                Following = go.transform;
                desiredPosition = Following.transform.position + offset;
            }
        }

        private Vector3 desiredPosition;

        private Vector3 lastPosition;

        private void Update()
        {
            if (Following == null)
            {
                UpdateFollower();
            }
            else
            {
                if (Following.GetComponent<Rigidbody2D>().velocity.y > .5f) return; // User is jumping, do not follow

                desiredPosition = new Vector3(Following.transform.position.x + offset.x, Following.transform.position.y + offset.y, offset.z);

                if (desiredPosition.y > lastPosition.y || desiredPosition != lastPosition)
                {
                    var newPos = new Vector3(desiredPosition.x, lastPosition.y, desiredPosition.z);

                    transform.position = Vector3.MoveTowards(transform.position, newPos, moveSpeed * Time.deltaTime);

                    if (desiredPosition.y > lastPosition.y)
                        lastPosition = desiredPosition;
                    else lastPosition = newPos;
                }
            }
        }
    }
}