using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core.Lightning;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Melee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace LostbeltTraveler.Items.Weapons.Melee
{
    public class RaceAgainstTime : LostbeltWeapon
    {
        public override EnumDamageClass Category => EnumDamageClass.Melee;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.Eternity);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Purple);
            Item.SetUpNoUseGraphicItem(false);
            Item.damage = 333;
            Item.useTime = Item.useAnimation = 5;
            Item.UseSound = SoundID.Item91 with { Volume = 0.7f, Pitch = 0.4f };
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 1;
            Item.shootSpeed = 16f;
            Item.shoot = ProjectileType<RaceAgainstTimeProj>();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            //这里直接用MainMW应该没问题，多人同步后面再看看
            int c = Main.rand.Next(1, 4);
            for (int i = 0; i < c; i++)
            {
                Vector2 direction = player.ToMouseVector2().ToRandVelocity(ToRadians(7));
                Projectile proj = Projectile.NewProjectileDirect(source, position, direction * Item.shootSpeed, type, damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<EternalBar>(15).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
}
