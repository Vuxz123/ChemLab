using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.element;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.util;
using Unity.VisualScripting;
using UnityEngine;

namespace com.ethnicthv.chemlab.engine.formula
{
    
    public class Formula : IFormula
    {
        
        private readonly Dictionary<Atom, List<Bond>> _structure;

        // <-- mutate by adding Atom to the structure -->
        private Atom _startAtom;
        private float _mass;
        // <-- end -->

        // <-- mutate by adding structure to the formula -->
        private readonly List<FormulaRing> _rings;
        // <-- end -->

        public bool IsAromatic => _rings.Any(ring => ring.IsAromatic);
        public bool IsCyclic => _rings.Any();

        private Atom _currentAtom;

        private Formula()
        {
            _startAtom = null;
            _mass = 0;
            _structure = new Dictionary<Atom, List<Bond>>();
            _rings = new List<FormulaRing>();
        }

        private Formula(Dictionary<Atom, List<Bond>> structure, Atom startAtom)
        {
            _mass = 0;
            _structure = structure;
            _startAtom = startAtom;
        }

        public object Clone()
        {
            var formula = new Formula();
            foreach (var (key, value) in _structure)
            {
                formula._structure[key] = new List<Bond>(value);
            }

            foreach (var ring in _rings)
            {
                formula._rings.Add(ring.CloneRing(formula));
            }

            formula._startAtom = _startAtom;
            formula._mass = _mass;

            return formula;
        }

        #region Builder

        public Atom GetCurrentAtom()
        {
            return _currentAtom;
        }

        public static Formula CreateInstance(Dictionary<Atom, List<Bond>> structure, Atom startAtom)
        {
            return new Formula(structure, startAtom);
        }

        public static Formula CreateNewFormula(Atom startAtom)
        {
            var formula = new Formula
            {
                _startAtom = startAtom,
                _currentAtom = startAtom
            };
            formula._structure.Add(startAtom, new List<Bond>());
            Debug.Log(formula._structure.ContainsKey(startAtom));
            return formula;
        }

        public static Formula CreateNewCarbonFormula()
        {
            return CreateNewFormula(new Atom(Element.Carbon));
        }

        public static Formula CreateNewChainCarbonFormula(int length)
        {
            var formula = CreateNewFormula(new Atom(Element.Carbon));
            for (var i = 0; i < length - 1; i++)
            {
                formula.AddAtom(new Atom(Element.Carbon));
            }

            return formula;
        }

        public static FormulaRing CreateNewRingCarbonFormula(int size)
        {
            if (size < 3)
            {
                throw new Exception("Ring size must be greater than 2");
            }

            var formula = CreateNewFormula(new Atom(Element.Carbon));
            var ring = new FormulaRing(size, formula);
            formula._rings.Add(ring);
            return ring;
        }

        public Formula AddAtom(Atom newAtom, Bond.BondType bondType = Bond.BondType.Single)
        {
            //Note: Check current state of the current top atom
            var currentAtomData = CheckAtomData(_currentAtom);
            if (currentAtomData.AvailableConnectivity <= 0)
            {
                throw new Exception("Atom has no available connectivity");
            }

            FormulaHelper.AddAtomToStructure(_currentAtom, newAtom, _structure, bondType);
            _mass += newAtom.GetProperty().AtomicMass;
            _currentAtom = newAtom;

            return this;
        }

        public Formula AddStructure(Formula structure, Bond.BondType bondType = Bond.BondType.Single)
        {
            AddAtom(structure._startAtom, bondType);
            foreach (var (key, value) in structure.GetStructure())
            {
                foreach (var bond in value)
                {
                    AddAtom(key, bond.GetBondType());
                }
            }

            // Note: add rings to formula
            _rings.AddRange(structure._rings);

            return this;
        }

        public FormulaRing AddRing(int size, Atom ringStartAtom, Bond.BondType bondType = Bond.BondType.Single)
        {
            AddAtom(ringStartAtom, bondType);
            var ring = new FormulaRing(size, this);
            _rings.Add(ring);
            return ring;
        }

        public Formula MoveToAtom(Atom atom)
        {
            _currentAtom = atom;
            return this;
        }

        #endregion

        #region Formula Implementation
        
        public Dictionary<Atom, List<Bond>> CloneStructure()
        {
            var newStructure = new Dictionary<Atom, List<Bond>>();
            foreach (var temp in _structure)
            {
                var (atom, bonds) = temp;
                newStructure[atom] = new List<Bond>(bonds);
            }

            return newStructure;
        }

        public IReadOnlyDictionary<Atom, IReadOnlyList<Bond>> GetStructure()
        {
            var result = new Dictionary<Atom, IReadOnlyList<Bond>>();
            foreach (var (key, value) in _structure)
            {
                result[key] = new ReadOnlyCollection<Bond>(value);
            }

            return new ReadOnlyDictionary<Atom, IReadOnlyList<Bond>>(result);
        }

        public IReadOnlyList<IFormulaRing> GetRings()
        {
            return _rings;
        }

        public float GetMass()
        {
            return _mass;
        }

        public Atom GetStartAtom()
        {
            return _startAtom;
        }

        public IReadOnlyList<Atom> GetAtoms()
        {
            return _structure.Keys.ToList();
        }

        public IReadOnlyList<Bond> GetBonds()
        {
            return _structure.Values.SelectMany(bonds => bonds).ToList();
        }
        
        public IReadOnlyList<Bond> GetAtomBonds(Atom atom)
        {
            return _structure[atom];
        }

        public FormulaAtomData CheckAtomData(Atom atom)
        {
            var element = atom.GetElement();
            var isCarbon = element == Element.Carbon;
            var hydrogenCount = isCarbon ? 4 - _structure[atom].Count : 0;
            var availableConnectivity = int.MaxValue;
            var neighbors = new List<Atom>();
            var isInFormula = _structure.ContainsKey(atom);
            
            if (!isInFormula)
                return new FormulaAtomData(element, false, false, isCarbon, hydrogenCount,
                    availableConnectivity, neighbors);
            
            var inRing = _rings.Any(ring => ring.RingAtoms.Contains(atom));
            availableConnectivity = FormulaHelper.GetAvailableConnectivity(atom, _structure);
            neighbors = _structure[atom].Select(bond => bond.GetDestinationAtom()).ToList();
            
            return new FormulaAtomData(element, true, inRing, isCarbon, hydrogenCount,
                availableConnectivity, neighbors);
        }

        public void RemoveBond(Atom atom1, Atom atom2)
        {
            var atom1Data = CheckAtomData(atom1);
            var atom2Data = CheckAtomData(atom2);
            switch (atom1Data.InRing)
            {
                case false when !atom2Data.InRing:
                {
                    var bond = _structure[atom1].First(b => b.GetDestinationAtom() == atom2);
                    RemoveBondNonCheckRing(bond);
                    
                    var stack = new Stack<Atom>();
                    
                    stack.Push(bond.GetDestinationAtom());

                    while (stack.TryPop(out var temp))
                    {
                        foreach (var neighbor in _structure[temp].Where(neighbor => neighbor.GetDestinationAtom() == atom1))
                        {
                            RemoveBondNonCheckRing(neighbor);
                            stack.Push(neighbor.GetSourceAtom());
                        }
                    }
                    
                    break;
                }
                case true when atom2Data.InRing:
                {
                    foreach (var ring in _rings)
                    {
                        ring.RemoveBond(atom1, atom2);
                    }

                    break;
                }
            }
        }

        #endregion

        private void RemoveBondNonCheckRing(Bond bond)
        {
            var sourceAtom = bond.GetSourceAtom();
            var destinationAtom = bond.GetDestinationAtom();
            _structure[sourceAtom].Remove(bond);
            _structure[destinationAtom].Remove(bond);
            if (_structure[sourceAtom].Count == 0)
            {
                _structure.Remove(sourceAtom);
            }
            if (_structure[destinationAtom].Count == 0)
            {
                _structure.Remove(destinationAtom);
            }
        }

        public class FormulaRing : IFormulaRing
        {
            public readonly Formula Formula;
            public readonly int Size;
            public Atom StartAtom { get; }
            public bool IsAromatic { get; private set; }

            public bool IsUnstable => _numOfDoubleBond is 3 or 4 or > 8;

            internal readonly List<Bond> RingBonds;
            internal readonly List<Atom> RingAtoms;
            private readonly Dictionary<int, Bond> _branches;

            private int _numOfDoubleBond;
            private bool _isFormed;

            public FormulaRing(int size, Formula formula)
            {
                if (size < 3)
                {
                    throw new Exception("Ring size must be greater than 2");
                }

                Size = size;
                Formula = formula;
                StartAtom = Formula._currentAtom;
                RingBonds = new List<Bond>(size);
                RingAtoms = new List<Atom>(size);
                _branches = new Dictionary<int, Bond>(size);

                RingAtoms.Add(StartAtom);
            }

            public FormulaRing SetAtom(Atom atom, Bond.BondType bondType = Bond.BondType.Single)
            {
                Formula.AddAtom(atom, bondType);
                RingAtoms.Add(atom);
                RingBonds.Add(Formula._structure[atom].First());

                if (bondType == Bond.BondType.Double)
                {
                    _numOfDoubleBond++;
                }

                return this;
            }

            public FormulaRing AddBranchC(int position, Atom sideBranch,
                Bond.BondType bondType = Bond.BondType.Single)
            {
                if (position < 0 || position >= Size)
                {
                    throw new Exception("Branch position is out of range");
                }

                if (_branches.ContainsKey(position))
                {
                    throw new Exception("Branch is already added");
                }

                // Note: add sideBranch to formula
                Formula.MoveToAtom(RingAtoms[position]);
                Formula.AddAtom(sideBranch, bondType);

                _branches.Add(position, Formula._structure[sideBranch].First());

                return this;
            }

            public FormulaRing AddBranchC(int position, Formula sideBranch,
                Bond.BondType bondType = Bond.BondType.Single)
            {
                if (position < 0 || position >= Size)
                {
                    throw new Exception("Branch position is out of range");
                }

                if (_branches.ContainsKey(position))
                {
                    throw new Exception("Branch is already added");
                }

                // Note: add sideBranch to formula
                Formula.MoveToAtom(RingAtoms[position]);
                Formula.AddStructure(sideBranch, bondType);

                _branches.Add(position, Formula._structure[sideBranch._startAtom].First());

                return this;
            }
            
            /// <summary>
            /// A method to form the ring.
            /// This method will create a ring for the formula.
            /// </summary>
            /// <param name="nextCurrentAtomIndex">
            /// The index of the next atom for the builder to continue building the formula.
            /// </param>
            /// <param name="bondType">
            /// The bond type between the start atom and the end atom.
            /// </param>
            /// <returns>
            /// The formula with the formed ring.
            /// </returns>
            /// <exception cref="Exception">
            /// Throw an exception if the ring is already formed or the ring size is not match with the ring structure.
            /// </exception>
            public Formula FormRing(int nextCurrentAtomIndex, Bond.BondType bondType = Bond.BondType.Single)
            {
                if (_isFormed)
                {
                    throw new Exception("Ring is already formed");
                }

                if (Size != RingAtoms.Count)
                {
                    throw new Exception("Ring size is not match with ring structure");
                }

                // Note: set bond between start atom and end atom
                FormulaHelper.AddBondToStructure(StartAtom, Formula._currentAtom, Formula._structure, bondType);
                RingBonds.Add(Formula._structure[Formula._currentAtom].First());
                if (bondType == Bond.BondType.Double)
                {
                    _numOfDoubleBond++;
                }

                // Note: check if ring is aromatic
                if (RingAtoms.Count == 6 && _numOfDoubleBond == 3)
                {
                    var tempBool = 0;
                    for (var i = 0; i < RingAtoms.Count; i++)
                    {
                        if (RingAtoms[i].GetElement() != Element.Carbon)
                        {
                            IsAromatic = false;
                            break;
                        }

                        switch (RingBonds[i].GetBondType())
                        {
                            case Bond.BondType.Single:
                                if (tempBool == 1)
                                {
                                    IsAromatic = false;
                                    break;
                                }

                                tempBool = -1;
                                break;
                            case Bond.BondType.Double:
                                if (tempBool == 1)
                                {
                                    IsAromatic = false;
                                    break;
                                }

                                tempBool = 1;
                                break;
                            case Bond.BondType.Aromatic:
                            case Bond.BondType.Triple:
                            default:
                                IsAromatic = false;
                                break;
                        }

                        if (IsAromatic == false)
                        {
                            break;
                        }
                    }
                }

                // Note: update current atom
                Formula._currentAtom = RingAtoms[nextCurrentAtomIndex];
                _isFormed = true;
                return Formula;
            }

            /// <summary>
            /// A method to return the formula if the ring is already formed.
            /// This method is used to mutate the formula's ring after the ring is formed.
            /// </summary>
            /// <returns>
            /// The formula with the formed ring.
            /// </returns>
            /// <exception cref="Exception">
            /// Throw an exception if the ring is not formed yet.
            /// </exception>
            public Formula Return()
            {
                if (!_isFormed)
                {
                    throw new Exception("Ring is not formed yet");
                }

                return Formula;
            }

            public void RemoveBond(Atom atom1, Atom atom2)
            {
                if (!RingAtoms.Contains(atom1) || !RingAtoms.Contains(atom2)) return;
                var deletedBond = RingBonds.First(bond =>
                    bond.GetDestinationAtom() == atom1 && bond.GetSourceAtom() == atom2 ||
                    bond.GetDestinationAtom() == atom2 && bond.GetSourceAtom() == atom1);
                RingBonds.Remove(deletedBond);
                Formula.RemoveBondNonCheckRing(deletedBond);
                Formula._rings.Remove(this);
            }

            public void AddBranch(int position, Atom sideBranch,
                Bond.BondType bondType = Bond.BondType.Single)
            {
                AddBranchC(position, sideBranch, bondType);
            }

            public void AddBranch(int position, IFormula sideBranch, Bond.BondType bondType = Bond.BondType.Single)
            {
                if (sideBranch is Formula formula)
                {
                    AddBranchC(position, formula, bondType);
                }
            }

            public FormulaAtomData CheckAtomData(Atom atom)
            {
                return Formula.CheckAtomData(atom);
            }

            public FormulaRing CloneRing(Formula formula)
            {
                var ring = new FormulaRing(Size, formula);
                ring.RingAtoms.AddRange(RingAtoms);
                ring.RingBonds.AddRange(RingBonds);
                ring._branches.AddRange(_branches);
                ring._numOfDoubleBond = _numOfDoubleBond;
                ring._isFormed = _isFormed;
                return ring;
            }
        }
    }
}