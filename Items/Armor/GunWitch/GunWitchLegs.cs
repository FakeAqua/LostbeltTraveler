using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Methods;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Armor.GunWitch
{
    [AutoloadEquip(EquipType.Legs)]
    public class GunWitchLegs : LostbeltArmor
    {
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Gunwitch);
            base.SetStaticDefaults();
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.defense = 16;
        }
        public float Damage = .1f;
        public float MoveSpeed = .3f;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), MoveSpeed.ToPercent());
        public override void UpdateEquip(Player player)
        {
            player.GetDamage<ExecutorDamageClass>() += Damage;
            player.moveSpeed += MoveSpeed;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.DefenderMedal, 15).
                AddTile(TileID.WorkBenches).
                DisableDecraft().
                Register();
        }
    }
}
