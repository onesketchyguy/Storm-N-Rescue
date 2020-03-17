using UnityEngine;

namespace Utility
{
    /// <summary>
    /// A tool bag to help speed things up a bit.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Returns the mouse's position in world space.
        /// </summary>
        /// <returns></returns>
        public static Vector3 GetMousePosition()
        {
            Vector3 moPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            return moPos;
        }

        /// <summary>
        /// Converts a Vector3 to a viewport position.
        /// </summary>
        /// <param name="worldpoint"></param>
        /// <returns></returns>
        public static Vector3 ToViewportSpace(Vector3 worldpoint)
        {
            return Camera.main.WorldToViewportPoint(new Vector3(worldpoint.x, worldpoint.y, -Camera.main.transform.position.z));
        }

        /// <summary>
        /// Converts a Vector3 to screen space.
        /// </summary>
        /// <param name="worldpoint"></param>
        /// <returns></returns>
        public static Vector3 ToScreenSpace(Vector3 worldpoint)
        {
            return Camera.main.WorldToScreenPoint(new Vector3(worldpoint.x, worldpoint.y, -Camera.main.transform.position.z));
        }

        /// <summary>
        /// Returns the mouse position locked to the grid.
        /// </summary>
        /// <returns></returns>
        public static Vector2Int GridLockedMousePosition()
        {
            Vector2 moPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            return new Vector2Int(Mathf.FloorToInt(moPos.x), Mathf.FloorToInt(moPos.y));
        }

        /// <summary>
        /// Returns a random RGB value.
        /// </summary>
        /// <returns></returns>
        public static Color GetRandomColor()
        {
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f);
            float b = Random.Range(0f, 1f);

            return new Color(r, g, b, 1);
        }

        /// <summary>
        /// Returns the top right corner of the screen.
        /// </summary>
        public static Vector3 ScreenMax { get { return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z)); } }

        /// <summary>
        /// Returns the absolute middle of the screen.
        /// </summary>
        public static Vector3 ScreenMid { get { return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z)); } }

        /// <summary>
        /// Returns the bottom left corner of the screen.
        /// </summary>
        public static Vector3 ScreenMin { get { return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z)); } }

        /// <summary>
        /// Returns true if the given position is not within the bounds of the screen.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="ScreenPadding"></param>
        /// <returns></returns>
        public static bool OffScreen(Vector3 position, float ScreenPadding = 1)
        {
            float minY = ScreenMin.y - ScreenPadding;
            float maxY = ScreenMax.y + ScreenPadding;

            float minX = ScreenMin.x - ScreenPadding;
            float maxX = ScreenMax.x + ScreenPadding;

            return (position.y > maxY) || (position.y < minY) || (position.x > maxX) || (position.x < minX);
        }

        /// <summary>
        /// A delegate for a value being modified
        /// </summary>
        /// <param name="amount"></param>
        public delegate void ValueModified(float amount);
    }
}