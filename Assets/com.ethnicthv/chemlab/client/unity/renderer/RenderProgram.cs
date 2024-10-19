using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.render;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.formula;
using UnityEngine;
using UnityEngine.Rendering;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public class RenderProgram : MonoBehaviour
    {
        public static RenderProgram Instance { get; private set; }
        
        public Material atomMaterial;
        public Material bondMaterial;
        
        private GenericCompoundModel _compoundModel;
        private GenericCompoundRenderer _compoundRenderer;
        
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
            Formula formula = Formula.CreateNewCarbonFormula().AddAtom(new Atom(Element.Oxygen), Bond.BondType.Double);
            _compoundModel = new GenericCompoundModel(formula, Vector3.zero);
            _compoundRenderer = new GenericCompoundRenderer();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void Render(CommandBuffer commandBuffer, ScriptableRenderContext context)
        {
            _compoundRenderer.Render(_compoundModel, commandBuffer, context);
        }
    }
}