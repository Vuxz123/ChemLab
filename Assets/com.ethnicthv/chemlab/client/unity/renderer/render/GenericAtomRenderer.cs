using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.unity.renderer.type;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace com.ethnicthv.chemlab.client.unity.renderer.render
{
    public class GenericAtomRenderer : IRenderer<RenderAtomRenderable>
    {
        public void Render(RenderAtomRenderable atomModel, RasterCommandBuffer commandBuffer, RasterGraphContext context)
        {
            var atoms = atomModel.Atoms;
            var atomsCount = atoms.Count;
            var matrices = new Matrix4x4[atomsCount];

            for (var i = 0; i < atomsCount; i++)
            {
                matrices[i] = atoms[i].GetModelMatrix();
            }
            Debug.Log(atoms[0].GetMesh());
            Debug.Log(matrices[0]);
            
            commandBuffer.DrawMeshInstanced(atoms[0].GetMesh(), 0, RenderProgram.Instance.atomMaterial, -1, matrices);
        }

        public void RenderGizmos(RenderAtomRenderable renderable)
        {
        }
    }
}