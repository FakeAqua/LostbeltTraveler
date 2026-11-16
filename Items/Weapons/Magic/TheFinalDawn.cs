using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Magic
{
    public class TheFinalDawn : LostbeltWeapon
    {
        public override bool IsLoadingEnabled(Mod mod)
        {
            return false;
        }
        public override EnumDamageClass Category => EnumDamageClass.Magic;
        public override void SetStaticDefaults()
        {
            ScarletItemIDSets.IsHeldProjItem[Type] = true;
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Solar);
        }
        public override void ExSD()
        {
            Item.damage = 4560;
            Item.SetUpNoUseGraphicItem(true);
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.useTime = Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = .5f;
            Item.shootSpeed = 16f;
        }
        public override bool CanShoot(Player player)
        {
            return false;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }
}
