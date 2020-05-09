using UnityEngine;

namespace LowEngine
{
    using Friendly;
    using Helpers;

    public class PlayerManager : MonoBehaviour, IDamagable
    {
        public MaxableValue Health { get; set; }

        public Civilian carrying;

        public AudioClip hurt;
        private float lastHurtSound;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

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
                    carrying.GetComponent<Rigidbody>().velocity = Vector2.zero;
                    carrying.GetComponent<Rigidbody>().angularVelocity = Vector2.zero;

                    //carrying.GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;

                    carrying.GetComponent<Collider>().enabled = false;

                    carrying.transform.position = transform.position;
                }
            }
        }

        public void ThrowCivilian()
        {
            if (carrying == null) return;

            var faceDir = transform.localScale.x; //(GetComponent<SpriteRenderer>().flipX ? -1 : 1);

            rigidbody.AddForce(Vector3.right * -faceDir);

            carrying.transform.position = transform.position + Vector3.right * (faceDir / 2f);

            var tossForce = (Vector3.right * faceDir * 300 + Vector3.up * 100);
            carrying.Toss(rigidbody.velocity + tossForce); // Ensure that when we toss our object, we sustain it's velocity

            carrying.GetComponent<Collider>().enabled = true;

            carrying = null; // Thrown

            LowEngine.Audio.AudioManager.instance.PlayThrowSound(transform.position);
        }

        public void Hurt(float damageToDeal)
        {
            if (hurt == null) return;

            if (lastHurtSound > Time.time) return;

            Audio.AudioManager.PlayClip(hurt, transform.position);

            lastHurtSound = Time.time + (hurt.length + 0.01f);

            Health.ModifyValue(-damageToDeal);

            if (Health.empty) ThrowCivilian();
        }
    }
}