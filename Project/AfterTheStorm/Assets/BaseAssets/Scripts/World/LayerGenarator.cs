using UnityEngine;
using UnityEngine.Tilemaps;

namespace LowEngine.LayerGeneration
{
    /// <summary>
    /// Creates a layer of cake for the player to walk on.
    /// </summary>
    public class LayerGenarator : MonoBehaviour
    {
        public TileBase InnerBuilding;
        public TileBase BlankTile;
        public TileBase Wall;
        public TileBase Fire;
        public TileBase Rain;

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
        }

        /// <summary>
        /// Create a layer based on the current layer.
        /// </summary>
        private void GenerateLayer()
        {
            if (generating) return;
            generating = true;

            int civsPlaced = 0;

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x * 6; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x * 3), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    RainMap.SetTile(pos, Rain);
                }
            }

            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    Vector3Int pos = new Vector3Int((int)(x - offSet.x), (int)(y + offSet.y), 0); // Store the position so we can us it again later.

                    if (y > 0)
                    {
                        if (x == 0 || x == size.x - 1) // walls
                        {
                            WalkableMap.SetTile(pos, Wall);
                            WallMap.SetTile(pos, Wall);
                        }
                        else
                        if (y == 0 || y == size.y - 1) // cieling and floor
                        {
                            WalkableMap.SetTile(pos, Wall);
                            WallMap.SetTile(pos, BlankTile);
                        }
                        else
                        {
                            //Inside the building

                            var fireHere = (int)Random.Range(0, size.x) == 1;
                            if (fireHere)
                            {
                                HazardMap.SetTile(pos, Fire);
                            }

                            var placeStrikePoint = (int)Random.Range(0, size.x / 2) == 1;

                            if (placeStrikePoint)
                            {
                                Instantiate(StrikePoint, pos, Quaternion.identity);
                            }

                            if (civsPlaced < 2 && !fireHere) // Limit 2 civilians per story
                            {
                                var placeCivilian = (int)Random.Range(0, size.x / 2) == 1;

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

            offSet = new Vector3(size.x / 2, offSet.y + size.y - 1); // Move the offset down so we can work with the next layer down next time.

            generating = false;
        }
    }
}