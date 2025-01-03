using com.ethnicthv.chemlab.client.api.render;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;
using Color = UnityEngine.Color;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public class ChemicalCompoundRenderFeature : ScriptableRendererFeature
    {
        private class ChemicalCompoundRenderPass : ScriptableRenderPass
        {
            private class PassData
            {
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                using (var builder = renderGraph
                           .AddRasterRenderPass<PassData>("ChemicalCompoundRenderPass", out var passData))
                {
                    if (RenderProgram.Instance != null)
                    {
                        RenderProgram.Instance.CheckModelMatrix();
                    }
                    
                    var resourceData = frameData.Get<UniversalResourceData>();
                    
                    builder.SetRenderAttachment(resourceData.activeColorTexture, 0);
                    builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture);
                    
                    builder.AllowPassCulling(false);
                    builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
                }
            }

            static void ExecutePass(PassData data, RasterGraphContext context)
            {
                if (RenderProgram.Instance == null) return;
                
                RenderProgram.Instance.RenderCompound(context.cmd, context);
            }
        }

        private ChemicalCompoundRenderPass _renderPass;

        public override void Create()
        {
            _renderPass = new ChemicalCompoundRenderPass
            {
                renderPassEvent = RenderPassEvent.AfterRenderingOpaques
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_renderPass);
        }
    }
}