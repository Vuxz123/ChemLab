using com.ethnicthv.chemlab.engine.api.atom;

namespace com.ethnicthv.chemlab.engine.api.formula
{
    public interface IBondBreaker<A> where A : IAtom
    {
        public void BreakBond(A atom1, A atom2);
    }
}