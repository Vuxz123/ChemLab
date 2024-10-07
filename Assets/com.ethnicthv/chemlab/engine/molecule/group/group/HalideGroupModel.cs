using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.api.molecule.group;
using com.ethnicthv.chemlab.engine.formula;

namespace com.ethnicthv.chemlab.engine.molecule.group.group
{
    public class HalideGroupModel : IGroupModel
    {
        public static readonly Formula Formula = Formula.CreateNewFormula(new Halogen());
        
        public IFormula GetFormula()
        {
            throw new System.NotImplementedException();
        }

        public bool IsContainsAtom(Atom atom)
        {
            throw new System.NotImplementedException();
        }
    }
}