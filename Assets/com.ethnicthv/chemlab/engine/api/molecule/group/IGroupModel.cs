using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.molecule.formula;

namespace com.ethnicthv.chemlab.engine.api.molecule.group
{
    public interface IGroupModel
    {
        public IFormula GetFormula();
        public bool IsContainsAtom(Atom atom);
    }
}