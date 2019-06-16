using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hostile
{
    public class LightningManager : MonoBehaviour
    {
        private List<Transform> strikeablePositions = new List<Transform>();

        public GameObject LightningEffect;
        public TileBase fire;

        private void UpdateStrikeablePositions()
        {
            strikeablePositions.Clear();

            var objects = GameObject.FindGameObjectsWithTag("StrikePosition");

            foreach (var item in objects)
            {
                if (strikeablePositions.Contains(item.transform) == false) strikeablePositions.Add(item.transform);
            }
        }

        public Lightning[] lightningBolts;

        private float waitBeforeNextStrike = 5;

        private float lastStrike;

        private void Awake()
        {
            //lastStrike = waitBeforeNextStrike;
        }

        private void FixedUpdate()
        {
            if (strikeablePositions == null || strikeablePositions.Count == 0)
            {
                UpdateStrikeablePositions();
                return;
            }

            if (lastStrike < Time.time)
            {
                for (int i = 0; i < lightningBolts.Length; i++)
                {
                    var bolt = lightningBolts[i];

                    if (bolt.Active) continue;

                    if (strikeablePositions[0] == null)
                    {
                        UpdateStrikeablePositions();

                        return;
                    }

                    var obj = strikeablePositions[0].gameObject;
                    bolt.StrikePoint(obj.transform.position);
                    Destroy(obj);

                    UpdateStrikeablePositions();
                    lastStrike = Time.time + waitBeforeNextStrike;
                    break;
                }
            }

            waitBeforeNextStrike = Random.Range(0.1f, 3f);
        }
    }
}