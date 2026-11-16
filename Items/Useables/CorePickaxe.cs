using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Graphics.Particles;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Instances.Items;
using LostbeltTraveler.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Useables
{
    public class CorePickaxe : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Useables;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.DamageType = DamageClass.Melee;
            Item.useTime = Item.useAnimation = 15;
            Item.scale *= 1.2f;
            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.axe = 200.SetAxePower();
            Item.rare = ItemRarityID.Red;
            Item.useTurn = true;
            Item.knockBack = 12f;
            Item.pick = 230;
            Item.tileBoost += 5;
            Item.damage = 100;
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {

            if (player.itemAnimation % 5 == 0)
            {
                for (int i = 0; i < 5; i++)
                    new ShinyOrbParticle(new Vector2(hitbox.X + hitbox.Width / 2, hitbox.Y) + Main.rand.NextVector2Square(0, hitbox.Width / 2), RandVelTwoPi(1f, 2.4f), Color.Orange, 40, 0.45f, affactedByGravity: true).Spawn();
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddRecipeGroup(LostbeltRecipeGroup.AnyLunarPickaxe).
                AddIngredient<DisasterBar>(10).
                AddTile(TileID.LunarCraftingStation).
                Register();

        }
    }
}
