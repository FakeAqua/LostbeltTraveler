using LostbeltTraveler.Core.ParticleECS;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Requirement
{
    public class TechBladeLaser : ModProjectile
    {
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public NPC CurTarget = null;
        public enum State
        {
            Shoot,
            AttackLaser
        }
        public ref float Timer => ref Projectile.ai[0];
        public State AttackState
        {
            get => (State)Projectile.ai[1];
            set => Projectile.ai[1] = (float)value;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 16;
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.Opacity = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
            Projectile.penetrate = -1;
        }
        public override void AI()
        {
            Projectile.rotation += ToRadians(7.5f);
            if (AttackState == State.Shoot)
            {
                Projectile.velocity *= .95f;
                Projectile.Opacity = Lerp(Projectile.Opacity, 1, 0.1f);
                Timer++;
                if (Main.rand.NextBool(4))
                    ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(2), Projectile.SafeDir() * 0, Color.SkyBlue, 40, 1, Projectile.velocity.ToRotation() + PiOver2, Projectile.scale * .15f * 2, 3);
                if (Timer > Projectile.MaxUpdates * 30f)
                {
                    SoundEngine.PlaySound(LostbeltSoundAssets.Lightning_Strike with { MaxInstances = 1, Pitch = .63f, PitchVariance = .2f }, Projectile.Center);
                    if (Projectile.GetTargetSafe(out CurTarget, true, 1200, true))
                    {
                        AttackState = State.AttackLaser;
                        Projectile.extraUpdates = 25;
                        InitLaserAttack();
                        Projectile.velocity = Projectile.Center.GetNormalVector2(CurTarget.Center) * 5;
                        Timer = 0;
                    }
                    else
                        Projectile.Kill();
                }
            }
            else if (AttackState == State.AttackLaser)
            {
                ECSParticle.HRShinyOrb(Projectile.Center, Projectile.velocity * 0f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 40, .85f, 0.075f * 1.2f, 0.25f);
                ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(2), Projectile.SafeDir() * 0, Color.SkyBlue, 40, 1, Projectile.velocity.ToRotation() + PiOver2, Projectile.scale * .15f * 2, 3);
            }
            else
            {

            }
        }
        public void InitLaserAttack()
        {
            float glowScale = Projectile.scale * .3f;
            ECSParticle.CrossGlow(Projectile.Center, Color.DeepSkyBlue, 120, 0.85f, glowScale, 0.1f);
            ECSParticle.CrossGlow(Projectile.Center, Color.White, 120, .85f, glowScale * .96f, 0.1f);
            for (int i = 0; i < 8; i++)
            {
                float args = TwoPi / 8f * i;
                ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePos(16), Vector2.Zero, Color.SkyBlue, 45, 1, args, Projectile.scale * .65f, 3);
            }
            float ringScale = Projectile.scale * 1.2f;
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.SkyBlue, 70, 1, 0, ringScale, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .75f, .2f, 1);
            ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .45f, .2f, 1);
            for (int i = 0; i < 32; i++)
            {
                ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(60), 1.2f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.9f, 1.1f) * .1f, RandRotTwoPi, .25f);
            }

            for (int i = 0; i < 16; i++)
            {
                float args = TwoPi / 4f * i + PiOver4;
                ECSParticle.LightntingGlow(Projectile.Center.ToRandCirclePosEdge(6), args.ToRotationVector2() * Main.rand.NextFloat(0.1f, 1f) * 4.8f, Color.LightSkyBlue, 45, 1, Projectile.scale * .65f);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (AttackState == State.AttackLaser)
            {
                for (int i = 0; i < 8; i++)
                {
                    float args = TwoPi / 8f * i;
                    ECSParticle.HighResolutionThunder(Projectile.Center.ToRandCirclePosEdge(16), Vector2.Zero, Color.SkyBlue, 45, 1, args, Projectile.scale * .5f, 3);
                }
                float ringScale = Projectile.scale * 1.2f;
                ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.SkyBlue, 70, 1, 0, ringScale, .2f, 1);
                ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .85f, .2f, 1);
                ECSParticle.Ring(Projectile.Center, Vector2.Zero, Color.LightSkyBlue, 70, 1, 0, ringScale * .65f, .2f, 1);
                for (int i = 0; i < 32; i++)
                {
                    ECSParticle.TurbulenceShinyOrb(Projectile.Center.ToRandCirclePos(60), 1.2f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 45, 1, Projectile.scale * Main.rand.NextFloat(.9f, 1.1f) * .1f, RandRotTwoPi, .25f);
                }
                ECSParticle.ShinyCrossStarSmall(Projectile.Center, Vector2.Zero, Color.SkyBlue, 45, 1, ringScale, 0.02f);
                ECSParticle.ShinyCrossStarSmall(Projectile.Center, Vector2.Zero, Color.White, 45, 1, ringScale * .85f, 0.02f);

                Projectile.Kill();
            }

        }
        public SpriteBatch SB { get => Main.spriteBatch; }
        public override bool PreDraw(ref Color lightColor)
        {
            if (AttackState == State.AttackLaser)
                return false;
            int length = Projectile.oldPos.Length;
            Texture2D oldtex = LostbeltModAssets.Particle_SharpTear;
            Vector2 oldtexOrig = oldtex.Size() / 2f;
            for (int i = length - 1; i >= 0; i--)
            {
                float ratios = (1f - i / (float)length);
                Vector2 oldpos = Projectile.oldPos[i] - Main.screenPosition + Projectile.Size / 2f;
                Color c = Color.Lerp(Color.SkyBlue, Color.LightSkyBlue, ratios).ToAddColor(10);
                float oldscale = Lerp(.14f, 1f, ratios);
                float opa = Lerp(.1f, .5f, ratios) * Projectile.Opacity;
                Vector2 sharpScale = new Vector2(0.43f, 1.4f);
                Vector2 sharpPos = oldpos - new Vector2(0, 0).RotatedBy(Projectile.oldRot[i]);
                for (int j = -1; j < 2; j += 2)
                {
                    float oldRot = Projectile.oldRot[i] + PiOver4 * j;
                    SB.FastDraw(oldtex, sharpPos, c.ToAddColor(0) * opa, oldRot, oldtexOrig, sharpScale * oldscale, 0);
                    SB.FastDraw(oldtex, sharpPos, Color.White.ToAddColor(0) * opa, oldRot, oldtexOrig, sharpScale * oldscale * .5f, 0);
                }
            }

            SB.EnterShaderArea();
            Vector2 pos = Projectile.Center - Main.screenPosition;
            Texture2D tex = LostbeltModAssets.Particle_CrossGlow.Value;
            float scale = Projectile.scale * .12f;
            SB.FastDraw(tex, pos, SetColor(Color.SkyBlue) * .45f, 0, tex.Size() / 2f, scale, 0);
            SB.FastDraw(tex, pos, SetColor(Color.White) * .45f, 0, tex.Size() / 2f, scale * .96f, 0);
            SB.EndShaderArea();
            return false;
        }
        public Color SetColor(Color c) => c * Projectile.Opacity;
        public override void OnKill(int timeLeft)
        {
            base.OnKill(timeLeft);
        }
    }
}
