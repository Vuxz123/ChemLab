using com.ethnicthv.chemlab.client.api.ui;
using com.ethnicthv.chemlab.client.api.ui.compound;
using com.ethnicthv.chemlab.client.ui.compound;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui
{
    public class UIManager : MonoBehaviour, IUIManager
    {
        public static IUIManager Instance { get; private set; }
        
        public ICompoundPanelController CompoundPanelController => compoundPanelController;
        
        [SerializeField] private CompoundPanelController compoundPanelController; 
        
        private void Awake()
        {
            Instance = this;
        }
    }
}