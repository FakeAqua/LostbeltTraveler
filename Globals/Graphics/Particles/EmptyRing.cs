using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Core.ParticleSystem;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class EmptyRing : BaseParticle
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        private bool AltRing;
        private bool FadeIn;
        private float TargetScale;
        public override BlendState UseBlendStateID =>BlendState.Additive;

        public EmptyRing(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, float opacity, bool altRing = false, bool fadeIn = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifeTime;
            Scale = TargetScale = scale;
            AltRing = altRing;
            Opacity = opacity;
            FadeIn = fadeIn;
        }
        public override void OnSpawn()
        {
            if (FadeIn)
                Scale = 0;
        }
        public override void Update()
        {
            Velocity *= 0.97f;
            if (Time > (int)(Lifetime * 0.75f))
            {
                Scale = Lerp(Scale, 0f, 0.15f);
                Opacity *= 0.99f;
            }
            else if (FadeIn)
                Scale = Lerp(Scale, TargetScale, 0.2f);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = AltRing ? LostbeltModAssets.Particle_RingHard.Value : LostbeltModAssets.Particle_Ring.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, 0, texture.Size() * 0.5f, Scale, 0, 0f);
            base.Draw(spriteBatch);
        }
    }
}
