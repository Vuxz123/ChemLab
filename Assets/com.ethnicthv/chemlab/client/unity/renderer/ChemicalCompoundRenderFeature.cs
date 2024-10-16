using System.Drawing;
using com.ethnicthv.chemlab.client.api.render;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public class ChemicalCompoundRenderFeature : ScriptableRendererFeature
    {
        private class ChemicalCompoundRenderPass : ScriptableRenderPass
        {
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                var cmd = new CommandBuffer();
                
                

                context.ExecuteCommandBuffer(cmd);
                cmd.Release();

                var camera = renderingData.cameraData.camera;
                
            }
        }
        
        private ChemicalCompoundRenderPass _renderPass;

        public override void Create()
        {
            _renderPass = new ChemicalCompoundRenderPass();
            _renderPass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_renderPass);
        }
    }
}