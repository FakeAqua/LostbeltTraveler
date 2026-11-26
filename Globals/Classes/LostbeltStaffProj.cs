using LostbeltTraveler.Globals.Database.Enums;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace LostbeltTraveler.Globals.Classes
{
    /// <summary>
    /// 一个通用的法杖射弹，或者说45°转角武器的通用射弹
    /// </summary>
    public class LostbeltStaffProj: LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public void SetUpHoldoutData(int gunType, float recoilPower, int lifeTime, Vector2 holdoutoffset)
        {
            ItemLifeTime = lifeTime;
            GunItemType = gunType;
            RecoilPower = recoilPower;
            HoldProjOffset = holdoutoffset;
        }
        /// <summary>
        /// 这把后坐力武器的”法杖“的ID
        /// <br>用于给予后坐力的贴图</br>
        /// </summary>
        public int GunItemType = ItemID.BeeGun;
        public ref float RecoilPower => ref Projectile.ai[0];
        public int ItemLifeTime
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }
        public Vector2 HoldProjOffset = Vector2.Zero;
        public override void SetDefaults()
        {
            Projectile.timeLeft = 10000;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.noEnchantmentVisuals = true;
        }
        public override void AI()
        {
            int duration = Owner.itemAnimationMax; // Define the duration the projectile will exist in frames

            // Reset projectile time left if necessary
            if (Projectile.timeLeft > duration)
            {
                Projectile.timeLeft = duration;
            }
            Projectile.velocity = (Main.MouseWorld - Owner.Center).ToSafeNormalize();
            Projectile.rotation = Projectile.velocity.ToRotation();
            int dir = Math.Sign(Main.MouseWorld.X - Owner.Center.X);
            Projectile.spriteDirection = dir;
            Owner.heldProj = Projectile.whoAmI; // Update the player's held projectile id
            Owner.ChangeDir(dir);
            Owner.ControlPlayerArm(Projectile.rotation);

            if (RecoilPower != 0)
            {
                float pullBack = RecoilPower;
                float animationProgress = 0.5f - Owner.itemTime / (float)Owner.itemTimeMax;
                if (animationProgress < .4f)
                    pullBack -= 2.75f * (float)Math.Pow((.6f - animationProgress) / .6f, 2f);
                Projectile.Center = Owner.MountedCenter + Projectile.rotation.ToRotationVector2() * pullBack;
            }
            else
                Projectile.Center = Owner.MountedCenter;
            Projectile.position.Y += Owner.gfxOffY;

        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? CanDamage()
        {
            return false;
        }
        public bool FirstFrame = false;
        public override bool PreDraw(ref Color lightColor)
        {
            if (!FirstFrame)
            {
                FirstFrame = true;
                return false;
            }
            Texture2D tex = TextureAssets.Item[GunItemType].Value;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 offset = HoldProjOffset * new Vector2(Owner.direction, 1);
            float drawRot = Projectile.rotation+MathHelper.PiOver4;
            if (Projectile.spriteDirection < 0)
            {
                drawRot += MathHelper.PiOver2;
            }
            drawPos += offset.RotatedBy(drawRot);
            Vector2 rotPoint = tex.Size() / 2f;
            SpriteEffects se = (Projectile.spriteDirection * Owner.gravDir) == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Main.spriteBatch.Draw(tex, drawPos, null, Color.White, drawRot, rotPoint, Projectile.scale, se, 0);
            return false;
        }
    }
}
