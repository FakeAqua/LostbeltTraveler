using ContinentOfJourney;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.IDSets;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items
{
    public partial class LostbeltGlobalItems : GlobalItem
    {
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (ScarletItemIDSets.GrantsBoosterAfterSon[item.type] && item.IsExecutorWeapon() && DownedBossSystem.downedSon && player.Lostbelt().weaponUpgradePostSon)
                damage *= 10f;
            if (LostbeltConfigServer.Instance.ModWeaponDamageMult != 1f && item.damage > 0)
            {
                damage *= LostbeltConfigServer.Instance.ModWeaponDamageMult;
            }
        }
        public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
        {
            base.ModifyManaCost(item, player, ref reduce, ref mult);
        }

    }
}
