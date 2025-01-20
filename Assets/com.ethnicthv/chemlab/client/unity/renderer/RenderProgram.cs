using System.Collections.Generic;
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
    
    [ExecuteInEditMode]
    public class RenderProgram : MonoBehaviour
    {
        // <-- program properties -->
        public static RenderProgram Instance { get; private set; }

        private RenderProcessor _renderProcessor = new();
        // <-- end of program properties -->

        // <-- renderers -->
        private BondRenderer _bondRenderer = new();
        private GenericAtomRenderer _atomRenderer = new();
        // <-- end of renderers -->

        // <-- state -->
        private bool _isDirty = false;
        // <-- end of state -->
        
        // <-- shader properties -->
        private static readonly int Color = Shader.PropertyToID("_AtomColor");
        // <-- end of shader properties -->

        private void Awake()
        {
            Debug.Log("RenderProgram Awake");
            if (Instance == null)
            {
                Instance = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            Debug.Log("RenderProgram Start");
            // var formula = Formula.CreateNewCarbonFormula().AddAtom(new Atom(Element.Carbon));
            // var start = formula.GetStartAtom();
            // formula.MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
            //     .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
            //     .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
            //     .AddAtom(new Atom(Element.Carbon))
            //     .AddAtom(new Atom(Element.Carbon));
            // start = formula.GetCurrentAtom();
            // formula.MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
            //     .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
            //     .MoveToAtom(start).AddAtom(new Atom(Element.Carbon));

            var formula = Formula.CreateNewFormula(new Atom(Element.Chlorine)).AddAtom(new Atom(Element.Hydrogen));
            
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
            _renderProcessor.AddFormula(formula, offset);
            _isDirty = true;
        }

        public void RenderAtom(Stack<Matrix4x4> matricesStack, RenderState state = RenderState.Opaque)
        {
            _renderProcessor.ForeachElement((element, renderable) =>
            {
                _atomRenderer.Render(renderable, matricesStack, state);
            });
        }
        
        public void RenderBond(
            Stack<Matrix4x4> matricesStack1, 
            Stack<Matrix4x4> matricesStack2, 
            Stack<Matrix4x4> matricesStack3, 
            RenderState state = RenderState.Opaque)
        {
            _renderProcessor.ForeachSingleBond(model =>
            {
                _bondRenderer.Render(model, matricesStack1, state);
            });
            
            _renderProcessor.ForeachDoubleBond(model =>
            {
                _bondRenderer.Render(model, matricesStack2, state);
            });
            
            _renderProcessor.ForeachTripleBond(model =>
            {
                _bondRenderer.Render(model, matricesStack3, state);
            });
        }

        public void CheckModelMatrix()
        {
            if (!_isDirty) return;
            _renderProcessor.Refresh();
            _renderProcessor.Recalculate();
            _isDirty = false;
        }
    }

    public enum RenderState
    {
        Opaque,
        Depth
    }
}