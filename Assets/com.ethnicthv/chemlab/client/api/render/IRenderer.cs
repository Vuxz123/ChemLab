using UnityEngine;

namespace com.ethnicthv.chemlab.client.api.render
{
    public interface IRenderer<T> where T : IRenderable
    {
        public void Render(T renderable, Camera camera);
        
        public void RenderGizmos(T renderable);
    }
}