using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.molecule.formula
{
    public interface IFormula: ICloneable, IFormulaAtomDataChecker
    {
        public Dictionary<Atom, List<Bond>> CloneStructure();
        public IReadOnlyDictionary<Atom, IReadOnlyList<Bond>> GetStructure();
        public IReadOnlyList<IFormulaRing> GetRings();
        public float GetMass();
        public Atom GetStartAtom();
        public IReadOnlyList<Atom> GetAtoms();
        public IReadOnlyList<Bond> GetBonds();
        public IReadOnlyList<Bond> GetAtomBonds(Atom atom);
    }
}