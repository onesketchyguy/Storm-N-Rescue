using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class FadeIn : MonoBehaviour
    {
        public Graphic graphic;

        public float fadeTime = 1;

        public Color FadeFrom = Color.black;
        public Color FadeTo = Color.clear;

        private void OnValidate()
        {
            if (graphic != null)
                graphic.color = FadeFrom;
        }

        private void Start()
        {
            StartCoroutine(Fade(fadeTime));
        }

        private IEnumerator Fade(float fadeTime)
        {
            graphic.color = FadeFrom;

            yield return null;

            while (graphic.color != FadeTo)
            {
                graphic.color = Color.Lerp(graphic.color, FadeTo, fadeTime * Time.deltaTime);
                yield return null;
            }
        }
    }
}