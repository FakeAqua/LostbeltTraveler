using ContinentOfJourney.Items.Material;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using LostbeltTraveler.Projs.General;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class FinalBarBalloon : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public int LifeRegen = 4;
        public int LifeStat = 300;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(LifeStat, LifeRegen.ToLifeRegenFormat());
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, Globals.Database.Enums.ShinyRarityType.Hallowed);
        }
        public override void ExSD()
        {
            Item.defense = 5;
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Orange);
            Item.Lostbelt().drawBuffIcon = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.ApplyMaxLifeBonus(LifeStat);
            player.Lostbelt().lifeRegenPool += LifeRegen;
            //player.GetJumpState(ExtraJump.CloudInABottle).Enable();
            //player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            //player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
            //player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            //player.GetJumpState(ExtraJump.FartInAJar).Enable();
            //player.GetJumpState<SupremeBalloonJump>().Enable();
            //player.GetJumpState<LifeBalloonExJump>().Enable();
            player.GetJumpState<FinalBarBalloonJump>().Enable();
            player.jumpBoost = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 1.6f;
            player.noFallDmg = true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<LifeBalloon>().
                AddIngredient<FinalBar>(10).
                AddTile(FinalAnvilTile).
                Register();
        }
    }
    public class FinalBarBalloonJump : ExtraJump
    {
        public override Position GetDefaultPosition() => AfterBottleJumps;
        public override float GetDurationMultiplier(Player player)
        {
            return 2.8f;
        }
        public override IEnumerable<Position> GetModdedConstraints()
        {
            yield return new After(GetInstance<LifeBalloonExJump>());
        }

        public override void OnRefreshed(Player player)
        {
        }
        public override void ShowVisuals(Player player)
        {
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;
            Vector2 center = player.Top + new Vector2(0, offsetY);

            ECSParticle.HighResolutionThunder(center.ToRandCirclePos(16), Vector2.Zero, Color.LightGoldenrodYellow, 40, 1, player.velocity.ToRotation() + PiOver2, .3f, 3);
        }
        public override void UpdateHorizontalSpeeds(Player player)
        {
            ModifyJump(ref player, 1.5f, 2.5f);
        }
        public void ModifyJump(ref Player player, float runAcc, float maxRunSpeed)
        {
            player.runAcceleration *= runAcc;
            player.maxRunSpeed *= maxRunSpeed;
        }
        public override void OnStarted(Player player, ref bool playSound)
        {
            // Spawn rings of fire particles
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;
            Vector2 center = player.Center + new Vector2(0, offsetY);


            playSound = false;
            for (int i = 0; i < 3; i++)
            {
                ECSParticle.HighResolutionThunder(center.ToRandCirclePos(16), Vector2.Zero, Color.Goldenrod, 40, 1, RandRotTwoPi, 1, 3);
            }

            Projectile.NewProjectileDirect(player.GetSource_FromThis(), center, Vector2.Zero, ProjectileType<FinalBarBalloonThunder>(), 5000, 1, player.whoAmI);
            ScarletSound(LostbeltSoundAssets.Lightning_Strike, player.Bottom, .7f, pitch: .52f, pitchVariance: .2f, instances: 0);


            player.GetJumpState(this).Available = true;
            base.OnStarted(player, ref playSound);
        }
    }
}
