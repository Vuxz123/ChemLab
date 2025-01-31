using com.ethnicthv.chemlab.engine.api.molecule;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.formula;

namespace com.ethnicthv.chemlab.engine.molecule
{
    public class Molecule : IMolecule
    {
        private readonly Formula _formula;
        
        public Molecule(Formula formula)
        {
            _formula = formula;
        }

        public IFormula GetFormula()
        {
            return _formula;
        }
    }
}