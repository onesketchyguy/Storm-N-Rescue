using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Create a toggler for an array of objects.
    /// </summary>
    public class Tabber : MonoBehaviour
    {
        [Tooltip("Tabs to switch through. Will automatically set the first tab to the default tab.")]
        public GameObject[] tabs;

        private void Start() => ToggleToTab(tabs.FirstOrDefault()); // Set the current tab to the default tab

        /// <summary>
        /// Will toggle back to the default front tab
        /// </summary>
        /// <param name="tab"></param>
        public void ToggleFromTab(GameObject tab) => ToggleToTab(tabs.FirstOrDefault());

        /// <summary>
        /// Will toggle to a specified tab
        /// </summary>
        /// <param name="tab"></param>
        public void ToggleToTab(GameObject tab)
        {
            foreach (var item in tabs)
                item.SetActive(item == tab);
        }
    }
}