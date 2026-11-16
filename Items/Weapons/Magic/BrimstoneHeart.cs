using LostbeltTraveler.Assets.Registers;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class BrimstoneHeart : LostbeltWeaponoutItemClass
    {
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public bool StopKilling = false;
        public override void ExSSD()
        {
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Donator);
        }
        public override void ExSD()
        {
            Item.damage = 124;
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Donator;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
            Item.Lostbelt().drawBuffIconAndDetail = true;
            Item.Lostbelt().OwnerName = "苏利";
            Item.knockBack = 2;
            Item.mana = 21;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = Item.useAnimation = 60;
            Item.shoot = ProjectileType<BrimstoneHeartHeldProj>();
            Item.shootSpeed = 16f;
            Item.crit = 20;
        }
        public override void UpdateInventory(Player player)
        {
            if (player.name == Item.Lostbelt().OwnerName)
            {
                Item.SetNameOverride("永恒燃烧的爱");
            }
            base.UpdateInventory(player);
        }
        public override bool CanRightClick()
        {
            return Main.keyState.PressingShift();
        }
        public override void RightClick(Player player)
        {
            player.Lostbelt().brimstoneHeartKilling = !player.Lostbelt().brimstoneHeartKilling;
        }
        public override bool ConsumeItem(Player player) => false;
        public override void SaveData(TagCompound tag)
        {
            base.SaveData(tag);
        }
        public override void LoadData(TagCompound tag)
        {
            base.LoadData(tag);
        }
        public override void HoldItem(Player player)
        {
            base.HoldItem(player);
            if (!player.Lostbelt().brimstoneHeartKilling)
            {
                player.buffImmune[BuffID.Regeneration] = true;
                if (player.lifeRegen > 0)
                    player.lifeRegen /= 2;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.ObsidianRose).
                AddIngredient(ItemID.SoulofSight, 5).
                AddIngredient(ItemID.SoulofFright, 5).
                AddIngredient(ItemID.SoulofMight, 5).
                AddTile(TileID.MythrilAnvil).
            Register();
        }
    }
}
