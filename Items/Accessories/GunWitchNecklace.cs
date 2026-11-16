using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Executor;
using LostbeltTraveler.Globals.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class GunWitchNecklace : LostbeltItemClass
    {
        public float Damage = .1f;
        public float AttackSpeed = .1f;
        public override string AssetPath => AssetHandler.Equips;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Damage.ToPercent(), AttackSpeed.ToPercent());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Gunwitch);
        }
        public override void ExSD()
        {
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<GenericDamageClass>() += Damage;
            if (player.HeldItem.CheckExecuteTypes(ExecutorWeaponType.Firearm))
                player.GetAttackSpeed<ExecutorDamageClass>() += AttackSpeed;
        }
    }
}
