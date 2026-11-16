using LostbeltTraveler.Core.ParticleSystem;
using Terraria;

namespace LostbeltTraveler.Globals.Graphics.Particles
{
    public class GeneralMountedPlayerParticle : BaseParticle
    {
        public BlendState UseBlendState = BlendState.Additive;
        public int MountedPlayer = -1;
        public float TargetScale = 0;
        public float TargetOpac = 0;
        public Texture2D UseTex = LostbeltModAssets.Particle_RingShiny.Value;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override BlendState UseBlendStateID => UseBlendState;
        public GeneralMountedPlayerParticle(Texture2D useTex, Color color, float rot, float scale, float opac, int lifeTime, int playerIndex, BlendState blendStateID)
        {
            UseTex = useTex;
            DrawColor = color;
            Scale = 0;
            Opacity = 0;
            TargetScale = scale;
            TargetOpac = opac;
            Rotation = rot;
            MountedPlayer = playerIndex;
            UseBlendState = blendStateID;
            Lifetime = lifeTime;
        }
        public override void OnSpawn()
        {
            base.OnSpawn();
        }
        public override void Update()
        {

            if (MountedPlayer != -1)
            {
                Player p = Main.player[MountedPlayer];
                if (p.dead)
                {
                    Kill();
                    return;
                }

                if (LifetimeRatio > .9f)
                {
                    //1->0
                    float rat = (1 - LifetimeRatio) / .1f;
                    Opacity = Lerp(0, Opacity, rat);
                }
                else
                {
                    Opacity = Lerp(Opacity, TargetOpac + .1f, .25f);
                    Scale = Lerp(Scale, TargetScale + .01f, .2f);

                }
            }
            else
            {
                Kill();
            }
            base.Update();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Player p = Main.player[MountedPlayer];
            if (!p.dead)
            {
                Position = p.MountedCenter;
                Position.Y += p.gfxOffY;
            }

            Texture2D tex = UseTex;
            spriteBatch.Draw(tex, Position - Main.screenPosition, null, DrawColor * Opacity, Rotation, tex.Size() / 2f, Scale, 0, 0);
            base.Draw(spriteBatch);
        }
    }
}
