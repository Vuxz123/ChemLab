using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.render;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using UnityEngine;

namespace com.ethnicthv.chemlab.client
{
    [ExecuteAlways]
    public class Test : MonoBehaviour
    {
        private SingleBondModel _singleBondModel;
        private GenericAtomModel _atomModel;
        
        private SingleBondRenderer _singleBondRenderer;
        private GenericAtomRenderer _atomRenderer;
        
        private Camera _camera;

        private void Awake()
        {

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            Initialize();
        }

        private void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            _camera = Camera.main;
            Debug.Log(_atomModel.GetAtom().GetElement());
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif
            _atomRenderer.Render(_atomModel, _camera);
            _singleBondRenderer.Render(_singleBondModel, _camera);
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (_atomModel == null || _singleBondModel == null)
            {
                Initialize();
            }

            // Draw the atom model in the scene view
            _atomRenderer.RenderGizmos(_atomModel);
            _singleBondRenderer.RenderGizmos(_singleBondModel);
        }
#endif

        private void Initialize()
        {
            _atomModel = new GenericAtomModel(new Atom(Element.Hydrogen, 0));
            _atomRenderer = new GenericAtomRenderer();
            
            _singleBondModel = new SingleBondModel(1.0f);
            _singleBondRenderer = new SingleBondRenderer();
            _singleBondModel.Position = new Vector3(0, 0, 0);
        }
    }
}