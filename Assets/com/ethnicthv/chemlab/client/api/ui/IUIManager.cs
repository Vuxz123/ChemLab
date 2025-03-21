﻿using com.ethnicthv.chemlab.client.api.ui.compound;
using com.ethnicthv.chemlab.client.api.ui.contents;
using com.ethnicthv.chemlab.client.api.ui.element;
using com.ethnicthv.chemlab.client.api.ui.options;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.api.ui
{
    public interface IUIManager
    {
        public ICompoundPanelController CompoundPanelController { get; }
        public IElementPanelManager ElementPanelManager { get; }
        public IOptionsPanelController OptionsPanelController { get; }
        public IContentPanelController ContentPanelController { get; }
        
        public bool IsHoverPanelOpen();
        public GameObject OpenHoverPanel(GameObject hoverPanelPrefab);
        public void CloseHoverPanel();
    }
}