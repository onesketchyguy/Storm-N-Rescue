namespace LowEngine.Helpers
{
    /// <summary>
    /// All damagable objects must derive from this interface!
    /// </summary>
    public interface IDamagable
    {
        /// <summary>
        /// The health variable for a given object.
        /// </summary>
        MaxableValue Health { get; set; }

        /// <summary>
        /// Damage this players health bar with a specified value.
        /// </summary>
        /// <param name="damageToDeal">Amount of damage to deal. Please ensure you use a posative value here!</param>
        void Hurt(float damageToDeal);
    }
}