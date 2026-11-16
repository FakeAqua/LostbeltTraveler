using LostbeltTraveler.Buffs.Pets;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Projs.Pets;
using LostbeltTraveler.Rarity.RarityShiny;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Pets
{
    public class WhaleItem : LostbeltPetItem
    {
        public override int PetProjType => ProjectileType<WhaleProj>();
        public override int PetBuffType => BuffType<WhaleBuff>();
    }
    public abstract class LostbeltPetItem : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.Pet";
        public override string Texture => $"LostbeltTraveler/Assets/Texture/Pets/Pet_{GetType().Name}";
        //封住这个sd避免误重写
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.RarePets);
        }
        public virtual int PetProjType => 0;
        public virtual int PetBuffType => 0;
        public sealed override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(gold: 50);
            Item.noMelee = true;
            Item.useAnimation = Item.useTime = 20;
            Item.UseSound = SoundID.Item2;
            Item.rare = RarityType<RarePets>();
            Item.master = true;
            Item.buffType = PetBuffType;
            Item.shoot = PetProjType;
            Item.Lostbelt().CanDrawIcon = true;
        }
        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
                player.AddBuff(Item.buffType, 3600);
            return true;
        }
    }
}
