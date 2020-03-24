using UnityEngine;

namespace LowEngine
{
    /// <summary>
    /// Destroys itself after falling past the bottom of the screen.
    /// </summary>
    public class DestroyAfterFall : MonoBehaviour
    {
        private void FixedUpdate()
        {
            if (Time.timeSinceLevelLoad <= 10) return;

            if (transform.position.y < Utilities.ScreenMin.y)
            {
                Destroy(gameObject);
            }
        }
    }
}