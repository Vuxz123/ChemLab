using com.ethnicthv.chemlab.client.api.core.game;
using com.ethnicthv.chemlab.client.api.ui;
using UnityEngine;
using UnityEngine.UIElements;

namespace com.ethnicthv.chemlab.client.ui.utility
{
    public class PouringPanelController : MonoBehaviour , IOpenablePanel , ICloseablePanel
    {


        private IMixtureContainer _original;
        private IMixtureContainer _target;
        
        public void SetupPanel(IMixtureContainer original, IMixtureContainer target)
        {
            _original = original;
            _target = target;
        }
        
        public void OpenPanel()
        {
            
        }

        public void ClosePanel()
        {
            throw new System.NotImplementedException();
        }
    }
}