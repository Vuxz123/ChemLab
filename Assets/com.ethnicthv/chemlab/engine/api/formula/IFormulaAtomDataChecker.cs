using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public interface IFormulaAtomDataChecker<A> where A : IAtom
    {
        public FormulaAtomData CheckAtomData(A atom);
    }
}