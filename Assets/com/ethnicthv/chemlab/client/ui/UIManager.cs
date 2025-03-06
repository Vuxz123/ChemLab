using com.ethnicthv.chemlab.client.api.ui;
using com.ethnicthv.chemlab.client.api.ui.compound;
using com.ethnicthv.chemlab.client.api.ui.contents;
using com.ethnicthv.chemlab.client.api.ui.element;
using com.ethnicthv.chemlab.client.api.ui.options;
using com.ethnicthv.chemlab.client.ui.compound;
using com.ethnicthv.chemlab.client.ui.contents;
using com.ethnicthv.chemlab.client.ui.element;
using com.ethnicthv.chemlab.client.ui.options;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }
        
        public ICompoundPanelController CompoundPanelController => compoundPanelController;
        public IElementPanelManager ElementPanelManager => elementPanelManager;
        public IOptionsPanelController OptionsPanelController => optionsPanelController;
        public IContentPanelController ContentPanelController => contentPanelController;
        
        
        [SerializeField] private CompoundPanelController compoundPanelController; 
        [SerializeField] private ElementPanelManager elementPanelManager;
        [SerializeField] private OptionsPanelController optionsPanelController;
        [SerializeField] private ContentPanelController contentPanelController;
        
        private void Awake()
        {
            Instance = this;
        }
    }
}