using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public Vector3 offset = new Vector3(0, 0, -10);

        public float moveSpeed = 10;

        public Transform Following;

        private void UpdateFollower()
        {
            var go = GameObject.FindGameObjectWithTag("Player");

            if (go != null) Following = go.transform;
        }

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

                transform.position = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            }
        }
    }
}