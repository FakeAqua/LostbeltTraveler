using ContinentOfJourney.Items.Accessories;
using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Projs.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Ranged
{
    public class PandorasBurger : LostbeltWeaponoutItemClass
    {
        public override EnumDamageClass Category => EnumDamageClass.Ranged;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Supporter);
        }
        public override void ExSD()
        {
            Item.damage = 114514;
            Item.useTime = Item.useAnimation = 40;
            Item.shoot = ProjectileType<PandorasBurgerHeldProj>();
            Item.shootSpeed = 16f;
            Item.Lostbelt().ItemBelongTo = EnumItemOwner.Supporter;
            Item.Lostbelt().OwnerName = "锯角";
        }
        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            //float ratios = LostbeltMethods.GetDamageBonusRatio(114514, );
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Burger, 5).
                AddIngredient<SwordmasterBadge>().
                AddIngredient<BullseyeBadge>().
                AddIngredient<ArchmageBadge>().
                AddIngredient<CounsellorBadge>().
                AddIngredient<ExecutorBadge>().
                AddIngredient<EssenceofBright>(30).
                AddIngredient<EssenceofDarkness>(30).
                AddIngredient<EssenceofDeath>(30).
                AddIngredient<EssenceofLife>(30).
                AddIngredient<EssenceofMatter>(30).
                AddIngredient<EssenceofNothingness>(30).
                AddIngredient<EssenceofTime>(30).
                AddTile(TileID.DemonAltar).
                Register();
        }
    }
}
