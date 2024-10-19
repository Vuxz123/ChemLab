using System.Collections.Generic;
using com.ethnicthv.chemlab.client.model.bond;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.formula;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.model
{
    public class GenericCompoundModel : GroupModel
    {
        private Vector3 _offset;
        
        private Dictionary<Element, List<GenericAtomModel>> _atoms = new();
        private List<SingleBondModel> _1Bonds = new();
        private List<DoubleBondModel> _2Bonds = new();
        private List<TripleBondModel> _3Bonds = new();

        public GenericCompoundModel(Formula formula, Vector3 offset)
        {
            _offset = offset;
            SetupModel(formula);
        }

        public override Vector3 GetPosition()
        {
            return _offset;
        }

        public override Quaternion GetRotation()
        {
            return Quaternion.identity;
        }

        public override Matrix4x4 GetModelMatrix()
        {
            throw new System.NotImplementedException();
        }
        
        public Dictionary<Element, List<GenericAtomModel>> GetAtoms()
        {
            return _atoms;
        }
        
        public List<SingleBondModel> Get1Bonds()
        {
            return _1Bonds;
        }
        
        public List<DoubleBondModel> Get2Bonds()
        {
            return _2Bonds;
        }
        
        public List<TripleBondModel> Get3Bonds()
        {
            return _3Bonds;
        }
        
        private void SetupModel(IFormula formula)
        {
            var atoms = formula.GetAtoms();
            foreach (var a in atoms)
            {
                var element = a.GetElement();
                if (!_atoms.ContainsKey(element))
                {
                    _atoms[element] = new List<GenericAtomModel>();
                }
                _atoms[element].Add(new GenericAtomModel(a));
            }

            CalculateAtomPositions();
            AddBondModel();
        }
        
        private void CalculateAtomPositions()
        {
            var curPos = new Vector3(0, 0, 0);
            foreach (var (_, list) in _atoms)
            {
                foreach (var atomModel in list)
                {
                    atomModel.Position = curPos;
                    curPos += new Vector3(3, 0, 0);
                }
                curPos += new Vector3(0, 3, 0);
            }
        }
        
        private void AddBondModel()
        {
        }
    }
}