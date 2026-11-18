using LostbeltTraveler.Globals.Classes;
using LostbeltTraveler.Globals.Database.Enums;
using LostbeltTraveler.Projs.Melee;
using System;
using Terraria;
using Terraria.ID;

namespace LostbeltTraveler.Projs.Executor.Thrown
{
    public class LunarHammerExecution : LostbeltProjs
    {
        public override string Texture => GetInstance<LunarHammerProj>().Texture;
        public override EnumDamageClass Category => EnumDamageClass.Executor;
        private enum DoType
        {
            IsArcRotating,
            IsHanging
        }
        private DoType AttackType
        {
            get => (DoType)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }
        private ref float AttackTimer => ref Projectile.ai[1];
        private int FlareCounts
        {
            get => (int)Projectile.ai[2];
            set => Projectile.ai[2] = value;
        }
        public float RotProgress = 0;
        private int StartSpinTime => 30 * Projectile.extraUpdates;
        public override void SetStaticDefaults()
        {
            Projectile.ToTrailSetting(10);
        }
        public override void ExSD()
        {
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.height = Projectile.width = 66;
            Projectile.timeLeft = 480;
            Projectile.extraUpdates = 3;
            Projectile.localNPCHitCooldown = 25 * Projectile.extraUpdates;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void ProjAI()
        {
            Lighting.AddLight(Projectile.Center, Color.DarkSeaGreen.ToVector3() * 1f);
            Projectile.rotation += 0.2f;
            switch (AttackType)
            {
                case DoType.IsArcRotating:
                    DoArcRotating();
                    break;
                case DoType.IsHanging:
                    DoHanging();
                    break;
            }
        }
        private void DoArcRotating()
        {
            DoArcRot();
            //Timer应延后自增避免出现执行问题
            AttackTimer += 1;
            ReleaseDarkEnegry();
            if (RotateTime > 1)
            {
                //跳出去避免后续继续调用attacktimer的时候出问题
                Projectile.netUpdate = true;
                AttackType = DoType.IsHanging;
            }
        }
        private void ReleaseDarkEnegry()
        {
            if (Projectile.GetTargetSafe(out NPC target, canPassWall: true))
            {
                //下方是一段基于主射弹当前速度而做出动态变化的射弹生成代码
                //表现来说是，衍生射弹的生成频率将会与主射弹的速度会成正比，并尽可能控制在需要的固定间隔内
                //基础生成间隔
                const int BaseSpawnSpeed = 10;
                //射弹的飞行速度
                const float BaseTravelSpeed = 22f;
                //最小生成间隔
                const float MinSpawnSpeed = 15f;
                //最大生成间隔
                const float MaxSpawnSpeed = 24;
                //计算当前速度的模长
                float curSpeed = Projectile.velocity.Length();
                //基于射弹速度间隔进行生成刻计算
                float dynamicSpawnSpeed = BaseTravelSpeed / curSpeed * BaseSpawnSpeed;
                //控制在合理范围内
                dynamicSpawnSpeed = Clamp(dynamicSpawnSpeed, MinSpawnSpeed, MaxSpawnSpeed);
                //向下取整
                int spawnRates = (int)Math.Round(dynamicSpawnSpeed);
                Vector2 direction = Projectile.Center.GetNormalVector2(target.Center);
                if (AttackTimer % spawnRates == 0)
                {
                    //这里的暗温魔能会必中
                    float baseFlareSpeed = Main.rand.NextFloat(12f, 16f);
                    //依据锤子当前的速度，以对数的形式给予伤害加成
                    int flareDamage = (int)(Projectile.damage + 2 * (float)Math.Log(Projectile.velocity.Length() / 1.5));
                    Vector2 velocity = direction * baseFlareSpeed;
                    if (Projectile.owner != Main.myPlayer)
                        return;
                    //鬼魂音效
                    ScarletSound(SoundID.Item91, Projectile.Center);
                    //生成
                    Projectile flares = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, ProjectileID.MoonlordArrowTrail, flareDamage, 1.1f, Owner.whoAmI, 0f, Main.rand.Next(3));
                    flares.extraUpdates = 3;
                    flares.tileCollide = false;
                }
            }
        }
        private void DoHanging()
        {
            if (Projectile.GetTargetSafe(out NPC target, Projectile.Lostbelt().GlobalTargetIndex, true, 1800f, true))
            {
                Projectile.HomingTarget(target.Center, 1800f, 24f, 20f);
                Projectile.extraUpdates = 4;
            }
            else
                Projectile.extraUpdates = 3;
        }
        //下面这段圆弧代码需要重置
        private float RotateTime = 0;
        private bool _isArcRotating = false;
        private float _arcStartRotation;
        private float TotalArcAngle;
        private float _originalSpeed;
        private float _prevArcAngle;
        private void DoArcRot()
        {
            //如果已反向，且AttackTimer不会在被置零，返回，不执行下方AI
            if (RotateTime < 2 && !_isArcRotating)
            {
                float wtf = TwoPi;
                //随机取用角度
                TotalArcAngle = Main.rand.NextBool() ? wtf : -wtf;
                _isArcRotating = true;
                _arcStartRotation = Projectile.velocity.ToRotation();
                _originalSpeed = Projectile.velocity.Length();
                //尚未反向，缓存这个角度
                if (RotateTime == 0)
                    _prevArcAngle = TotalArcAngle;
                else
                    TotalArcAngle = -_prevArcAngle;
                Projectile.velocity *= .28f;
            }
            if (_isArcRotating)
            {
                //首次画圆，执行0~StartSpin，第二次画圆，执行StartSpinTime ~ StartSpinTime * 2
                float progress = RotateTime == 0
                    ? (float)AttackTimer / StartSpinTime
                    : (float)(AttackTimer - StartSpinTime) / StartSpinTime;
                float curRot = _arcStartRotation + TotalArcAngle * progress;
                //加速
                float speed = Projectile.velocity.Length() + 0.21f * AttackTimer;
                if (speed > _originalSpeed)
                    speed = _originalSpeed;

                Projectile.velocity = curRot.ToRotationVector2() * speed;
                //如果进程结束
                if (progress >= 1f)
                {
                    Projectile.velocity = curRot.ToRotationVector2() * _originalSpeed;
                    _isArcRotating = false;
                    //首次反向结束，重置计时器准备反向
                    AttackTimer = StartSpinTime;
                    RotateTime += 1;
                }
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return base.OnTileCollide(oldVelocity);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            //非挂载情况返回不执行。
            if (AttackType != DoType.IsHanging)
                return;

            if (FlareCounts < 3)
                FlareCounts++;

            int flareDamage = (int)(Projectile.damage / 2 * Math.Log(1 + FlareCounts));
            ScarletSound(SoundID.Item88, Projectile.Center);
            for (int i = 0; i < FlareCounts; i++)
            {
                Vector2 pos = Projectile.Center - Vector2.UnitY * 800f + Main.rand.NextFloat(-150f, 150f) * Vector2.UnitX;
                ScarletSound(SoundID.Item88, Projectile.Center);
                Vector2 normalVec = pos.GetNormalVector2(target.Center);
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), pos, normalVec * 18f, ProjectileType<LunarHammerMissle>(), Projectile.damage, 1, Projectile.owner);
                proj.localAI[1] = Main.rand.NextFloat(0.27f, 1.01f);

            }
            //NightmareArrowDrop(target, flareDamage);

            int leastHitCD = 6 * Projectile.extraUpdates;
            Projectile.localNPCHitCooldown -= 5 * Projectile.extraUpdates;
            if (Projectile.localNPCHitCooldown < leastHitCD)
                Projectile.localNPCHitCooldown = leastHitCD;
            base.OnHitNPC(target, hit, damageDone);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.GetProjDrawData(out Texture2D projTex, out Vector2 drawPos, out Vector2 ori);
            int length = Projectile.oldPos.Length;
            for (int i = 0; i < length; i++)
            {
                float progress = (float)i / length;
                Vector2 oldPos = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition;
                float oldRot = Projectile.oldRot[i];
                Color c = Color.Lerp(Color.SkyBlue, Color.DarkSeaGreen, progress);
                float scale = (1f - (progress * 0.45f)) * Projectile.scale;
                float opac = Lerp(1f, 0f, progress);
                SB.FastDraw(projTex, oldPos + Main.rand.NextVector2Circular(3, 3), c.ToAddColor() * .75f * opac * opac, oldRot, ori, scale * 1.1f, 0);
                Color c2 = Color.Lerp(Color.CornflowerBlue, Color.DarkSeaGreen, progress);
                SB.FastDraw(projTex, oldPos, c2.ToAddColor(150) * .9f * opac * opac, oldRot, ori, scale, 0);
            }
            for (int i = 0; i < 3; i++)
            {
                SB.FastDraw(projTex, drawPos + Main.rand.NextVector2Circular(3, 3), Color.LightSkyBlue.ToAddColor() * .5f, Projectile.rotation, ori, Projectile.scale * 1.05f, 0);
            }
            SB.FastDraw(projTex, drawPos, lightColor, Projectile.rotation, ori, Projectile.scale, 0);
            float LerpValue = 1f;
            float lerpRot = Lerp(-PiOver2, PiOver2, EaseOutCubic(LerpValue));
            float opac1 = EaseInCubic(LerpValue);
            GemYoyoProj.DrawAmethystSmear(Projectile, Color.Aquamarine * opac1, Color.LightSkyBlue * opac1, 3.5f * LerpValue, lerpRot);
            return false;
        }
    }
}
