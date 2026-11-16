using LostbeltTraveler.Globals.Configs;
using LostbeltTraveler.Globals.Database.IDSets;
using LostbeltTraveler.Items.Accessories;
using Newtonsoft.Json.Serialization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Instances.Items
{
    public partial class LostbeltGlobalItems : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ItemID.PocketMirror)
            {
                player.Lostbelt().pocketMirror = !hideVisual;
            }
            if(item.wingSlot!=-1)
            {
                player.Lostbelt().wingsType = item.type;
            }
            UpdateParryShield(item, player);
        }
        public override void UpdateVanity(Item item, Player player)
        {
            if (item.type == ItemID.PocketMirror)
            {
                player.Lostbelt().pocketMirror = true;
            }
        }
        public override void UpdateInfoAccessory(Item item, Player player)
        {
            base.UpdateInfoAccessory(item, player);
        }
        public override bool WingUpdate(int wings, Player player, bool inUse)
        {
            return base.WingUpdate(wings, player, inUse);
        }
        public override bool WingGlidingSpeeds(Item item, Player player, ref float gravityMultiplier, ref float maxSpeedMultiplier, bool prevented)
        {
            return base.WingGlidingSpeeds(item, player, ref gravityMultiplier, ref maxSpeedMultiplier, prevented);
        }
        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            bool vortex = player.Lostbelt().wingsType == ItemID.WingsVortex && player.Lostbelt().wingsChargeType == ItemType<ChargeFeatherVortex>();
            if (vortex)
            {
                ascentWhenFalling = .85f;
                ascentWhenRising = .15f;
                maxCanAscendMultiplier = 1;
                maxAscentMultiplier = 3;
                constantAscend = .135f;
            }
            base.VerticalWingSpeeds(item, player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
        }
        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            if (item.type == ItemID.WingsVortex)
            {
                speed = 9f;
                acceleration = .37f;
            }
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (LostbeltConfigClient.Instance.DrawIcon && CanDrawGhost)
            {
                //在UpdateInventory内更新帧图的绘制，因为tooltip的draw实际上只会执行一次
                GhostTimer++;
                if (GhostTimer > 5)
                {
                    GhostFrame++;
                    GhostTimer = 0;
                }
                if (GhostFrame >= 16)
                    GhostFrame = 1;
            }
            if (!setTintIcon)
            {
                tintIconDrawLerp = Lerp(tintIconDrawLerp, 0f, 0.2f);
                if (tintIconDrawLerp <= 0.02f)
                    tintIconDrawLerp = 0;
            }
            else
            {
                tintIconDrawLerp = Lerp(tintIconDrawLerp, 1f, 0.2f);
                if (tintIconDrawLerp >= 0.98f)
                    tintIconDrawLerp = 1;
            }
            setTintIcon = false;
        }

    }

}
