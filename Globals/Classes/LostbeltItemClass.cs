using LostbeltTraveler.Globals.Handlers;
using Terraria.ModLoader;

namespace LostbeltTraveler.Globals.Classes
{
    public abstract class LostbeltItemClass : ModItem, ILocalizedModType
    {
        public virtual string AssetPath => string.Empty;
        //力竭了
        public new string LocalizationCategory => $"Items.{AssetPath.LocalizedHelper()}";
        public override string Texture => AssetPath + GetType().Name;
        public override void SetDefaults()
        {
            Item.width = Item.height = 16;
            Item.Lostbelt().CanDrawIcon = true;
            ExSD();
        }
        public virtual void ExSD() { }
    }
}
