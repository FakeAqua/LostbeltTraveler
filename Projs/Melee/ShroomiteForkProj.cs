using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Items.Weapons.Melee;
using LostbeltTraveler.Rarity.RarityShinyMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class ShroomiteForkProj : LostbeltProjs
    {
        public override string Texture => GetInstance<ShroomiteFork>().Texture;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(8);
        }
        public override void ExSD()
        {
            Projectile.width = Projectile.height = 16;
            Projectile.MaxUpdates = 3;
            Projectile.penetrate = 2;
            Projectile.SetupImmnuity(30);
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.IsOutScreen())
                return;
            if (Main.rand.NextBool())
            ECSParticle.ShinyCrossStarECS(Projectile.Center.ToRandCirclePosEdge(8), Projectile.velocity / 8f, RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .3f, .2f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for(int i =0;i<20;i++)
            {
            ECSParticle.ShinyCrossStarECS(target.Center.ToRandCirclePosEdge(8),RandVelTwoPi(1.2f,7.6f), 
                RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), 40, 1, Main.rand.NextFloat(.9f, 1.1f) * .8f, .2f);
            }
            for(int i =0;i<10;i++)
            {
                ECSParticle.TurbulenceShinyOrb(target.Center, 2.2f, RandLerpColor(Color.RoyalBlue, Color.LightSkyBlue), 45, 1, 0.3f,glowMult:.3f);
            }
            Projectile p= Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileType<ShroomiteForkSlash>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            if (target.IsLegal())
                p.Lostbelt().CurStoredTarget = target;
            ScarletSound(LostbeltSoundAssets.Atom_Strike, target.Center);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.GetTexture();
            Texture2D sharp = LostbeltModAssets.Particle_SharpTear;
            Vector2 pos = Projectile.Center - Main.screenPosition;
            float rotFix = (Projectile.spriteDirection == -1 ? PiOver2 + PiOver4 : PiOver4);
            float rot = Projectile.rotation + rotFix;
            Vector2 rotPoint = tex.Size() / 2f;
            SpriteEffects se = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            int length = Projectile.oldPos.Length;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i] + rotFix;
                Color sharpColor = Color.Lerp(Color.RoyalBlue, Color.CornflowerBlue, progress);
                Color c = Color.Lerp(Color.RoyalBlue, Color.SkyBlue, progress);
                float scale = Lerp(1f, .45f, progress)*.8f;
                float opac = Lerp(1f, 0f, progress);
                Vector2 sharpScale = new Vector2(Lerp(.75f, .25f, progress), Lerp(1.35f, 0.35f, progress));
                SB.FastDraw(sharp, oldPos, sharpColor.ToAddColor(0) * opac*opac, Projectile.oldRot[i]+PiOver2, sharp.Size()/2f, sharpScale*1f,0);
                SB.FastDraw(sharp, oldPos, Color.White.ToAddColor(0)*.35f * opac*opac, Projectile.oldRot[i]+PiOver2, sharp.Size()/2f, sharpScale,0);
                SB.FastDraw(tex, oldPos+Main.rand.NextVector2Circular(3,3), c.ToAddColor() * .7f * opac, oldRot, rotPoint, scale * 1.1f, se);
                Color c2 = Color.Lerp(Color.DarkBlue, Color.DodgerBlue, progress);
                SB.FastDraw(tex, oldPos, c2.ToAddColor(20) * 1f * opac*opac, oldRot, rotPoint, scale, se);
            }
            SB.EnterShaderArea();
            Texture2D orb = LostbeltModAssets.Particle_KiraStar.Value;
            float scale2 = Projectile.scale * .5f;
            SB.FastDraw(orb, pos, Color.RoyalBlue, 0, orb.Size() / 2f, scale2, 0, 0);
            SB.FastDraw(orb, pos, Color.White, 0, orb.Size() / 2f, scale2*.9f, 0, 0);
            SB.EndShaderArea();
            for (int i = 0; i < 8; i++)
            {
                SB.FastDraw(tex, pos + (TwoPi / 8 * i).ToRotationVector2() * 1.5f+Main.rand.NextVector2Circular(3,3), Color.CornflowerBlue.ToAddColor(), rot, rotPoint, Projectile.scale, se);
            }
            SB.FastDraw(tex, pos, Color.White, rot, rotPoint, Projectile.scale, se);
            return false;
        }

    }
}
