using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LowEngine
{
    public class GameManager : MonoBehaviour
    {
        internal PlayerManager player
        {
            get
            {
                return FindObjectOfType<PlayerManager>();
            }
        }

        internal bool playerDead
        {
            get
            {
                if (player == null) return true;

                return (player.Health.currentValue <= 0);
            }
        }

        public static GameManager instance;

        public string LevelToLoadOnDeath = "WinScreen";

        public GameObject WelcomeMessage;

#if UNITY_STANDALONE
        private float quitTime;
#endif

        private void Start()
        {
            instance = this;
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad < 4) return;

            if (WelcomeMessage != null && WelcomeMessage.activeSelf == true)
                WelcomeMessage.SetActive(false);

            if (playerDead && !alreadyLoading)
            {
                StartCoroutine(LoadNewLevel());
            }

            if (player.gameObject.transform.position.y < Utilities.ScreenMin.y - 1)
            {
                player.Hurt(30);
            }

#if UNITY_STANDALONE
            if (Input.GetKey(KeyCode.Escape))
            {
                quitTime += Time.deltaTime;
            }

            if (quitTime > 1)
            {
                Application.Quit();
            }
#endif
        }

        private bool alreadyLoading = false;

        private IEnumerator LoadNewLevel()
        {
            alreadyLoading = true;

            var soundTrack = GameObject.Find("SoundTrack").GetComponent<AudioSource>();

            while (soundTrack.volume > 0.01f)
            {
                soundTrack.volume = Mathf.Lerp(soundTrack.volume, 0, Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSecondsRealtime(1.5f);

            SceneManager.LoadScene(LevelToLoadOnDeath);
        }
    }
}