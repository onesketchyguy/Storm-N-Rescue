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
                targetPos = Following.transform.position + offset;
            }
        }

        private Vector3 targetPos;

        private void Update()
        {
            if (Following == null)
            {
                UpdateFollower();
            }
            else
            {
                if (Following.GetComponent<Rigidbody2D>().velocity.y > .5f) return; // User is jumping, do not follow

                var padding = Camera.main.orthographicSize - (Camera.main.orthographicSize / 2f);
                var moveDist = padding - 1;
                var desiredPosition = new Vector3(Following.transform.position.x + offset.x, Following.transform.position.y + offset.y, offset.z);

                Debug.DrawLine(new Vector3(Utilities.ScreenMin.x + padding, Utilities.ScreenMin.y), new Vector3(Utilities.ScreenMax.x - padding, Utilities.ScreenMax.y));

                Debug.DrawLine(Following.transform.position, targetPos);

                if (Time.timeSinceLevelLoad > 4)
                {
                    if (Following.transform.position.x > Utilities.ScreenMax.x - padding || Following.transform.position.x < Utilities.ScreenMin.x + padding)
                    {
                        targetPos = desiredPosition + (Vector3.right * (Following.transform.position.x > Utilities.ScreenMax.x - padding ? moveDist : -moveDist));
                    }

                    if (Following.transform.position.y > Utilities.ScreenMax.y - padding / 2f || Following.transform.position.y < Utilities.ScreenMin.y + padding / 2f)
                    {
                        targetPos = desiredPosition + (Vector3.up * (Following.transform.position.y > Utilities.ScreenMax.y - padding / 2f ? moveDist / 2f : -moveDist / 2f));
                    }
                }

                if (transform.position != targetPos)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                }
            }
        }
    }
}