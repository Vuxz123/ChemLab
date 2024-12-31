using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace com.ethnicthv.chemlab.client.api.render
{
    public interface IRenderer<T> where T : IRenderable
    {
        public void Render(T renderable, RasterCommandBuffer commandBuffer, RasterGraphContext context);
        
        public void RenderGizmos(T renderable);
    }
}