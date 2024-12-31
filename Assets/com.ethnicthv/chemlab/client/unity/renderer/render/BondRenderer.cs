using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.model.bond;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace com.ethnicthv.chemlab.client.unity.renderer.render
{
    public class BondRenderer : IRenderer<SingleBondModel>
    {
        public void Render(SingleBondModel renderable, RasterCommandBuffer commandBuffer, RasterGraphContext context)
        {
            var mesh = renderable.GetMesh();
            var matrix = renderable.GetModelMatrix();
            
            // render mesh
            commandBuffer.DrawMesh(mesh, matrix, RenderProgram.Instance.bondMaterial, 0, 0);
        }

        public void RenderGizmos(SingleBondModel renderable)
        {
            var mesh = renderable.GetMesh();
            var position = renderable.GetPosition();
            var rotation = renderable.GetRotation();
            
            // render gizmos
            Gizmos.DrawMesh(mesh, position, rotation);
        }
    }
}