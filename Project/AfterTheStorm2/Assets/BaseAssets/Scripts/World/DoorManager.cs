using LowEngine;
using LowEngine.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class DoorManager : MonoBehaviour
    {
        private PlayerManager _player;

        private PlayerManager Player
        {
            get
            {
                if (_player == null) _player = FindObjectOfType<PlayerManager>();

                return _player;
            }
        }

        public float range = 0.5f;

        [Range(5, 100)]
        public float doorCheckRange = 10;

        private float lastMovement;

        public Vector3 GetClosestDoor(Vector3 measureFrom)
        {
            var closest = measureFrom;

            foreach (var item in doors)
            {
                var distToClosest = closest == measureFrom ? doorCheckRange : Vector2.Distance(transform.position, closest);
                var distToItem = Vector2.Distance(transform.position, item);

                if (distToClosest > distToItem && item.y > measureFrom.y) // Ensure they are above us
                {
                    closest = item;
                }
            }

            return closest;
        }

        internal List<Vector3> doors = new List<Vector3>();

        private void GetPlayerDistanceToDoor()
        {
            RemoveHiddenDoors();

            if (lastMovement + 0.5f > Time.time) return;

            for (int i = 0; i < doors.Count - 1; i++)
            {
                if (lastMovement + 0.5f > Time.time) break;

                var door = doors[i];

                if (door == null) continue;

                if (Vector2.Distance(door, Player.transform.position) <= range)
                {
                    if (Player.GetComponent<IMovement>().input.z < 0) // pressed down
                    {
                        // Take player to next door up
                        var floorOffset = Vector3.up * 0.5f;
                        Player.transform.position = doors[i + 1] + floorOffset;
                        LowEngine.Audio.AudioManager.instance.PlayClimbSound(Camera.main.transform.position);

                        lastMovement = Time.time;
                    }
                }
            }
        }

        private void RemoveHiddenDoors()
        {
            for (int i = doors.Count - 1; i >= 0; i--)
            {
                var item = doors[i];
                if (item == null)
                {
                    doors.RemoveAt(i);

                    continue;
                }

                if (item.y < Utility.Utilities.ScreenMin.y - 2)
                {
                    // Remove the doors that are below the scrren
                    doors.RemoveAt(i);
                }
            }
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < 4) return;

            GetPlayerDistanceToDoor();
        }
    }
}