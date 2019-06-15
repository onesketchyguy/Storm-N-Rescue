using UnityEngine;

namespace LowEngine
{
    using Helpers;
    using UnityEngine.Tilemaps;

    public class DestroyEnviroment : MonoBehaviour, ICombat
    {
        [SerializeField] private float DigRange = 0.5f;
        public LayerMask Diggable;

        public GameObject Effect;

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

            var dir = Vector3.right * faceDirection;

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

                if (tilemap.GetTile(position) != null)
                {
                    tilemap.SetTile(position, null);

                    if (Effect != null)
                        Instantiate(Effect, position, Quaternion.identity);
                }
                else
                {
                    Debug.Log("No Tile hit");
                }
            }

            attacking = false;
        }
    }
}