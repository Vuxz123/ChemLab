using System.Collections.Generic;
using com.ethnicthv.chemlab.client.model.bond;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace com.ethnicthv.chemlab.client.unity.renderer.pass
{
    public class ChemicalCompoundRenderPass : ScriptableRenderPass
    {
        private readonly Material _atomMaterial;
        private readonly Material _bondMaterial;

        private readonly Mesh _atomMesh;
        private readonly Mesh _oneBondMesh;
        private readonly Mesh _twoBondMesh;
        private readonly Mesh _threeBondMesh;

        public ChemicalCompoundRenderPass(Material atomMaterial, Material bondMaterial, Mesh atomMesh, Mesh oneBondMesh, Mesh twoBondMesh, Mesh threeBondMesh)
        {
            _atomMaterial = atomMaterial;
            _bondMaterial = bondMaterial;
            _atomMesh = atomMesh;
            _oneBondMesh = oneBondMesh;
            _twoBondMesh = twoBondMesh;
            _threeBondMesh = threeBondMesh;
        }

        private class PassData
        {
            public Stack<Matrix4x4> MatricesStack0;
            public Stack<Matrix4x4> MatricesStack1;
            public Stack<Matrix4x4> MatricesStack2;
            public Stack<Matrix4x4> MatricesStack3;
            public Mesh AtomMesh;
            public Mesh OneBondMesh;
            public Mesh TwoBondMesh;
            public Mesh ThreeBondMesh;
            public Material AtomMaterial;
            public Material BondMaterial;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
        {
            using (var builder = renderGraph
                       .AddRasterRenderPass<PassData>("ChemicalCompoundRenderPass", out var passData))
            {
                if (RenderProgram.Instance != null)
                {
                    RenderProgram.Instance.CheckModelMatrix();
                    
                    passData.AtomMaterial = _atomMaterial;
                    passData.BondMaterial = _bondMaterial;
                    passData.AtomMesh = _atomMesh;
                    passData.OneBondMesh = _oneBondMesh;
                    passData.TwoBondMesh = _twoBondMesh;
                    passData.ThreeBondMesh = _threeBondMesh;
                    passData.MatricesStack0 = new Stack<Matrix4x4>();
                    passData.MatricesStack1 = new Stack<Matrix4x4>();
                    passData.MatricesStack2 = new Stack<Matrix4x4>();
                    passData.MatricesStack3 = new Stack<Matrix4x4>();
                    
                    var volumeComponent =
                        VolumeManager.instance.stack.GetComponent<ChemicalCompoundVolume>();
                    
                    //Note: setting the bond radius
                    BondModel.BondRadius = volumeComponent.bondRadius.value;

                    RenderProgram.Instance.RenderAtom(passData.MatricesStack0);
                    RenderProgram.Instance.RenderBond(
                        passData.MatricesStack1, passData.MatricesStack2, passData.MatricesStack3);

                    builder.AllowPassCulling(false);
                }

                var resourceData = frameData.Get<UniversalResourceData>();

                builder.SetRenderAttachment(resourceData.activeColorTexture, 0);
                builder.SetRenderAttachmentDepth(resourceData.activeDepthTexture);
                builder.SetRenderFunc((PassData data, RasterGraphContext context) => ExecutePass(data, context));
            }
        }

        private static void ExecutePass(PassData data, RasterGraphContext context)
        {
            if (RenderProgram.Instance == null) return;

            var cmd = context.cmd;

            if (data.AtomMaterial != null && data.AtomMesh != null)
            {
                cmd.DrawMeshInstanced(data.AtomMesh, 0, data.AtomMaterial, 1, data.MatricesStack0.ToArray());
            }

            if (data.BondMaterial != null && data.OneBondMesh != null && data.TwoBondMesh != null && data.ThreeBondMesh != null)
            {
                // Debug.Log("Drawing bonds : one = " + data.MatricesStack1.Count + ", two = " + data.MatricesStack2.Count + ", three = " + data.MatricesStack3.Count + ".");
                cmd.DrawMeshInstanced(data.OneBondMesh, 0, data.BondMaterial, 0, data.MatricesStack1.ToArray());
                cmd.DrawMeshInstanced(data.TwoBondMesh, 0, data.BondMaterial, 0, data.MatricesStack2.ToArray());
                cmd.DrawMeshInstanced(data.ThreeBondMesh, 0, data.BondMaterial, 0, data.MatricesStack3.ToArray());
            }
        }
    }
}