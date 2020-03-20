using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainMenu
{
    public class Enabler : MonoBehaviour
    {
        public GameObject[] objectsToEnable;

        public void Toggle()
        {
            SetItems(!objectsToEnable.FirstOrDefault().activeSelf);
        }

        public void SetItems(bool value)
        {
            foreach (var item in objectsToEnable)
            {
                item.SetActive(value);
            }
        }
    }
}