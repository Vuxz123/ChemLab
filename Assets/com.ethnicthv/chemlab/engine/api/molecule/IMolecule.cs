using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.api.atom;
using com.ethnicthv.chemlab.engine.api.molecule.formula;
using com.ethnicthv.chemlab.engine.api.molecule.group;

namespace com.ethnicthv.chemlab.engine.api.molecule
{
    public interface IMolecule
    {
        public IFormula GetFormula();
        public IReadOnlyCollection<MoleculeGroup> GetGroups();
        public IReadOnlyCollection<Atom> GetAtomsInGroup(MoleculeGroup group);
    }
}