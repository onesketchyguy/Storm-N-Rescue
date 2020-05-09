using LowEngine.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace LowEngine
{
    public class PlayerHealthKeeper : MonoBehaviour
    {
        private PlayerManager player;

        public Image Fill;

        public Text percentageKeeper;

        private void Update()
        {
            if (player == null)
            {
                SetFillAmount(0);

                player = FindObjectOfType<PlayerInput>().GetComponent<PlayerManager>();
            }
            else
            {
                SetFillAmount(1 - ((player.Health.MaxValue - player.Health.currentValue) / player.Health.MaxValue));
            }
        }

        private void SetFillAmount(float val)
        {
            Fill.fillAmount = val;

            if (percentageKeeper != null)
            {
                percentageKeeper.text = $"{System.Math.Round(val * 100, 0)}%";
            }
        }
    }
}