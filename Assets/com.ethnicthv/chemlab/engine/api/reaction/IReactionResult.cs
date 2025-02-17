using System;
using System.Collections.Generic;
using com.ethnicthv.chemlab.engine.molecule;

namespace com.ethnicthv.chemlab.engine.api.reaction
{
    public interface IReactionResult : IComparable<IReactionResult>
    {
        public int GetPriority();
        public Dictionary<Molecule, float> GetConsumedMolecules(ReactionTickContext context);
        public Dictionary<Molecule, float> GetProducedMolecules(ReactionTickContext context);
    }
}