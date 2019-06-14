﻿using UnityEngine;
using UnityEngine.Tilemaps;

namespace LowEngine.LayerGeneration
{
    /// <summary>
    /// Creates a layer of cake for the player to walk on.
    /// </summary>
    public class LayerGenarator : MonoBehaviour
    {
        public TileBase TileToUse;

        public Tilemap WalkableMap; // The layer the player will be walking on/destroying
        public Tilemap WallMap; // The layer the player will NOT be walking on, but will be visable.

        public Vector2 size = new Vector2(10, 3);
        private Vector3 offSet { get; set; }

        /// <summary>
        /// Wether or not a layer is currently being made.
        /// </summary>
        public bool generating { get; private set; }

        [Header("Obstacles")]
        public GameObject Bomb;

        private void Start()
        {
            offSet = new Vector3(size.x / 2, 0);

            GenerateLayer();
        }

        private void Update()
        {
            DrawCamPos();

            // Create new layers
            if (WallMap.GetTile(new Vector3Int(0, (int)Utilities.ScreenMax.y + 5, 0)) == null)
            {
                GenerateLayer();
            }

            if (WallMap.GetTile(new Vector3Int(0, (int)Utilities.ScreenMin.y - 5, 0)) != null)
            {
                //DestroyTopLayers();
            }
        }

        private void DrawCamPos()
        {
            var topLeft = new Vector3(Utilities.ScreenMin.x, Utilities.ScreenMax.y);
            var bottomLeft = Utilities.ScreenMin;

            var bottomRight = new Vector3(Utilities.ScreenMax.x, Utilities.ScreenMin.y);
            var topRight = Utilities.ScreenMax;

            Debug.DrawLine(topLeft, topRight);
            Debug.DrawLine(topLeft, bottomLeft);
            Debug.DrawLine(topRight, bottomRight);
            Debug.DrawLine(bottomLeft, bottomRight);
        }

        /// <summary>
        /// Create a layer based on the current layer.
        /// </summary>
        private void GenerateLayer()
        {
            if (generating) return;
            generating = true;

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    WallMap.SetTile(pos, TileToUse);

                    if (x == 0 || x == size.x - 1 || y == 0 || y == size.y - 1)
                    {
                        WalkableMap.SetTile(pos, TileToUse);
                    }
                }
            }

            offSet = new Vector3(size.x / 2, offSet.y + size.y - 1); // Move the offset down so we can work with the next layer down next time.

            generating = false;
        }

        private void DestroyTopLayers()
        {
            for (int x = 0; x < 18; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Vector3Int pos = new Vector3Int((int)(x + -9), (int)((Utilities.ScreenMax.y + y)), 0); // Store the position so we can us it again later.

                    if (WallMap.GetTile(pos) != null)
                    {
                        WalkableMap.SetTile(pos, null);
                        WallMap.SetTile(pos, null);
                    }
                }
            }
        }
    }
}