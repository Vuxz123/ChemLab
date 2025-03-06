using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.client.api.core.game;
using com.ethnicthv.chemlab.client.chemistry;
using com.ethnicthv.chemlab.client.core.game;
using com.ethnicthv.chemlab.client.ui;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api;
using com.ethnicthv.chemlab.engine.api.mixture;
using com.ethnicthv.chemlab.engine.mixture;
using com.ethnicthv.chemlab.engine.molecule;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.game
{
    public class BottleBehaviour : MonoBehaviour, IInteractable, IMixtureContainer, IChemicalTicker
    {
        [SerializeField] private float maxVolume = 1f;
        [SerializeField] private GameObject fillerPrefab;
        [SerializeField] private List<SpriteRenderer> fillers;
        [SerializeField] private Transform fillersParent;
        
        private Mixture _contents;
        private float _volumn;

        private readonly Dictionary<SpriteRenderer, LiquidPart> _fillerParts = new();
        
        private static readonly int FillThreshold = Shader.PropertyToID("_FillThreshold");
        private static readonly int Fill = Shader.PropertyToID("_Fill");

        private void OnEnable()
        {
            InteractableManager.RegisterInteractable(gameObject, this);
            ChemicalTickerHandler.AddTicker(this);
        }

        private void OnDisable()
        {
            InteractableManager.UnregisterInteractable(gameObject);
            ChemicalTickerHandler.RemoveTicker(this);
        }

        private SpriteRenderer CreateFiller()
        {
            var filler = Instantiate(fillerPrefab, fillersParent);
            fillers.Add(filler.GetComponent<SpriteRenderer>());
            return filler.GetComponent<SpriteRenderer>();
        }

        private void UpdateLiquidContent()
        {
            var phases = _contents.SeparatePhases(maxVolume);
            phases.LiquidMixture.UpdateColor();
            var color = phases.LiquidMixture.GetColor();
            var fill = phases.LiquidVolume / maxVolume;
            LiquidPart part = new LiquidPart {height = fill, color = color};
            _fillerParts[CreateFiller()] = part;
            
            UpdateLiquidDisplay();
        }

        private void UpdateLiquidDisplay()
        {
            //Note: Sort the fillers by the height of the fillers from the lowest to the highest
            fillers.Sort((a, b) => _fillerParts[a].height.CompareTo(_fillerParts[b].height));

            var prevFill = 0f;
            foreach (var f in fillers)
            {
                var part = _fillerParts[f];
                UpdateFiller(f, part.height, prevFill, part.color);
                prevFill = part.height;
            }
        }

        private void UpdateFiller(SpriteRenderer display, float fillAmount, float threshold, Color color)
        {
            display.material.SetFloat(Fill, fillAmount);
            display.material.SetFloat(FillThreshold, threshold);
            display.color = color;
        }

        public void OnInteract()
        {
            Debug.Log("Interacted with bottle");
        }

        public List<(string name, Action onClick)> GetOptions()
        {
            return new List<(string name, Action onClick)>()
            {
                ("View Content", ViewContent),
                ("test2", () => Debug.Log("test2")),
                ("test3", () => Debug.Log("test3"))
            };
        }

        public void OnHover()
        {
        }

        public GameObject GetHoverPanel()
        {
            return null;
        }

        public Transform GetMainTransform()
        {
            return transform.parent;
        }

        public void OnDrop(GameObject other)
        {
            if (other == null) return;
            Debug.Log("Dropped on " + other.name);
        }

        public List<(string name, Action onClick)> GetDropOptions(GameObject other)
        {
            return new List<(string name, Action onClick)>()
            {
                ("test1", () => Debug.Log("test1: " + other.name)),
                ("test2", () => Debug.Log("test2: " + other.name)),
                ("test3", () => Debug.Log("test3: " + other.name))
            };
        }

        public float GetVolumn()
        {
            return _volumn;
        }

        public void SetVolumn(float volumn)
        {
            _volumn = volumn;
        }

        public Mixture GetMixture()
        {
            return _contents;
        }
        
        public void SetMixture(Mixture mixture)
        {
            _contents = mixture;
            UpdateLiquidContent();
        }

        public void SetMixtureAndVolumn(Mixture mixture, float volumn)
        {
            SetMixture(mixture);
            SetVolumn(volumn);
        }

        public (Mixture mixture, float volumn) GetMixtureAndVolumn()
        {
            return (_contents, _volumn);
        }

        public void Tick()
        {
            _contents?.Tick();
        }

        private void ViewContent()
        {
            UIManager.Instance.ContentPanelController.SetupMixtureToDisplay(_contents, _volumn);
            UIManager.Instance.ContentPanelController.OpenPanel();
        }
    }
}