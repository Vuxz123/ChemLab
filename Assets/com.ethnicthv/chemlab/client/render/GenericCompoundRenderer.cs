using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.unity.renderer;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ethnicthv.chemlab.client.render
{
    public class GenericCompoundRenderer : IRenderer<GenericCompoundModel>
    {
        private GenericAtomRenderer _atomRenderer = new();
        private BondRenderer _bondRenderer = new();

        public void Render(GenericCompoundModel renderable, CommandBuffer commandBuffer, ScriptableRenderContext context)
        {
            foreach (var (ele, models) in renderable.GetAtoms())
            {
                var matrixList = new Matrix4x4[models.Count];
                for (var i = 0; i < models.Count; i++)
                {
                    matrixList[i] = models[i].GetModelMatrix();
                }

                commandBuffer.DrawMeshInstanced(models[0].GetMesh(), 0, RenderProgram.Instance.atomMaterial, 0, matrixList);
            }
        }

        public void RenderGizmos(GenericCompoundModel renderable)
        {
            Gizmos.DrawSphere(Vector3.zero, 5.0f);
        }
    }
}