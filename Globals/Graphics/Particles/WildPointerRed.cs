using LostbeltTraveler.Core.ParticleSystem;
using LostbeltTraveler.Items.Weapons.Melee;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class WildPointerRed : BaseParticle
    {
        public override BlendState UseBlendStateID => BlendState.AlphaBlend;
        public WildPointerRed(Vector2 position, int lifetime, float rotation, float scale, float opacity)
        {
            Position = position;
            Lifetime = lifetime;
            Rotation = rotation;
            Scale = scale;
            Opacity = opacity;
            DrawColor = Color.Red;
        }
        public override void OnSpawn()
        {
        }
        public override void Update()
        {
            DrawColor *= 1 - LifetimeRatio;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = Request<Texture2D>(GetInstance<WildPointerThrown>().Texture).Value;
            spriteBatch.Draw(texture, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, texture.Size() / 2, Scale, 0, 0f);
        }
    }
}
