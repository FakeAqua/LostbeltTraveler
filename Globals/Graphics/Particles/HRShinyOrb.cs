using LostbeltTraveler.Core.ParticleSystem;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class HRShinyOrb : BaseParticle
    {
        public bool UseRot = false;
        public override BlendState UseBlendStateID => BlendState.Additive;
        public HRShinyOrb(Vector2 position, Vector2 velocity, Color color, int lifetime, float scale, float opacity = 1f)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifetime;
            Opacity = opacity;
            Scale = scale;
        }

        public override void OnSpawn()
        {
        }

        public override void Update()
        {
            if (!UseRot)
                Velocity *= 0.9f;
            else
                Velocity *= 0.94f;
            Scale = Lerp(Scale, 0, EaseInCubic(LifetimeRatio));

            if (UseRot)
            {
                Velocity = Velocity.RotatedBy(0.03f);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = LostbeltModAssets.Particle_HRShinyOrb.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, 0, texture.Size() / 2, Scale, SpriteEffects.None, 0);
        }
    }
}
