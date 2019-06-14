using UnityEngine;

namespace LowEngine
{
    public static class Utilities
    {
        public static Vector3 GetMousePosition()
        {
            Vector3 moPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            return moPos;
        }

        public static Vector3 ToViewportSpace(Vector3 worldpoint)
        {
            return Camera.main.WorldToViewportPoint(new Vector3(worldpoint.x, worldpoint.y, -Camera.main.transform.position.z));
        }

        public static Vector3 ToScreenSpace(Vector3 worldpoint)
        {
            return Camera.main.WorldToScreenPoint(new Vector3(worldpoint.x, worldpoint.y, -Camera.main.transform.position.z));
        }

        public static Vector2 GridLockedMousePosition()
        {
            Vector2 moPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            moPos.x = (int)moPos.x;
            moPos.y = (int)moPos.y;

            return moPos;
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

        public static Vector3 ScreenMax
        {
            get
            {
                return Camera.main.ViewportToWorldPoint(new Vector3(1, 1, -Camera.main.transform.position.z));
            }
        }

        public static Vector3 ScreenMid
        {
            get
            {
                return Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z));
            }
        }

        public static Vector3 ScreenMin
        {
            get
            {
                return Camera.main.ViewportToWorldPoint(new Vector3(0, 0, -Camera.main.transform.position.z));
            }
        }

        public static bool OffScreen(Vector3 position, float ScreenPadding = 1)
        {
            float minY = ScreenMin.y - ScreenPadding;
            float maxY = ScreenMax.y + ScreenPadding;

            float minX = ScreenMin.x - ScreenPadding;
            float maxX = ScreenMax.x + ScreenPadding;

            return (position.y > maxY) || (position.y < minY) || (position.x > maxX) || (position.x < minX);
        }
    }
}