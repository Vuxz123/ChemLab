using System.Collections.Generic;
using com.ethnicthv.chemlab.client.api.render;
using com.ethnicthv.chemlab.client.model.bond;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace com.ethnicthv.chemlab.client.unity.renderer.render
{
    public class BondRenderer : IRenderer<BondModel>
    {
        public void Render(BondModel renderable, Stack<Matrix4x4> matricesStack, RenderState renderState)
        {
            matricesStack.Push(renderable.GetModelMatrix());
        }

        public void RenderGizmos(BondModel renderable)
        {
            var mesh = renderable.GetMesh();
            var position = renderable.GetPosition();
            var rotation = renderable.GetRotation();
            
            // render gizmos
            Gizmos.DrawMesh(mesh, position, rotation);
        }
    }
}