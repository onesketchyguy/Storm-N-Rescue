using Friendly;
using System.Linq;
using UnityEngine;

namespace LowEngine.Helpers
{
    /// <summary>
    /// Retrieves the input from unity, then feeds it
    /// into a component deriving from the interface IMovement.
    /// </summary>
    public class AIInput : MonoBehaviour
    {
        private PlayerManager playerComponent;
        private DestroyEnviroment destroyEnviroment;

        /// <summary>
        /// Get the unity movementInput input.
        /// </summary>
        public Vector4 MoveInput;

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

        private Vector4 CombatInput;

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
                    storedCombatComponent = GetComponent<ICombat>(); // Get the component from the utility system and store it.

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

        private Civilian[] civilians;
        private Civilian target;

        private Vector3 safeZone;

        private void CheckForCivilians()
        {
            var list = new System.Collections.Generic.List<Civilian>();

            foreach (var item in CivilianBlackboard.civilians.ToArray())
            {
                if (item.transform.position.y <= transform.position.y)
                {
                    list.Add(item);
                }
            }

            civilians = list.ToArray();
            target = civilians.FirstOrDefault();
        }

        private void Start()
        {
            playerComponent = GetComponent<PlayerManager>();
            destroyEnviroment = GetComponent<DestroyEnviroment>();
            safeZone = FindObjectOfType<CivilianSafeZone>().transform.position;
        }

        private void Update()
        {
            // Check for walls
            Transform hit = destroyEnviroment.CheckForWall();
            CombatInput.x = (hit != null) ? 1 : 0;

            CheckForCivilians();

            if (playerComponent.carrying == null)
            {
                if (target != null)
                    MoveInput.x = Mathf.Clamp(target.transform.position.x - transform.position.x,
                        -1f, 1f);
                else
                    MoveInput.x = 0;
            }
            else
            {
                MoveInput.x = Mathf.Clamp(safeZone.x - transform.position.x,
                        -1f, 1f);
            }

            SendInput();
        }

        private void SendInput()
        {
            if (damagableComponent != null)
            {
                if (damagableComponent.Health.empty)
                {
                    combatComponent.input = Vector4.zero;
                    movementComponent.input = Vector4.zero;

                    return;
                }
            }

            if (movementComponent == null) // Check if a movement component exists
            {
                if (Time.timeSinceLevelLoad > 1) // If we don't have access to the movement component one second after launching this scene.
                {
                    Debug.LogError($"No component of type LowEngine.Helpers.IMovement on {gameObject.name}. Movement not possible."); // Log out an error.

                    Destroy(this); // Remove this component.
                }

                return; // If the movementComponent is not existant then don't continue.
            }

            if (combatComponent == null) // Check if a combat component exists
            {
                if (Time.timeSinceLevelLoad > 1) // If we don't have access to the combat component one second after launching this scene.
                {
                    Debug.LogError($"No component of type LowEngine.Helpers.ICombat on {gameObject.name}. Combat not possible."); // Log out an error.

                    Destroy(this); // Remove this component.
                }

                return; // If the combatComponent is not existant then don't continue.
            }

            movementComponent.input = MoveInput; // Set the input for the user.

            combatComponent.input = CombatInput; // Set the combat input for the user.
        }
    }
}