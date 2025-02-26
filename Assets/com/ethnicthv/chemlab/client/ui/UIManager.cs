using com.ethnicthv.chemlab.client.api.ui;
using com.ethnicthv.chemlab.client.api.ui.compound;
using com.ethnicthv.chemlab.client.api.ui.element;
using com.ethnicthv.chemlab.client.ui.compound;
using com.ethnicthv.chemlab.client.ui.element;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }
        
        public ICompoundPanelController CompoundPanelController => compoundPanelController;
        public IElementPanelManager ElementPanelManager => elementPanelManager;
        
        [SerializeField] private CompoundPanelController compoundPanelController; 
        [SerializeField] private ElementPanelManager elementPanelManager;
        
        private void Awake()
        {
            Instance = this;
        }
    }
}