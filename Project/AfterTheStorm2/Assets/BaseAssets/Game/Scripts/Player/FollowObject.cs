using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class FollowObject : MonoBehaviour
    {
        public Transform following;

        public float speed = 1;

        public MoveType moveType;

        public enum MoveType
        {
            MoveTowards,
            Lerp,
            Slerp
        }

        // Update is called once per frame
        private void Update()
        {
            if (following == null)
            {
                Destroy(gameObject);
                return;
            }

            switch (moveType)
            {
                case MoveType.MoveTowards:
                    transform.position = Vector3.MoveTowards(transform.position, following.position, speed * Time.deltaTime);
                    break;

                case MoveType.Lerp:
                    transform.position = Vector3.Lerp(transform.position, following.position, speed * Time.deltaTime);
                    break;

                case MoveType.Slerp:
                    transform.position = Vector3.Slerp(transform.position, following.position, speed * Time.deltaTime);
                    break;

                default:
                    break;
            }
        }
    }
}