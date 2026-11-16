using LostbeltTraveler.Globals.Database.Enums;
using Terraria;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Classes
{
    public abstract class LostbeltWeapon : ModItem, ILocalizedModType
    {
        public virtual EnumDamageClass Category { get; }
        public new string LocalizationCategory => $"Weapons.{Category}";
        public override string Texture => $"LostbeltTraveler/Assets/Texture/Items/Weapons/{GetType().Name}";
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.DamageType = GetDamageClass;
            Item.Lostbelt().CanDrawIcon = true;
            ExSD();
        }
        public virtual void ExSD() { }
        public DamageClass GetDamageClass
        {
            get
            {
                return Category switch
                {
                    EnumDamageClass.Melee => DamageClass.Melee,
                    EnumDamageClass.Ranged => DamageClass.Ranged,
                    EnumDamageClass.Magic => DamageClass.Magic,
                    EnumDamageClass.Summon => DamageClass.Summon,
                    _ => DamageClass.Generic,
                };
            }
        }
    }
}
