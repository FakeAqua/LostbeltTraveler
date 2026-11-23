using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace LostbeltTraveler.ReVisual.Class
{
    public class ReVisualPlayer : ModPlayer
    {
        static ReVisualPlayer()
        {
            _fieldAccessors = new Dictionary<string, (Func<ReVisualPlayer, bool>, Action<ReVisualPlayer, bool>)>();

            var saveableFields = typeof(ReVisualPlayer)
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.Name.Contains("re") && f.FieldType == typeof(bool));

            foreach (var field in saveableFields)
            {
                string fieldName = field.Name;
                Func<ReVisualPlayer, bool> getter = (player) => (bool)field.GetValue(player);
                Action<ReVisualPlayer, bool> setter = (player, value) => field.SetValue(player, value);

                _fieldAccessors.Add(fieldName, (getter, setter));
            }
        }

        public bool reChlorophyteKnife = false;
        public bool reHellfire = false;
        public bool reCobaltKnife = false;
        public bool reOrihalcumKnife = false;
        public bool reAdamantiteKnife = false;
        public bool reMythrilKnife = false;
        public bool rePalladiumKnife = false;
        public bool reTitaniumKnife = false;
        public bool reBloodthirst = false;
        public bool reBackStabber = false;
        public bool reOreknife = false;
        public bool reLonginus = false;
        public bool reAccelerationism = false;
        public bool rePurplePuffer = false;
        public bool reCasta = false;
        public bool reSkeletronYoyo = false;
        public bool reRomanCandle = false;
        public bool reCrusadersCrossbow = false;
        public bool reKingBeeGun = false;
        public bool reBloodScepter = false;
        public bool reLiquidBow = false;
        public bool reTenNoon = false;

        public static readonly Dictionary<string, (Func<ReVisualPlayer, bool> Getter, Action<ReVisualPlayer, bool> Setter)> _fieldAccessors;
        public override void SaveData(TagCompound tag)
        {
            foreach (var kvp in _fieldAccessors)
            {
                string fieldName = kvp.Key;
                bool value = kvp.Value.Getter(this);
                tag.Add(fieldName, value);
            }
        }
        public override void LoadData(TagCompound tag)
        {
            foreach (var kvp in _fieldAccessors)
            {
                string fieldName = kvp.Key;
                bool value = tag.GetBool(fieldName);
                kvp.Value.Setter(this, value);
            }
        }
    }
}
