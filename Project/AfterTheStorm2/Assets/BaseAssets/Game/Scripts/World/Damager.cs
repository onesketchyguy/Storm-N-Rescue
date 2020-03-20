using UnityEngine;
using LowEngine.Helpers;

namespace Hostile
{
    public class Damager : MonoBehaviour
    {
        /// <summary>
        /// Amount of damage to deal on contact with the user.
        /// </summary>
        [Tooltip("Amount of damage to deal on contact with the user.")]
        public int damage = 10;

        private void OnTriggerStay2D(Collider2D collision)
        {
            var damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.Hurt(damage);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            var damagable = collision.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.Hurt(damage);
            }
        }
    }
}