using ContinentOfJourney.NPCs;
using ContinentOfJourney.NPCs.Boss_BigDipper;
using ContinentOfJourney.NPCs.Boss_GoblinChariot;
using ContinentOfJourney.NPCs.Boss_MarquisMoonsquid;
using ContinentOfJourney.NPCs.Boss_PriestessRod;
using ContinentOfJourney.NPCs.Boss_ScarabBelief;
using ContinentOfJourney.NPCs.Boss_TheLifebringer;
using ContinentOfJourney.NPCs.Boss_TheOverwatcher;
using ContinentOfJourney.NPCs.Boss_TheSon;
using ContinentOfJourney.NPCs.Boss_WallofShadow;
using ContinentOfJourney.NPCs.Boss_WorldsEndEverlastingFallingWhale;
using LostbeltTraveler.Globals.Systems;
using LostbeltTraveler.Items.Accessories;
using LostbeltTraveler.Items.Materials;
using LostbeltTraveler.Items.Pets;
using LostbeltTraveler.Items.Useables;
using LostbeltTraveler.Items.Weapons.Executor.Assistance;
using LostbeltTraveler.Items.Weapons.Executor.Caster;
using LostbeltTraveler.Items.Weapons.Executor.ColdSteel;
using LostbeltTraveler.Items.Weapons.Executor.Firearm;
using LostbeltTraveler.Items.Weapons.Executor.Misc;
using LostbeltTraveler.Items.Weapons.Executor.Thrown;
using LostbeltTraveler.Items.Weapons.Magic;
using LostbeltTraveler.Items.Weapons.Melee;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static LostbeltTraveler.Globals.Methods.LostbeltMethods;

namespace LostbeltTraveler.Globals.Instances.NPCs
{
    public partial class LostbeltGlobalNPCs : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            switch (npc.type)
            {
                case NPCID.MartianSaucerCore:
                    npcLoot.AddLootSimple(ItemType<TheMars>(), 4);
                    npcLoot.AddLootSimple(ItemType<ASMD>(), 4);
                    break;
            }
            switch (npc.type)
            {
                case NPCID.Golem:
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<DisasterEssence>(), 1, 10, 20);
                    break;
                case NPCID.Plantera:
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<GrassKnife>(), 4);
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<PestilenceFlower>(), 4);
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<EmblemColdSteel>(), 4);
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<EmblemThrown>(), 4);
                    break;
                case NPCID.WallofFlesh:
                    npcLoot.Add(ItemDropRule.ByCondition(new ScarletWearingFullCowboy(), ItemType<EmblemExecutor>()));
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<EmblemExecutor>(), 4);
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<MoltenKnife>(), 4);
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<Fleshtumor>(), 4);
                    break;
                case NPCID.BigMimicCorruption:
                case NPCID.BigMimicCrimson:
                case NPCID.BigMimicHallow:
                case NPCID.BigMimicJungle:
                    npcLoot.AddLootSimple(ItemType<PurePrismFate>(), 1, 20, 40);
                    break;
                case NPCID.Mimic:
                case NPCID.IceMimic:
                    npcLoot.AddLootSimple(ItemType<PurePrismFate>(), 1, 10, 30);
                    break;
                case NPCID.SkeletronHead:
                    ApplyMasterLoot(ref npcLoot, ItemType<PureDagger>(), 2);
                    break;
                case NPCID.MoonLordCore:
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<PrunusMume>(), 4);
                    break;
                case NPCID.EyeofCthulhu:
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<TearEye>(), 4);
                    break;
                case NPCID.DukeFishron:
                    ApplyNoBossBagLoot(ref npcLoot, ItemType<FishronKnife>(), 4);
                    break;
            }
            if (npc.boss && npc.type != NPCID.KingSlime)
            {
                ApplyMasterLoot(ref npcLoot, ItemType<UnregisteredSpiritOrigin>(), 5);
            }
            if (npc.type == NPCType<WorldsEndEverlastingFallingWhale>())
                ApplyMasterLoot(ref npcLoot, ItemType<WhaleItem>(), 4);
            if (npc.type == NPCType<TheOverwatcher>())
                ApplyMasterLoot(ref npcLoot, ItemType<WatcherItem>(), 4);
            if (npc.type == NPCType<TheLifebringerHead>())
                ApplyMasterLoot(ref npcLoot, ItemType<LifeWormItem>(), 4);

            if (npc.type == NPCType<WallofShadow>())
            {
                ApplyMasterLoot(ref npcLoot, ItemType<ShadowItem>(), 4);
                ApplyMasterLoot(ref npcLoot, ItemType<ShadowCast>(), 5);
                ApplyNoBossBagLoot(ref npcLoot, ItemType<DeathTolls>(), 4);
                ApplyNoBossBagLoot(ref npcLoot, ItemType<ExecutorBadge>(), 4);
            }
            if (npc.type == NPCType<MarquisMoonsquid>())
                ApplyMasterLoot(ref npcLoot, ItemType<SquidItem>(), 4);
            if (npc.type == NPCType<ScarabBelief>())
            {
                ApplyMasterLoot(ref npcLoot, ItemType<NoneItem>(), 4);
                ApplyMasterLoot(ref npcLoot, ItemType<SacarbWings>(), 50);
            }
            if (npc.type == NPCType<TheSon>())
            {
                ApplyMasterLoot(ref npcLoot, ItemType<SonItem>(), 4);
                ApplyFirstTimeLoot(ref npcLoot, new FirstTimeKillingTheSon(), ItemType<TheSonBuff>(), onFailDropRate: 5);
            }
            if (npc.type == NPCType<PolarMimic>())
            {
                npcLoot.AddLootSimple(ItemType<Frostlight>(), 4, 1, 1);
            }
            if (npc.type == NPCType<DesertMimic>() || npc.type == NPCType<PolarMimic>() || npc.type == NPCType<TempleMimic>())
            {
                npcLoot.AddLootSimple(ItemType<PurePrismFate>(), 1, 30, 50);
            }
            if (npc.type == NPCType<PriestessRod>())
            {
                ApplyNoBossBagLoot(ref npcLoot, ItemType<ClimaticHawstring>(), 4);
            }
            if (npc.type == NPCType<GoblinChariot>())
            {
                ApplyNoBossBagLoot(ref npcLoot, ItemType<AngryBomb>(), 4);
            }
            if (npc.type == NPCType<BigDipper>())
            {
                ApplyMasterLoot(ref npcLoot, ItemType<TheSevenStar>(), 1);
            }
        }
        public override void ModifyGlobalLoot(GlobalLoot globalLoot)
        {
            LeadingConditionRule disasterEssenceRule = new(Condition.InUnderworld.ToDropCondition(ShowItemDropInUI.Always));
            IItemDropRuleCondition rule = Condition.DownedGolem.ToDropCondition(ShowItemDropInUI.Always);
            disasterEssenceRule.OnSuccess(ItemDropRule.ByCondition(rule, ItemType<DisasterEssence>(), 6));
            globalLoot.Add(disasterEssenceRule);

            LeadingConditionRule crownofSilveryLightRule = new(Condition.InHallow.ToDropCondition(ShowItemDropInUI.Always));
            crownofSilveryLightRule.OnSuccess(ItemDropRule.ByCondition(new PostSupremeCondition(), ItemType<CrownofSilveryLight>(), 5, 1, 6));
            globalLoot.Add(crownofSilveryLightRule);



        }
    }
}
