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
    internal class ParticlesComponent : DrawableComponent
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private class ParticleEffect
        {
            public CircleShape Shape { get; set; } = new CircleShape();
            public Vector2f Velocity { get; set; }
            public float RemainingTime { get; set; }
        }

        public float CircleRadius { get; set; }
        public uint PointCount { get; set; }
        public Color FillColor { get; set; }
        public Vector2f PositionOffset { get; set; }
        public Vector2f RandomOffsetRange { get; set; }
        public float EffectSpeed { get; set; }
        public float EffectDuration { get; set; }
        public int MaxActiveEffects { get; set; } = 100;

        private ResourceManager ResourceManager { get; set; }
        private Random Random { get; set; } = new Random();

        private List<ParticleEffect> EffectsRingBuffer = new List<ParticleEffect>();
        private int StartIndex { get; set; }
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
                ParticleEffect particleEffect = new ParticleEffect();
                particleEffect.Shape = new CircleShape(CircleRadius, PointCount);
                particleEffect.Shape.FillColor = FillColor;

                EffectsRingBuffer.Add(particleEffect);
            }
        }

        public override void Update(float deltaTime)
        {
            for (int i = StartIndex; i != NextIndex; i = (i + 1) % EffectsRingBuffer.Count)
            {
                ParticleEffect particleEffect = EffectsRingBuffer[i];
                if (particleEffect.RemainingTime < 0)
                {
                    StartIndex = (StartIndex + 1) % EffectsRingBuffer.Count;
                    continue;
                }

                particleEffect.Shape.Position += particleEffect.Velocity * deltaTime;
                particleEffect.RemainingTime -= deltaTime;
            }

            Debug.DrawText($"StartIndex({StartIndex}) NextIndex({NextIndex})", new Vector2f(0, 0));
        }

        public override void Draw(GraphicsManager graphicsManager)
        {
            for (int i = StartIndex; i != NextIndex; i = (i + 1) % EffectsRingBuffer.Count)
            {
                ParticleEffect particleEffect = EffectsRingBuffer[i];
                graphicsManager.Draw(particleEffect.Shape);
            }
        }

        public void OnHealthChanged(object sender, HealthChangeEventArgs e)
        {
            CreateParticles();
        }

        private void CreateParticles()
        {
            for (int i = 0; i < 10; i++)
            {
                ParticleEffect particleEffect = EffectsRingBuffer[NextIndex];
                NextIndex = (NextIndex + 1) % EffectsRingBuffer.Count;

                particleEffect.RemainingTime = EffectDuration;
                particleEffect.Velocity = EffectSpeed * RandomOnUnitCircle();

                Vector2f randomOffset = new Vector2f(
                    RandomOffsetRange.X * (Random.NextSingle() - 0.5f),
                    RandomOffsetRange.Y * (Random.NextSingle() - 0.5f));
                Vector2f position = Owner.Transform.Position + PositionOffset - randomOffset;

                particleEffect.Shape.Position = position;
            }
        }

        private Vector2f RandomOnUnitCircle()
        {
            float angle = Random.NextSingle() * 2 * MathF.PI;
            return new Vector2f(MathF.Cos(angle), MathF.Sin(angle));
        }
    }
}
