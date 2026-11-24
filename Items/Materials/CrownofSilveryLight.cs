using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Materials
{
    public class CrownofSilveryLight : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Materials;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateWhite);
            ItemID.Sets.ItemNoGravity[Type] = true;
        }
        public override void ExSD()
        {
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Blue;
            Item.material = true;
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.Lostbelt().noFancyFlavorTooltip = true;
            Item.value = Item.sellPrice(gold: 4, silver: 30);
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Type, out Texture2D itemTexture, out Rectangle itemFrame);
            Vector2 drawOrigin = itemFrame.Size() / 2;
            Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
            for (int i = 0; i < 16; i++)
                spriteBatch.Draw(itemTexture, drawPosition + (TwoPi / 16f * i).ToRotationVector2() * 1.2f, itemFrame, Color.White.ToAddColor(), rotation, drawOrigin, scale, SpriteEffects.None, 0);
            spriteBatch.Draw(itemTexture, drawPosition, itemFrame, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0);
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe(15).
                AddIngredient<FinalBar>().
                DisableDecraft().
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
