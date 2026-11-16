using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Globals.Players;
using LostbeltTraveler.Globals.Players.Dashes;
using LostbeltTraveler.Globals.Systems;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories

{
    public class ChargeFeatherBroken : LostbeltItemClass
    {
        public virtual int MatchWing => ItemID.CreativeWings;
        public virtual float RestoredCharge => .05f;
        public override string AssetPath => AssetHandler.Equips;
        public static int WingTimeMax = 15;
        //public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs($"[i:{ItemID.Search.GetName(MatchWing)}]");
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.Lostbelt().CanDrawIcon = true;
            Item.accessory = true;
            ExSD();
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Orange);
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (player.Lostbelt().wingsChargeType == equippedItem.type && player.Lostbelt().wingsChargeType!=incomingItem.type)
                return false;
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Player p = Main.LocalPlayer;
            LostbeltPlayer mp = p.Lostbelt();
            bool equal = mp.wingsType == MatchWing && mp.wingsChargeType == Type;
            if (equal)
            {
                Texture2D tex = LostbeltModAssets.Particle_GlowSquareThick.Value;
                //spriteBatch.Draw(tex, position, null, drawColor.ToAddColor(), 0, tex.Size()/2f, scale*.28f, 0, 0);
                tex = LostbeltModAssets.Particle_GlowSquareBig.Value;
                spriteBatch.Draw(tex, position, null, Color.GreenYellow.ToAddColor(), 0, tex.Size()/2f, scale*0.24f, 0, 0);
                spriteBatch.Draw(tex, position, null, Color.YellowGreen.ToAddColor(), 0, tex.Size()/2f, scale*0.24f*.95f, 0, 0);
            }
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (RestoredCharge == 0)
                return;
            int index = tooltips.FindIndex(t => t.Name.Contains("Tooltip")&&t.Mod == "Terraria");
            string value = Mod.GetLocalizationKey("Items.Accessories.ChargeFeatherBroken.Globals").ToLangValue().ToFormatValue(RestoredCharge.ToPercent());
            tooltips.CreateTooltipDirect(value, Color.GreenYellow, LineName: "ChargeFeatherGlobal", index: index);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.Lostbelt().wingsChargeType = Type;
            player.Lostbelt().chargeWingsRestore = RestoredCharge;
            player.ApplyDash(GetInstance<FeatherDash>().Type);
            //UpdateGlobal(player);
            //if (player.Lostbelt().wingsType == MatchWing)
            //    UpdateMatchWings(player);
        }
        public virtual void UpdateGlobal(Player player)
        {

        }
        public virtual void UpdateMatchWings(Player player)
        {
        }
    }
}
