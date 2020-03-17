using UnityEngine;

namespace LowEngine.Helpers
{
    public delegate void TakeDamage(float amount);

    /// <summary>
    /// The base variables for any movement object.
    /// </summary>
    public interface IMovement
    {
        /// <summary>
        /// The input sent by the user.
        /// X = Horizontal input.
        /// Y = Jump input
        /// Z = Vertical input. (Forward back)
        /// W = Running input.
        /// </summary>
        Vector4 input { get; set; }

        /// <summary>
        /// Accessed by the animator.
        /// Walkspeed * run multiplier.
        /// </summary>
        Vector2 maxSpeed { get; }

        /// <summary>
        /// Accessed by the animator.
        /// Use the magnitude of your characters velocity.
        /// </summary>
        Vector2 currentSpeed { get; set; }

        /// <summary>
        /// On taking fall damage this will be invoked.
        /// </summary>
        TakeDamage fallDamageCallback { get; set; }
    }
}