using ContinentOfJourney.Items.Accessories;
using LostbeltTraveler.Core.ParticleECS;
using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Globals.Database.List;
using LostbeltTraveler.Globals.Handlers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LostbeltTraveler.Items.Accessories
{
    public class SupremeBundleBalloon : LostbeltItemClass
    {
        public override string AssetPath => AssetHandler.Equips;
        public override void SetStaticDefaults()
        {
            LostbeltList.ShinyRarityItemDictionary.Add(Type, ShinyRarityType.FateGolden);
        }
        public override void ExSD()
        {
            Item.accessory = true;
            Item.SetUpRarityPrice(ItemRarityID.Lime);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetJumpState(ExtraJump.CloudInABottle).Enable();
            player.GetJumpState(ExtraJump.BlizzardInABottle).Enable();
            player.GetJumpState(ExtraJump.SandstormInABottle).Enable();
            player.GetJumpState(ExtraJump.TsunamiInABottle).Enable();
            player.GetJumpState(ExtraJump.FartInAJar).Enable();
            player.GetJumpState<SupremeBalloonJump>().Enable();
            player.jumpBoost = true;
            player.autoJump = true;
            player.jumpSpeedBoost += 1.6f;
            player.noFallDmg = true;
            base.UpdateAccessory(player, hideVisual);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.HorseshoeBundle).
                AddIngredient(ItemID.BalloonHorseshoeFart).
                AddIngredient(ItemID.BalloonHorseshoeHoney).
                AddIngredient(ItemID.BalloonHorseshoeSharkron).
                AddIngredient<DarkHorseshoeBalloon>().
                AddIngredient(ItemID.LunarBar, 15).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
    public class SupremeBalloonJump : ExtraJump
    {
        public override Position GetDefaultPosition() => AfterBottleJumps;
        public override float GetDurationMultiplier(Player player)
        {
            return player.GetModPlayer<SupremeBalloonJumpPlayer>().remainingJumpes switch
            {
                1 => 1.1f,
                2 => 1.5f,
                3 => 0.7f,
                4 => 0.9f,
                _ => 1f
            };
        }
        public override void OnRefreshed(Player player)
        {
            player.GetModPlayer<SupremeBalloonJumpPlayer>().remainingJumpes = 5;
        }
        public override void ShowVisuals(Player player)
        {
            ref int jumps = ref player.GetModPlayer<SupremeBalloonJumpPlayer>().remainingJumpes;
            // Spawn rings of fire particles
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;
            Vector2 center = player.Top + new Vector2(0, offsetY);

            if (jumps == 5)
            {
                int numDusts = 36;

            }
            else if (jumps == 4)
            {
                for (int i = 0; i < 4; i++)
                    ECSParticle.ManaStarDust(center.ToRandCirclePos(16), -player.velocity * .15f,
                        RandLerpColor(Color.Gold, Color.Yellow), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, true, BlendState.Additive);

            }
            else if (jumps == 3)
            {
                for (int i = 0; i < 4; i++)
                    ECSParticle.DarkMoonDust(center.ToRandCirclePos(16), -player.velocity * .15f,
                        RandLerpColor(Color.Purple, Color.DarkViolet), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, BlendState.Additive);
            }
            else if (jumps == 2)
            {
                for (int i = 0; i < 4; i++)
                    ECSParticle.DarkMoonDust(center.ToRandCirclePos(16), -player.velocity * .15f,
                        RandLerpColor(Color.LightSkyBlue, Color.RoyalBlue), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, BlendState.Additive);

            }
            else if (jumps == 1)
            {
                for (int i = 0; i < 4; i++)
                    ECSParticle.ShinyCrossStarECS(center.ToRandCirclePos(16), -player.velocity * .15f,
                        RandLerpColor(Color.Violet, Color.Pink), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .5f, .2f);

            }
            else
            {
                for (int i = 0; i < 4; i++)
                    ECSParticle.ShinyCrossStarECS(center.ToRandCirclePos(16), -player.velocity * .15f,
                        RandLerpColor(Color.Lime, Color.LimeGreen), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .5f, .2f);
            }

            base.ShowVisuals(player);
        }
        public override void UpdateHorizontalSpeeds(Player player)
        {
            int ratios = (player.GetModPlayer<SupremeBalloonJumpPlayer>().remainingJumpes);
            switch (ratios)
            {
                case 1:
                    ModifyJump(ref player, 2.85f, 1.65f);
                    break;
                case 0:
                    ModifyJump(ref player, 3f, 2.5f);
                    break;
                case 2:
                    ModifyJump(ref player, 3.1f, 3.55f);
                    break;
                case 3:
                    ModifyJump(ref player, 3.25f, 1.75f);
                    break;
                case 4:
                    ModifyJump(ref player, 1.55f, 1.15f);
                    break;
            }
        }
        public void ModifyJump(ref Player player, float runAcc, float maxRunSpeed)
        {
            player.runAcceleration *= runAcc;
            player.maxRunSpeed *= maxRunSpeed;
        }
        public override void OnStarted(Player player, ref bool playSound)
        {
            ref int jumps = ref player.GetModPlayer<SupremeBalloonJumpPlayer>().remainingJumpes;
            // Spawn rings of fire particles
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;

            offsetY -= 16;
            Vector2 center = player.Center + new Vector2(0, offsetY);


            // Play a different sound
            playSound = false;
            if (jumps == 5)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 vel = RandVelTwoPi(1.5f, 3.6f);
                    ECSParticle.ManaStarDust(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                        RandLerpColor(Color.Gold, Color.Yellow), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, true, BlendState.Additive);
                }

            }
            else if (jumps == 4)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 vel = RandVelTwoPi(1.5f, 3.6f);
                    ECSParticle.DarkMoonDust(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                        RandLerpColor(Color.Purple, Color.DarkViolet), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, BlendState.Additive);
                }
            }
            else if (jumps == 3)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 vel = RandVelTwoPi(1.5f, 3.6f);
                    ECSParticle.DarkMoonDust(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                        RandLerpColor(Color.LightSkyBlue, Color.RoyalBlue), 40, 1, RandRotTwoPi, Main.rand.NextFloat(.9f, 1.1f) * .5f, BlendState.Additive);
                }

            }
            else if (jumps == 2)
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 vel = RandVelTwoPi(1.5f, 3.6f);
                    ECSParticle.ShinyCrossStarECS(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                        RandLerpColor(Color.Violet, Color.Pink), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .5f, .2f);
                }

            }
            else
            {
                for (int i = 0; i < 24; i++)
                {
                    Vector2 vel = RandVelTwoPi(1.5f, 3.6f);
                    ECSParticle.ShinyCrossStarECS(center.ToRandCirclePos(16), new Vector2(vel.X, vel.Y / 2f),
                        RandLerpColor(Color.Lime, Color.LimeGreen), 45, 1, Main.rand.NextFloat(.9f, 1.1f) * .5f, .2f);
                }
            }

            float pitch = jumps switch
            {
                1 => 0.5f,
                2 => 0.4f,
                3 => 0.3f,
                4 => 0.2f,
                5 => 0.1f,
                6 => 0f,
                7 => -0.1f,
                8 => -0.2f,
                9 => -0.3f,
                10 => -0.4f,
                _ => 0
            };
            switch (jumps)
            {
                case 1:
                    ScarletSound(SoundID.Item142, player.Bottom);
                    break;
                case 2:
                    ScarletSound(SoundID.Item143, player.Bottom);
                    break;
                case 3:
                    ScarletSound(SoundID.Item144, player.Bottom);
                    break;
                case 4:
                    ScarletSound(SoundID.Item143, player.Bottom, pitch: -.2f);
                    break;
                case 5:
                    ScarletSound(SoundID.Item142, player.Bottom, pitch: -.2f);
                    break;
            }


            //SoundEngine.PlaySound(SoundID.Item8 with { Pitch = pitch, PitchVariance = 0.04f }, player.Bottom);

            // Decrement the jump counter
            jumps--;
            //Main.NewText(jumps);
            // Allow the jump to be used again while the jump counter is > 0
            if (jumps > 0)
                player.GetJumpState(this).Available = true;
            base.OnStarted(player, ref playSound);
        }
    }
    public class SupremeBalloonJumpPlayer : ModPlayer
    {
        public int remainingJumpes;
    }
}
