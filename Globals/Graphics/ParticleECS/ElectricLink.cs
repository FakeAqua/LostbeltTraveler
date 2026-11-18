using LostbeltTraveler.Core.ParticleECS;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.ParticleECS
{
    public class ElectricLink : ECSParticleBehavior
    {
        public override void OnSpawn(ref ECSParticleData data)
        {
        }
        public override void Update(ref ECSParticleData data)
        {
            data.Opacity *= .97f;
            data.Scale *= .98f;
            data.Velocity *= .97f;
            base.Update(ref data);
        }
        public override void Draw(ref ECSParticleData data)
        {
            Texture2D tex = LostbeltModAssets.Particle_ElectricLink.Value;
            Color color = data.DrawColor;
            SpriteEffects seff = data.aibool0 ? SpriteEffects.FlipVertically : 0;
            Main.spriteBatch.Draw(tex, data.Position - Main.screenPosition, null, color * data.Opacity, data.Rotation, tex.Size() / 2f, data.Scale, seff, 0);
        }
    }
}
