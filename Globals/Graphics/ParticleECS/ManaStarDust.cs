using LostbeltTraveler.Core.ParticleECS;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.ParticleECS
{
    public class ManaStarDust : ECSParticleBehavior
    {
        public override void OnSpawn(ref ECSParticleData data)
        {
        }
        public override void Update(ref ECSParticleData data)
        {
            data.Scale *= .97f;
            data.Opacity *= .98f;
            if (data.aibool0)
                data.Rotation += (data.Velocity.X > 0).ToDirectionInt() * .2f;
            data.Velocity *= .97f;
            if (data.aibool0)
            {

            }
            base.Update(ref data);
        }
        public override void Draw(ref ECSParticleData data)
        {
            Texture2D tex = LostbeltModAssets.Particle_ManaStarDust.Value;
            Rectangle frame = tex.Bounds;
            frame.Height /= 2;
            frame.Y += frame.Height;
            frame.Height -= 2;
            Color color = data.DrawColor * data.Opacity;
            Main.spriteBatch.Draw(tex, data.Position - Main.screenPosition, frame,
                color * 0.5f, data.Rotation, new Vector2(13), data.Scale, 0, 0);
            frame.Y -= frame.Height + 2;
            Main.spriteBatch.Draw(tex, data.Position - Main.screenPosition, frame, color, data.Rotation, new Vector2(13), data.Scale, 0, 0);
            base.Draw(ref data);
        }
    }
}
