using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Useables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.BestiaryGirl)
            {
                shop.ToCustomValue<RuShiWoWen>(0, 30, 0, 0);
            }
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.ToCustomValue<AxeCharm>(0, 5, 0, 0);
            }
            if (shop.NpcType == NPCID.Wizard)
            {
                shop.ToCustomValue<CrystallizedLore>(0, 60, 0, 0);
            }
            if (shop.NpcType == NPCID.Clothier)
            {
                shop.ToCustomValue<CombatSlot>(0, 50, 0, 0);
            }
        }

        public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
        {
            bool isMale = ScarletNPCIDSets.IsMaleNPC[npc.type];
            bool isFemale = ScarletNPCIDSets.IsFemaleNPC[npc.type];
            Player player = Main.LocalPlayer;
            if (!player.Lostbelt().loveRing)
                return;
            foreach (var item in items)
            {
                if (item is null || item.IsAir)
                    continue;
                float modify = 0.60f;

                if (player.Male && isFemale)
                    modify -= 0.05f;
                if (!player.Male && isMale)
                    modify -= 0.05f;
                item.shopCustomPrice = (int)((item.shopCustomPrice ?? item.GetStoreValue()) * modify);
            }
        }
    }
}
