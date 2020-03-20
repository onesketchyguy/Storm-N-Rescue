using LowEngine.Audio;
using LowEngine.Helpers;
using UnityEngine;

namespace LowEngine
{
    /// <summary>
    /// Allows for both arial and ground movement, including a ground pound, and can feed fall damage.
    /// </summary>
    public class Movement : MonoBehaviour, IMovement
    {
        #region IMovement items

        public Vector4 input { get; set; }

        public Vector2 maxSpeed { get; private set; }
        public Vector2 currentSpeed { get; set; }
        public TakeDamage fallDamageCallback { get; set; }

        #endregion IMovement items

        private Vector4 lastInput;

        private new Rigidbody rigidbody;

        // Inspector items
        public float speed = 5.5f;

        private bool movingRight = true;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            var velocity = rigidbody.velocity;

            velocity -= (Vector3)lastInput;

            // Add the user input
            var xMove = (input.x * speed);
            velocity += new Vector3(xMove, 0, 0);

            // Move in that direction (visual only)
            var leanAmount = ((-input.x * 2) + -currentSpeed.x);
            rigidbody.rotation = Quaternion.Euler(transform.forward * leanAmount * Time.deltaTime);

            // Face in the direction we are moving (visual kinda stuff)
            if (input.x != 0)
                movingRight = (velocity.x > 0);

            transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);

            // Set the variables
            rigidbody.velocity = currentSpeed = velocity;

            lastInput = new Vector4(xMove, 0, 0);
        }
    }
}