namespace Bullets
{
    public class HealthChangeEventArgs : EventArgs
    {
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public float HealthDelta { get; set; }

        public override string ToString()
        {
            return $"[HealthChangeEventArgs] CurrentHealth({CurrentHealth}) MaxHealth({MaxHealth}) HealthDelta({HealthDelta})";
        }
    }
}
