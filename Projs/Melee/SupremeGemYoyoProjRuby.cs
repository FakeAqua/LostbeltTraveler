using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Core.PixelatedRender;
using LostbeltTraveler.Core.Primitives.Trail;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Melee
{
    public class SupremeGemYoyoProjRuby : LostbeltProjs, IPixelatedRenderer
    {
        public override string Texture => GetVanillaAssetPath(VanillaAsset.Item, ItemID.Ruby);
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public ref float Timer => ref Projectile.ai[0];
        public List<Vector2> OldPos = new List<Vector2>();
        public List<float> OldRot = new List<float>();

        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(32);
        }
        public override void ExSD()
        {
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.MaxUpdates = 3;
            Projectile.width = Projectile.height = 16;
            Projectile.SetupImmnuity(-1);
            Projectile.penetrate = 1;
            Projectile.damage = 0;
        }
        public int CountProjectiles(int projectileID)
        {
            int c = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (!proj.active || proj.type != projectileID)
                    continue;
                c++;
            }
            return c;
        }
        public override void ProjAI()
        {
            OldPos.Add(Projectile.Center);
            OldRot.Add(Projectile.velocity.ToRotation());
            if (OldPos.Count > 15)
            {
                OldPos.RemoveAt(0);
                OldRot.RemoveAt(0);
            }
            OnIntersect();
            RubyAI();

            if (Projectile.IsOutScreen())
                return;
        }
        public void OnIntersect()
        {
            if (!Projectile.Hitbox.Intersects(Owner.Hitbox))
                return;
            Projectile.Kill();
            Owner.ScarletHeal(1);
            int dagger = ProjectileType<SupremeGemYoyoProjAmber>();
            int c = CountProjectiles(dagger);
            if (c >= 5)
                return;

            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.ToSafeNormalize() * 16f, dagger, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
            float maxAngle = TwoPi;
            if (c == 0)
                maxAngle = 0;
            float index = 1;
            maxAngle = maxAngle > TwoPi ? TwoPi : maxAngle;
            foreach (var ap in Main.ActiveProjectiles)
            {
                if (ap.owner == Projectile.owner && ap.type == dagger)
                {
                    ap.localAI[0] = (index / 4f) * maxAngle - maxAngle / 2;
                    ap.ai[2] = index;
                    ap.netUpdate = true;
                    index++;
                }
            }
        }
        public void RubyAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Main.rand.NextBool(10))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center.ToRandCirclePosEdge(6), DustID.GemRuby);
                d.velocity = Projectile.velocity / 4f;
                d.noGravity = true;
                d.scale *= Main.rand.NextFloat(.8f, 1.1f);
            }

            if (Projectile.timeLeft > 50)
            {
                Projectile.scale = Lerp(Projectile.scale, 1.01f, .12f);
                if (Projectile.scale > 1 && Timer > 25)
                {
                    if (!Owner.dead)
                        Projectile.HomingTarget(Owner.Center, -1, 10f, 10f);
                    else
                        Projectile.Kill();
                }
            }
            else
            {
                Projectile.scale = Lerp(Projectile.scale, 0f, .12f);
                if (Projectile.scale <= 0.2f)
                    Projectile.Kill();
            }
            Timer++;

        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnKill(int timeLeft)
        {
            Vector2 spawnPos = Projectile.Center;
            ECSParticle.HRShinyOrb(spawnPos, Vector2.Zero, Color.Crimson, 40, 1, 0.2f, 0.75f);
            for (int i = 0; i < 5; i++)
            {
                ECSParticle.ShinyCrossStarECS(spawnPos, RandVelTwoPi(4), RandLerpColor(Color.Crimson, Color.Red), 45, 1, 0.5f, 0.2f);
            }

            base.OnKill(timeLeft);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //不要试图理解这里的码，我测，我一直热重载调试出来的，直接拿过去抄然后自己改改参数就行了

            float opac = 1;
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.Crimson * 1.15f * opac, 1.15f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.Red * opac, 1f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.White * opac * .845f, 0.85f, offsetHeight: 0f);
            LostbeltMethods.EndShaderAreaPixel();

        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length - 1;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i] + PiOver2;
                Color c = Color.Lerp(Color.White, Color.Red, progress);
                float scale = (1f - (progress * 0.38f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress) * .67f;
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(10) * .75f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.Red, Color.Crimson, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(0) * .75f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) +
                    (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.Crimson.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
            SB.FastDraw(projTex, drawPos, Color.White, Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);

            return false;
        }
        public void DrawTrails(Asset<Texture2D> useTex, Color drawColor, float multipleSize = 1f, float alphaValue = 1f, float offsetHeight = 1f)
        {
            float laserLength = 150;
            Effect shader = LostbeltShaderAssets.TerrarRayLaser;
            shader.Parameters["LaserTextureSize"].SetValue(useTex.Size());
            shader.Parameters["targetSize"].SetValue(new Vector2(laserLength, useTex.Height()));
            shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly * -150f);
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * .85f);
            shader.Parameters["uFadeoutLength"].SetValue(0.8f);
            shader.Parameters["uFadeinLength"].SetValue(0.13f);
            shader.CurrentTechnique.Passes[0].Apply();
            if (OldPos.Count < 3)
                return;
            //做掉可能存在的零向量
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = OldPos.Count;
            for (int j = posCount - 1; j >= 0; j--)
            {
                if (OldPos[j] == Vector2.Zero)
                    continue;
                float rot = OldRot[j];
                Vector2 posOffset = Main.rand.NextVector2Circular(1.5f, 1.5f);
                trailDrawDates.Add(new(OldPos[j] + posOffset, drawColor, new Vector2(0, 50 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);

        }

    }
}
