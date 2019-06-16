using LowEngine;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Friendly
{
    public class Civilian : MonoBehaviour
    {
        public CivilianSprites[] possibleSprites;

        private CivilianSprites m_sprites;

        private bool tossed;

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

        public int scoreToAdd = 15;

        private void Awake()
        {
            // Load in a sprite
            m_sprites = possibleSprites[Random.Range(0, possibleSprites.Length)];
            sprite = m_sprites.restingSprite;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (PlayerManager.carrying != null) return;

                // Being carried
                sprite = m_sprites.pickedUpSprite;
                PlayerManager.carrying = this;
            }
            else
            if (tossed)
            {
                var Map = collision.gameObject.GetComponent<Tilemap>();

                if (Map)
                {
                    var pos = new Vector3Int((int)transform.position.x, (int)transform.position.y - 1, 0);

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
                if (transform.position.y < Utilities.ScreenMin.y - 2)
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

        public void Toss(Vector2 force)
        {
            GetComponent<Rigidbody2D>().AddForce(force);

            tossed = true;
        }
    }
}