using System.Collections.Generic;
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
        private List<Molecule> _molecules;
        private readonly Formula[] _formulas = new Formula[5];
        private readonly string[] _formulasNames = new string[5];

        public TextMeshProUGUI formulaName;

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
                var formula = Formula.CreateNewFormula(new Atom(Element.Chlorine)).AddAtom(new Atom(Element.Hydrogen));

                _formulas[1] = formula;
                _formulasNames[1] = "HCl";
                
                mixtures.Add(Mixture.Pure(Molecule.Builder.Create(true).Structure(formula).Build()), 0.5f);
            }

            {
                var carbon = new Atom(Element.Carbon);
                var formula = Formula.CreateNewFormula(new Atom(Element.Hydrogen))
                    .AddAtom(carbon)
                    .AddAtom(new Atom(Element.Oxygen), Bond.BondType.Double);
                formula.MoveToAtom(carbon)
                    .AddAtom(new Atom(Element.Oxygen))
                    .AddAtom(new Atom(Element.Hydrogen));

                _formulas[3] = formula;
                _formulasNames[3] = "HCOOH";
                
                mixtures.Add(Mixture.Pure(Molecule.Builder.Create(true).Structure(formula).Build()), 0.5f);
            }

            {
                var formula = Formula.CreateNewFormula(new Atom(Element.Nitrogen)).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));

                _formulas[4] = formula;
                _formulasNames[4] = "NH4";
                
                mixtures.Add(Mixture.Pure(Molecule.Builder.Create(true).Structure(formula).Build()), 0.5f);
            }
            
            _mixture = Mixture.Mix(mixtures);
            
            _molecules = new List<Molecule>(_mixture.GetMixtureComposition().Keys);
            
            _mixture.Tick();
            
            UpdateRenderEntity();
        }
        
        private int _currentFormula;
        
        public void UpdateRenderEntity()
        {
            var molecule = _molecules[_currentFormula];
            foreach (var (atom, bonds) in molecule.GetFormula().GetStructure())
            {
                Debug.Log(ElementProperty.GetElementProperty(atom.GetElement()).GetSymbol() + " " + bonds.Count);
            }
            
            UIManager.Instance.CompoundPanelController.SetDisplayedMolecule(molecule);
        }
        
        public void NextFormula()
        {
            _currentFormula = (_currentFormula + 1) % _molecules.Count;
            UpdateRenderEntity();
        }
        
        public void PreviousFormula()
        {
            _currentFormula = (_currentFormula - 1 + _molecules.Count) % _molecules.Count;
            UpdateRenderEntity();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                NextFormula();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                PreviousFormula();
            }
        }
    }
}