using System;
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
    public class RenderProcessor
    {
        private readonly PositionCalculator _calculator = new();
        
        private readonly LinkedList<(IFormula, Vector3)> _storageFormulas = new();

        private readonly Dictionary<Element, List<GenericAtomModel>> _atoms = new();
        private readonly List<SingleBondModel> _1Bonds = new();
        private readonly List<DoubleBondModel> _2Bonds = new();
        private readonly List<TripleBondModel> _3Bonds = new();
        
        public void ForeachElement(Action<Element, RenderAtomRenderable> action)
        {
            foreach (var temp in _atoms)
            {
                var (element, atoms) = temp;
                action(element, new RenderAtomRenderable(atoms));
            }
        }
        
        public void ForeachSingleBond(Action<SingleBondModel> action)
        {
            foreach (var bond in _1Bonds)
            {
                action(bond);
            }
        }
        
        public void ForeachDoubleBond(Action<DoubleBondModel> action)
        {
            foreach (var bond in _2Bonds)
            {
                action(bond);
            }
        }
        
        public void ForeachTripleBond(Action<TripleBondModel> action)
        {
            foreach (var bond in _3Bonds)
            {
                action(bond);
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
                var atomsQueue = new Queue<(Atom, GenericAtomModel)>();
                var atomsVisited = new HashSet<Atom>();
                
                var atomModelDict = new Dictionary<Atom, GenericAtomModel>();
                
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
                    var atomModel = new GenericAtomModel(atom)
                    {
                        ParentAtom = prevAtomModel
                    };

                    if (!_atoms.ContainsKey(atom.GetElement()))
                    {
                        _atoms[atom.GetElement()] = new List<GenericAtomModel>();
                    }

                    _atoms[atom.GetElement()].Add(atomModel);

                    foreach (var bond in formula.GetAtomBonds(atom))
                    {
                        //Note: if destination atom is visited, skip
                        if (atomsVisited.Contains(bond.GetDestinationAtom())) continue;
                        
                        atomsQueue.Enqueue((bond.GetDestinationAtom(), atomModel));
                    }
                    
                    atomModelDict[atom] = atomModel;
                    
                    //Note: skip first atom
                    if (prevAtomModel == null)
                    {
                        continue;
                    }
                    
                    //Note: calculate atom position
                    var curARadius = ElementAtomRadius.Radius[atom.GetElement()];
                    var prevARadius = ElementAtomRadius.Radius[prevAtomModel.GetAtom().GetElement()];
                    var distance = prevARadius + curARadius + 1;
                    Debug.Log("Distance: " + distance);
                    
                    var dirVec = _calculator.GetCurrentPosition(formula, atom, prevAtomModel);
                    
                    Debug.Log("DirVec: " + dirVec + " - " + dirVec.magnitude);
                    Debug.Log("Post Mult: " + dirVec * distance + " - " + (dirVec * distance).magnitude);
                    
                    atomModel.Position = prevAtomModel.Position + dirVec * distance;
                    //Note: add offset
                    atomModel.Position += offset;
                }

                var structure = formula.CloneStructure();

                //Note: Generate bonds
                foreach( var (atom, model) in atomModelDict)
                {
                    foreach (var bond in structure[atom])
                    {
                        if (!atomModelDict.ContainsKey(bond.GetDestinationAtom())) 
                            throw new Exception("Missing atom model: " + bond.GetDestinationAtom());
                        
                        var destModel = atomModelDict[bond.GetDestinationAtom()];
                
                        var dirVec = destModel.Position - model.Position;
                        var rotation = Quaternion.FromToRotation(Vector3.up, dirVec);
                        var length = dirVec.magnitude;
                        
                        switch (bond.GetBondType())
                        {
                            case Bond.BondType.Single:
                                _1Bonds.Add(new SingleBondModel(model.Position, rotation, length));
                                break;
                            case Bond.BondType.Double:
                                _2Bonds.Add(new DoubleBondModel(model.Position, rotation, length));
                                break;
                            case Bond.BondType.Triple:
                                _3Bonds.Add(new TripleBondModel(model.Position, rotation, length));
                                break;
                            case Bond.BondType.Aromatic:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        
                        //Note: remove reverse bond from destination atom
                        structure[bond.GetDestinationAtom()].RemoveAll(b => b.GetDestinationAtom() == atom);
                    }
                }
            }
        }
    }
}