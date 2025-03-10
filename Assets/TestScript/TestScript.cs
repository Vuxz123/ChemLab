﻿using System.Collections.Generic;
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
                { Mixture.Pure(Molecules.Water), 0.5f }, { Mixture.Pure(Molecules.SulfuricAcid), 0.5f }
            };
            
            {
                var formula = Formula.CreateNewCarbonFormula().AddAtom(new Atom(Element.Hydrogen));
                var start = formula.GetStartAtom();
                formula.MoveToAtom(start).AddAtom(new Atom(Element.Hydrogen))
                    .MoveToAtom(start).AddAtom(new Atom(Element.Hydrogen))
                    .MoveToAtom(start).AddAtom(new Atom(Element.Carbon))
                    .AddAtom(new Atom(Element.Carbon), Bond.BondType.Triple)
                    .AddAtom(new Atom(Element.Carbon));
                start = formula.GetCurrentAtom();
                formula.MoveToAtom(start).AddAtom(new Atom(Element.Hydrogen))
                    .MoveToAtom(start).AddAtom(new Atom(Element.Hydrogen))
                    .MoveToAtom(start).AddAtom(new Atom(Element.Hydrogen));

                _formulas[0] = formula;
                _formulasNames[0] = "C4H6";
                
                mixtures.Add(Mixture.Pure(Molecule.Builder.Create(true).Structure(formula).Build()), 0.5f);
            }

            {
                _formulasNames[1] = "HCl";
                
                mixtures.Add(Mixture.Pure(Molecules.HydrochloricAcid), 0.5f);
            }

            {
                _formulasNames[3] = "HCOOH";
                
                mixtures.Add(Mixture.Pure(Molecules.AceticAcid), 0.5f);
            }

            {
                _formulasNames[4] = "NH4";
                
                mixtures.Add(Mixture.Pure(Molecules.Ammonium), 0.5f);
            }
            
            _mixture = Mixture.Mix(mixtures);
            
            bottleBehaviour.SetMixture(_mixture);
            bottleBehaviour.SetVolumn(1);
        }
    }
}