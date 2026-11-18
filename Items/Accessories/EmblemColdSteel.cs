using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class EmblemColdSteel : LostbeltItemClass
    {
        public float Damage = .10f;
        public int Crit = 10;
        public static float MaxDamageMult = .75f;
        public override void SetStaticDefaults()
        {
            Type.ShimmerTo(ItemType<EmblemFirearm>());
        }
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", MaxDamageMult.ToPercent());
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HeldItem.CheckExecuteTypes(ExecutorWeaponType.ColdSteel))
            {
                player.GetDamage<ExecutorDamageClass>() += Damage;
                player.GetCritChance<ExecutorDamageClass>() += Crit;
            }
        }
    }
}
