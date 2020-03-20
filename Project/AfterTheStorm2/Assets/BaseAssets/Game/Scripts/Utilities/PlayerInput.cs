using UnityEngine;

namespace LowEngine.Helpers
{
    /// <summary>
    /// Retrieves the input from unity, then feeds it
    /// into a component deriving from the interface IMovement.
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {
        /// <summary>
        /// The controller assigned to this player.
        /// </summary>
        public int playerNumber = 0;

        /// <summary>
        /// The unity input name for the run button.
        /// </summary>
        public string RunButton = "Fire3";

        /// <summary>
        /// Get the unity movementInput input.
        /// </summary>
        public Vector4 MoveInput => new Vector4(
            (Input.GetAxisRaw($"Horizontal{playerNumber}")), //The left and right input from the user
            (Input.GetButton($"Jump_{playerNumber}") ? 1 : 0), //The vertical input of the user
            (Input.GetAxisRaw($"Vertical{playerNumber}")), //The forward and back input of the user
            (Input.GetButton($"{RunButton}_{playerNumber}") ? 1 : 0) //The running input of the user
            );

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

        private Vector4 CombatInput => new Vector4(
            (Input.GetButton($"Fire1_{playerNumber}") ? 1 : 0) /*The first button of the user*/,
            (Input.GetButton($"Fire2_{playerNumber}") ? 1 : 0) /*The second button of the user*/,
            (Input.GetButton($"Cancel_{playerNumber}") ? 1 : 0) /*The stop input of the user*/,
            (Input.GetButton($"Submit_{playerNumber}") ? 1 : 0) /*The enter key for the user*/
            );

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

        private void Update()
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