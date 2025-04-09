using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.molecule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.ethnicthv.chemlab.client.ui.menu.newcompound
{
    public class NewCompoundController : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private TMP_InputField formulaInput;
        [SerializeField] private TMP_InputField densityInput;
        [SerializeField] private TMP_InputField blInput;
        [SerializeField] private TMP_InputField dmInput;
        [SerializeField] private TMP_InputField shcInput;
        [SerializeField] private TMP_InputField mhcInput;
        [SerializeField] private TMP_InputField lhInput;
        [SerializeField] private TMP_InputField liquidColorInput;
        [SerializeField] private TMP_InputField burningColorInput;
        [SerializeField] private TMP_InputField burnIntensityInput;
        [SerializeField] private TMP_InputField gasColorInput;
        [SerializeField] private TMP_InputField solidColorInput;
        [SerializeField] private List<GameObject> disableObjects;
        
        [Header("View Control")]
        [SerializeField] private GameObject structureView;
        [SerializeField] private GameObject tagEditorView;
        
        [Header("Tag Editor")]
        [SerializeField] private Toggle tagPrefab;
        [SerializeField] private Transform tagContainer;
        private Dictionary<MoleculeTag, Toggle> _tagToggles = new();
        private Queue<MoleculeTag> _selectedTags = new();

        private void Awake()
        {
            SetupTags();
        }
        
        private void OpenView(int view)
        {
            structureView.SetActive(view == 0);
            tagEditorView.SetActive(view == 1);
        }
        
        private void SetupTags()
        {
            foreach (var moleculeTag in Enum.GetValues(typeof(MoleculeTag)))
            {
                var toggle = Instantiate(tagPrefab, tagContainer);
                toggle.gameObject.SetActive(true);
                toggle.GetComponentInChildren<TextMeshProUGUI>().text = moleculeTag.ToString();
                toggle.isOn = false;
                _tagToggles.Add((MoleculeTag)moleculeTag, toggle);
            }
        }

        public void SubmitTag()
        {
            _selectedTags.Clear();
            foreach (var (moleculeTag, toggle) in _tagToggles)
            {
                if (toggle.isOn)
                {
                    _selectedTags.Enqueue(moleculeTag);
                }
            }
        }
    }
}