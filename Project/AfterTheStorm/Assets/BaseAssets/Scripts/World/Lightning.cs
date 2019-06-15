using UnityEngine;
using LowEngine;
using System.Collections;
using UnityEngine.Tilemaps;

namespace Hostile
{
    [RequireComponent(typeof(LineRenderer))]
    public class Lightning : MonoBehaviour
    {
        private LineRenderer _renderer;

        public TileBase Fire;

        public bool Active
        {
            get
            {
                for (int i = 0; i < _renderer.positionCount; i++)
                {
                    if (_renderer.GetPosition(i) != Vector3.zero) return true;
                }

                return false;
            }
        }

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();

            Reset();
        }

        private void Reset()
        {
            _renderer = GetComponent<LineRenderer>();

            for (int i = 0; i < _renderer.positionCount; i++)
            {
                _renderer.SetPosition(i, Vector3.zero);
            }
        }

        public void StrikePoint(Vector3 position)
        {
            _renderer.positionCount = Random.Range(10, 20);

            var startPos = Utilities.ScreenMax * 2 + (Vector3.right * Random.Range(-Utilities.ScreenMax.x * 2, 0));

            for (int i = 0; i < _renderer.positionCount - 1; i++)
            {
                _renderer.SetPosition(i, startPos);
            }

            StartCoroutine(LightningEffect(position));
        }

        private IEnumerator LightningEffect(Vector3 target)
        {
            for (int i = 0; i < _renderer.positionCount - 1; i++)
            {
                yield return new WaitForSeconds(i * 0.01f);

                var maxDist = Vector3.Distance(_renderer.GetPosition(i), target) - (_renderer.positionCount - i);

                var x_range = Random.Range(.5f, 3f);
                var y_range = Random.Range(.5f, 1.5f);

                if (i != _renderer.positionCount - 1 && i != 0)
                {
                    var pos = Vector3.MoveTowards(_renderer.GetPosition(i), target + new Vector3(Random.Range(-x_range, x_range), Random.Range(-y_range, y_range)), maxDist);

                    _renderer.SetPosition(i, pos);
                }
                else
                if (i != 0)
                {
                    _renderer.SetPosition(i, target);
                }
            }

            // Explode
            foreach (var map in FindObjectsOfType<Tilemap>())
            {
                if (map.name.ToLower().Contains("back")) continue;
                if (map.name.ToLower().Contains("walk")) continue;

                Debug.Log("Hazard map");

                map.SetTile(new Vector3Int((int)target.x, (int)target.y, (int)target.z), Fire);
            }

            yield return null;

            StartCoroutine(ResetLightningEffect());
        }

        private IEnumerator ResetLightningEffect()
        {
            var target = _renderer.GetPosition(_renderer.positionCount - 1);
            int iterations = 0;
            while (_renderer.GetPosition(0) != target)
            {
                if (iterations > 3) break;

                yield return null;

                for (int i = 0; i < _renderer.positionCount - 1; i++)
                {
                    yield return new WaitForSeconds(0.01f);

                    var maxDist = Vector3.Distance(_renderer.GetPosition(i), target) - (_renderer.positionCount - i);

                    var x_range = Random.Range(.5f, 3f);
                    var y_range = Random.Range(.5f, 1.5f);

                    var pos = Vector3.MoveTowards(_renderer.GetPosition(i), target + new Vector3(Random.Range(-x_range, x_range), Random.Range(-y_range, y_range)), maxDist);

                    _renderer.SetPosition(i, pos);
                }

                iterations++;
            }

            Reset();
        }
    }
}