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
    public class SupremeGemYoyoProj : LostbeltProjs, IPixelatedRenderer

    {
        public override string Texture => GetInstance<DiamondYoyoProj>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public float RotationSpeed = 0;
        public List<Vector2> OldPos = new List<Vector2>();
        public List<float> OldRot = new List<float>();
        public float HittingFrame = 0;
        public bool IsCloneYoyo = false;
        public bool SpawnSmear = false;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Type] = -1;
            ProjectileID.Sets.YoyosMaximumRange[Type] = LostbeltMethods.TilePixel(120);
            ProjectileID.Sets.YoyosTopSpeed[Type] = 9;
            Projectile.ToTrailSetting(48);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = ProjAIStyleID.Yoyo;
            Projectile.penetrate = -1;
            Projectile.MaxUpdates = 7;
            Projectile.SetupImmnuity(6);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
            ScarletSound(SoundID.DD2_FlameburstTowerShot, Projectile.Center);
            for (int i = 0; i < 16; i++)
                ECSParticle.SmokeParticle(Projectile.Center, Projectile.velocity.ToRandVelocity(ToRadians(16), 1, 16f), Color.White, 45, RandRotTwoPi, 1, 0.4f, false, BlendState.Additive);
            for (int i = 0; i < 16; i++)
                ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(2f), Projectile.velocity.ToRandVelocity(ToRadians(16), 1, 16), RandLerpColor(Color.White, Color.LightSkyBlue), 45, RandRotTwoPi, .51f, .31f, true, BlendState.Additive);
            for (int i = 0; i < 16; i++)
                ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePos(1.2f), Projectile.velocity.ToRandVelocity(ToRadians(16), 1, 16), RandLerpColor(Color.White, Color.LightSkyBlue), 45, 1, Main.rand.NextFloat(.85f, 1.15f) * .64f, 0.2f, BlendState.Additive);


        }
        public void CheckCloneYoyo()
        {
            int mainYoyo = -1;
            foreach (var proj in Main.ActiveProjectiles)
            {
                //遍历是否为主悠悠球。如果是，跳出去
                //由于泰拉瑞亚射弹列表的特性与悠悠球手套的特性，遍历时遇到的第一个悠悠球总会是主悠悠球。不需要额外判定
                if (proj.IsLegalFriendlyProj() && proj.type == Type && proj.owner == Owner.whoAmI)
                {
                    mainYoyo = proj.whoAmI;
                    break;
                }
            }
            //检查这个悠悠球是否为当前的悠悠球的索引
            if (Projectile.whoAmI != mainYoyo)
                IsCloneYoyo = true;
        }
        public override void ProjAI()
        {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 2f);
            CheckCloneYoyo();
            if (IsCloneYoyo)
                Projectile.MaxUpdates = 6;
            else
            {
                if (!SpawnSmear && !Owner.HasProj<SupremeGemYoyoProjAmethystSmear>())
                {
                    Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                        ProjectileType<SupremeGemYoyoProjAmethystSmear>(), Projectile.originalDamage, Projectile.knockBack, Owner.whoAmI);
                    p.ai[0] = Projectile.whoAmI;
                    SpawnSmear = true;
                }

            }
            //OldPos和OldRot用于存顶点轨迹需要的信息
            //Projectile.OldPos才直接用于画球体本身
            OldPos.Add(Projectile.Center);
            OldRot.Add(Projectile.velocity.ToRotation());
            if (OldPos.Count > 48)
            {
                OldPos.RemoveAt(0);
                OldRot.RemoveAt(0);
            }
            //减少自转速度，因为使用了高EU的取巧方案来确保球的残影能够整齐而不是间隔太远
            if (HittingFrame > 0)
                HittingFrame--;
            if (SmearFrame > 0)
                SmearFrame--;
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool(8))
                ECSParticle.ShinyCrossStarSmall(Projectile.Center.ToRandCirclePos(16), -Projectile.velocity * .14f, RandLerpColor(Color.SkyBlue, Color.LightSkyBlue), 50, 1, 0.26f, 0f);
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (IsCloneYoyo)
                return null;
            Rectangle rec = Utils.CenteredRectangle(Projectile.Center, new Vector2(16 * 8));
            //这里的判定是因为，只有主悠悠球本身首次命中了，才能直接抛出副悠悠球
            //这个悠悠球的伤害判定已经用SupremeGemYoyoProjAmethystSmear取代了，其大部分的攻击逻辑也会跟随Smear本身而不是跟随主球自己
            return rec.Intersects(targetHitbox) && Projectile.numHits < 1;
        }
        public float SmearFrame = 0;
        public float HitOpac = 0;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            HittingFrame = Projectile.MaxUpdates * 20;
            SmearFrame = Projectile.MaxUpdates * 40;
            base.OnHitNPC(target, hit, damageDone);
        }
        public ScarletDrawLayer LayerToRenderTo => ScarletDrawLayer.BeforeDusts;
        public BlendState BlendState => BlendState.Additive;
        public void RenderPixelated(SpriteBatch sb)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return;
            LostbeltMethods.EnterShaderAreaPixel(BlendState.Additive);
            //不要试图理解这里的码，我测，我一直热重载调试出来的，直接拿过去抄然后自己改改参数就行了
            float opac = Clamp(Lerp(0f, 1f, Projectile.velocity.Length()), 0, 1);
            if (IsCloneYoyo)
                opac *= .9f;
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.RoyalBlue * 1f * opac, 1.15f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.LightSkyBlue * .85f * opac, 1f);
            DrawTrails(LostbeltModAssets.Trail_TerraRayFlow.Texture, Color.White * .65f * opac, 0.85f, offsetHeight: 0f);
            LostbeltMethods.EndShaderAreaPixel();

        }


        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            PixelatedRenderManager.BeginDrawProj = true;
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length - 10;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i];
                Color c = Color.Lerp(Color.RoyalBlue, Color.LightSkyBlue, progress);
                float scale = (1f - (progress * 0.6f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress) * .75f;
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor(10) * .75f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.White, Color.LightSkyBlue, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(0) * .75f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 8; i++)
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(1.5f, 1.5f) +
                    (TwoPi / 8f * i).ToRotationVector2() * 1.5f, Color.AliceBlue.ToAddColor(), Projectile.rotation, projTex.Size() / 2f, Projectile.scale, 0);
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
            shader.Parameters["uColor"].SetValue(drawColor.ToVector4() * 1f);
            shader.Parameters["uFadeoutLength"].SetValue(0.4f);
            shader.Parameters["uFadeinLength"].SetValue(0.05f);
            shader.CurrentTechnique.Passes[0].Apply();
            if (OldPos.Count < 3)
                return;
            DrawSetting drawSetting = new DrawSetting(useTex.Value, true);
            List<TrailDrawDate> trailDrawDates = [];
            int posCount = OldPos.Count;
            for (int j = posCount - 1; j >= 0; j--)
            {
                if (OldPos[j] == Vector2.Zero)
                    continue;
                float rot = OldRot[j];
                trailDrawDates.Add(new(OldPos[j], drawColor, new Vector2(0, 45 * multipleSize * Projectile.scale), rot));
            }
            TrailRender.DrawTrail([.. trailDrawDates], drawSetting);
        }
    }
}
