using System.Collections.Generic;
using System.Linq;
using com.ethnicthv.chemlab.client.api.ui.element;
using com.ethnicthv.chemlab.client.game;
using com.ethnicthv.chemlab.client.ui;
using com.ethnicthv.chemlab.engine.api;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.formula;
using com.ethnicthv.chemlab.engine.mixture;
using com.ethnicthv.chemlab.engine.molecule;
using TMPro;
using UnityEngine;

namespace TestScript
{
    public class TestScript : MonoBehaviour
    {
        private Mixture _mixture;
        private readonly Formula[] _formulas = new Formula[5];
        private readonly string[] _formulasNames = new string[5];

        public BottleBehaviour bottleBehaviour; 

        private void Start()
        {
            var mixtures = new Dictionary<Mixture, float>()
            {
                { Mixture.Pure(Molecules.Water), 0.5f }, { Mixture.Pure(Molecules.SulfuricAcid), 0.1f }
            };

            {
                _formulasNames[1] = "HCl";
                
                mixtures.Add(Mixture.Pure(Molecules.HydrochloricAcid), 0.2f);
            }

            {
                _formulasNames[3] = "HCOOH";
                
                mixtures.Add(Mixture.Pure(Molecules.AceticAcid), 0.2f);
            }
            
            _mixture = Mixture.Mix(mixtures);
            
            bottleBehaviour.SetVolume(1);
            bottleBehaviour.SetMixture(_mixture);
            bottleBehaviour.AddSolidMolecule(Molecules.Sodium, 0.2f);
            bottleBehaviour.AddSolidMolecule(Molecules.Copper, 0.2f);
        }
    }
}