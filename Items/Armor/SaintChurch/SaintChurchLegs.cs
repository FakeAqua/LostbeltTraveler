using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.SaintChurch
{
    [AutoloadEquip(EquipType.Legs)]
    public class SaintChurchLegs : LostbeltArmor
    {
        public float Damage = .02f;
        public float MoveSpeed = .3f;
        public float RunSpeedAcceleration = 1.25f;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), MoveSpeed.ToPercent(), RunSpeedAcceleration + "x");
        public override void ExSD()
        {
            Item.defense = 10;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
            player.moveSpeed += MoveSpeed;
            player.runAcceleration *= RunSpeedAcceleration;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DeepBar>(4).
                AddIngredient(ItemID.Silk, 4).
                AddTile(TileID.MythrilAnvil).
                Register();
        }
    }
}
