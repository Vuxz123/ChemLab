using com.ethnicthv.chemlab.client.api.ui.utility;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui.utility
{
    public class UtilityUIManager : MonoBehaviour
    {
        public IPouringPanelController PouringPanelController => pouringPanelController;
        
        [SerializeField] private PouringPanelController pouringPanelController;
    }
}