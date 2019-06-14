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
                lastY = go.transform.position.y - 10;
            }
        }

        private float lastY;
        private Vector3 targetPos;

        private void Update()
        {
            if (Following == null)
            {
                UpdateFollower();
            }
            else
            {
                if (Following.GetComponent<Rigidbody2D>().velocity.y > .5f) return;

                var desiredPosition = new Vector3(Following.transform.position.x + offset.x, Following.transform.position.y + offset.y, offset.z);

                if (desiredPosition.y > lastY)
                {
                    targetPos = desiredPosition;
                }

                if (Time.timeSinceLevelLoad > 4)
                {
                    if (Following.transform.position.x > Utilities.ScreenMax.x - 2 || Following.transform.position.x < Utilities.ScreenMin.x + 2)
                    {
                        float dist = Camera.main.orthographicSize + (Camera.main.orthographicSize / 2f);

                        targetPos = desiredPosition + ((Following.transform.position.x > Utilities.ScreenMax.x - 1) ? new Vector3(-dist, 0) : new Vector3(dist, 0));
                    }
                }

                if (transform.position != desiredPosition)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                    lastY = transform.position.y;
                }
            }
        }
    }
}