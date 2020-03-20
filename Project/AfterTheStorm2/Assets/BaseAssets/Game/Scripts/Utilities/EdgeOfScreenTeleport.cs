using UnityEngine;

namespace LowEngine.Helpers
{
    public class EdgeOfScreenTeleport : MonoBehaviour
    {
        private float padding = 3;

        private float lastTimeMoved;

        public float waitBeforeTeleportingAgain = 0;

        private void Start()
        {
            lastTimeMoved = Time.time + waitBeforeTeleportingAgain;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Time.time > lastTimeMoved)
            {
                if (Utilities.OffScreen(transform.position))
                {
                    Vector3 newPos = transform.position;

                    if (Utilities.OffScreen(new Vector3(transform.position.x, Utilities.ScreenMid.y)) == true)
                    {
                        if (transform.position.x > Utilities.ScreenMax.x)
                        {
                            newPos.x = Utilities.ScreenMin.x - padding; // Off screen right, move left
                        }
                        else
                        {
                            newPos.x = Utilities.ScreenMax.x + padding; // Off screen left, move right
                        }
                    }
                    if (Utilities.OffScreen(new Vector3(Utilities.ScreenMid.x, transform.position.y)) == true)
                    {
                        if (transform.position.y > Utilities.ScreenMax.y)
                        {
                            newPos.y = Utilities.ScreenMin.y - padding; // Off screen up, move down
                        }
                        else
                        {
                            newPos.y = Utilities.ScreenMax.y + padding; // Off screen down, move up
                        }
                    }

                    transform.position = newPos; // Set the position to the new position.

                    lastTimeMoved = Time.time + waitBeforeTeleportingAgain + Random.Range(-(waitBeforeTeleportingAgain / 2), (waitBeforeTeleportingAgain / 2));

                    if (GetComponent<Rigidbody2D>())
                    {
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                }
            }
        }
    }
}