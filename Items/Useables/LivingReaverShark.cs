using ContinentOfJourney.Tiles;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class LivingReaverShark : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Life);
        }
        public override void ExSD()
        {
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.pick = 250;
            Item.tileBoost += 5;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 888;
            Item.knockBack = .5f;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = Item.useAnimation = 15;
            Item.UseSound = SoundID.Item1;
        }
        public override void HoldItem(Player player)
        {
            player.AddBuff(BuffID.Regeneration, 2);
            player.AddBuff(BuffID.WellFed3, 2);
            player.AddBuff(BuffID.Ironskin, 2);
            player.AddBuff(BuffID.Swiftness, 2);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            base.MeleeEffects(player, hitbox);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FishingCoinEmerial>(30).
                AddTile(TileType<FountainofLife>()).
                Register();

        }
    }
}
