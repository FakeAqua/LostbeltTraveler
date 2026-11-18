using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Graphics.Particles;
using Terraria;

namespace LostbeltTraveler.Projs.General
{
    public class FishingFoodProj : LostbeltProjs
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public ref float Timer => ref Projectile.ai[0];
        public float ReadySpawnTime => 30 * Projectile.MaxUpdates;
        public float LerpValue => Utils.GetLerpValue(0, ReadySpawnTime, Timer, true);
        public int FoodType
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.MaxUpdates = 3;
            Projectile.noEnchantmentVisuals = true;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public override void ProjAI()
        {
            Timer++;
            if (LerpValue < 1)
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, Owner.MountedCenter, LerpValue);
            }
            else
            {
                Projectile.tileCollide = true;
                Projectile.velocity = -Vector2.UnitY * 3;
                if (Timer > ReadySpawnTime * 2)
                {
                    Vector2 center = Owner.Center - Vector2.UnitY * 50;
                    for (int i = 0; i < 8; i++)
                    {
                        float progress = Lerp(.7f, 1f, i / 8f);
                        new NoiseShockRing(center, Vector2.Zero, Color.White, 60, 1, progress * .5f, -1, Vector2.Zero, false).Spawn();
                    }
                    for (int i = 0; i < 20; i++)
                    {
                        ECSParticle.ShinyCrossStarECS(center, RandVelTwoPi(0.52f, 6.4f), Color.White, 40, 1, 0.4f, .2f);
                    }
                    ECSParticle.KiraStar(center, Color.White, 30, 1, 1);
                    ScarletSound(LostbeltSoundAssets.Moonlight_Ding, center, pitch: -.4f, volume: .45f);
                    LostbeltMethods.ScarletSpawnItem(Owner.GetSource_FromThis(), center, Owner.Hitbox, FoodType);
                    Projectile.Kill();
                }
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SB.EnterShaderArea();
            Texture2D tex = LostbeltModAssets.Particle_HRShinyOrb.Value;
            Vector2 center = Projectile.Center - Main.screenPosition;
            SB.FastDraw(tex, center, Color.White, 0, tex.Size() / 2f, 0.6f * LerpValue, 0);
            SB.EndShaderArea();
            return false;
        }
    }
}
