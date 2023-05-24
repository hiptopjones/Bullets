using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DamageComponent : Component
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public int Damage { get; set; }

        public override string ToString()
        {
            return $"[DamageComponent] Damage({Damage})";
        }
    }
}
