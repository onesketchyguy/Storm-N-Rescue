using UnityEngine;
using UnityEngine.Tilemaps;

namespace LowEngine
{
    using Helpers;

    public class DestroyEnviroment : MonoBehaviour, ICombat
    {
        [SerializeField] private float DigRange = 0.5f;
        public LayerMask Diggable;

        public float rayStartOffset = 0.5f;

        public GameObject WallEffect;
        public GameObject FireEffect;
        public GameObject GlassEffect;

        public Vector4 input { get; set; }
        public bool attacking { get; set; }

        private float lastAttack;

        private int faceDirection
        {
            get
            {
                return transform.localScale.x > 0 ? 1 : -1;
            }
        }

        private void LateUpdate()
        {
            if (lastAttack + 0.1f < Time.time)
            {
                attacking = false;
            }

            if (input.x != 0) Attack();
        }

        public void Attack()
        {
            lastAttack = Time.time;

            var direction = transform.right * faceDirection;
            var start = transform.position + (direction * rayStartOffset);
            RaycastHit hitInfo;
            Physics.Raycast(start, direction, out hitInfo, DigRange, Diggable, QueryTriggerInteraction.UseGlobal);

            if (hitInfo.transform != null)
            {
                var hit = hitInfo.transform;

                if (hit == null)
                {
                    attacking = false;
                    return;
                }

                Debug.DrawRay(start, direction * hitInfo.distance, Color.red, 1f);

                if (hit.tag == "Destructable")
                    DestroyTile(hit);
            }
            else
            {
                Debug.DrawRay(start, direction * DigRange, Color.white, 1f);
            }

            attacking = false;
        }

        private void DestroyTile(Transform hit)
        {
            if (hit != null)
            {
                if (FireEffect != null && hit.name.ToLower().Contains("fire"))
                    Instantiate(FireEffect, hit.position, Quaternion.identity);
                else
                if (GlassEffect != null && hit.name.ToLower().Contains("window"))
                    Instantiate(GlassEffect, hit.position, Quaternion.identity);
                else
                if (WallEffect != null)
                    Instantiate(WallEffect, hit.position, Quaternion.identity);

                Destroy(hit.gameObject);
            }
        }
    }
}