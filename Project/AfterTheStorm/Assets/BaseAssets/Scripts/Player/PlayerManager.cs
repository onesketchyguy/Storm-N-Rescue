using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEngine
{
    using Helpers;

    public class PlayerManager : MonoBehaviour, IDamagable
    {
        public MaxableValue Health { get; set; }

        private void Awake()
        {
            Health = new MaxableValue(30);

            var movement = GetComponent<IMovement>();

            if (movement != null)
            {
                movement.fallDamageCallback += Hurt;
            }
        }

        public void Hurt(float damageToDeal)
        {
            Health.ModifyValue(-damageToDeal);

            // Play an animation
        }
    }
}