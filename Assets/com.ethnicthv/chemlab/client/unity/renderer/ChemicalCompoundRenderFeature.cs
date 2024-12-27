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
                using var builder = renderGraph
                    .AddRenderPass<PassData>("ChemicalCompoundRenderPass", out var passData);
                builder.SetRenderFunc((PassData data, RenderGraphContext context) => ExecutePass(data, context));
            }

            static void ExecutePass(PassData data, RenderGraphContext context)
            {
                // Clear the render target to black
                context.cmd.ClearRenderTarget(true, true, Color.black);
                if (RenderProgram.Instance == null)
                {
                    Debug.LogError("RenderProgram is null! Please make sure that the RenderProgram is created.");
                }
                else
                {
                    //Pass one, Recalculate the model matrix if needed
                    
                    //Pass two, Render
                    RenderProgram.Instance.RenderCompound(context.cmd, context.renderContext);
                }
            }
        }

        private ChemicalCompoundRenderPass _renderPass;

        public override void Create()
        {
            _renderPass = new ChemicalCompoundRenderPass();
            _renderPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_renderPass);
        }
    }
}