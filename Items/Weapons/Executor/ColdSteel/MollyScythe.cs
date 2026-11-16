using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Executor.ColdSteel
{
    public class MollyScythe : ExecutorWeaponClass
    {
        public override int ExecutionProgress => 16;
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.ColdSteel;
        public override void ExSSD()
        {
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
        }
        public override void ExSD()
        {
            Item.damage = 813;
            Item.useTime = Item.useAnimation = 30;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.Lostbelt().CanDrawIcon = false;
            Item.Lostbelt().CanDrawGhost = true;
            Item.shootSpeed = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 5;
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.Lostbelt().OwnerName = "初镜巡弱杂骸音";
        }
    }
}
