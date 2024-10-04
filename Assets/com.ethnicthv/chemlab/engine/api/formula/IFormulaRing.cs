using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.formula;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public interface IFormulaRing<A> : IFormulaAtomDataChecker<A> , IBondBreaker<A> where A : IAtom
    {
        public void AddBranch(int position, A sideBranch,
            Bond.BondType bondType = Bond.BondType.Single);
        public void AddBranch(int position, IFormula<A> sideBranch,
            Bond.BondType bondType = Bond.BondType.Single);
    }
}