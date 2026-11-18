using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Items.Material;
using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Projs.General;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class SteadyBreath : LostbeltItemClass
    {
        public float Damage = .20f;
        public int Crit = 10;
        public static float ExtraDamage = 1.1f;
        public static float ChanceToCrit = .35f;
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), Crit + "%", ExtraDamage + "x", ChanceToCrit.ToPercent());
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.VortexFance);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Purple);

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<RangedDamageClass>() += Damage;
            player.GetCritChance<RangedDamageClass>() += Crit;
            player.Lostbelt().preciousTargetLevel = 2;
            if (player.HeldItem.IsLegal() && player.HeldItem.DamageType.CountsAsClass<RangedDamageClass>() && !hideVisual)
            {
                if (!player.IsInInventory())
                    player.Lostbelt().cursorID = 1;
                if (player.whoAmI == Main.myPlayer && !player.HasProj<PreciousTargetCross>())
                {
                    Projectile proj = Projectile.NewProjectileDirect(player.GetSource_Accessory(Item), Main.MouseWorld, Vector2.Zero, ProjectileType<PreciousTargetCross>(), 0, 0, player.whoAmI);
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<PreciousTarget>().
                AddIngredient<BullseyeBadge>().
                AddIngredient<LivingBar>(15).
                AddTile<FinalAnvil>().
                Register();
        }
    }
}
