using NLog;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Bullets
{
    internal class DamageNumbersComponent : DrawableComponent
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private class DamageNumberEffect
        {
            public Text DamageText { get; set; } = new Text();
            public bool IsEffectActive { get; set; }
            public float RemainingTime { get; set; }
        }

        public int FontId { get; set; }
        public uint FontSize { get; set; }
        public Color FillColor { get; set; }
        public Vector2f PositionOffset { get; set; }
        public Vector2f RandomOffsetRange { get; set; }
        public Vector2f EffectVelocity { get; set; }
        public float EffectDuration { get; set; }
        public int MaxActiveEffects { get; set; } = 5;

        private ResourceManager ResourceManager { get; set; }
        private Random Random { get; set; } = new Random();

        private List<DamageNumberEffect> EffectsRingBuffer = new List<DamageNumberEffect>();
        private int NextIndex { get; set; }

        public override void Awake()
        {
            ResourceManager = ServiceLocator.Instance.GetService<ResourceManager>();
            if (ResourceManager == null)
            {
                throw new Exception($"Unable to retrieve resource manager from service locator");
            }
        }

        public override void Start()
        {
            for (int i = 0; i < MaxActiveEffects; i++)
            {
                DamageNumberEffect damageNumberEffect = new DamageNumberEffect();
                damageNumberEffect.DamageText.Font = ResourceManager.GetFont(FontId);
                damageNumberEffect.DamageText.CharacterSize = FontSize;
                damageNumberEffect.DamageText.FillColor = FillColor;

                EffectsRingBuffer.Add(damageNumberEffect);
            }
        }

        public override void Update(float deltaTime)
        {
            foreach (DamageNumberEffect damageNumberEffect in EffectsRingBuffer)
            {
                if (damageNumberEffect.IsEffectActive)
                {
                    if (damageNumberEffect.RemainingTime < 0)
                    {
                        damageNumberEffect.IsEffectActive = false;
                        continue;
                    }

                    damageNumberEffect.DamageText.Position += EffectVelocity * deltaTime;
                    damageNumberEffect.RemainingTime -= deltaTime;
                }
            }
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            int currentIndex = NextIndex;
            while (true)
            {
                DamageNumberEffect damageNumberEffect = EffectsRingBuffer[currentIndex];
                if (damageNumberEffect.IsEffectActive)
                {
                    graphicsManager.Draw(damageNumberEffect.DamageText);
                }

                // Navigate the collection in insertion order to ensure correct z order
                currentIndex = (currentIndex + 1) % EffectsRingBuffer.Count;
                if (currentIndex == NextIndex)
                {
                    break;
                }
            }
        }

        public void OnHealthChanged(object sender, HealthChangeEventArgs e)
        {
            // Use a ring buffer index to ensure we add the next effect in order
            DamageNumberEffect damageNumberEffect = EffectsRingBuffer[NextIndex];
            NextIndex = (NextIndex + 1) % EffectsRingBuffer.Count;

            damageNumberEffect.DamageText.DisplayedString = e.HealthDelta.ToString();

            damageNumberEffect.IsEffectActive = true;
            damageNumberEffect.RemainingTime = EffectDuration;

            Vector2f randomOffset = new Vector2f(
                RandomOffsetRange.X * (Random.NextSingle() - 0.5f),
                RandomOffsetRange.Y * (Random.NextSingle() - 0.5f));
            Vector2f position = Owner.Transform.Position + PositionOffset - randomOffset;

            damageNumberEffect.DamageText.Position = position;
        }

        public override string ToString()
        {
            return $"[HealthBarComponent] FontId({FontId}) FontSize({FontSize}) FillColor({FillColor}) PositionOffset({PositionOffset}) RandomOffsetRange({RandomOffsetRange}) EffectVelocity({EffectVelocity}) EffectDuration({EffectDuration}) MaxActiveEffects({MaxActiveEffects})";
        }
    }
}
