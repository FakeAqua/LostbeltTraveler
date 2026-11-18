using ContinentOfJourney.Buffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class SacarbWings : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(180, 9f, 2.5f);
            base.SetStaticDefaults();
        }

        public override void ExSD()
        {
            Item.defense = 50;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.accessory = true;
            Item.master = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //直接调用光女的无限飞饰品
            player.Lostbelt().infiniteFlightTime = true;
            player.longInvince = true;
            player.noFallDmg = true;
            player.noKnockback = true;
            player.buffImmune[BuffType<IcarusBuff>()] = true;
            player.buffImmune[BuffType<MonarchButterflyBuff>()] = true;
            player.buffImmune[BuffType<PlagueBuff>()] = true;
            player.buffImmune[BuffType<DivineFireBuff>()] = true;
        }
        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.35f;
            maxCanAscendMultiplier = 1.2f;
            maxAscentMultiplier = 3f;
            constantAscend = 0.235f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            speed = 8.25f;
            acceleration = 1.2f;
        }
    }
}
