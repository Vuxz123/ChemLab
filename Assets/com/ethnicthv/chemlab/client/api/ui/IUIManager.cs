using com.ethnicthv.chemlab.client.api.ui.compound;
using com.ethnicthv.chemlab.client.api.ui.element;

namespace com.ethnicthv.chemlab.client.api.ui
{
    public interface IUIManager
    {
        public ICompoundPanelController CompoundPanelController { get; }
        public IElementPanelManager ElementPanelManager { get; }
    }
}