using UnityEngine;

namespace World
{
    public class Door : MonoBehaviour
    {
        public Transform _ClosestDoorToMe;

        public Transform ClosestDoorToMe
        {
            get { return _ClosestDoorToMe; }
            set
            {
                if (value != transform)
                {
                    _ClosestDoorToMe = value;
                }
            }
        }

        public float range { get; set; } = 0.5f;

        public void Move(Transform objectToMove)
        {
            if (ClosestDoorToMe != null)
            {
                objectToMove.position = ClosestDoorToMe.transform.position + Vector3.up * 0.5f;

                LowEngine.Audio.AudioManager.instance.PlayClimbSound(transform.position);
            }
            else
            {
                ClosestDoorToMe = FindObjectOfType<DoorManager>().GetClosestDoor(transform);

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