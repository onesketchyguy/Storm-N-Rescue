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
            if (Time.timeSinceLevelLoad < 30) return;

            if (transform.position.y < Utilities.ScreenMin.y - 5)
            {
                Destroy(gameObject);
            }
        }
    }
}