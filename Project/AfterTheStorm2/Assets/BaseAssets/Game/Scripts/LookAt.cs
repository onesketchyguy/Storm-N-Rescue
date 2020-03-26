using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class LookAt : MonoBehaviour
    {
        public Transform lookAtObject;

        private void Update()
        {
            if (lookAtObject == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.LookAt(lookAtObject.position);
        }
    }
}