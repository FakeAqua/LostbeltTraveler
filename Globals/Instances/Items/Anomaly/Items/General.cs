using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.Localization;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items.Anomaly.Items
{
    public class AnomalousGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return LostbeltConfigServer.Instance.AnomalousInfestation;
        }
        public override void SetDefaults(Item entity)
        {
            switch (entity.type)
            {
                case ItemID.NightsEdge:
                case ItemID.TrueNightsEdge:
                    entity.Lostbelt().drawBuffIconAndDetail = true;
                    break;
            }
        }
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.PocketMirror)
            {
                player.Lostbelt().pocketMirror = !hideVisual;
            }
        }
        public override void UpdateVanity(Item item, Player player)
        {
            if (item.type == ItemID.PocketMirror)
            {
                player.Lostbelt().pocketMirror = true;
            }
            base.UpdateVanity(item, player);
        }
        public override void UpdateArmorSet(Player player, string set)
        {
            base.UpdateArmorSet(player, set);
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.NightsEdge:
                case ItemID.TrueNightsEdge:
                    tooltips.CreateTooltipDirect(
                        ScarletTextSets.GenericText.CombineModName(
                            ScarletTextSets.GenericText.ApplyDoT.ToFormatValue("[ScarletDebuff/LostbeltTraveler/ForeverNightBuff]")),
                        Color.LightGray, Mod, "ApplyDoTName");
                    break;
            }
            if (item.type == ItemID.PocketMirror)
            {
                int index = tooltips.FindLineIndexLast("Tooltip");
                string path = Mod.GetLocalizationKey($"Database.PocketMirrorModiflication");
                tooltips.CreateTooltip(path, LineName: "PocketMirrorModiflication", color: Color.SkyBlue, index: index + 2, args: Main.LocalPlayer.Lostbelt().pocketMirror.ToString());
            }
        }
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            switch (item.type)
            {
                case ItemID.NightsEdge:
                case ItemID.TrueNightsEdge:
                    target.AddBuff(BuffType<ForeverNightBuff>(), 60);
                    break;
            }

            base.OnHitNPC(item, player, target, hit, damageDone);
        }
        public override void AddRecipes()
        {
            base.AddRecipes();
        }
    }
}
