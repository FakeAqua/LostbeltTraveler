using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Projs.Pets;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Pets
{
    public class DracoItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<DracoProj>();
        public override int PetBuffType => BuffType<DracoBuff>();
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public IReadOnlyList<TooltipLine> CacheTooltipList = null;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            CacheTooltipList = tooltips;
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            List<SideTextPanelData> list = [];
            string[] textBundle = this.GetLocalizedValue("FlavorTooltip").Split('\n');
            for (int i = 0; i < textBundle.Length; i++)
            {
                list.Add(new(new(Mod, "FlavorTooltip" + i, textBundle[i]), 1));
            }
            int y2 = SideTextPanelMethods.DrawSideTextEdgePanel(lines, list, x, y, Color.DarkRed, true, .85f, Color.Black * .24f, Color.DarkRed);
            return base.PreDrawTooltip(lines, ref x, ref y);
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Silk, 15).
                AddIngredient(ItemID.JungleRose).
                DisableDecraft().
                AddTile(TileID.Loom).
                Register();
        }
    }
}
