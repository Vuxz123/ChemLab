using System.Collections.Generic;
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
        public void Render(RenderAtomRenderable atomModel, Stack<Matrix4x4> matricesStack, RenderState renderState)
        {
            var atoms = atomModel.Atoms;
            var atomsCount = atoms.Count;

            for (var i = 0; i < atomsCount; i++)
            {
                matricesStack.Push(atoms[i].GetModelMatrix());
            }
        }

        public void RenderGizmos(RenderAtomRenderable renderable)
        {
        }
    }
}