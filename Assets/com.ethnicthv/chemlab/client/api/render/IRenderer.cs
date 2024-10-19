using UnityEngine;
using UnityEngine.Rendering;

namespace com.ethnicthv.chemlab.client.api.render
{
    public interface IRenderer<T> where T : IRenderable
    {
        public void Render(T renderable, CommandBuffer commandBuffer, ScriptableRenderContext context);
        
        public void RenderGizmos(T renderable);
    }
}