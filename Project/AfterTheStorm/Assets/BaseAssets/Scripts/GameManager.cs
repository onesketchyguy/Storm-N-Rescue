using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LowEngine
{
    public class GameManager : MonoBehaviour
    {
        private PlayerManager player
        {
            get
            {
                return FindObjectOfType<PlayerManager>();
            }
        }

        private bool playerDead
        {
            get
            {
                if (player == null) return true;

                return (player.Health.currentValue <= 0);
            }
        }

        public string LevelToLoadOnDeath = "WinScreen";

        public GameObject WelcomeMessage;

        private void Update()
        {
            if (Time.timeSinceLevelLoad < 3) return;

            if (WelcomeMessage.activeSelf == true)
                WelcomeMessage.SetActive(false);

            if (playerDead && !alreadyLoading)
            {
                StartCoroutine(LoadNewLevel());
            }

            if (player.gameObject.transform.position.y < Utilities.ScreenMin.y - 2)
            {
                player.Hurt(30);
            }
        }

        private bool alreadyLoading = false;

        private IEnumerator LoadNewLevel()
        {
            alreadyLoading = true;

            yield return new WaitForSecondsRealtime(3.5f);

            SceneManager.LoadScene(LevelToLoadOnDeath);
        }
    }
}