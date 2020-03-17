using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hostile
{
    public class LightningManager : MonoBehaviour
    {
        internal List<Vector3> strikeablePositions = new List<Vector3>();

        public GameObject LightningEffect;
        public TileBase[] fire;

        public Lightning[] lightningBolts;

        private float waitBeforeNextStrike = 5;

        private float lastStrike;

        private bool StrikeRandomPoint()
        {
            int boltIndex = Random.Range(0, lightningBolts.Length - 1);
            int pointIndex = Random.Range(0, strikeablePositions.Count - 1);

            var bolt = lightningBolts[boltIndex];
            var point = strikeablePositions[pointIndex];

            if (point == null || bolt == null || bolt.Active)
            {
                return false;
            }

            bolt.StrikePoint(point);

            return true;
        }

        private void FixedUpdate()
        {
            if (strikeablePositions == null || strikeablePositions.Count <= 1)
            {
                return;
            }

            if (lastStrike < Time.time)
            {
                if (StrikeRandomPoint())
                {
                    lastStrike = Time.time + waitBeforeNextStrike;
                    waitBeforeNextStrike = Random.Range(0.1f, 1.8f);
                }

                for (int i = strikeablePositions.Count - 1; i >= 0; i--)
                {
                    if (strikeablePositions[i].y < LowEngine.Utilities.ScreenMin.y - 1)
                        strikeablePositions.RemoveAt(i);
                }
            }
        }
    }
}