using LostbeltTraveler.Core.ParticleSystem;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class ShinySquareSplit : BaseParticle
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        private bool FadeIn;
        private float TargetScale;
        private float RotSpeed = 0;
        public override BlendState UseBlendStateID => BlendState.Additive;
        public ShinySquareSplit(Vector2 position, Vector2 velocity, Color color, int lifeTime, float scale, float rot = 0, float rotSpeed = 0, float opacity = 1f, bool fadeIn = false)
        {
            Position = position;
            Velocity = velocity;
            DrawColor = color;
            Lifetime = lifeTime;
            Rotation = rot;
            Scale = TargetScale = scale;
            FadeIn = fadeIn;
            Opacity = opacity;
            RotSpeed = rotSpeed;
        }
        public override void OnSpawn()
        {
            if (FadeIn)
                Scale = 0;
        }
        public override void Update()
        {
            Velocity *= 0.97f;
            if (Time > (int)(Lifetime * 0.80f))
            {
                Scale = Lerp(Scale, 0f, 0.10f);
                Opacity *= 0.80f;
            }
            else if (FadeIn)
                Scale = Lerp(Scale, TargetScale, 0.3f);
            if (RotSpeed != 0)
                Rotation += RotSpeed;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = LostbeltModAssets.Particle_ShinySquareSplit.Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() * 0.5f, Scale, 0, 0f);
            //spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() * 0.5f, Scale, SpriteEffects.FlipHorizontally,0f);
        }
    }
}
