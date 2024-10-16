using com.ethnicthv.chemlab.client.api.model;
using com.ethnicthv.chemlab.client.api.render;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.render
{
    public class GenericAtomRenderer : IRenderer<IAtomModel>
    {
        public void Render(IAtomModel atomModel, Camera camera)
        {
            var mesh = atomModel.GetMesh();
            var matrix = atomModel.GetModelMatrix();
            
            Material material = new Material(Shader.Find("Standard"));
            
            // render mesh
            Graphics.DrawMesh(mesh, matrix, material, 0, camera);
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