using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Accessories
{
    public abstract class GemYoyoRope : LostbeltItemClass
    {
        public virtual string StringName => "None";
        public override string AssetPath => AssetHandler.Equips;
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.LightPurple);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().gemYoyoStrings = StringName;
        }
    }
    public class YoyoRopeAmethyst : GemYoyoRope
    {
        public override string StringName => "Amethyst";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Amethyst, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
    public class YoyoRopeTopaz : GemYoyoRope
    {
        public override string StringName => "Topaz";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Topaz, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }


    public class YoyoRopeAmber : GemYoyoRope
    {
        public override string StringName => "Amber";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Amber, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }


    public class YoyoRopeSapphire : GemYoyoRope
    {
        public override string StringName => "Sapphire";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Sapphire, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
    public class YoyoRopeDiamond : GemYoyoRope
    {
        public override string StringName => "Diamond";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Diamond, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }

    }
    public class YoyoRopeOnyx : GemYoyoRope
    {
        public override string StringName => "Onyx";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient<Onyx>(10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }

    }
    public class YoyoRopeRuby : GemYoyoRope
    {
        public override string StringName => "Ruby";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Ruby, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }

    }
    public class YoyoRopeEmerald : GemYoyoRope
    {
        public override string StringName => "Emerald";
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.WhiteString).
                AddIngredient(ItemID.Emerald, 10).
                AddTile(TileID.TinkerersWorkbench).
                Register();
        }
    }
}
