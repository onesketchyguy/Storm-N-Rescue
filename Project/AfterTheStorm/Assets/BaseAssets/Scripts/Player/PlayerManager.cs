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

        private float lasthit;

        public AudioClip hurt;
        private float lastHurtSound;

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
                if (GetComponent<ICombat>().input.y > 0)
                {
                    ThrowCivilian();
                }
                else
                {
                    carrying.transform.position = transform.position;

                    carrying.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    carrying.GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;

                    carrying.GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        private void ThrowCivilian()
        {
            if (carrying == null) return;

            var faceDir = (GetComponent<SpriteRenderer>().flipX ? -1 : 1);

            carrying.transform.position = transform.position + Vector3.right * (faceDir / 2f);

            carrying.Toss(Vector2.right * faceDir * 300 + Vector2.up * 100);

            carrying.GetComponent<Collider2D>().enabled = true;

            carrying = null; // Thrown
        }

        public void Hurt(float damageToDeal)
        {
            if (lasthit > Time.time) return;

            Health.ModifyValue(-damageToDeal);
            lasthit = Time.time + (Time.deltaTime * 5); // 5 frame wait between damages

            if (Health.empty) ThrowCivilian();

            if (lastHurtSound > Time.time) return;

            Audio.AudioManager.PlayClip(hurt, transform.position);

            lastHurtSound = Time.time + (hurt.length * 2);
        }
    }
}