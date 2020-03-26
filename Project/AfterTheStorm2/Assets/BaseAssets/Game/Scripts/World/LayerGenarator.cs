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
        public LayerMask wallMask;

        [Header("Tiles")]
        public GameObject InnerBuilding;

        public GameObject Wall;
        public GameObject[] FireTiles;
        public GameObject Window;
        public GameObject Door;

        [Header("Tilemaps")]
        public Transform WalkableMap; // The layer the player will be walking on/destroying

        public Transform HazardMap; // The layer the player can be damaged by
        public Transform WallMap; // The layer the player will NOT be walking on, but will be visable.
        public Transform RainMap; // A layer the player will NOT be walking on, but will be visable.

        public Vector2 gridOffset;
        public Vector2 size = new Vector2(10, 3);
        private Vector3 offSet { get; set; }

        public float startOffsetY = -2.5f;

        /// <summary>
        /// Wether or not a layer is currently being made.
        /// </summary>
        public bool generating { get; private set; }

        public GameObject Civilian;

        private int layers;
        private int maxLayers;

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
            offSet = new Vector3(size.x / 2, startOffsetY); // Move the offset down so we can work with the next layer up next time.

            dm = FindObjectOfType<World.DoorManager>();
            lm = FindObjectOfType<Hostile.LightningManager>();

            maxLayers = Random.Range(5, 15);

            GenerateLayer();
        }

        private void Update()
        {
            // Create new layers
            if (CheckLayer(0, Utilities.ScreenMax.y + 5) == false)
                GenerateLayer();
        }

        private bool CheckLayer(float x, float y)
        {
            var origin = new Vector3(x, y, -5);
            var direction = Vector3.forward;

            var ray = new Ray(origin, direction);
            RaycastHit hitInfo;

            Debug.DrawRay(origin, direction * 1000, Color.red, 1);

            Physics.Raycast(ray, out hitInfo, 1000, wallMask, QueryTriggerInteraction.UseGlobal);

            return (hitInfo.transform != null);
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
            if (generating || layers >= maxLayers) return;
            generating = true;

            //Environment
            for (int y = 0; y < size.y; y++)
            {
                var spawns = GetSpawns();

                for (int x = 0; x < size.x; x++)
                {
                    Vector3 pos = new Vector3(Mathf.FloorToInt(x - offSet.x), Mathf.FloorToInt(y + offSet.y), 0); // Store the position so we can us it again later.

                    if (x == 0 || x == size.x - 1) // walls
                    {
                        if (y == 1 && (x == 0 || x == size.x - 1)) // Middle area
                        {
                            // Descide wether or not to put a window here

                            var placeWindow = Random.Range(0, 2) == 1;

                            if (placeWindow)
                            {
                                // Create a window here
                                Instantiate(Window, pos + (Vector3)gridOffset, Quaternion.identity, WalkableMap);
                            }
                            else
                            {
                                // Create a wall here
                                Instantiate(Wall, pos + (Vector3)gridOffset, Quaternion.identity, WalkableMap);
                            }
                        }
                        else
                        {
                            // Create a wall here
                            Instantiate(Wall, pos + (Vector3)gridOffset, Quaternion.identity, WalkableMap);
                        }

                        // Create a wall here
                        Instantiate(Wall, pos + (Vector3)gridOffset, Quaternion.identity, WallMap);
                    }
                    else
                    if (y == size.y - 1) // cieling
                    {
                        // Create a wall here
                        Instantiate(Wall, pos + (Vector3)gridOffset, Quaternion.identity, WalkableMap);
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
                                    // Create a Fire here
                                    Instantiate(FireTiles[Random.Range(0, FireTiles.Length)], pos + (Vector3)gridOffset, Quaternion.identity, HazardMap);

                                    break;

                                case Spawn.lightingPoint:
                                    lm.strikeablePositions.Add(pos);
                                    break;

                                case Spawn.civilian:
                                    Instantiate(Civilian, pos + (Vector3)gridOffset, Quaternion.identity, WallMap);
                                    break;

                                case Spawn.door:
                                    // Create a Door here
                                    Instantiate(Door, pos + (Vector3)gridOffset, Quaternion.identity, WallMap);
                                    dm.doors.Add(pos + (Vector3)gridOffset + Vector3.one * 0.5f);

                                    _continue = true;
                                    break;

                                default:
                                    break;
                            }

                            if (_continue) continue;
                        }

                        // Create a wall here
                        Instantiate(InnerBuilding, pos + (Vector3)gridOffset, Quaternion.identity, WallMap);
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

                    //HazardMap.SetTile(pos, null);
                    //WalkableMap.SetTile(pos, null);
                    //WallMap.SetTile(pos, null);

                    if (y == max - 1) // setup next minimum so we don't loop through already cleared blocks
                        minimumLevel = max;
                }
            }
        }
    }
}