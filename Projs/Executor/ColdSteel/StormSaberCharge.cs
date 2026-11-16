using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;

namespace LostbeltTraveler.Projs.Executor.ColdSteel
{
    public class StormSaberCharge : LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public enum State
        {
            Idle,
            Attack
        }
        public ref float Timer => ref Projectile.ai[0];
        public float TimeRatios => Clamp(Timer / (Projectile.MaxUpdates * 5f), 0f, 1f);
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(24);
        }
        public override void ExSD()
        {
            Projectile.extraUpdates = 10;
            Projectile.width = Projectile.height = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.Opacity = 1f;
            Projectile.SetupImmnuity(-1);
        }
        public override void ProjAI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.timeLeft = 2;
            //Projectile.velocity = Vector2.Zero;
            Projectile.Opacity = Lerp(Projectile.Opacity, 0, .01f);
            if (Projectile.IsOutScreen())
                return;
            //ECSParticle.LiliesFire(Projectile.Center.ToRandCirclePos(4), Projectile.velocity / 8f, RandLerpColor(Color.WhiteSmoke, Color.Silver), 45, RandRotTwoPi, 1, 0.3f, true, BlendState.Additive);

        }
        public override void OnFirstFrame()
        {
            base.OnFirstFrame();
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!Projectile.Lostbelt().FirstFrame)
                return false;
            SB.EnterShaderArea();
            Texture2D tex = LostbeltModAssets.Texture_RarityGlow.Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Vector2 trailOrig = new Vector2(tex.Width / 2f, 20);
            Vector2 targetTrailPos = drawPos - Vector2.UnitX.RotatedBy(Projectile.rotation) * 1f;
            Vector2 scale = new Vector2(3f, 14f * TimeRatios);
            float rot = Projectile.rotation + PiOver2;
            SB.Draw(tex, targetTrailPos, null, Color.White * 8f * Projectile.Opacity, rot, trailOrig, scale, 0, 0);
            SB.Draw(tex, targetTrailPos, null, Color.White * .8f * Projectile.Opacity, rot, trailOrig, scale * new Vector2(0.85f, 1.05f), 0, 0);

            Texture2D noiseTex = LostbeltModAssets.Noise_Aura.Value;
            targetTrailPos = drawPos - Vector2.UnitX.RotatedBy(Projectile.rotation) * 800f - Vector2.UnitY * 10f * Projectile.direction;
            LostbeltMethods.ApplyAlphaCut(new Vector4(.2f, .3f, 0, 0), new Vector2(-Main.GlobalTimeWrappedHourly * 0.5f, 0f), new Vector2(2.5f, 1f), Color.White);
            SB.Draw(noiseTex, targetTrailPos, null, Color.White, Projectile.rotation, trailOrig, new Vector2(6f, .13f), 0, 0);
            noiseTex = LostbeltModAssets.Noise_Misc.Value;
            SB.Draw(noiseTex, targetTrailPos, null, Color.White, Projectile.rotation, trailOrig, new Vector2(6f, .13f), 0, 0);

            SB.EndShaderArea();
            return false;
        }
    }
}
