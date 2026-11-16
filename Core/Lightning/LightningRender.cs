using LostbeltTraveler.Core.DeepGlowSystem;
using LostbeltTraveler.Core.Primitives.Trail;
using System.Collections.Generic;
using Terraria;

namespace LostbeltTraveler.Core.Lightning
{
    public partial class LightningBuilder
    {
        public static List<TrailDrawDate> trailDrawData = [];
        private static List<Vector2> _drawPosBuffer = [];
        private static List<float> _drawWidthBuffer = [];
        public static void DrawLightning(On_Main.orig_DrawDust orig, Main self)
        {
            orig(self);
            if (HasAnyLightning)
            {
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                DrawSetting setting = new(LostbeltModAssets.Trail_Lightning4.Value, SamplerState.PointWrap);
                for (int i = 0; i < LostbeltLightnings.Length; i++)
                {
                    if (LostbeltLightnings[i].Active)
                    {
                        LostbeltLightning lightning = LostbeltLightnings[i];
                        Draw(lightning.CachedTrails, lightning.Color * lightning.Opacity, 1f, setting);
                    }
                }
                Main.spriteBatch.End();
                DeepGlow.SubmitCustomGlow(() =>
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    ApplyLightningShader();
                    for (int i = 0; i < LostbeltLightnings.Length; i++)
                    {
                        if (LostbeltLightnings[i].Active)
                        {
                            LostbeltLightning lightning = LostbeltLightnings[i];
                            ApplyValue(lightning);
                            Draw(lightning.CachedTrails, lightning.Color, 1.2f, setting);
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null);
                });
            }
        }
        public static void ApplyLightningShader()
        {
            Effect effect = LostbeltShaderAssets.LightningShader;
            effect.Parameters["uvMullt"].SetValue(new Vector2(1f, 1f));
            effect.Parameters["noiseMult"].SetValue(new Vector2(1f, 0.5f));
            effect.Parameters["noiseAdd"].SetValue(new Vector2(Main.GlobalTimeWrappedHourly * -0.2f, 0f));
            effect.Parameters["distortionMult"].SetValue(0.4f);
            effect.Parameters["uFadeoutLeftLength"].SetValue(0.05f);
            effect.Parameters["uFadeinRigtLength"].SetValue(0.05f);
            effect.CurrentTechnique.Passes[0].Apply();
            LostbeltModAssets.Noise_MiscNoise.Value.ApplyGraphicTexture(SamplerState.PointWrap, 1);
            LostbeltModAssets.Noise_Distortion.Value.ApplyGraphicTexture(SamplerState.PointWrap, 2);
        }
        public static void ApplyValue(LostbeltLightning lightning)
        {
            Effect effect = LostbeltShaderAssets.LightningShader;
            effect.Parameters["uvAdd"].SetValue(new Vector2(Main.GlobalTimeWrappedHourly * -0.2f + lightning.RandomFlowOffset, 0f));
            effect.Parameters["fade"].SetValue(1 - lightning.Opacity);
            effect.CurrentTechnique.Passes[0].Apply();
        }
        public static void Draw(List<List<TrailDrawDate>> OldPos, Color color, float widthMult, DrawSetting setting)
        {
            foreach (List<TrailDrawDate> list in OldPos)
            {
                trailDrawData.Clear();
                foreach (TrailDrawDate data in list)
                {
                    trailDrawData.Add(new TrailDrawDate(data.PosDate, color, data.PrimitivesOffset * widthMult, data.PrimitivesHeightRot));
                }
                TrailRender.RenderTrail(trailDrawData.ToArray(), setting);
            }
        }
    }
}
