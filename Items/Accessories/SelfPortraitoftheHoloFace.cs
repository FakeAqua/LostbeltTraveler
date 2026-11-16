using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class SelfPortraitoftheHoloFace : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public static float DotDamageMult = 1;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DotDamageMult.ToPercent());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Matter);
            Type.ShimmerEach<SelfPortraitoftheBleachingOne>();
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().dotMult += DotDamageMult;
            player.Lostbelt().badLifeRegenMult *= .5f;
            player.Lostbelt().selfPortraitType = -2;
        }
    }
}
