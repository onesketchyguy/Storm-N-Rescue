using UnityEngine;
using UnityEngine.Tilemaps;

namespace LowEngine
{
    using Helpers;

    public class DestroyEnviroment : MonoBehaviour, ICombat
    {
        [SerializeField] private float DigRange = 0.5f;
        public LayerMask Diggable;

        public GameObject WallEffect;
        public GameObject FireEffect;

        public Vector4 input { get; set; }
        public bool attacking { get; set; }

        private float lastAttack;

        private int faceDirection
        {
            get
            {
                return GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            }
        }

        private void LateUpdate()
        {
            if (lastAttack + 0.1f < Time.time)
            {
                attacking = false;
            }
        }

        public void Attack()
        {
            lastAttack = Time.time;

            Vector3 targetPosition = transform.right * faceDirection;
            RaycastHit2D hitInfo;

            if ((hitInfo = Physics2D.Raycast(transform.position, targetPosition, DigRange, Diggable)) == true)
            {
                var tilemap = hitInfo.transform.gameObject.GetComponent<Tilemap>();

                Debug.DrawRay(transform.position, targetPosition * DigRange, Color.red, 2f);

                if (tilemap == null)
                {
                    attacking = false;

                    Debug.Log("No tilemap hit");

                    return;
                }

                Vector3Int position = new Vector3Int(
                (int)hitInfo.point.x + (faceDirection > 0 ? 0 : -1),
                Mathf.FloorToInt(hitInfo.point.y),
                0);

                DestroyTile(position, tilemap);
            }

            var HazardMap = GameObject.Find("HazardMap").GetComponent<Tilemap>();

            for (int y = (int)transform.position.y - 1; y < (int)transform.position.y + 1; y++)
            {
                for (int x = (int)transform.position.x - 2; x < (int)transform.position.x + 1; x++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    if (position.x > transform.position.x && faceDirection == 1 || position.x < transform.position.x && faceDirection == -1) DestroyTile(position, HazardMap);
                }
            }

            attacking = false;
        }

        private void DestroyTile(Vector3Int position, Tilemap tilemap)
        {
            if (tilemap.GetTile(position) != null)
            {
                if (tilemap.GetTile(position).name.ToLower().Contains("fire"))
                {
                    if (FireEffect != null)
                        Instantiate(FireEffect, position, Quaternion.identity);
                }
                else
                if (WallEffect != null)
                    Instantiate(WallEffect, position, Quaternion.identity);

                tilemap.SetTile(position, null);
            }
        }
    }
}