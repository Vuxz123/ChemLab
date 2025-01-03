using System.Collections.Generic;
using com.ethnicthv.chemlab.client.model;
using com.ethnicthv.chemlab.client.model.bond;
using com.ethnicthv.chemlab.client.model.position;
using com.ethnicthv.chemlab.client.unity.renderer.type;
using com.ethnicthv.chemlab.engine;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using UnityEngine;

namespace com.ethnicthv.chemlab.client.unity.renderer
{
    public delegate void ForeachElementDelegate(Element element, RenderAtomRenderable renderable);
    public class RenderProcesser
    {
        private PositionCalculator _calculator = new();
        
        private readonly LinkedList<(IFormula, Vector3)> _storageFormulas = new();

        private readonly Dictionary<Element, List<GenericAtomModel>> _atoms = new();
        private readonly List<SingleBondModel> _1Bonds = new();
        private readonly List<DoubleBondModel> _2Bonds = new();
        private readonly List<TripleBondModel> _3Bonds = new();
        
        public void ForeachElement(ForeachElementDelegate action)
        {
            foreach (var temp in _atoms)
            {
                var (element, atoms) = temp;
                action(element, new RenderAtomRenderable(atoms));
            }
        }

        public void AddFormula(IFormula formula, Vector3 offset)
        {
            _storageFormulas.AddLast((formula, offset));
            Refresh();
        }

        public void RemoveFormula(IFormula formula)
        {
            var node = _storageFormulas.First;
            while (node != null)
            {
                if (node.Value.Item1 == formula)
                {
                    _storageFormulas.Remove(node);
                    break;
                }

                node = node.Next;
            }

            Refresh();
        }

        public void Clear()
        {
            _storageFormulas.Clear();
        }

        public void Refresh()
        {
            _atoms.Clear();
            _1Bonds.Clear();
            _2Bonds.Clear();
            _3Bonds.Clear();
        }

        public void Recalculate()
        {
            
            foreach (var temp in _storageFormulas)
            {
                var (formula, offset) = temp;
                var structure = formula.GetStructure();
                var atomsQueue = new Queue<(Atom, GenericAtomModel)>();
                var atomsVisited = new HashSet<Atom>();
                
                atomsQueue.Enqueue((formula.GetStartAtom(), null));

                while (atomsQueue.TryDequeue(out var value))
                {
                    var (atom, prevAtomModel) = value;
                    
                    //Note: Check for visited atoms
                    if (atomsVisited.Contains(atom))
                    {
                        continue;
                    }

                    atomsVisited.Add(atom);

                    //Note: Main logic
                    var atomModel = new GenericAtomModel(atom);
                    atomModel.ParentAtom = prevAtomModel;
                    
                    if (!_atoms.ContainsKey(atom.GetElement()))
                    {
                        _atoms[atom.GetElement()] = new List<GenericAtomModel>();
                    }

                    _atoms[atom.GetElement()].Add(atomModel);

                    foreach (var bond in structure[atom])
                    {
                        //Note: if destination atom is visited, skip
                        if (atomsVisited.Contains(bond.GetDestinationAtom())) continue;
                        
                        atomsQueue.Enqueue((bond.GetDestinationAtom(), atomModel));

                        //Note: else add the bond
                        if (bond.GetBondType() == Bond.BondType.Single)
                        {
                            _1Bonds.Add(new SingleBondModel(2));
                        }
                        else if (bond.GetBondType() == Bond.BondType.Double)
                        {
                            _2Bonds.Add(new DoubleBondModel(2));
                        }
                        else if (bond.GetBondType() == Bond.BondType.Triple)
                        {
                            _3Bonds.Add(new TripleBondModel(2));
                        }
                    }
                    
                    //Note: skip first atom
                    if (prevAtomModel == null)
                    {
                        continue;
                    }
                    
                    //Note: calculate atom position
                    atomModel.Position = prevAtomModel.Position + _calculator.GetCurrentPosition(formula, atom, prevAtomModel) * 2;
                    Debug.Log("Atom position: " + atomModel.Position);
                }
            }
        }
    }
}