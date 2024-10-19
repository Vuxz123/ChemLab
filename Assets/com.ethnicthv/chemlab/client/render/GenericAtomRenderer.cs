using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.unity.renderer;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ethnicthv.chemlab.client.render
{
    public class GenericAtomRenderer : IRenderer<IAtomModel>
    {
        public void Render(IAtomModel atomModel, CommandBuffer commandBuffer, ScriptableRenderContext context)
        {
            var mesh = atomModel.GetMesh();
            var matrix = atomModel.GetModelMatrix();
            
            // render mesh
            commandBuffer.DrawMesh(mesh, matrix, RenderProgram.Instance.atomMaterial, 0, 0);
        }

        public void RenderGizmos(IAtomModel renderable)
        {
            var mesh = renderable.GetMesh();
            var position = renderable.GetPosition();
            var rotation = renderable.GetRotation();
            
            // render gizmos
            Gizmos.DrawMesh(mesh, position, rotation);
        }
    }
}