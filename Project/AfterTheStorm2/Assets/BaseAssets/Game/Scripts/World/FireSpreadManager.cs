using LowEngine;
using LowEngine.LayerGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace World
{
    public class FireSpreadManager : MonoBehaviour
    {
        public TileBase[] Fire;

        private List<Vector3Int> flames = new List<Vector3Int>();

        private int minimumLevel;

        private Tilemap HazardMap;
        private Tilemap WallMap;

        [Range(0.0f, 1f)]
        public float spreadChance = 0.5f;

        private LayerGenarator Generator;

        private void Awake()
        {
            Generator = FindObjectOfType<LayerGenarator>();

            //HazardMap = Generator.HazardMap;
            //WallMap = Generator.WallMap;

            InvokeRepeating(nameof(SpreadFire), 5, 1);
        }

        private void SpreadFire()
        {
            flames.Clear();

            var size = Generator.size;

            var max = (int)(Utility.Utilities.ScreenMax.y + size.y);
            for (int y = minimumLevel; y < max; y++)
            {
                for (int x = 0; x < size.x * 2; x++)
                {
                    var pos = new Vector3Int((int)(x - size.x), y, 0);

                    if (HazardMap.GetTile(pos) != null) flames.Add(pos);
                }
            }

            for (int i = 0; i < flames.Count; i++)
            {
                var pos = flames[i];
                var spread = (flames.Count - Random.Range(0, flames.Count)) / flames.Count <= spreadChance;

                var placed = false;
                if (spread)
                {
                    for (int y = pos.y - 2; y < pos.y + 2; y++)
                    {
                        if (placed) break;

                        for (int x = pos.x - 2; x < pos.x + 2; x++)
                        {
                            if (placed) break;

                            var spreadPos = new Vector3Int(pos.x + x, pos.y + y, 0);

                            if (spreadPos == pos) continue;

                            if (WallMap.GetTile(spreadPos) != null && HazardMap.GetTile(spreadPos) == null)
                            {
                                HazardMap.SetTile(spreadPos, Fire[Random.Range(0, Fire.Length)]);

                                minimumLevel = max;

                                placed = true;
                            }
                        }
                    }
                }
            }
        }
    }
}