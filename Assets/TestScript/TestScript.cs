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

        public BottleBehaviour bottleBehaviour; 
        public SolidHolderBehaviour solidHolderBehaviour;

        private void Start()
        {
            var mixtures = new Dictionary<Mixture, float>()
            {
                { Mixture.Pure(Molecules.Water), 0.9f }, 
                { Mixture.Pure(Molecules.SulfuricAcid), 0.1f }
            };

            var (mixture, volume) = Mixture.Mix(mixtures);
            
            bottleBehaviour.SetVolume(volume);
            bottleBehaviour.SetMixture(mixture);
            solidHolderBehaviour.AddSolidMolecule(Molecules.Copper, 0.2f);
        }
    }
}