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

        public new SpriteRenderer renderer;

        private bool tossed;
        private bool fallingWithOther;
        private bool naturalDeath;

        public GameObject MurderEffect;
        public GameObject ThrowThroughWindowEffect;

        private Sprite sprite
        {
            get
            {
                return renderer.sprite;
            }
            set
            {
                renderer.sprite = value;
            }
        }

        public MaxableValue Health { get; set; }

        public int scoreToAdd = 15;
        public int scoreToAdd_WindowBreak = 10;
        public int scoreLossOnDeath = 10;

        public float tossedOutWindowVelocity = 5;

        private void Awake()
        {
            Health = new MaxableValue(5);

            if (renderer == null) renderer = GetComponent<SpriteRenderer>();

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
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "PickUps" || collision.gameObject.tag == "Player")
            {
                fallingWithOther = true;
            }

            if (tossed)
            {
                var Map = collision.gameObject.GetComponent<Tilemap>();

                if (Map)
                {
                    var rb = GetComponent<Rigidbody2D>();

                    if (rb.velocity.y > -1f)
                    {
                        var velocity_pos = new Vector3Int(Mathf.FloorToInt(transform.position.x - 2), Mathf.FloorToInt(transform.position.y), 0);
                        BoundsInt blockPos = new BoundsInt(velocity_pos, new Vector3Int(5, 2, 1));

                        foreach (var point in blockPos.allPositionsWithin)
                        {
                            var item = Map.GetTile(point);

                            if (item != null && item.name.Contains("Window"))
                            {
                                Map.SetTile(point, null);

                                var newVel = renderer.flipX ? tossedOutWindowVelocity : -tossedOutWindowVelocity;

                                rb.velocity = new Vector2(newVel, 0);

                                if (ThrowThroughWindowEffect != null)
                                {
                                    scoreToAdd = scoreToAdd_WindowBreak;

                                    Instantiate(ThrowThroughWindowEffect, transform.position, ThrowThroughWindowEffect.transform.rotation);
                                }

                                return;
                            }
                        }

                        var myPos = new Vector3Int((int)transform.position.x, (int)transform.position.y - 1, 0);

                        if (Map.GetTile(myPos) != null && Map.GetTile(myPos).name.Contains("Ground"))
                        {
                            HitGround();

                            return;
                        }
                    }
                }

                sprite = m_sprites.restingSprite;
            }
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad < 5) return;

            if (tossed || GetComponent<Rigidbody2D>().velocity != Vector2.zero)
            {
                if (transform.position.y < Utilities.ScreenMin.y - 1)
                {
                    HitGround();
                }
            }
        }

        private void OnDestroy()
        {
            if (!naturalDeath && GameManager.instance.playerDead == false)
                HitGround();
        }

        private void HitGround()
        {
            if (fallingWithOther == true) scoreToAdd = 5;
            else if (tossed == false)
            {
                Murdered("Civilian left behind!");

                return;
            }

            Score.ModifyScore(scoreToAdd, "Civilian rescued!");

            //Play sound
            LowEngine.Audio.AudioManager.instance.PlayPickupCoinSound(transform.position);
            Destroy(gameObject);
        }

        private void Murdered(string murderReason)
        {
            Score.ModifyScore(-scoreLossOnDeath, murderReason);

            Died();
        }

        private void Died()
        {
            if (MurderEffect != null)
            {
                Instantiate(MurderEffect, transform.position, Quaternion.identity);
            }

            naturalDeath = true;

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
                if (tossed) Murdered("Civilian killed!");
                else Died();
            }
        }
    }
}