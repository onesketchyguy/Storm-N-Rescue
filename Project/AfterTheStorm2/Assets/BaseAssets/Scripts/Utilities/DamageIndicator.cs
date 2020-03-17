using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LowEngine.UI
{
    public class DamageIndicator : MonoBehaviour
    {
        public Image indicator;

        public PlayerManager player;

        private float lastKnownHealth;
        private float currentHealth;

        private float percentChange
        {
            get
            {
                var val = (lastKnownHealth - currentHealth) / lastKnownHealth;

                return (val > 0 ? val : 0);
            }
        }

        private float timeSinceDamaged;

        [Range(0, 1)]
        public float weight = 0.5f;

        private void Start()
        {
            if (player == null)
                player = FindObjectOfType<PlayerManager>();

            lastKnownHealth = player.Health.MaxValue;

            player.Health.OnValueModifiedCallback += HealthModified;

            indicator.color = Color.clear;
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad < 2) return;

            if (currentHealth != lastKnownHealth)
            {
                indicator.color = Color.Lerp(indicator.color, Color.red - new Color(0, 0, 0, weight - percentChange), Time.deltaTime);

                if (Time.time > timeSinceDamaged + 0.2f)
                {
                    currentHealth = lastKnownHealth;
                }
            }
            else
            {
                indicator.color = Color.Lerp(indicator.color, Color.clear, Time.deltaTime);
            }
        }

        private void HealthModified(float currentHealth)
        {
            if (currentHealth < lastKnownHealth)
            {
                this.currentHealth = currentHealth;
                timeSinceDamaged = Time.time;
            }
        }
    }
}