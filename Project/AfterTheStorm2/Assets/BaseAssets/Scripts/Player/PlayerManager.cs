using UnityEngine;

namespace LowEngine
{
    using Friendly;
    using Helpers;

    public class PlayerManager : MonoBehaviour, IDamagable
    {
        public MaxableValue Health { get; set; }

        public static Civilian carrying;

        public AudioClip hurt;
        private float lastHurtSound;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            Health = new MaxableValue(30);

            var movement = GetComponent<IMovement>();

            if (movement != null)
                movement.fallDamageCallback += Hurt;
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

            var tossForce = (Vector2.right * faceDir * 300 + Vector2.up * 100);
            carrying.Toss(rigidbody.velocity + tossForce); // Ensure that when we toss our object, we sustain it's velocity

            carrying.GetComponent<Collider2D>().enabled = true;

            carrying = null; // Thrown

            LowEngine.Audio.AudioManager.instance.PlayThrowSound(transform.position);
        }

        public void Hurt(float damageToDeal)
        {
            if (lastHurtSound > Time.time) return;

            Audio.AudioManager.PlayClip(hurt, transform.position);

            lastHurtSound = Time.time + (hurt.length + 0.01f);

            Health.ModifyValue(-damageToDeal);

            if (Health.empty) ThrowCivilian();
        }
    }
}