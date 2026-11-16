using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace LostbeltTraveler.Assets.Registers
{
    public class LostbeltNPCAssets : ModSystem
    {
        public static Dictionary<string, string> NPCDictionary;
        public static Dictionary<string, string> BossDictionary;
        public static string ApplyNPC(string key)
        {
            if (NPCDictionary.TryGetValue(key, out string value))
                return value;
            return null;
        }
            public static string ApplyBoss(string key)
        {
            if (BossDictionary.TryGetValue(key, out string value))
                return value;
            return null;
        }
        public override void Load()
        { 
            static string LoadAssetNPC(string name)
            {
                return "LostbeltTraveler/Assets/Texture/NPCs/Enemy/" + name;
            }
            static string LoadAssetBoss(string name)
            {
                return "LostbeltTraveler/Assets/Texture/NPCs/Bosses/" + name + "/" + name + "Boss";
            }

            NPCDictionary = new Dictionary<string, string>
            {
                {"ExecutorCelestialTower", LoadAssetNPC("ExecutorCelestialTower") },
            };

            BossDictionary = new Dictionary<string, string>
            {
                {"DyradEye", LoadAssetBoss("DyradEye") },
            };
        }
    }
}
