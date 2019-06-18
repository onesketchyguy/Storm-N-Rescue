using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LowEngine.LayerGeneration
{
    /// <summary>
    /// Creates a layer of cake for the player to walk on.
    /// </summary>
    public class LayerGenarator : MonoBehaviour
    {
        [Header("Tiles")]
        public TileBase InnerBuilding;

        public TileBase BlankTile;
        public TileBase Wall;
        public TileBase Fire;
        public TileBase Rain;
        public TileBase Window;

        [Header("Tilemaps")]
        public Tilemap WalkableMap; // The layer the player will be walking on/destroying

        public Tilemap HazardMap; // The layer the player can be damaged by
        public Tilemap WallMap; // The layer the player will NOT be walking on, but will be visable.
        public Tilemap RainMap; // A layer the player will NOT be walking on, but will be visable.

        public Vector2 size = new Vector2(10, 3);
        private Vector3 offSet { get; set; }

        /// <summary>
        /// Wether or not a layer is currently being made.
        /// </summary>
        public bool generating { get; private set; }

        public GameObject Civilian;

        public GameObject StrikePoint;

        public GameObject Door;

        private void Start()
        {
            offSet = new Vector3(size.x / 2, 0);

            GenerateLayer();
        }

        private void Update()
        {
            // Create new layers
            if (WallMap.GetTile(new Vector3Int(0, (int)Utilities.ScreenMax.y + 5, 0)) == null)
            {
                GenerateLayer();
            }

            if (WallMap.GetTile(new Vector3Int(0, (int)(Utilities.ScreenMin.y + size.y), 0)) != null && Time.timeSinceLevelLoad > 30)
            {
                ClearLowerLevels();
            }
        }

        /// <summary>
        /// Create a layer based on the current layer.
        /// </summary>
        private void GenerateLayer()
        {
            if (generating) return;
            generating = true;

            int civsPlaced = 0;

            //Rain
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x * 6; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x * 3), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    RainMap.SetTile(pos, Rain);
                }
            }

            //Environment
            for (int y = 0; y < size.y; y++)
            {
                var doorPlaced = false;

                for (int x = 0; x < size.x; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    if (y > 0)
                    {
                        if (x == 0 || x == size.x - 1) // walls
                        {
                            if (y == 1 && x == 0)
                            {
                                WalkableMap.SetTile(pos, Window);
                            }
                            else
                            {
                                WalkableMap.SetTile(pos, Wall);
                            }

                            WallMap.SetTile(pos, Wall);
                        }
                        else
                        if (y == size.y - 1) // cieling
                        {
                            WalkableMap.SetTile(pos, Wall);
                            WallMap.SetTile(pos, BlankTile);
                        }
                        else
                        {
                            //Inside the building
                            var placeDoor = (int)Random.Range(0, size.x) == 1 || x == size.x - 2;

                            if (y == 1 && !doorPlaced) // Floor level
                            {
                                if (placeDoor)
                                {
                                    Instantiate(Door, pos + Door.transform.position, Quaternion.identity);

                                    doorPlaced = true;
                                }
                            }

                            var fireHere = (int)Random.Range(0, size.x) == 1 && placeDoor == false;
                            if (fireHere)
                            {
                                HazardMap.SetTile(pos, Fire);
                            }

                            var placeStrikePoint = (int)Random.Range(0, size.x / 2) == 1 && placeDoor == false;

                            if (placeStrikePoint)
                            {
                                Instantiate(StrikePoint, pos, Quaternion.identity);
                            }

                            if (civsPlaced < 2 && !fireHere) // Limit 2 civilians per story
                            {
                                var placeCivilian = (int)Random.Range(0, size.x / 2) == 1 && placeDoor == false;

                                if (placeCivilian)
                                {
                                    Instantiate(Civilian, pos, Quaternion.identity);

                                    civsPlaced++;
                                }
                            }

                            WallMap.SetTile(pos, InnerBuilding);
                        }
                    }
                    else
                    if (x == 0 || x == size.x - 1)
                    {
                        WallMap.SetTile(pos, Wall);
                        WalkableMap.SetTile(pos, Wall);
                    }
                    else
                    {
                        WallMap.SetTile(pos, InnerBuilding);
                    }
                }
            }

            offSet = new Vector3(size.x / 2, offSet.y + size.y - 1); // Move the offset down so we can work with the next layer up next time.

            generating = false;
        }

        private int minimumLevel;

        private void ClearLowerLevels()
        {
            var max = (int)Utilities.ScreenMin.y - 3;
            for (int y = minimumLevel; y < max; y++)
            {
                for (int x = 0; x < size.x * 2; x++)
                {
                    var pos = new Vector3Int((int)(x - size.x), (int)(y), 0);

                    HazardMap.SetTile(pos, null);
                    WalkableMap.SetTile(pos, null);
                    WallMap.SetTile(pos, null);

                    if (y == max - 1) // setup next minimum so we don't loop through already cleared blocks
                        minimumLevel = max;
                }
            }
        }
    }
}