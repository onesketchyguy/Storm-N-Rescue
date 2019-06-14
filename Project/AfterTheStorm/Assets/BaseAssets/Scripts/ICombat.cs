using UnityEngine;

namespace LowEngine.Helpers
{
    public interface ICombat
    {
        /// <summary>
        /// The attack input for any character.
        /// </summary>
        Vector4 input { get; set; }

        /// <summary>
        /// Weatrher or not this object is attacking.
        /// </summary>
        bool attacking { get; set; }
    }
}