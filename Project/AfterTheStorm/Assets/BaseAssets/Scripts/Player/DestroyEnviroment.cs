using UnityEngine;

namespace LowEngine
{
    using Helpers;

    public class DestroyEnviroment : MonoBehaviour, ICombat
    {
        public Vector4 input { get; set; }
        public bool attacking { get; set; }

        private void Update()
        {
        }
    }
}