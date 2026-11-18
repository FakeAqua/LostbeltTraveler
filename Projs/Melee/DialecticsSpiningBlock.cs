using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Items.Weapons.Melee;
using Terraria;

namespace LostbeltTraveler.Projs.Melee
{
    public class DialecticsSpiningBlock : LostbeltFriendlyProj
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override string Texture => LostbeltModAssets.Specific_AimLabBox.Path;
        public ref float Timer => ref Projectile.ai[0];
        public int MountedTargetIndex
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public bool ShouldDisapper = false;
        public override void ExSD()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.localNPCHitCooldown = 60;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.extraUpdates = 0;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.light = 0.5f;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {

            Projectile.rotation += ToRadians(1f);
            if (ShouldDisapper)
            {
                Projectile.scale = Lerp(Projectile.scale, 0f, 0.2f);
                Projectile.Opacity = Projectile.scale;
                if (Projectile.scale <= 0.02f)
                    Projectile.Kill();
                return;
            }
            else
            {
                Projectile.timeLeft = 300;
                Projectile.scale += 0.1f;
                Projectile.scale = Clamp(Projectile.scale, 0f, 1f);
                ShouldDisapper = !(Owner.active && Owner.HeldItem.type == ItemType<DialecticsThrown>());
            }
            if (Projectile.GetTargetSafe(out NPC target, false))
            {
                Projectile.Center = Vector2.Lerp(Projectile.Center, target.Center, 0.5f);
                target.Lostbelt().Dialectics_Mark = true;
            }
            else
            {
                //单位不合法的时候立刻处死出去
                ShouldDisapper = true;
            }
        }
        public override bool PreKill(int timeLeft)
        {
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            return base.PreKill(timeLeft);
        }
        public override bool? CanDamage() => false;
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = Projectile.ApplyDirectionOverride(target);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            Texture2D cube = LostbeltModAssets.Particle_ShinySquareSplit.Value;
            Texture2D ring = LostbeltModAssets.Particle_RingShiny.Value;
            Texture2D kiraStar = LostbeltModAssets.Particle_KiraStar.Value;
            SB.EnterShaderArea();
            for (int i = 0; i < 4; i++)
            {
                QuickDraw(PiOver2 * i, 0.41f);
            }
            void QuickDraw(float rotPlus, float scale)
            {
                SB.Draw(cube, drawPos, null, Color.RoyalBlue, Projectile.rotation + rotPlus, cube.ToOrigin(), Projectile.scale * scale, 0, 0);
            }
            SB.EndShaderArea();
            return false;
        }
        public void DrawCube(Color color, float scale)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 dir = Projectile.rotation.ToRotationVector2();
            Tex2DWithPath cube = LostbeltModAssets.Texture_WhiteCube;
            SB.Draw(cube.Value, drawPos + dir.RotatedBy(PiOver2) * 30f, null, color, Projectile.rotation, cube.Value.Size() / 2f, new Vector2(0.2f, 2f) * Projectile.scale * 0.8f * scale, 0, 0);
            SB.Draw(cube.Value, drawPos + dir.RotatedBy(Pi) * 30f, null, color, Projectile.rotation + PiOver2, cube.Value.Size() / 2f, new Vector2(0.2f, 2f) * Projectile.scale * 0.8f * scale, 0, 0);
            SB.Draw(cube.Value, drawPos + dir.RotatedBy(PiOver2) * -30f, null, color, Projectile.rotation, cube.Value.Size() / 2f, new Vector2(0.2f, 2f) * Projectile.scale * 0.8f * scale, 0, 0);
            SB.Draw(cube.Value, drawPos + dir.RotatedBy(Pi) * -30f, null, color, Projectile.rotation + PiOver2, cube.Value.Size() / 2f, new Vector2(0.2f, 2f) * Projectile.scale * 0.8f * scale, 0, 0);

        }
    }
}
