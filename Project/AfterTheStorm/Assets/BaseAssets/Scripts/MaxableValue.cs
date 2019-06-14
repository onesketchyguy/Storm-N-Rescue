namespace LowEngine
{
    /// <summary>
    /// Holds a value for the user.
    /// Contains a max, and a minimum.
    /// </summary>
    [System.Serializable]
    public class MaxableValue
    {
        public delegate void ValueChanged(float newValue);

        /// <summary>
        /// A callback delegate for the value changing.
        /// </summary>
        public ValueChanged OnValueModifiedCallback;

        /// <summary>
        /// A callback delegate for the value changing, outs the change amount
        /// </summary>
        public ValueChanged OnValueModifiedCallback_ChangeAmount;

        /// <summary>
        /// The max this value can reach.
        /// </summary>
        public int MaxValue = 30;

        /// <summary>
        /// Returns the values empty state.
        /// Current value less than or equal to zero;
        /// </summary>
        public bool empty => currentValue <= 0;

        public float currentValue { get; private set; } = 30;

        /// <summary>
        /// Changes the health of the character in question.
        /// </summary>
        /// <param name="mod">Use a negative value to damage, and a posative to heal.</param>
        public void ModifyValue(float mod)
        {
            // Add the modification to the currentValue, then clamp it between the max and zero.
            float newValue = (currentValue + mod >= MaxValue) ? MaxValue : (currentValue + mod > 0) ? currentValue + mod : 0;

            // Apply the new value.
            currentValue = newValue;

            // Callback to any listeners.
            if (OnValueModifiedCallback != null)
            {
                OnValueModifiedCallback.Invoke(currentValue);
            }
            if (OnValueModifiedCallback_ChangeAmount != null)
            {
                float modAmount = (currentValue + mod >= MaxValue) ? 0 : (currentValue + mod > 0) ? mod : 0;

                OnValueModifiedCallback_ChangeAmount.Invoke(modAmount);
            }
        }

        public void Reset()
        {
            currentValue = MaxValue;
        }

        public void Clear()
        {
            currentValue = 0;
        }

        public MaxableValue(int maxValue, ValueChanged onValueModifiedCallback)
        {
            OnValueModifiedCallback += onValueModifiedCallback;
            MaxValue = maxValue;
        }

        public MaxableValue(int maxValue = 3, float currentValue = -1)
        {
            if (currentValue == -1) this.currentValue = maxValue;
            else this.currentValue = currentValue;
            MaxValue = maxValue;
        }
    }
}