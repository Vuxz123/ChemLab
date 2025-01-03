using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.unity.renderer.render;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.formula;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public class RenderProgram : MonoBehaviour
    {
        public static RenderProgram Instance { get; private set; }

        private RenderProcesser _renderProcesser = new();

        public Mesh atomMesh;

        public Material atomMaterial;
        public Material bondMaterial;

        private BondRenderer _bondRenderer = new();
        private GenericAtomRenderer _atomRenderer = new();

        private bool isDirty = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            var formula = Formula.CreateNewCarbonFormula().AddAtom(new Atom(Element.Carbon));
            var start = formula.GetStartAtom();
            formula.MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                .AddAtom(new Atom(Element.Carbon))
                .AddAtom(new Atom(Element.Carbon));
            start = formula.GetCurrentAtom();
            formula.MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                .MoveToAtom(start).AddAtom(new Atom(Element.Carbon));
            
            RegisterRenderEntity(formula, Vector3.zero);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void RegisterRenderEntity(Formula formula, Vector3 offset)
        {
            _renderProcesser.AddFormula(formula, offset);
            isDirty = true;
        }

        public void RenderCompound(RasterCommandBuffer commandBuffer, RasterGraphContext context)
        {
            _renderProcesser.ForeachElement((element, renderable) =>
            {
                _atomRenderer.Render(renderable, commandBuffer, context);
            });
        }

        public void CheckModelMatrix()
        {
            if (!isDirty) return;
            _renderProcesser.Refresh();
            _renderProcesser.Recalculate();
            isDirty = false;
        }
    }
}