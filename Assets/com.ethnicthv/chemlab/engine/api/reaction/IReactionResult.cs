using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public interface IReactionResult
    {
        public Dictionary<Molecule, float> GetConsumedMolecules();
        public Dictionary<Molecule, float> GetProducedMolecules();
    }
}