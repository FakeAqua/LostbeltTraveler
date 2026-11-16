using ContinentOfJourney.Tiles;
using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class DarkenRockFish : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
        }
        public override void ExSD()
        {
            //重设定了。这个是真的会影响hitbox的
            Item.width = Item.height = 42;
            Item.DamageType = DamageClass.Melee;
            Item.damage = 1203;
            Item.useTime = Item.useAnimation = 30;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4;
            Item.autoReuse = true;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
            Item.hammer = 200;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            base.MeleeEffects(player, hitbox);
        }
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(player, target, hit, damageDone);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FishingCoinEmerial>(30).
                AddTile(TileType<FinalAnvil>()).
                Register();

        }

    }
}
