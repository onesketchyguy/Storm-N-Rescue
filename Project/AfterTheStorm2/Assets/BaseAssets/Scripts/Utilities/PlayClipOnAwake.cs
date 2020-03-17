using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LowEngine.Audio
{
    public class PlayClipOnAwake : MonoBehaviour
    {
        [Tooltip("Clip to play on awake.")]
        public AudioClip clip;

        [Space]
        public bool Loop;

        // Start is called before the first frame update
        private void Start()
        {
            if (clip == null) return;

            AudioManager.PlayClip(clip, transform.position, AudioManager.GetClipVolume(transform.position));

            if (Loop)
            {
                Invoke("Start", clip.length + 0.1f);
            }
        }
    }
}