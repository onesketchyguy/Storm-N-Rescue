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

        private float lastMovement;

        private Transform SetClosestDoor(Transform measureFrom)
        {
            var closest = measureFrom;

            foreach (var item in FindObjectsOfType<Door>())
            {
                var distToClosest = closest == measureFrom ? 100 : Vector2.Distance(transform.position, closest.position);
                var distToItem = Vector2.Distance(transform.position, item.transform.position);

                if (distToClosest > distToItem && item.transform.position.y > measureFrom.position.y) // Ensure they are above us
                {
                    closest = item.transform;
                }
            }

            return closest;
        }

        private List<Door> doors = new List<Door>();

        private void UpdateDoors()
        {
            RemoveHiddenDoors();

            if (lastMovement + 0.5f > Time.time) return;

            for (int i = 0; i < doors.Count; i++)
            {
                Door door = doors[i];

                if (door == null) continue;

                if (door.ClosestDoorToMe == null) door.ClosestDoorToMe = SetClosestDoor(door.transform);
                if (Vector2.Distance(door.transform.position, Player.transform.position) <= range)
                {
                    if (Player.GetComponent<IMovement>().input.z < 0) // pressed down
                    {
                        // Take player to next door up
                        door.Move(Player.transform);

                        lastMovement = Time.time;
                    }
                }
            }
        }

        private void UpdateDoorCount()
        {
            doors.Clear();

            var _Doors = FindObjectsOfType<Door>();

            foreach (var item in _Doors)
            {
                if (!doors.Contains(item))
                    doors.Add(item);
            }
        }

        private void RemoveHiddenDoors()
        {
            var toDestroy = new List<GameObject>();

            foreach (var item in doors)
            {
                if (item == null) continue;

                if (item.transform.position.y < Utilities.ScreenMin.y - 2)
                {
                    toDestroy.Add(item.gameObject);
                }
            }

            foreach (var item in toDestroy)
            {
                Destroy(item);
            }

            if (toDestroy.Count > 0) UpdateDoorCount();
        }

        private void Awake()
        {
            Invoke("UpdateDoorCount", 1);
        }

        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad < 4) return;

            UpdateDoors();
        }
    }
}