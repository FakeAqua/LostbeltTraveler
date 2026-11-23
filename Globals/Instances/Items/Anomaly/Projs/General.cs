using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Globals.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items.Anomaly.Projs
{
    public partial class AnomalousGlobalProjs : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return LostbeltConfigServer.Instance.AnomalousInfestation;
        }
        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (projectile.type)
            {
                case ProjectileID.NightsEdge:
                case ProjectileID.TrueNightsEdge:
                    target.AddBuff(BuffType<ForeverNightBuff>(), 60);
                    break;
            }
        }
    }
}
