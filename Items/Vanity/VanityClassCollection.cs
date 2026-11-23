using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Methods.SideTextPanel;
using LostbeltTraveler.Globals.Players.VanitySets;
using LostbeltTraveler.Items.Useables;
using LostbeltTraveler.Rarity.RarityDrawHandler;
using LostbeltTraveler.Rarity.RarityParticles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Vanity
{
    public struct VanityData(Color glowColor, Color edgeColor, Color mainColor, float glowMult = 1)
    {
        public Color GlowColor = glowColor;
        public Color EdgeColor = edgeColor;
        public Color MainColor = mainColor;
        public float GlowMult = glowMult;
    }
    public class VanityEffectClass : ModRarity
    {
        public override Color RarityColor => Color.Red;
        public static List<RaritySparkle> RaritySparkles = [];
        public static List<RaritySparkle> FlavorSparkles = [];
        public static void DrawItemName(DrawableTooltipLine line, VanityData data, Color particleColor1, Color particleColor2)
        {
            PostDrawRarity(ref RaritySparkles, line, particleColor1, particleColor2);
            RarityDrawHelper.DrawCustomTooltipLine(line, data.GlowColor, data.EdgeColor, data.MainColor, data.GlowMult);
        }
        public static void DrawFlavorTooltipName(DrawableTooltipLine line, VanityData data, Color particleColor1, Color particleColor2)
        {
            PostDrawRarity(ref FlavorSparkles, line, particleColor1, particleColor2, true);
            RarityDrawHelper.DrawCustomTooltipLine(line, data.GlowColor, data.EdgeColor, data.MainColor, data.GlowMult);

        }
        public static void DrawMisc(DrawableTooltipLine line, VanityData data, Color particleColor1, Color particleColor2)
        {
            RarityDrawHelper.DrawCustomTooltipLine(line, data.GlowColor, data.EdgeColor, data.MainColor, 0f);
        }

        public static void PostDrawRarity(ref List<RaritySparkle> particleList, DrawableTooltipLine tooltipLine, Color c, Color c2, bool slowdown = false)
        {
            //在这里手动创建新的粒子，然后我们再将其添加进需要的表单内
            Vector2 textSize = tooltipLine.Font.MeasureString(tooltipLine.Text);
            if (Main.rand.NextBool(10))
            {
                float scale = Main.rand.NextFloat(0.30f * 0.5f, 0.30f) * 1.2f;
                int lifetime = 160;
                Vector2 position = GetParticlePosition(tooltipLine);
                Vector2 velocity = -Vector2.UnitY * Main.rand.NextFloat(0.25f, 0.55f) * (1 + slowdown.ToInt() * -0.75f);
                RarityShinyOrb rarityShinyOrb = new(position, velocity, RandLerpColor(c, c2), lifetime, scale);
                particleList.Add(rarityShinyOrb);
            }
            //最后更新他。
            RarityDrawHelper.UpdateTooltipParticles(tooltipLine, ref particleList);
        }
        public static Vector2 GetParticlePosition(DrawableTooltipLine line)
        {
            Vector2 textSize = line.Font.MeasureString(line.Text);
            return Main.rand.NextVector2FromRectangle(new(-(int)(textSize.X * 0.5f), -(int)(textSize.Y * 0.5f), (int)textSize.X, (int)(textSize.Y * 0.35f)));
        }
    }
    public abstract class AccVanityItem : LostbeltItemClass
    {
        public virtual VanityData VanityData { get; set; }
        public virtual Color ParticleColor1 => Color.White;
        public virtual Color ParticleColor2 => Color.White;
        public static string ItemPath => "LostbeltTraveler/Assets/Texture/Items/Vanity/";
        public virtual bool DrawFlavorTooltip => false;
        public virtual string VanityName => string.Empty;
        public string VanityPrefix => ItemPath + VanityName;
        public override string AssetPath => AssetHandler.Vanity;
        public virtual bool HasFlavorTooltip => true;
        public override bool AllowPrefix(int pre) => false;
        public override bool ConsumeItem(Player player) => false;
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.rare = RarityType<VanityEffectClass>();
            Item.vanity = true;
            Item.accessory = true;
            Item.Lostbelt().CanDrawIcon = true;
            ExSD();
        }
        public override void Load()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            EquipLoader.AddEquipTexture(Mod, $"{VanityPrefix}Head", EquipType.Head, this);
            EquipLoader.AddEquipTexture(Mod, $"{VanityPrefix}Body", EquipType.Body, this);
            EquipLoader.AddEquipTexture(Mod, $"{VanityPrefix}Legs", EquipType.Legs, this);
            ExLoad();

        }

        public virtual void ExLoad()
        {
        }
        public override void SetStaticDefaults()
        {

            LostbeltList.VanityItemDictionary.Add(Type, GetType().Name);
            if (Main.netMode == NetmodeID.Server)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
            ExSSD();
        }
        public virtual void ExSSD() { }
        public static int FlavorTooltipIndex = -1;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            ExModifyTooltip(tooltips);
        }
        public virtual void ExModifyTooltip(List<TooltipLine> tooltips) { }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
                player.GetModPlayer<ScarletVanityPlayer>().accVanityID = Type;
        }
        public float FirstLineY = -1;
        public IReadOnlyList<TooltipLine> CacheTooltipList = null;
        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<ScarletVanityPlayer>().accVanityID = Type;
        }
        public override bool PreDrawTooltip(ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
        {
            if (HasFlavorTooltip)
            {
                string mainText = this.GetLocalizationKey("FlavorTooltip").ToLangValue();
                List<SideTextPanelData> data = 
                [
                    new(new TooltipLine(Mod, "DetailName", mainText) { OverrideColor = VanityData.MainColor }, 1),
                ];
                mainText = this.GetLocalizationKey("DetailTooltip").ToLangValue();
                string[] textBundle = mainText.Split('\n');
                for (int i = 0; i < textBundle.Length; i++)
                {
                    data.Add(new(new(Mod, "DetailTooltip" + i, textBundle[i]) { OverrideColor = VanityData.MainColor }, 1));
                }
                SideTextPanelMethods.DrawSideTextEdgePanelWithTitleEdge(lines, data, x, y,VanityData.EdgeColor, VanityData.EdgeColor, true, 0.85f, 
                    Color.Lerp(VanityData.GlowColor, Color.Black, .4f) * 1f, Color.Lerp(VanityData.EdgeColor,Color.White,.3f) ,useShinyBorder:true);
            }
            return base.PreDrawTooltip(lines, ref x, ref y);
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return true;
        }
        public override void PostDrawTooltipLine(DrawableTooltipLine line)
        {
            //通常情况下，物品不可能没有名字，而物品名称通常都在第一行，所以可以用这个来记录第一行的坐标
            if (line.IsItemName())
            {
                VanityEffectClass.DrawItemName(line, VanityData, ParticleColor1, ParticleColor2);
            }
            if ((line.Name == "Vanity" || line.Name == "Equipable") && line.Mod == "Terraria")
            {
                if (Item.Lostbelt().ItemBelongTo != Globals.Database.Enums.EnumItemOwner.Donator)
                    VanityEffectClass.DrawMisc(line, VanityData, ParticleColor1, ParticleColor2);
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<FreehandFrame>().
                DisableDecraft().
                Register();
        }
    }
}
