using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.General
{
    public class HeadsplosionBoom : LostbeltProjs
    {
        public override EnumDamageClass Category => EnumDamageClass.Typeless;
        public override string Texture => LostbeltModAssets.InvisAsset.Path;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Projectile.penetrate = -1;
            Projectile.timeLeft = 40;
            Projectile.SetupImmnuity(40);
            Projectile.width = Projectile.height = 80;
        }
        public override void ProjAI()
        {
            base.ProjAI();
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetCrit();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Ichor, GetSeconds(2));
        }
    }
}
