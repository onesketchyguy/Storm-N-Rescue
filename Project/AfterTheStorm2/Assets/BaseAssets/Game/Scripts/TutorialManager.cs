using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LowEngine.Helpers
{
    public class TutorialManager : MonoBehaviour
    {
        private PlayerManager _player;

        private PlayerManager Player
        {
            get
            {
                if (_player == null) _player = FindObjectOfType<PlayerManager>();

                return _player;
            }
        }

        /// <summary>
        /// Time before movement keys arrive.
        /// </summary>
        private float moveKeys = 10;

        private Vector3 startPosition;

        /// <summary>
        /// Time before showing destroy wall key.
        /// </summary>
        private float attackKey = 15;

        private int timesAttacked;

        /// <summary>
        /// Time before showing go up stairs key.
        /// </summary>
        private float stairsKey = 25;

        /// <summary>
        /// Time before showing rescue key.
        /// </summary>
        private float rescueKey = 5;

        private int timesRescued;
        private float timeBeforeCarriedCivilians;

        public Text TutorialText;

        private float timeShowingMessage;

        private void ShowMessage(string message)
        {
            TutorialText.text = message;

            timeShowingMessage = Time.time;
        }

        private string CurrentMessage
        {
            get
            {
                return TutorialText.text;
            }
        }

        private void Start()
        {
            startPosition = Player.transform.position;
            ShowMessage("");
        }

        private void Update()
        {
            if (Time.timeSinceLevelLoad > 120) { ShowMessage(""); Destroy(gameObject); } // Two minutes in

            if (Time.timeSinceLevelLoad < 3) return;

            if (timeShowingMessage + 2 < Time.time)
            {
                ShowMessage("");
            }

            var loadTime = Time.timeSinceLevelLoad + 3;

            if (loadTime > moveKeys && Vector3.Distance(Player.transform.position, startPosition) < 2)
            {
                // Display Move keys.
                ShowMessage("Press left/D and right/D to move.");
            }
            else
            {
                if (CurrentMessage == "Press left/D and right/D to move.") ShowMessage("");
            }

            if (Player.GetComponent<ICombat>().input.x != 0) timesAttacked++;

            if (loadTime > attackKey && timesAttacked < 1)
            {
                // Display Attack key.
                ShowMessage("Press C or left click to break walls and fight fires.");
            }
            else
            {
                if (CurrentMessage == "Press C or left click to break walls.") ShowMessage("");
            }

            if (PlayerManager.carrying == true)
            {
                if (Player.GetComponent<ICombat>().input.y != 0) timesRescued++;

                if (timeBeforeCarriedCivilians + rescueKey < Time.time && timesRescued < 1)
                {
                    // Display Attack key.
                    ShowMessage("Press X or right click to toss civilians to safety.");
                }
                else
                {
                    if (CurrentMessage == "Press X or right click to toss civilians to safety.") ShowMessage("");
                }
            }
            else
            {
                timeBeforeCarriedCivilians = Time.time;
            }

            if (loadTime > stairsKey && Vector3.Distance(new Vector3(0, Player.transform.position.y), new Vector3(0, startPosition.y)) < 2.5)
            {
                // Display stairs key.
                ShowMessage("Press Down/S when near stair cases to go up.");
            }
            else
            {
                if (CurrentMessage == "Press Down/S when near stair cases to go up.") ShowMessage("");
            }
        }
    }
}