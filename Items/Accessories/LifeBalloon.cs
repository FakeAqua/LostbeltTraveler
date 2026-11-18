using ContinentOfJourney.Buffs;
using ContinentOfJourney.Items.Material;
using ContinentOfJourney.Tiles;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class LifeBalloon : LostbeltItemClass
    {
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Life);
        }
        public override string AssetPath => AssetHandler.Equips;
        public int MaxLife = 300;
        public int LifeRegen = 2;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(MaxLife, LifeRegen.ToLifeRegenFormat());
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Red);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ApplyMaxLifeBonus(MaxLife);
            player.Lostbelt().lifeRegenPool += LifeRegen;
            player.buffImmune[BuffType<PlagueBuff>()] = true;
            player.GetJumpState(ExtraJump.CloudInABottle).Enable();
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
            player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            player.GetJumpState(ExtraJump.FartInAJar).Enable();
            player.GetJumpState<SupremeBalloonJump>().Enable();
            player.GetJumpState<LifeBalloonExJump>().Enable();
            player.jumpBoost = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 1.6f;
            player.noFallDmg = true;

        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SupremeBundleBalloon>().
                AddIngredient<EssenceofLife>(10).
                AddIngredient(ItemID.Apple, 5).
                AddTile(TileType<FountainofLife>()).
                Register();
        }

    }
    public class LifeBalloonExJump : ExtraJump
    {
        public override Position GetDefaultPosition() => AfterBottleJumps;
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(GetInstance<SupremeBalloonJump>());
        }
        public override float GetDurationMultiplier(Player player)
        {
            return 3f;
        }
        public override void OnRefreshed(Player player)
        {
            // Reset the jump counter
            player.Lostbelt().LifeBalloonAccJumps = 5;
        }
        public override void ShowVisuals(Player player)
        {
            base.ShowVisuals(player);
        }
        public override void UpdateHorizontalSpeeds(Player player)
        {
            player.runAcceleration *= 3;
            player.maxRunSpeed *= 2;
        }
        public override void OnStarted(Player player, ref bool playSound)
        {
            // Get the jump counter
            ref int jumps = ref player.Lostbelt().LifeBalloonAccJumps;

            // Spawn rings of fire particles
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;

            Vector2 center = player.Top + new Vector2(0, offsetY);

            // Play a different sound
            playSound = false;
            for (int i = 0; i < 24; i++)
            {
                Vector2 vel = RandVelTwoPi(1.5f, 21.6f);
                ECSParticle.LiliesPetal(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                    RandLerpColor(Color.LimeGreen, Color.ForestGreen), 45, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .12f, 0.51f, false, 0, true, false, BlendState.AlphaBlend);
            }
            SoundEngine.PlaySound(SoundID.Item42 with { PitchVariance = 0.04f }, player.Bottom);
            // Decrement the jump counter
            jumps--;
            // Allow the jump to be used again while the jump counter is > 0
            if (jumps > 0)
                player.GetJumpState(this).Available = true;
        }

    }
}
