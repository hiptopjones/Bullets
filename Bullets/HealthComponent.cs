using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal partial class HealthComponent : Component
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public EventHandler<HealthChangeEventArgs> HealthChanged;
        public EventHandler HealthDepleted;

        public float MaxHealth { get; set; }

        public float CurrentHealth { get; private set; }

        public override void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (CurrentHealth == 0)
            {
                return;
            }

            CurrentHealth = MathF.Max(CurrentHealth - damage, 0);
            RaiseHealthChanged(damage);

            if (CurrentHealth == 0)
            {
                RaiseHealthDepleted();
            }
        }

        private void RaiseHealthChanged(float damage)
        {
            HealthChanged?.Invoke(this, new HealthChangeEventArgs
            {
                CurrentHealth = CurrentHealth,
                MaxHealth = MaxHealth,
                HealthDelta = damage
            });
        }

        private void RaiseHealthDepleted()
        {
            HealthDepleted?.Invoke(this, null);
        }

        public override string ToString()
        {
            return $"[HealthComponent] CurrentHealth({CurrentHealth}) MaxHealth({MaxHealth})";
        }
    }
}
