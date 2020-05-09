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

        private PlayerManager player
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

        internal List<Vector3> doors = new List<Vector3>();

        private void GetPlayerDistanceToDoor()
        {
            if (lastMovement + 0.5f > Time.time) return;

            void MovePlayer(int doorIndex)
            {
                var col = player.GetComponent<Collider>();
                col.enabled = false;
                var rigidbody = player.GetComponent<Rigidbody>();
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                // Take player to next door up
                var doorPos = doors[doorIndex];
                var newPos = new Vector3(doorPos.x, doorPos.y, player.transform.position.z);
                player.transform.position = newPos;
                LowEngine.Audio.AudioManager.instance.PlayClimbSound(Camera.main.transform.position);

                lastMovement = Time.time;

                col.enabled = true;
            }

            for (int i = 0; i < doors.Count - 1; i++)
            {
                if (lastMovement + 0.5f > Time.time) break;

                var door = doors[i];

                if (door == null) continue;

                if (Vector2.Distance(door, player.transform.position) <= range)
                {
                    Debug.Log("Player in range...");

                    if (player.GetComponent<IMovement>().input.z > 0) // pressed up
                    {
                        MovePlayer(i + 1);
                    }
                    else if (player.GetComponent<IMovement>().input.z < 0) // pressed down
                    {
                        if (i > 0)
                            MovePlayer(i - 1);
                    }
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