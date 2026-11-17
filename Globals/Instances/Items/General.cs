using ContinentOfJourney.Items;
using ContinentOfJourney.Items.Whips;
using LostbeltTraveler.Buffs.Debuffs;
using LostbeltTraveler.Buffs.Status;
using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Methods;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Armor.DragonHunter;
using LostbeltTraveler.Items.Weapons.Executor.Thrown;
using MonoMod.Core.Platforms;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items
{
    public partial class LostbeltGlobalItems : GlobalItem
    {
        public override bool InstancePerEntity => true;
        public bool EnableCritDamage = false;
        public bool CanDrawIcon = false;
        public float CritsDamageBonus = 0f;
        private int GhostTimer = 0;
        private int GhostFrame = 0;
        public bool EnableExecutorVersion = false;
        public bool CanDrawGhost = false;
        public int ExecutionProj = -1;
        public bool ForceTacticalExecution = false;
        public bool ForceAutomaticExecution = false;
        public bool NotFinished = false;
        //控制purePrism的运动
        public float tintIconDrawLerp = 0;
        public bool purePrismLerpIn = false;
        public bool purePrismLerpOut = false;
        public bool setTintIcon = false;
        public bool isShivering = false;
        public EnumItemOwner ItemBelongTo = EnumItemOwner.None;
        public float simpleImmersiveBackpackValue = 1f;
        public float simpleImmersiveBackpackValueAlt = 1f;
        public bool DrawFloatingTextBox = false;
        public bool borderlandWeapon = false;
        /// <summary>
        /// shorthand
        /// </summary>
        public override void SetStaticDefaults()
        {
            LostbeltMethods.ShimmerEach(ItemID.PaladinsHammer, ItemID.PaladinsShield);
            LostbeltMethods.ShimmerTo(ItemType<DeathTolls>(), ItemType<DestroyerBlade>());
            LostbeltMethods.ShimmerTo(ItemType<Neptune>(), ItemType<DeathTolls>());

        }

        public Player LocalPlayer => Main.LocalPlayer;
        public void HandleLerpValue(Item item, Vector2 position, float scale)
        {
            if (!LostbeltConfigClient.Instance.SimpleImmersiveInventory)
            {
                simpleImmersiveBackpackValue = 1f;
                simpleImmersiveBackpackValueAlt = 1f;
            }
            bool hasImmersiveInventory = ModLoader.HasMod("ImmersiveInventory");
            bool isHovering = MouseHoveringAnySlot(position, scale);
            float maxScale = isHovering ? 1.35f : 1f;
            //开启沉浸背包mod下会禁用这一条的更改
            if (isHovering && hasImmersiveInventory)
                simpleImmersiveBackpackValue = 1f;
            simpleImmersiveBackpackValue = Lerp(simpleImmersiveBackpackValue, maxScale, 0.15f);
            //这个用于控模组图标的放缩
            simpleImmersiveBackpackValueAlt = Lerp(simpleImmersiveBackpackValueAlt, maxScale, 0.15f);
        }

        private bool MouseHoveringAnySlot(Vector2 slotPos, float drawScale)
        {
            float hitRadius = 28f * Main.inventoryScale;
            return Vector2.Distance(Main.MouseScreen, slotPos) < hitRadius;
        }
        public override void RightClick(Item item, Player player)
        {
            base.RightClick(item, player);
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            var usPlayer = player.Lostbelt();
            if (usPlayer.terraRecipe)
            {
                if (item.type < VanillaMaxItem)
                {

                    string fullName = ItemID.Search.GetName(item.type);
                    if (LostbeltList.LegalFoodListName.Contains(fullName))
                    {
                        if (!usPlayer.terrariaRecipeEatenList.Contains(fullName))
                        {
                            usPlayer.terrariaRecipeEatenFoodCounts += 1;
                            usPlayer.terrariaRecipeEatenList.Add(fullName);
                        }
                        usPlayer.terrariaRecipeNotEatenList.Remove(fullName);
                    }

                }
                else
                {
                    ModItem modItem = item.ModItem;
                    if (modItem is not null)
                    {
                        string fullName = modItem.FullName;
                        if (LostbeltList.LegalFoodListName.Contains(fullName))
                        {
                            if (!usPlayer.terrariaRecipeEatenList.Contains(fullName))
                            {
                                usPlayer.terrariaRecipeEatenFoodCounts += 1;
                                usPlayer.terrariaRecipeEatenList.Add(fullName);
                            }
                            usPlayer.terrariaRecipeNotEatenList.Remove(fullName);
                        }
                    }
                }
            }
            if (item.healLife > 0 && player.Lostbelt().crimsonCharm)
            {
                player.AddBuff(BuffType<CrimsonCharmBuff>(), GreedyMask.OverSatuTime * 60);
                player.Lostbelt().crimsonCharmReduceTime += 1;
            }
            return true;
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
        public override void OnConsumeItem(Item item, Player player)
        {
            if (player.Lostbelt().protectorShiver)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(item.type), ItemID.GoldCoin, 50);
                if (Main.rand.NextBool(4))
                    player.QuickSpawnItem(player.GetSource_OpenItem(item.type), ItemID.PlatinumCoin, 30);
            }
        }

        public override void HoldItem(Item item, Player player)
        {
            var usPlayer = player.Lostbelt();
            if (usPlayer.GeneralWeaponBuffTimer == 0 && ScarletItemIDSets.SharedSameBuffTimer[item.type])
            {
                usPlayer.GeneralWeaponIndex = item.type;
            }
            //这里的判断如下：
            //tacticalExecutionManual (LostbeltPlayer类内) 只用于板正斧头，给代行者玩家自由切换手动处决与自动处决的模式
            //ForceToCustomExecute 则必须得否，即这个武器不能经过这里默认提供的管理
            //ForceToTactialExecute 便为强制自动处决
            bool usetactical = (ScarletItemIDSets.ForceToTacticalExecute[item.type] || usPlayer.tacticalExecutionManual) && (!ScarletItemIDSets.ForceToCustomExecute[item.type]);
            if (usetactical && !ScarletItemIDSets.ForceToAutomaticExecute[item.type])
            {
                usPlayer.tacticalExecution = true;
            }
            if (EnableCritDamage)
            {
                usPlayer.critDamageAll += CritsDamageBonus;
            }
            isShivering = usPlayer.protectorShiver;
            if (item.IsWeapon() && player.Lostbelt().dragonHunter)
            {
                usPlayer.critDamageAll += DragonHunterHead.FixedDamage;
            }
        }
    }
}
