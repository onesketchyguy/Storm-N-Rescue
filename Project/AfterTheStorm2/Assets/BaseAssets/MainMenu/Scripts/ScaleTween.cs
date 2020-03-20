using UnityEngine;

namespace MainMenu
{
    public class ScaleTween : MonoBehaviour
    {
        public LeanTweenType easeType;
        public AnimationCurve curve;

        public float duration = 0.5f;

        public float delay;

        private Vector3 scale;

        private void OnEnable()
        {
            if (scale == Vector3.zero)
            {
                scale = transform.localScale;

                LeanTween.scale(gameObject, Vector3.zero, 0);
            }
            else
            {
                if (easeType == LeanTweenType.animationCurve)
                    LeanTween.scale(gameObject, scale, duration).setDelay(delay).setEase(curve);
                else LeanTween.scale(gameObject, scale, duration).setDelay(delay).setEase(easeType);
            }
        }
    }
}