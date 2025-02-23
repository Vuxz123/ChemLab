using System;
using com.ethnicthv.chemlab.client.unity.renderer;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.formula;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestScript
{
    public class TestScript : MonoBehaviour
    {
        private Formula[] _formulas = new Formula[5];
        private String[] _formulasNames = new String[5];

        public Text formulaName;

        private void Start()
        {
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
            }

            {
                var formula = Formula.CreateNewFormula(new Atom(Element.Chlorine)).AddAtom(new Atom(Element.Hydrogen));

                _formulas[1] = formula;
                _formulasNames[1] = "HCl";
            }

            {
                var formula = Formula.CreateNewFormula(new Atom(Element.Oxygen)).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));

                _formulas[2] = formula;
                _formulasNames[2] = "H2O";
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
            }

            {
                var formula = Formula.CreateNewFormula(new Atom(Element.Nitrogen)).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));
                formula.MoveToAtom(formula.GetStartAtom()).AddAtom(new Atom(Element.Hydrogen));

                _formulas[4] = formula;
                _formulasNames[4] = "NH4";
            }
            
            UpdateRenderEntity();
        }
        
        private int _currentFormula = 0;
        
        public void UpdateRenderEntity()
        {
            formulaName.text = _formulasNames[_currentFormula];
            RenderProgram.Instance.RegisterRenderEntity(_formulas[_currentFormula], Vector3.zero);
            Debug.Log(_formulas[_currentFormula].Serialize());
        }
        
        public void NextFormula()
        {
            RenderProgram.Instance.UnregisterRenderEntity(_formulas[_currentFormula]);
            _currentFormula = (_currentFormula + 1) % _formulas.Length;
            UpdateRenderEntity();
        }
        
        public void PreviousFormula()
        {
            RenderProgram.Instance.UnregisterRenderEntity(_formulas[_currentFormula]);
            _currentFormula = (_currentFormula - 1 + _formulas.Length) % _formulas.Length;
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