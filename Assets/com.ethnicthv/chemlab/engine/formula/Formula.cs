using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using com.ethnicthv.chemlab.engine.api.formula;

namespace com.ethnicthv.chemlab.engine.formula
{
    public class Formula : IFormula
    {
        private readonly Dictionary<Atom, List<Bond>> _structure;
        private readonly List<Atom> _chargeAtoms;
        private Atom _startAtom;
        private float _mass;
        
        private List<FormulaRing> _rings;
        
        public bool IsAromatic => _rings.Any(ring => ring.IsAromatic);
        public bool IsCyclic => _rings.Any();

        private Atom _currentAtom;

        private Formula()
        {
            _chargeAtoms = new List<Atom>();
            _startAtom = null;
            _mass = 0;
            _structure = new Dictionary<Atom, List<Bond>>();
            _rings = new List<FormulaRing>();
        }

        private Formula(Dictionary<Atom, List<Bond>> structure, Atom startAtom)
        {
            _chargeAtoms = new List<Atom>();
            _mass = 0;
            _structure = structure;
            _startAtom = startAtom;
            if (startAtom.Charge != 0)
            {
                _chargeAtoms.Add(startAtom);
            }
        }

        public object Clone()
        {
            return new Formula(new Dictionary<Atom, List<Bond>>(_structure), _startAtom);
        }

        #region Builder

        private static Formula CreateNewFormula(Atom startAtom)
        {
            return new Formula
            {
                _startAtom = startAtom,
                _currentAtom = startAtom
            };
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

        public Formula AddAtom(Atom atom, Bond.BondType bondType = Bond.BondType.Single)
        {
            FormulaHelper.AddAtomToStructure(_currentAtom, atom, _structure, bondType);
            _mass += atom.GetProperty().AtomicMass;
            _currentAtom = atom;
            if (atom.Charge != 0)
            {
                _chargeAtoms.Add(atom);
            }

            return this;
        }

        public FormulaRing AddRing(int size, Atom ringStartAtom, Bond.BondType bondType = Bond.BondType.Single)
        {
            AddAtom(ringStartAtom, bondType);
            var ring = new FormulaRing(size, this);
            _rings.Add(ring);
            return ring;
        }

        #endregion

        #region Formula Implementation

        public IReadOnlyDictionary<Atom, IReadOnlyList<Bond>> GetStructure()
        {
            var result = new Dictionary<Atom, IReadOnlyList<Bond>>();
            foreach (var (key, value) in _structure)
            {
                result[key] = new ReadOnlyCollection<Bond>(value);
            }

            return new ReadOnlyDictionary<Atom, IReadOnlyList<Bond>>(result);
        }

        public IReadOnlyList<Atom> GetChargeAtom()
        {
            return new ReadOnlyCollection<Atom>(_chargeAtoms);
        }

        public float GetMass()
        {
            return _mass;
        }

        public Atom GetStartAtom()
        {
            return _startAtom;
        }

        #endregion

        public class FormulaRing
        {
            public readonly Formula Formula;
            public readonly int Size;
            public Atom StartAtom { get; private set; }
            public bool IsAromatic { get; private set; }

            public bool IsUnstable => _numOfDoubleBond is 3 or 4 or > 8;
            
            private readonly List<Bond> _ringBonds;
            private readonly List<Atom> _ringAtoms;
            private readonly Dictionary<int, Bond> _branches;

            private int _numOfDoubleBond;
            private bool _isFormed = false;

            public FormulaRing(int size, Formula formula)
            {
                if (size < 3)
                {
                    throw new Exception("Ring size must be greater than 2");
                }

                Size = size;
                Formula = formula;
                StartAtom = Formula._currentAtom;
                _ringBonds = new List<Bond>(size);
                _ringAtoms = new List<Atom>(size);
                _branches = new Dictionary<int, Bond>(size);

                _ringAtoms.Add(StartAtom);
            }

            public FormulaRing SetAtom(Atom atom, Bond.BondType bondType = Bond.BondType.Single)
            {
                Formula.AddAtom(atom, bondType);
                _ringAtoms.Add(atom);
                _ringBonds.Add(Formula._structure[atom].First());

                if (bondType == Bond.BondType.Double)
                {
                    _numOfDoubleBond++;
                }

                return this;
            }
            
            public FormulaRing AddBranch(int position, Atom atom, Bond.BondType bondType = Bond.BondType.Single)
            {
                Formula.AddAtom(atom, bondType);
                _branches.Add(position, Formula._structure[atom].First());

                if (bondType == Bond.BondType.Double)
                {
                    _numOfDoubleBond++;
                }

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
                if (Size != _ringAtoms.Count)
                {
                    throw new Exception("Ring size is not match with ring structure");
                }

                // Note: set bond between start atom and end atom
                FormulaHelper.AddBondToStructure(StartAtom, Formula._currentAtom, Formula._structure, bondType);
                _ringBonds.Add(Formula._structure[Formula._currentAtom].First());
                if (bondType == Bond.BondType.Double)
                {
                    _numOfDoubleBond++;
                }

                // Note: check if ring is aromatic
                if (_ringAtoms.Count == 6)
                {
                    var tempBool = 0;
                    for (var i = 0; i < _ringAtoms.Count; i++)
                    {
                        if (_ringAtoms[i].Element != Element.Carbon)
                        {
                            IsAromatic = false;
                            break;
                        }

                        switch (_ringBonds[i].GetBondType())
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
                Formula._currentAtom = _ringAtoms[nextCurrentAtomIndex];
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
        }
    }
}