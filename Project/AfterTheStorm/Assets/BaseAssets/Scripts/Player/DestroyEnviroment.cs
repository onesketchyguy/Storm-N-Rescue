using UnityEngine;

namespace LowEngine
{
    using Helpers;
    using UnityEngine.Tilemaps;

    public class DestroyEnviroment : MonoBehaviour, ICombat
    {
        public Vector4 input { get; set; }
        public bool attacking { get; set; }

        private void Update()
        {
            if (input.x != 0)  // Attacking
            {
                var dir = GetComponent<SpriteRenderer>().flipX ? Vector3.left : Vector3.right;

                var dist = 0.5f;

                RaycastHit2D RayCastInfo = Physics2D.Raycast(transform.position + dir, dir, dist);

                var pos = new Vector3Int((int)(RayCastInfo.point.x - dir.x), (int)RayCastInfo.point.y, 0);

                if (RayCastInfo.transform != null)
                {
                    Debug.Log("Hit" + RayCastInfo.transform.name);

                    var hit = RayCastInfo.transform.gameObject.GetComponent<Tilemap>();

                    if (hit != null)
                    {
                        hit.SetTile(pos, null);
                    }
                }

                Debug.DrawLine(transform.position, pos, Color.green, 5);
            }
        }
    }
}