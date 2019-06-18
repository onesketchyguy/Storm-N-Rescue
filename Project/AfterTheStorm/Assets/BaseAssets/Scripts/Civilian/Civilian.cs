using LowEngine;
using LowEngine.Helpers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Friendly
{
    public class Civilian : MonoBehaviour, IDamagable
    {
        public CivilianSprites[] possibleSprites;

        private CivilianSprites m_sprites;

        private bool tossed;

        public GameObject MurderEffect;
        public GameObject ThrowThroughWindowEffect;

        public AudioClip[] killedSounds;

        private Sprite sprite
        {
            get
            {
                return GetComponent<SpriteRenderer>().sprite;
            }
            set
            {
                GetComponent<SpriteRenderer>().sprite = value;
            }
        }

        public MaxableValue Health { get; set; }

        public int scoreToAdd = 15;
        public int scoreToAdd_WindowBreak = 10;
        public int scoreLossOnDeath = 10;

        private void Awake()
        {
            Health = new MaxableValue(5);

            // Load in a sprite
            m_sprites = possibleSprites[Random.Range(0, possibleSprites.Length)];
            sprite = m_sprites.restingSprite;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (PlayerManager.carrying != null) return;

                // Being carried
                sprite = m_sprites.pickedUpSprite;
                PlayerManager.carrying = this;

                LowEngine.Audio.AudioManager.instance.PlayPickUpCivilianSound(transform.position);
            }
            else
            if (tossed)
            {
                var Map = collision.gameObject.GetComponent<Tilemap>();

                if (Map)
                {
                    var rb = GetComponent<Rigidbody2D>();

                    var xVel = rb.velocity.x > 0 ? 0 : -2;

                    var velocity_pos = new Vector3Int((int)(transform.position.x + xVel), (int)(transform.position.y), 0);
                    var pos = new Vector3Int((int)transform.position.x, (int)transform.position.y - 1, 0);

                    if (Map.GetTile(velocity_pos) != null && Map.GetTile(velocity_pos).name.Contains("Window"))
                    {
                        Map.SetTile(velocity_pos, null);

                        rb.velocity = new Vector2(xVel * 5, 0);

                        if (ThrowThroughWindowEffect != null)
                        {
                            scoreToAdd = scoreToAdd_WindowBreak;

                            Instantiate(ThrowThroughWindowEffect, transform.position, ThrowThroughWindowEffect.transform.rotation);
                        }

                        return;
                    }

                    if (Map.GetTile(pos) != null && Map.GetTile(pos).name.Contains("Ground"))
                    {
                        HitGround();

                        return;
                    }
                }

                sprite = m_sprites.restingSprite;
            }
        }

        private void Update()
        {
            if (tossed)
            {
                if (transform.position.y < Utilities.ScreenMin.y - 1)
                {
                    HitGround();
                }
            }
        }

        private void HitGround()
        {
            Score.ModifyScore(scoreToAdd);

            //Play sound
            LowEngine.Audio.AudioManager.instance.PlayPickupCoinSound(transform.position);
            Destroy(gameObject);
        }

        private void Murdered()
        {
            Score.ModifyScore(-scoreLossOnDeath);

            LowEngine.Audio.AudioManager.PlayClip(killedSounds[0], transform.position);

            Died();
        }

        private void Died()
        {
            if (MurderEffect != null)
            {
                Instantiate(MurderEffect, transform.position, Quaternion.identity);
            }

            LowEngine.Audio.AudioManager.PlayClip(killedSounds[1], transform.position);

            Destroy(gameObject);
        }

        public void Toss(Vector2 force)
        {
            GetComponent<Rigidbody2D>().AddForce(force);

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