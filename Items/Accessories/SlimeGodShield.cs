using ContinentOfJourney.Buffs;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class SlimeGodShield : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.defense = 10;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.master = true;
            Item.Lostbelt().NotFinished = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ApplyDash(GetInstance<SlimeGodShieldDash>().Type);
            player.buffImmune[BuffType<IcarusBuff>()] = true;
            player.buffImmune[BuffID.ShadowFlame] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Frostburn2] = true;
            player.buffImmune[BuffID.OnFire] = true;
            player.buffImmune[BuffID.OnFire3] = true;
            player.buffImmune[BuffID.Burning] = true;
        }
    }
}
