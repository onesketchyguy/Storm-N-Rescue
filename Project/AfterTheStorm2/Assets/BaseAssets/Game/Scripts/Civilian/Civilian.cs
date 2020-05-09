using LowEngine;
using LowEngine.Helpers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Friendly
{
    public class Civilian : MonoBehaviour, IDamagable
    {
        private Rigidbody rigidBody;
        private bool tossed;

        public GameObject MurderEffect;
        public GameObject ThrowThroughWindowEffect;

        public MaxableValue Health { get; set; }

        public int scoreToAdd = 15;
        public int scoreToAdd_WindowBreak = 10;
        public int scoreLossOnDeath = 10;

        public float tossedOutWindowVelocity = 5;

        public bool DestroyOnBelowScreen;

        private void OnEnable()
        {
            Health = new MaxableValue(5);
            rigidBody = GetComponent<Rigidbody>();

            CivilianBlackboard.civilians.Add(this);
        }

        private void OnDisable()
        {
            CivilianBlackboard.civilians.Remove(this);
        }

        private void CreateEffect(GameObject effect)
        {
            if (effect != null)
            {
                var eff = ObjectManager.GetObject(effect);
                eff.transform.position = transform.position;
                eff.transform.rotation = effect.transform.rotation;
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var player = collision.gameObject.GetComponent<PlayerManager>();

                if (player.carrying != null) return;

                // Being carried
                player.carrying = this;

                LowEngine.Audio.AudioManager.instance.PlayPickUpCivilianSound(transform.position);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (tossed)
            {
                var item = collision.gameObject;

                if (item.name.Contains("Window"))
                {
                    //Map.SetTile(point, null);

                    var newVel = transform.localScale.x > 0 ? tossedOutWindowVelocity : -tossedOutWindowVelocity;

                    rigidBody.velocity = new Vector2(newVel, 0);

                    if (ThrowThroughWindowEffect != null)
                    {
                        scoreToAdd = scoreToAdd_WindowBreak;

                        CreateEffect(ThrowThroughWindowEffect);
                    }

                    return;
                }

                if (item.name.Contains("Ground"))
                {
                    HitGround();

                    return;
                }

                // Return to resting position
            }
        }

        private void Update()
        {
            if (DestroyOnBelowScreen == false) return;
            if (Time.timeSinceLevelLoad < 5) return;

            if (tossed || rigidBody.velocity != Vector3.zero)
            {
                if (transform.position.y < Utility.Utilities.ScreenMin.y - 1)
                {
                    HitGround();
                }
            }
        }

        public void HitGround()
        {
            if (tossed == false) scoreToAdd = 5;

            Score.ModifyScore(scoreToAdd);

            //Play sound
            LowEngine.Audio.AudioManager.instance.PlayPickupCoinSound(transform.position);
            ObjectManager.ReturnObject(gameObject);
        }

        private void Murdered()
        {
            Score.ModifyScore(-scoreLossOnDeath);

            Died();
        }

        private void Died()
        {
            if (MurderEffect != null)
            {
                CreateEffect(MurderEffect);
            }

            ObjectManager.ReturnObject(gameObject);
        }

        public void Toss(Vector2 force)
        {
            rigidBody.AddForce(force);

            tossed = true;
        }

        public void Hurt(float damageToDeal)
        {
            Health.ModifyValue(-damageToDeal);

            if (Health.empty)
            {
                if (tossed) Murdered();
                else Died();
            }
        }
    }
}