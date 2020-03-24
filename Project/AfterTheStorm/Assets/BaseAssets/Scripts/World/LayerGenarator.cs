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
        public TileBase[] FireTiles;
        public TileBase Rain;
        public TileBase Window;
        public TileBase Door;

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

        private int layers;

        private World.DoorManager dm;
        private Hostile.LightningManager lm;

        private enum Spawn
        {
            none,
            fire,
            lightingPoint,
            civilian,
            door
        }

        private void Start()
        {
            offSet = new Vector3(size.x / 2, -1); // Move the offset down so we can work with the next layer up next time.

            dm = FindObjectOfType<World.DoorManager>();
            lm = FindObjectOfType<Hostile.LightningManager>();

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

        private Spawn[] GetSpawns(Spawn[] spawns = null)
        {
            int civsPlaced = 0;

            bool floorEmpty = true;

            if (spawns == null)
                spawns = new Spawn[(int)size.x - 2];

            bool doorPlaced = false;
            for (int i = 0; i < spawns.Length; i++) // Generate this floor
            {
                if (spawns[i] == Spawn.none)
                    spawns[i] = (Spawn)Random.Range(0, System.Enum.GetValues(typeof(Spawn)).Length);

                if (spawns[i] == Spawn.civilian)
                {
                    if (civsPlaced >= 2)// limit 2 civilians per story
                    {
                        spawns[i] = (Spawn)Random.Range(0, System.Enum.GetValues(typeof(Spawn)).Length);
                    }

                    civsPlaced++;
                }

                if (spawns[i] == Spawn.door && doorPlaced)
                {
                    spawns[i] = Spawn.civilian;
                    civsPlaced++;
                }
                else if (spawns[i] != Spawn.door && !doorPlaced && i == spawns.Length - 1)
                {
                    spawns[i] = Spawn.door;
                    doorPlaced = true;
                }

                if (doorPlaced == false)
                    doorPlaced = spawns[i] == Spawn.door;

                if (spawns[i] != Spawn.none) floorEmpty = false;
            }

            if (floorEmpty)
            {
                return GetSpawns(spawns);
            }

            return spawns;
        }

        /// <summary>
        /// Create a layer based on the current layer.
        /// </summary>
        private void GenerateLayer()
        {
            if (generating) return;
            generating = true;

            //Rain
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x * 6; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x * 4), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    RainMap.SetTile(pos, Rain);
                }
            }

            //Environment
            for (int y = 0; y < size.y; y++)
            {
                var spawns = GetSpawns();

                for (int x = 0; x < size.x; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    if (x == 0 || x == size.x - 1) // walls
                    {
                        if (y == 1 && (x == 0 || x == size.x - 1)) // Middle area
                        {
                            if (layers != 0) // Ground level
                            {
                                // Descide wether or not to put a window here

                                var placeWindow = Random.Range(0, 2) == 1;

                                if (placeWindow)
                                    WalkableMap.SetTile(pos, Window);
                                else WalkableMap.SetTile(pos, Wall);
                            }
                            else
                            {
                                WalkableMap.SetTile(pos, Wall);
                            }
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
                        if (y != 0)
                        {
                            bool _continue = false;

                            // Inside the building
                            switch (spawns[x - 1])
                            {
                                case Spawn.none:
                                    break;

                                case Spawn.fire:
                                    HazardMap.SetTile(pos, FireTiles[Random.Range(0, FireTiles.Length)]);
                                    break;

                                case Spawn.lightingPoint:
                                    lm.strikeablePositions.Add(pos);
                                    break;

                                case Spawn.civilian:
                                    Instantiate(Civilian, pos, Quaternion.identity);
                                    break;

                                case Spawn.door:
                                    WallMap.SetTile(pos, Door);
                                    dm.doors.Add(pos + Vector3.one * 0.5f);

                                    _continue = true;
                                    break;

                                default:
                                    break;
                            }

                            if (_continue) continue;
                        }

                        WallMap.SetTile(pos, InnerBuilding);
                    }
                }
            }

            offSet = new Vector3(size.x / 2, offSet.y + size.y - 1); // Move the offset down so we can work with the next layer up next time.

            generating = false;

            layers++;
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