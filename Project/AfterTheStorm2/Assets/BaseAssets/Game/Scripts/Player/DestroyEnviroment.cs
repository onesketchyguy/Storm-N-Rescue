using UnityEngine;

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

        public Transform CheckForWall()
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
                    return null;
                }

                Debug.DrawRay(start, direction * hitInfo.distance, Color.red, 1f);

                if (hit.tag == "Destructable")
                {
                    return hit;
                }
            }
            else
            {
                Debug.DrawRay(start, direction * DigRange, Color.white, 1f);
            }

            attacking = false;

            return null;
        }

        public void Attack()
        {
            Transform hit = CheckForWall();

            if (hit != null)
            {
                DestroyTile(hit);
            }
        }

        private void DestroyTile(Transform hit)
        {
            if (hit != null)
            {
                if (FireEffect != null && hit.name.ToLower().Contains("fire"))
                    CreateEffect(FireEffect, hit.position);
                else
                if (GlassEffect != null && hit.name.ToLower().Contains("window"))
                    CreateEffect(GlassEffect, hit.position);
                else
                if (WallEffect != null)
                    CreateEffect(WallEffect, hit.position);

                ObjectManager.ReturnObject(hit.gameObject);
            }
        }

        private void CreateEffect(GameObject effect, Vector3 point)
        {
            var go = ObjectManager.GetObject(effect);
            go.transform.position = point;
            go.transform.rotation = Quaternion.identity;
        }
    }
}