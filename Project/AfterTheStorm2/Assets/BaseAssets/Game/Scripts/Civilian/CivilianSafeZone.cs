using LowEngine;
using UnityEngine;

namespace Friendly
{
    public class CivilianSafeZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                // Get a reference to our objects
                var player = other.GetComponent<PlayerManager>();
                if (player.carrying != null)
                {
                    // Remove the civilian from the player
                    player.ThrowCivilian();
                }
            }

            if (other.CompareTag("Civilian"))
            {
                // Remove the civilian from the scene, and add score
                var civ = other.GetComponent<Civilian>();
                civ.HitGround();
            }
        }
    }
}