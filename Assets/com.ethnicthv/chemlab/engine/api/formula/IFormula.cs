using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public interface IFormula : ICloneable
    {
        public IReadOnlyDictionary<Atom, IReadOnlyList<Bond>> GetStructure();
        public IReadOnlyList<Atom> GetChargeAtom();
        public float GetMass();
        public Atom GetStartAtom();
    }
}