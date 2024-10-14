using System;
using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.render;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using UnityEngine;

namespace com.ethnicthv.chemlab.client
{
    [ExecuteInEditMode]
    public class Test : MonoBehaviour
    {
        private GenericAtomModel _atomModel;
        private GenericAtomRenderer _atomRenderer;
        private Camera _camera;

        private void Awake()
        {
            Initialize();
        }
        
        private void Start()
        {
            _camera = Camera.main;
            Debug.Log(_atomModel.GetAtom().GetElement());
        }

        private void Update()
        {
            // do something
            _atomRenderer.Render(_atomModel, _camera);
        }
        
        // This method is called every frame in editor mode
        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                return;
            }
            
            if (_atomModel == null)
            {
                Initialize();
            }

            // Draw the atom model in the scene view
            _atomRenderer.Render(_atomModel, Camera.main);
        }
        
        private void Initialize()
        {
            _atomModel = new GenericAtomModel(new Atom(Element.Hydrogen, 0));
            _atomRenderer = new GenericAtomRenderer();
        }
    }
}