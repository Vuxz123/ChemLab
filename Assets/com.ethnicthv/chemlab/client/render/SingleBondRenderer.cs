using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.model;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.render
{
    public class SingleBondRenderer : IRenderer<SingleBondModel>
    {
        public void Render(SingleBondModel renderable, Camera camera)
        {
            var mesh = renderable.GetMesh();
            var matrix = renderable.GetModelMatrix();
            
            Material material = new Material(Shader.Find("Standard"));
            
            // render mesh
            Graphics.DrawMesh(mesh, matrix, material, 0, camera);
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