using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.molecule.formula
{
    public interface IFormula: ICloneable, IFormulaAtomDataChecker, IBondBreaker
    {
        public IReadOnlyDictionary<Atom, IReadOnlyList<Bond>> GetStructure();
        public IReadOnlyList<Atom> GetChargeAtom();
        public IReadOnlyList<IFormulaRing> GetRings();
        public float GetMass();
        public Atom GetStartAtom();
        public List<Atom> GetAtoms();
        public List<Bond> GetBonds();
    }
}