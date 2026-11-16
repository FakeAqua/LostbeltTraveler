using LostbeltTraveler.Core.ParticleSystem;
using System;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class ShinyOrbHard : BaseParticle
    {
        public override BlendState UseBlendStateID => base.UseBlendStateID;
        public bool AffectedByGravity = false;
        public bool GlowCenter = true;
        public Color InitColor;
        public float GlowCenterScale = 0.5f;
        public ShinyOrbHard(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
        }
        public override void Update()
        {
            Scale *= 0.93f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = LostbeltModAssets.Particle_ShinyOrb.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
        }
    }
}
