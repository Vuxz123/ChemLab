using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using com.ethnicthv.chemlab.client.api.ui.contents;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.molecule;
using com.ethnicthv.util.pool;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.ui.contents
{
    public class ContentListController : MonoBehaviour, IContentListController, IChemicalTicker
    {
        [SerializeField] private GameObject contentListItemPrefab;
        [SerializeField] private RectTransform itemContainer;

        private IMixture _mixture;
        private float _volumn;
        private OrderedDictionary _moleculeAmounts;
        
        private Pool<IContentListItemController> _itemPool;
        private Queue<IContentListItemController> _activeItems;
        
        private void Awake()
        {
            _itemPool = new Pool<IContentListItemController>(Factory);
            _activeItems = new Queue<IContentListItemController>();
        }

        private void OnEnable()
        {
            ChemicalTickerHandler.AddTicker(this);
        }

        private void OnDisable()
        {
            ChemicalTickerHandler.RemoveTicker(this);
            _mixture = null;
        }

        public void Setup(IMixture mixture, float volumn)
        {
            _mixture = mixture;
            _volumn = volumn;
            if (_mixture == null) return;
            _moleculeAmounts = new OrderedDictionary();

            foreach (var (molecule, moles) in mixture.GetMixtureComposition())
            {
                _moleculeAmounts[molecule] = moles;
            }
            
            UpdateList();
        }

        public void UpdateList()
        {
            while (_activeItems.TryDequeue(out var item))
            {
                _itemPool.Return(item);
            }

            var newDict = _mixture.GetMixtureComposition();
            foreach (var (molecule, mole) in newDict)
            {
                _moleculeAmounts[molecule] = mole;
            }
            
            foreach (Molecule molecule in _moleculeAmounts.Keys)
            {
                var mole = (float) _moleculeAmounts[molecule];
                var item = _itemPool.Get();
                item.Setup(molecule, _volumn * mole);
                _activeItems.Enqueue(item);
            }
        }
        
        private IContentListItemController Factory()
        {
            var item = Instantiate(contentListItemPrefab, itemContainer).GetComponent<ContentListItemController>();
            return item;
        }

        public void Tick()
        {
            if (_mixture == null) return;
            UpdateList();
        }
    }
}