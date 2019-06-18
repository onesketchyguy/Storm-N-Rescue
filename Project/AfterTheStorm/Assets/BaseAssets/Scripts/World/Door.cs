using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour
    {
        public Transform ClosestDoorToMe { get; set; }

        public float range { get; set; } = 0.5f;

        public void Move(Transform objectToMove)
        {
            if (ClosestDoorToMe != transform)
            {
                objectToMove.position = ClosestDoorToMe.transform.position + Vector3.up * 0.5f;
            }
            else
            {
                ClosestDoorToMe = null;

                Debug.LogError("Unable to move player!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            var dm = FindObjectOfType<DoorManager>();

            if (dm != null) range = dm.range;

            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}