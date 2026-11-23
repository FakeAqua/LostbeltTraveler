using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Systems.Conditions;
using LostbeltTraveler.Projs.Executor.Thrown;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Weapons.Executor.Thrown
{
    public class GaiaStriker : ExecutorWeaponClass
    {
        public override ExecutorWeaponType ExecutorWeaponType => ExecutorWeaponType.Throw;
        /// <summary>
        /// 调整这个以修改普通攻击血弹的爆裂次数
        /// 需注意的是血弹总会有两枚用于治疗玩家
        /// </summary>
        public static int BloodBulletCount = 8;
        /// <summary>
        /// 处决进程要求实际上为18次，因为每个非治疗性的血弹都会叠加
        /// </summary>
        public override int ExecutionProgress => 18 * (BloodBulletCount - 2);
        /// <summary>
        ///<para>盖亚重锤普通攻击时，每个爆开的血弹提供的治疗量</para>
        ///<para>处决攻击下默认取两倍</para>
        /// </summary>
        public static int BloodBulletHealNormal = 1;
        /// <summary>
        /// 盖亚宝箱爆开时，每个爆开的血弹提供的治疗量
        /// </summary>
        public static int BloodBulletHealLootChest = 100;
        /// <summary>
        /// 盖亚重锤仆从死亡时，每个爆开的血弹提供的治疗量
        /// </summary>
        public static int BloodBulletHealMinionDead = 5;
        public static int BloodBulletHealMinionDeadEarly = 10;
        public override void ExSSD()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.ScarletRed);
        }
        public override void ExSD()
        {
            Item.width = Item.height = 16;
            Item.damage = 78;
            Item.SetUpNoUseGraphicItem();
            Item.Lostbelt().drawFlavorTooltip = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
            Item.shootSpeed = 22;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 3;
            Item.UseSound = LostbeltSoundAssets.Gaia_Toss with { MaxInstances = 0, Pitch = -.36f, PitchVariance = .1f, Volume = .30f, Variants = [2] };
            Item.useTime = Item.useAnimation = 62;
            Item.shoot = ProjectileType<GaiaStrikerProj>();
        }
        public override bool CanShoot(Player player)
        {
            return !player.HasProj<GaiaStrikerHeldProj>();
        }
        public override void ExModifyTooltips(List<TooltipLine> tooltips)
        {
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Type, out Texture2D itemDrawFrame, out Rectangle frame);
            Vector2 ori = frame.Size() / 2f;
            Vector2 pos = Item.Bottom - Main.screenPosition - new Vector2(0, ori.Y);
            for (int i = 0; i < 16; i++)
                spriteBatch.Draw(itemDrawFrame, pos + (TwoPi / 16f * i).ToRotationVector2() * 2, null, Color.Crimson.ToAddColor(), rotation, ori, scale, 0, 0);
            spriteBatch.Draw(itemDrawFrame, pos, null, Color.White, rotation, ori, scale, 0, 0);
            return false;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            return base.PreDrawTooltipLine(line, ref yOffset);
        }
        public override void AddRecipes()
        {

            CreateRecipe().
                AddIngredient<DreamingLight>().
                AddIngredient<TheJudgement>().
                AddIngredient(ItemID.BrokenHeroSword).
                AddTile(TileID.MythrilAnvil).
                AddCondition(LostbeltCraftingConditions.AnyAfterCrafting).
                Register();
        }
    }
}
