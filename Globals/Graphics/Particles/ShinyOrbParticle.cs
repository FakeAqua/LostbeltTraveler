using LostbeltTraveler.Core.ParticleSystem;
using System;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class ShinyOrbParticle : BaseParticle
    {
        public int BlendStateType;
        public override BlendState UseBlendStateID => TheBlendState;
        public BlendState TheBlendState = BlendState.Additive;
        public bool AffectedByGravity = false;
        public bool GlowCenter = true;
        public float FadeOut;
        public Color InitColor;
        public float GlowCenterScale = 0.5f;
        public ShinyOrbParticle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
            BlendStateType = BlendStateID.Additive;
            FadeOut = 1f;
        }
        public ShinyOrbParticle(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, BlendState blendState = null, bool affactedByGravity = false, bool glowCenter = true, float glowCenterScale = 0.5f)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = InitColor = color;
            Lifetime = lifeTime;
            Scale = scale;
            TheBlendState = blendState ?? BlendState.Additive;
            AffectedByGravity = affactedByGravity;
            GlowCenter = glowCenter;
            GlowCenterScale = glowCenterScale;
            FadeOut = 1f;
        }

        public override void Update()
        {
            FadeOut -= 0.05f;
            Scale *= 0.93f;
            DrawColor = Color.Lerp(InitColor, InitColor * 0.2f, (float)Math.Pow(LifetimeRatio, 30));
            Velocity *= 0.95f;
            if (Velocity.Length() < 12f && AffectedByGravity)
            {
                Velocity.X *= 0.94f;
                Velocity.Y += 0.25f;
            }
            Rotation = Velocity.ToRotation() + PiOver2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2(1f, 1f) * Scale;
            Texture2D texture = LostbeltModAssets.Particle_ShinyOrb.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor, Rotation, texture.Size() * 0.5f, scale, 0, 0f);
            if (GlowCenter || GlowCenterScale >= 0)
                spriteBatch.Draw(texture, Position - Main.screenPosition, null, Color.White * FadeOut, Rotation, texture.Size() * 0.5f, scale * GlowCenterScale, 0, 0f);
        }
    }
}
