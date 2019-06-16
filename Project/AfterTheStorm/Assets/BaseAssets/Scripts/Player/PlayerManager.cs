using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEngine
{
    using Friendly;
    using Helpers;

    public class PlayerManager : MonoBehaviour, IDamagable
    {
        public MaxableValue Health { get; set; }

        public static Civilian carrying;

        private void Awake()
        {
            Health = new MaxableValue(30);

            var movement = GetComponent<IMovement>();

            if (movement != null)
            {
                movement.fallDamageCallback += Hurt;
            }
        }

        private void Update()
        {
            if (carrying != null)
            {
                if (GetComponent<DestroyEnviroment>().input.y > 0)
                {
                    ThrowCivilian();
                }
                else
                {
                    carrying.transform.position = transform.position;

                    carrying.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    carrying.GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        private void ThrowCivilian()
        {
            var faceDir = (GetComponent<SpriteRenderer>().flipX ? -1 : 1);

            carrying.transform.position = transform.position + Vector3.right * faceDir;

            carrying.Toss(Vector2.right * faceDir * 100 + Vector2.up * 10);

            carrying.GetComponent<Collider2D>().enabled = true;

            carrying = null; // Thrown
        }

        public void Hurt(float damageToDeal)
        {
            Health.ModifyValue(-damageToDeal);

            // Play an animation
        }
    }
}