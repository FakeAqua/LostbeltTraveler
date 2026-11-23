using LostbeltTraveler.Core;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using LostbeltTraveler.Items.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace LostbeltTraveler.Items.Accessories
{
    public class Samsara : LostbeltItemClass
    {
        public float DR = .05f;
        public int MaxLifeRegenSpeed = 16;
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(DR.ToPercent(), MaxLifeRegenSpeed.ToLifeRegenFormat());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Final);
        }
        public override void ExSD()
        {
            Item.defense = 30;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Yellow);
            Item.Lostbelt().drawBuffIconAndDetail = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.aggro += 1500;
            player.endurance += DR;
            player.noKnockback = true;
            float ratios = Utils.GetLerpValue(10, player.statLifeMax2, player.statLife, true);
            int life = (int)(MaxLifeRegenSpeed * (1 - ratios));
            if (life < 2)
                life = 2;
            player.Lostbelt().lifeRegenPool += life;
            if (player.statLife > player.statLifeMax2 * 0.25f)
            {
                player.hasPaladinShield = true;
                if (player.whoAmI != Main.myPlayer && player.miscCounter % 10 == 0)
                {
                    int myPlayer = Main.myPlayer;
                    if (Main.player[myPlayer].team == player.team && player.team != 0)
                    {
                        float teamPlayerXDist = player.position.X - Main.player[myPlayer].position.X;
                        float teamPlayerYDist = player.position.Y - Main.player[myPlayer].position.Y;
                        if ((float)Math.Sqrt(teamPlayerXDist * teamPlayerXDist + teamPlayerYDist * teamPlayerYDist) < 800f)
                            Main.player[myPlayer].AddBuff(BuffID.PaladinsShield, 20);
                    }
                }
            }
            player.ApplyDash(ScarletContent.DashType<SamsaraShieldDash>());
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HeroShield).
                AddIngredient(ItemID.FragmentSolar, 25).
                AddIngredient<CrownofSilveryLight>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
