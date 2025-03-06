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
        private readonly Dictionary<Molecule, IContentListItemController> _activeItems = new();
        
        private void Awake()
        {
            _itemPool = new Pool<IContentListItemController>(Factory);
        }

        private void OnEnable()
        {
            ChemicalTickerHandler.AddTicker(this);
        }

        private void OnDisable()
        {
            ChemicalTickerHandler.RemoveTicker(this);
            Reset();
        }

        private void Reset()
        {
            _mixture = null;
            _moleculeAmounts = null;
            var l = new List<Molecule>(_activeItems.Keys);
            foreach (var key in l)
            {
                _itemPool.Return(_activeItems[key]);
                _activeItems.Remove(key);
            }
            UpdateHeight(0);
        }

        public void Setup(IMixture mixture, float volumn)
        {
            Reset();
            _mixture = mixture;
            _volumn = volumn;
            
            if (_mixture == null) return;
            _moleculeAmounts = new OrderedDictionary();

            foreach (var (molecule, moles) in mixture.GetMixtureComposition())
            {
                _moleculeAmounts.Add(molecule, moles);
            }
            
            UpdateList();
        }

        private void UpdateList()
        {
            var mixtureComposition = _mixture.GetMixtureComposition();
            foreach (Molecule molecule in _moleculeAmounts.Keys)
            {
                if (mixtureComposition.ContainsKey(molecule)) continue;
                _moleculeAmounts.Remove(molecule);
                _itemPool.Return(_activeItems[molecule]);
                _activeItems.Remove(molecule);
            }

            foreach (var (molecule, moles) in mixtureComposition)
            {
                if (_moleculeAmounts.Contains(molecule))
                {
                    _moleculeAmounts[molecule] = moles;
                }
                else
                {
                    _moleculeAmounts.Add(molecule, moles);
                }
            }

            var height = 0;
            
            foreach (Molecule molecule in _moleculeAmounts.Keys)
            {
                if (_activeItems.TryGetValue(molecule, out var activeItem))
                {
                    activeItem.Setup(molecule, (float)_moleculeAmounts[molecule] * _volumn);
                }
                else
                {
                    var item = _itemPool.Get();
                    item.Setup(molecule, (float) _moleculeAmounts[molecule] * _volumn);
                    _activeItems.Add(molecule, item);
                }
                
                height += _activeItems[molecule].GetHeight();
            }
            
            UpdateHeight(height);
        }

        private void UpdateHeight(int height)
        {
            itemContainer.sizeDelta = new Vector2(itemContainer.sizeDelta.x, height);
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