using LowEngine.Audio;
using LowEngine.Helpers;
using UnityEngine;

namespace LowEngine.Animation
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour
    {
        public AudioClip DeathSound;

        public void PlayDeathSound()
        {
            AudioManager.PlayClip(DeathSound, Camera.main.transform.position + Camera.main.transform.forward * 5, AudioManager.GetClipVolume(transform.position));
        }

        /// <summary>
        /// The movement component for this unit.
        /// </summary>
        private IMovement storedMovementComponent;

        /// <summary>
        /// Component of type IMovement.
        /// </summary>
        private IMovement movementComponent
        {
            get //When we try to retrieve this value.
            {
                if (storedMovementComponent != null) //Check if this value has already been set.
                {
                    return storedMovementComponent; //If the component does exist, return it.
                }
                else
                {
                    storedMovementComponent = GetComponent<IMovement>(); // Get the component from the utility system and store it.

                    return storedMovementComponent; // Return the stored component.
                }
            }
        }

        /// <summary>
        /// The combat component for this unit.
        /// </summary>
        private ICombat storedCombatComponent;

        /// <summary>
        /// Component of type ICombat.
        /// </summary>
        private ICombat combatComponent
        {
            get //When we try to retrieve this value.
            {
                if (storedCombatComponent != null) //Check if this value has already been set.
                {
                    return storedCombatComponent; //If the component does exist, return it.
                }
                else
                {
                    storedCombatComponent = GetComponent<ICombat>();  // Get the component from the utility system and store it.

                    return storedCombatComponent; // Return the stored component.
                }
            }
        }

        /// <summary>
        /// The combat component for this unit.
        /// </summary>
        private IDamagable storedDamagable;

        /// <summary>
        /// Component of type ICombat.
        /// </summary>
        private IDamagable damagableComponent
        {
            get //When we try to retrieve this value.
            {
                if (storedDamagable != null) //Check if this value has already been set.
                {
                    return storedDamagable; //If the component does exist, return it.
                }
                else
                {
                    storedDamagable = GetComponent<IDamagable>(); // Get the component from the utility system and store it.

                    return storedDamagable; // Return the stored component.
                }
            }
        }

        private Animator animator
        {
            get
            {
                return GetComponent<Animator>();
            }
        }

        new private SpriteRenderer renderer
        {
            get
            {
                return GetComponent<SpriteRenderer>();
            }
        }

        private bool facingLeft;

        [HideInInspector] public bool isDead;

        private float lastAttack_1;
        private float lastAttack_2;

        public float waitBetweenAttack_1 = 0.1f;
        public float waitBetweenAttack_2 = 0.25f;

        private void Update()
        {
            //if (damagableComponent == null || damagableComponent.Health == null) return;

            /*
            if (damagableComponent.Health.empty && isDead == false)
            {
                isDead = true;

                animator.SetTrigger("Die");

                return;
            }
            */

            float speedX = 1 - (movementComponent.maxSpeed.x - movementComponent.currentSpeed.x) / movementComponent.maxSpeed.x;
            animator.SetFloat("speed.x", Mathf.Abs(speedX));

            float speedY = 1 - (movementComponent.maxSpeed.y - movementComponent.currentSpeed.y) / movementComponent.maxSpeed.y;
            animator.SetFloat("speed.y", speedY);

            if (speedX > 0.1f)
            {
                facingLeft = false;
            }
            else
            if (speedX < -0.1f)
            {
                facingLeft = true;
            }

            renderer.flipX = facingLeft;

            if (combatComponent != null && combatComponent.attacking == false)
                HandleAttacks();
        }

        private void HandleAttacks()
        {
            if (lastAttack_1 < Time.time)
            {
                combatComponent.attacking = false;

                if (combatComponent.input.x != 0)
                {
                    animator.SetTrigger("Attack1");

                    combatComponent.attacking = true;

                    lastAttack_1 = Time.time + waitBetweenAttack_1;
                }
            }
            else
            {
                animator.ResetTrigger("Attack1");
            }

            if (lastAttack_2 < Time.time)
            {
                if (combatComponent.input.y != 0)
                {
                    animator.SetTrigger("Attack2");

                    combatComponent.attacking = true;

                    lastAttack_2 = Time.time + waitBetweenAttack_2;
                }
            }
            else
            {
                animator.ResetTrigger("Attack2");
            }
        }
    }
}