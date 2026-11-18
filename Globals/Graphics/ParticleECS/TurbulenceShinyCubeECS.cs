using LostbeltTraveler.Core.ParticleECS;
using System;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.ParticleECS
{
    public class TurbulenceShinyCubeECS : ECSParticleBehavior
    {
        public override void Update(ref ECSParticleData data)
        {
            data.Opacity = (float)Math.Sin(data.LifetimeRatio * Pi);
            data.Velocity *= .95f;
            data.Scale *= .98f;
            if (data.Time % 3 == 0 && data.aibool0)
            {
                data.Position += Main.rand.NextVector2Circular(data.aifloat0, data.aifloat1);
            }
            base.Update(ref data);
        }
        public override void Draw(ref ECSParticleData data)
        {
            Texture2D texture = LostbeltModAssets.Texture_WhiteCube.Value;
            Main.spriteBatch.Draw(texture, data.Position - Main.screenPosition, null, data.DrawColor * data.Opacity, data.Rotation, texture.Size() / 2, data.Scale, SpriteEffects.None, 0f);
        }
    }
}
