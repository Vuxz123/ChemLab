using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public interface IFormula<A>: ICloneable, IFormulaAtomDataChecker<A>, IBondBreaker<A> where A : IAtom
    {
        public IReadOnlyDictionary<A, IReadOnlyList<Bond>> GetStructure();
        public IReadOnlyList<A> GetChargeAtom();
        public IReadOnlyList<IFormulaRing<A>> GetRings();
        public float GetMass();
        public A GetStartAtom();
    }
}